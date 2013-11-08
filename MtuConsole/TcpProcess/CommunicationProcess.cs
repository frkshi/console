using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using MtuConsole.Common;
using System.Data;

using DataAccess;
using DataEntity;

using System.Linq;

namespace MtuConsole.TcpProcess
{


    class CommunicationProcess : CommunicationProcessBase
    {


        public CommunicationProcess()
        {
            _senddatas = new List<SendData>();
        }



        public CommunicationProcess(SendMsg msg, SendToConsole getsendtoapi, IDecode getdecode, RWDatabase getrw, ManualResetEvent commdoneEvent, string getprocessid, int iport)
        {
            _servicelog = new MtuLog();
            commMsg = msg;
            doneEvent = commdoneEvent;
            m_decode = getdecode;
            sendtoapiobj = getsendtoapi;
            processid = getprocessid;
            _rwDatabase = getrw;
            _portID = iport;
            m_decode.RwDataBase = getrw;
        }





        public override void CommMsgProcess_Thread(object obj)
        {
            bool originalMessageSend = false;
            Message apimsg = new Message();


            int sourceid = 0;
            string rtuid = "";
            int debugi = 0;
            try
            {
                ArrayList resultArray = new ArrayList();

                sDataType dataType = new sDataType();

                //原始数据
                apimsg.Type = MessageType.Original;
                OriginalMessageBody tmpbody = new OriginalMessageBody();
                tmpbody.Datetime = DateTime.Now;
                tmpbody.Port = _portID.ToString();
                tmpbody.Data = commMsg.Msg;
                tmpbody.IP = commMsg.Msg;
                tmpbody.CommunicationId = processid;
                apimsg.Body = tmpbody;
                _servicelog.Debug("CommMsgProcess_Thread recieved:" + commMsg.Msg.ToString());
                debugi = 1;
                resultArray = m_decode.Trans2ArrayList(commMsg.Msg, out dataType, out rtuid);
                debugi = 2;
                if (resultArray != null)
                {
                    if (resultArray.Count == 0)
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
                //保存至DB
                SavetoDB(resultArray, dataType, rtuid);
                tmpbody.RtuId = rtuid;
                originalMessageSend = true;
                List<Message> sendmsgs = TransArray2String(resultArray, dataType, rtuid);


                if (sendmsgs != null)
                {
                    sendtoapiobj.Send(sendmsgs, commMsg.Msg);
                }
                doneEvent.Set();
            }
            catch (Exception e)
            {
                try
                {
                    if (sourceid == 0)
                    {
                        SaveSourceData(commMsg.Msg, rtuid, 0);
                    }
                    if (!originalMessageSend)
                    {
                        sendtoapiobj.Send(apimsg);
                    }
                }
                catch (Exception ex)
                {
                    _servicelog.Error(ex.Message, ex);
                }
                _servicelog.Error(e.Message, e);
                _servicelog.Error(debugi.ToString() + "errcode");
            }
        }
     
        /// <summary>
        /// 处理残留数据
        /// </summary>
        /// <param name="obj"></param>
        public override void RemainMsgProcess()
        {
            bool originalMessageSend = false;
            Message apimsg = new Message();
            int decodestatus = 0;//0为未解包，1:解包
            int sourceid = 0;
            string rtuid = "";
            try
            {
                ArrayList resultArray = new ArrayList();
                sDataType dataType = new sDataType();
                //原始数据
                apimsg.Type = MessageType.Original;
                OriginalMessageBody tmpbody = new OriginalMessageBody();
                tmpbody.Datetime = DateTime.Now;
                tmpbody.Port = _portID.ToString();
                tmpbody.Data = commMsg.Msg;
                tmpbody.IP = commMsg.RemoteIP;
                tmpbody.CommunicationId = processid;
                apimsg.Body = tmpbody;
                m_decode.MeasureSetting = commMsg.Measuresetting; 
                _servicelog.Debug("RemainMsgProcess_Thread recieved:" + commMsg.Msg.ToString());
                resultArray = m_decode.Trans2ArrayList(commMsg.Msg, out dataType, out rtuid);
                if (resultArray != null)
                {
                    if (resultArray.Count > 0)
                    {
                        decodestatus = 1;
                    }
                }

                //保存至DB
                SavetoDB(resultArray, dataType, rtuid);
                sendtoapiobj.Send(TransArray2String(resultArray, dataType, rtuid));
                tmpbody.RtuId = rtuid;
                
                sendtoapiobj.Send(apimsg);  //原始数据
                originalMessageSend = true;
                doneEvent.Set();
            }
            catch (Exception e)
            {
                try
                {
                    if (sourceid == 0)
                    {
                        SaveSourceData(commMsg.Msg, rtuid, 0);
                    }
                    if (!originalMessageSend)
                    {
                        sendtoapiobj.Send(apimsg);
                    }
                }
                catch (Exception ex)
                {
                    _servicelog.Error(ex.Message, ex);
                }
                _servicelog.Error(e.Message, e);
            }
        }

        private string GetMeasureNameByDataType(string dataID, string rtuid)
        {
            string result;

            try
            {
                DataRow[] typerows = m_decode.MeasureSetting.Select("datatype='" + dataID + "' and rtuid='" + rtuid + "'");
                result = typerows[0]["measurename"].ToString();
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
                result = dataID.ToString();
            }
            return result;

        }


        /// <summary>
        /// 将arraylist,datatype专成message
        /// </summary>
        /// <param name="getArray"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        private List<Message> TransArray2String(ArrayList getArray, sDataType datatype, string rtuID)
        {

            if (getArray == null)
            {
                return null;
            }
            else if (getArray.Count == 0)
            {
                return null;
            }
            List<Message> msgs = new List<Message>();

            Message apimsg = new Message();
            apimsg.Type = MessageType.Final;
            try
            {
                switch (datatype)
                {
                   
                    case sDataType.MeasureData:
                        MeasureMessageBody tmpbody = new MeasureMessageBody();
                        MeasureValueCollection tmpitems = new MeasureValueCollection();
                        sMeterData meterData;
                        tmpbody.Port = _portID.ToString();
                        tmpbody.CommunicationId = processid;
                        tmpbody.RtuId = rtuID;
                        tmpbody.IP = commMsg.RemoteIP;
                        meterData = (sMeterData)getArray[0];
                        DataRow[] typerows = m_decode.MeasureSetting.Select("datatype = '" + TransDataIDtoDatatype(meterData.DataID) + "' and rtuid='" + rtuID + "'");
                        if (typerows.Length > 0)
                        {
                            tmpbody.DataType = typerows[0]["DataType"].ToString();
                            tmpbody.MeasureId = typerows[0]["measureid"].ToString();
                            tmpbody.MeasureName = typerows[0]["measurename"].ToString();
                        }
                        for (int i = 0; i < getArray.Count; i++)
                        {
                            meterData = (sMeterData)getArray[i];
                            MeasureValue tmpvalue = new MeasureValue();
                            tmpvalue.Datetime = meterData.CollectDate;
                            tmpvalue.OriginalValue = CommonMethod.ConvertToDecimal(meterData.Data, 3).ToString();
                            tmpitems.AddItem(tmpvalue);
                        }
                        tmpbody.Items = tmpitems;
                        apimsg.Body = tmpbody;
                        msgs.Add(apimsg);
                        
                        break;


                    case sDataType.AlarmBreakLimitUp:    //上限报警
                    case sDataType.AlarmBreakLimitDown:  //下限报警
                    case sDataType.AlarmBreakLimitUpBack: //上限恢复
                    case sDataType.AlarmBreakLimitDownBack: //下限恢复
                        LimitAlertMessageBody tmpalart = new LimitAlertMessageBody();
                        sMeterData alarmlimitData = new sMeterData();
                        alarmlimitData = (sMeterData)getArray[0];
                        tmpalart.CommunicationId = processid;
                        tmpalart.Port = _portID.ToString();
                        tmpalart.MeasureName = GetMeasureNameByDataType(alarmlimitData.DataID, rtuID);
                        tmpalart.IP = commMsg.RemoteIP;
                        tmpalart.RtuId = rtuID;
                        tmpalart.OriginalValue = alarmlimitData.Data;
                        tmpalart.Datetime = alarmlimitData.CollectDate;
                        for (int n = 0; n < getArray.Count; n++)
                        {
                            AlertItem alertitem = new AlertItem();
                            sMeterData tempitemdata = (sMeterData)getArray[n];
                            alertitem.Time = tempitemdata.CollectDate;
                            alertitem.Value = tempitemdata.Data;
                            tmpalart.AddAlertValue(alertitem);
                        }
                        apimsg.Body = tmpalart;
                        msgs.Add(apimsg);
                        break;
                    case sDataType.AlarmSuddenBreak://突变报警
                    case sDataType.AlarmSuddenRise://突升报警
                        MutationAlertMessageBody tmpmutatiaon = new MutationAlertMessageBody();
                        sMeterData alarmSuddenLowData;
                        alarmSuddenLowData = (sMeterData)getArray[0];
                        tmpmutatiaon.CommunicationId = processid;
                        tmpmutatiaon.Port = _portID.ToString();
                        tmpmutatiaon.RtuId = rtuID;
                        tmpmutatiaon.IP = commMsg.RemoteIP;
                        tmpmutatiaon.MeasureName = GetMeasureNameByDataType(alarmSuddenLowData.DataID, rtuID);
                        tmpmutatiaon.OriginalValue1 = alarmSuddenLowData.Data;
                        sMeterData alarmsuddenlowdata2 = (sMeterData)getArray[1];
                        tmpmutatiaon.OriginalValue2 = alarmsuddenlowdata2.Data;
                        tmpmutatiaon.Datetime = alarmSuddenLowData.CollectDate;
                        for (int n = 0; n < getArray.Count; n++)
                        {
                            AlertItem alertitem = new AlertItem();
                            sMeterData tempitemdata = (sMeterData)getArray[n];
                            alertitem.Time = tempitemdata.CollectDate;
                            alertitem.Value = tempitemdata.Data;
                            tmpmutatiaon.AddAlertValue(alertitem);
                        }

                        apimsg.Body = tmpmutatiaon;
                        msgs.Add(apimsg);
                        break;

                  


                    //case sDataType.QueryResponse://查询回复

                    //    //将上报的参数信息抛给API
                    //    ResponseReadValue getitem = (ResponseReadValue)getArray[0];
                    //    DeviceResponseMessageBody responsebody = new DeviceResponseMessageBody();
                    //    string showstr = string.Empty;
                    //    responsebody.RtuId = rtuID;
                    //    responsebody.CommunicationId = processid;
                    //    responsebody.Port = _portID.ToString();
                    //    foreach (ParameterItem item in getitem.Values)
                    //    {
                    //        showstr = showstr + "[" + item.ParameterName + "]=" + item.Value + ";";
                    //    }
                    //    if (getitem.DataID == "98")
                    //    {
                    //        string batteryleft = "[电池余量]=" + Convert.ToInt16(CaculateBatteryLeft(Convert.ToInt32(getitem.Values[2].Value)) * 100).ToString() + "%;";
                    //        showstr = batteryleft + showstr;
                    //    }
                    //    responsebody.Message = showstr;
                    //    responsebody.DataType = API.Protocol.iq.mtu.DeviceDataTypeEnum.Data;
                    //    responsebody.ReceiveTime = DateTime.Now;
                    //    responsebody.Type = getitem.DataName;
                    //    responsebody.TypeCode = getitem.DataID;


                    //    // responsebody.TypeCode = "";

                    //    apimsg.Body = responsebody;
                    //    apimsg.Type = MessageType.Notice;
                    //    msgs.Add(apimsg);



                    //    break;

                    default:

                        break;
                }
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
            return msgs;
        }



        /// <summary>
        /// 保存已解数据至DB
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="typeid"></param>
        private void SavetoDB(ArrayList getArray, sDataType typeid, string rtuid)
        {

            try
            {

                if (getArray == null)
                {
                    return;
                }
                else if (getArray.Count == 0)
                {
                    return;
                }
                switch (typeid)
                {
                    #region 检测量

                    case sDataType.MeasureData://检测量数据
                   

                        for (int i = 0; i < getArray.Count; i++)
                        {
                            sMeterData meterData = new sMeterData();
                            MeasureData data = new MeasureData();
                            meterData = (sMeterData)getArray[i];
                            data.CollDatetime = meterData.CollectDate;
                            data.CollNum = CommonMethod.ConvertToDecimal(meterData.Data, 3);
                            data.RTUId = rtuid;
                            data.MeasureId = Convert.ToInt32(GetMeasureID(rtuid, TransDataIDtoDatatype(meterData.DataID)));
                            data.UpdateTime = DateTime.Now;
                            data.Sign =  (byte)1;
                            data.Tag = 1;
                            _rwDatabase.AddToWrite(data);


                        }

                        break;
                    #endregion
                    #region 上下限报警 突变报警

                    case sDataType.AlarmBreakLimitUp:
                    case sDataType.AlarmBreakLimitDown:  //上下限报警
                    case sDataType.AlarmSuddenBreak://突降报警
                    case sDataType.AlarmSuddenRise://突升报警
                    case sDataType.AlarmBreakLimitUpBack: //上限恢复
                    case sDataType.AlarmBreakLimitDownBack: //下限恢复
                        AlertDataDetail alertdata2 = new AlertDataDetail();
                        sMeterData alarmSuddenLowData;
                        alarmSuddenLowData = (sMeterData)getArray[0];
                        alertdata2.RTUId = rtuid;
                        alertdata2.CollTimes = GetCollectTimes(getArray);
                        alertdata2.CollNums = GetCollectNums(getArray);
                        alertdata2.MeasureId = Convert.ToInt32(GetMeasureID(rtuid, TransDataIDtoDatatype(alarmSuddenLowData.DataID)));
                        alertdata2.AlertTypeId = GetAlertTypeID(typeid);


                        alertdata2.Sign = 1;
                        if (alertdata2.AlertTypeId > 0)
                        {
                            _rwDatabase.AddToWriteAlertDetail(alertdata2);
                        }

                        break;
                    #endregion
                  
                    

                }
            }
            catch (Exception e)
            {

                _servicelog.Error(e.Message, e);
            }
            return;
            
        }

     
       
        /// <summary>
        /// 将查询回复转换为 entity rtusetting
        /// </summary>
        /// <param name="readvalue"></param>
        /// <returns></returns>
        private RTUSetting TranstoRtusetting(ResponseReadValue readvalue, out MeasureSetting pressuresetting)
        {
            RTUSetting result = new RTUSetting();
            pressuresetting = new MeasureSetting();
            switch (readvalue.DataID)
            {
                case "000":  //系统信息
                    //if (readvalue.Values.Count == 4)
                    //{
                    //    result.ProductType = readvalue.Values[0].Value.ToString();
                    //    result.SoftwareVersion = readvalue.Values[1].Value.ToString();
                    //    result.HardwareVersion = readvalue.Values[2].Value.ToString();
                    //}
                    break;
                case "010": //通讯信息
                    //if (readvalue.Values.Count ==6)
                    //{
                    //    result.APN = readvalue.Values[0].Value.ToString();
                    //    result.IP1 = readvalue.Values[1].Value.ToString();
                    //    result.Port1 = readvalue.Values[2].Value.ToString();
                    //}
                    break;
                case "020":
                    //if (readvalue.Values.Count == 3)
                    //{
                    //    result.SecretIP = readvalue.Values[0].Value.ToString();
                    //    result.SecretPort = readvalue.Values[1].Value.ToString();
                    //}

                    break;
                case "030": //DLA周期
                    if (readvalue.Values.Count == 3)
                    {
                        result.SaveCycle = CommonMethod.isInt(readvalue.Values[0].Value.ToString()) ? Convert.ToInt32(readvalue.Values[0].Value.ToString()) : 0;
                        result.SendCycle = CommonMethod.isInt(readvalue.Values[1].Value.ToString()) ? Convert.ToInt32(readvalue.Values[1].Value.ToString()) : 0;
                    }
                    break;
             
                case "090": //回差
                    if (readvalue.Values.Count == 2)
                    {
                        result.BackValue = CommonMethod.ConvertToDecimal(readvalue.Values[0].Value.ToString(), 2);
                    }
                    break;
             
                case "240"://报警设置
                    if (readvalue.Values.Count == 6)
                    {
                        pressuresetting.SendOutData = CommonMethod.isInt(readvalue.Values[0].Value.ToString()) ? CommonMethod.ToBool(readvalue.Values[0].Value.ToString()) : false;
                        pressuresetting.SendChangeData = CommonMethod.isInt(readvalue.Values[0].Value.ToString()) ? CommonMethod.ToBool(readvalue.Values[0].Value.ToString()) : false;
                        pressuresetting.IncreaseInterval = CommonMethod.ToInt(readvalue.Values[2].Value.ToString());
                        pressuresetting.UpperLimit = CommonMethod.ConvertToDecimal(readvalue.Values[3].Value.ToString(), 2);
                        pressuresetting.LowerLimit = CommonMethod.ConvertToDecimal(readvalue.Values[4].Value.ToString(), 2);
                        //result.PressureSendOutDataAlert = CommonMethod.isInt(readvalue.Values[0].Value.ToString()) ? Convert.ToInt32(readvalue.Values[0].Value.ToString()) : 0;
                        //result.PressureSendChangeAlert = CommonMethod.isInt(readvalue.Values[1].Value.ToString()) ? Convert.ToInt32(readvalue.Values[1].Value.ToString()) : 0;
                        //result.IncreaseInterval = CommonMethod.ConvertToDecimal(readvalue.Values[2].Value.ToString(), 2);
                        //result.Uplimit = CommonMethod.ConvertToDecimal(readvalue.Values[3].Value.ToString(), 2);
                        //result.LowLimit = CommonMethod.ConvertToDecimal(readvalue.Values[4].Value.ToString(),2);


                    }


                    break;
            }

            return result;
        }

        private string GetCollectNums(ArrayList arr)
        {
            string result = "";
            sMeterData meterdata = new sMeterData();
            for (int i = 0; i < 60; i++)
            {
                meterdata = (sMeterData)arr[i];
                result = result + meterdata.Data + ",";

            }
            result = result.TrimEnd(',');
            return result;
        }

        private string GetCollectTimes(ArrayList arr)
        {
            string result = "";
            sMeterData meterdata = new sMeterData();
            for (int i = 0; i < 60; i++)
            {
                meterdata = (sMeterData)arr[i];
                result = result + meterdata.CollectDate.ToString("yyyy-MM-dd HH:mm:ss") + ",";


            }
            result = result.TrimEnd(',');
            return result;
        }
        /// <summary>
        /// 向本地保存原始数据

        /// 返回数据ID
        /// </summary>
        /// <param name="sourcedata"></param>
        private int SaveSourceData(string sourcedata, string rtuid, int decodestatus)
        {
            CollectionData data = new CollectionData();
            data.MessageContent = sourcedata;
            data.SendTime = DateTime.Now;
            data.Status = decodestatus;

            _rwDatabase.AddToWrite(data);
            return 0;
        }





        private string GetMeasureIdByPort(string rtuid, string portid)
        {
            string result = string.Empty;

            DataRow[] dr = m_decode.MeasureSetting.Select("rtuid='" + rtuid + "' and portid='" + portid + "'");
            if (dr.Length > 0)
            {
                result = dr[0]["measureid"].ToString();
            }
            return result;
        }

        private string GetMeasureID(string rtuid, string dataid)
        {
            string result = "0";

            DataRow[] dr = m_decode.MeasureSetting.Select("rtuid='" + rtuid + "' and datatype='" + dataid + "'");
            if (dr.Length > 0)
            {
                result = dr[0]["measureid"].ToString();
            }
            return result;

        }

        /// <summary>
        /// 获取指定rtu,datatype的偏移量
        /// </summary>
        /// <param name="rtuid"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        private Decimal GetOffSet(string rtuid, string datatype)
        {

            Decimal result = 0;

            DataRow[] dr = m_decode.MeasureSetting.Select("rtuid='" + rtuid + "' and datatype='" + datatype + "'");
            if (dr.Length > 0)
            {
                result = CommonMethod.IsNumeric(dr[0]["offset"].ToString()) ? Convert.ToDecimal(dr[0]["offset"].ToString()) : 0;
            }
            return result;

        }

       

        private int GetAlertTypeID(sDataType alerttype)
        {
            int result = 0;


            try
            {
                switch (alerttype)
                {

                    case sDataType.AlarmBreakLimitUp: //越上限
                        result = 1;
                        break;
                    case sDataType.AlarmBreakLimitDown: //越下限
                        result = 2;
                        break;
                    case sDataType.AlarmSuddenBreak: //突降
                        result = 26;
                        break;
                    case sDataType.AlarmSuddenRise://突升
                        result = 25;
                        break;
                    case sDataType.AlarmBreakLimitUpBack://上限恢复
                        result = 23;
                        break;
                    case sDataType.AlarmBreakLimitDownBack://下限恢复
                        result = 24;
                        break;

                }
            }
            catch (Exception e)
            {
                _servicelog.Error(e.Message, e);
            }
            return result;
        }
       
        /// <summary>
        /// 将New DLA所涉标号,从真实值转换为数据库中标号
        /// </summary>
        /// <param name="dataid"></param>
        /// <returns></returns>
        private string TransDataIDtoDatatype(string dataid)
        {
            string result = "";
            switch (dataid)
            {
                case "1a0":
                case "180":
                    result = "01";
                    break;
                case "080":
                    result = "99";  //温度
                    break;
                case "1501":
                    result = "98"; //场强
                    break;
                case "1502":   //电池余量

                    result = "05"; //电量
                    break;
           
        
                default:
                    result = dataid;
                    break;
            }
            return result;
        }




    }

}
