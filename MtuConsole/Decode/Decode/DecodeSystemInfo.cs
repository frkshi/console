using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FunctionLib;
using MtuConsole.TcpProcess;
using MtuConsole.Common;
using DataEntity;
using System.Data;

namespace Decode
{
    /// <summary>
    /// SystemInfo
    /// </summary>
    public class DecodeSystemInfo : DecodeBase
    {
        //protected override ArrayList TransFormData(sFrameData data, out sDataType datatype)
        //{
        //    ArrayList result = new ArrayList();

        //    datatype = sDataType.None;
        //    return result;
        //}

        protected override ArrayList TransFormData(sFrameData data, out sDataType datatype)
        {
            ArrayList result = new ArrayList();

            ResponseReadValue readvalue;
            datatype = sDataType.QueryResponse;
            string[] parameternames;
            string[] parametervalues;
            ParameterItem item;

            readvalue = new ResponseReadValue();
            readvalue.DataID = Common.ConvertFromDataType(data.DataTag);
            readvalue.DataName = Common.ConvertFromDataTypetoName(data.DataTag);
            parameternames = Common.ConvertFromDataTypetoParameterNames(data.DataTag);

            switch (data.DataTag)
            {
                case DataTypeDatalog.D_000: //系统参数

                    parametervalues = data.DataString.Split('-');

                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i];

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }
                    }

                    break;
                case DataTypeDatalog.D_010:
                

                    parametervalues = data.DataString.Split('-');

                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i];

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }

                    break;

                case DataTypeDatalog.D_030:
                

                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i].ConvertFrom62().ToString();

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }
                    break;


                case DataTypeDatalog.D_150:   //时间+‘-’+场强+‘-’+电池电量消耗‘-’+终端连接次数
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        item = new ParameterItem();
                        item.Value = parametervalues[0].ConvertToDateFrom62();
                        item.ParameterName = parameternames[0];
                        readvalue.AddValue(item);

                        for (int i = 1; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();
                            item.Value = CommonMethod.ToInt(parametervalues[i]).ToString();   //该类参数非16进制
                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }
                    break;

                case DataTypeDatalog.D_250:  //需要改，
                case DataTypeDatalog.D_260:
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();
                            item.Value = parametervalues[i].ConvertFrom16().ToString();
                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }
                    }
                    break;
                case DataTypeDatalog.D_230:
                    
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == 2 && parametervalues.Length == 1)
                    {
                        item = new ParameterItem();
                        item.Value = parametervalues[0].Substring(0, 1);
                        item.ParameterName = parameternames[0];
                        readvalue.AddValue(item);

                        readvalue.AddValue(new ParameterItem { Value = parametervalues[0].Substring(1, 1), ParameterName = parameternames[1] });

                    }
                    break;
                case DataTypeDatalog.D_240:  //上限报警有效 + 突变报警有效+‘-’+上限值+‘-’+下限值‘-’+突变变化值
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == 5 && parametervalues.Length == 4)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {

                            item = new ParameterItem();
                            switch (i)
                            {

                                case 0://报警有效
                                    string alertstr = parametervalues[0].PadLeft(4, '0');
                                    item.Value = alertstr.Substring(0, 1);
                                    item.ParameterName = parameternames[0];
                                    readvalue.AddValue(item);

                                    item = new ParameterItem();
                                    item.Value = alertstr.Substring(1, 1);
                                    item.ParameterName = parameternames[1];
                                    readvalue.AddValue(item);
                                    break;
                                case 1:
                                case 2:
                                case 3:

                                    item.Value = parametervalues[i].ConvertHexToFloat().ToString("f3");

                                    item.ParameterName = parameternames[i];
                                    readvalue.AddValue(item);
                                    break;
                                case 4:
                                    item.Value = parametervalues[i].ConvertFrom16().ToString();

                                    item.ParameterName = parameternames[i];
                                    readvalue.AddValue(item);
                                    break;

                            }



                        }

                    }

                    break;

                case DataTypeDatalog.D_180: //压力
          


                    item = new ParameterItem();
                    item.Value = data.DataString.ConvertHexToFloat().ToString("f8");
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);

                    break;

                case DataTypeDatalog.D_200://系统运行状态
                
                    item = new ParameterItem();
                    item.Value = data.DataString;
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);

                    break;
                case DataTypeDatalog.D_220:
           
                    item = new ParameterItem();
                    item.Value = data.DataString;
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
              
                case DataTypeDatalog.D_050:  //时间

                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();
                            string datestr = "20{0}-{1}-{2} {3}:{4}:{5}";
                            if (parametervalues[i].Length == 6)
                            {
                                datestr = string.Format(datestr, parametervalues[i].Substring(0, 1).ConvertFrom62().ToString().PadLeft(2, '0'), parametervalues[i].Substring(1, 1).ConvertFrom62(), parametervalues[i].Substring(2, 1).ConvertFrom62(), parametervalues[i].Substring(3, 1).ConvertFrom62(), parametervalues[i].Substring(4, 1).ConvertFrom62(), parametervalues[i].Substring(5, 1).ConvertFrom62());
                            }
                            else
                            {
                                datestr = "";
                            }
                            item.Value = datestr;
                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }
                    }

                    break;

                case DataTypeDatalog.D_080: //当前温度
                    item = new ParameterItem();
                    item.Value = Convert.ToString(data.DataString.ConvertHexToFloat());
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
                case DataTypeDatalog.D_090: //回差值
                    item = new ParameterItem();
                    item.Value = data.DataString.ConvertHexToFloat().ToString();
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
                default:
                    break;
            }

            item = new ParameterItem();
            item.Value = data.TU;
            item.ParameterName = "终端编号";
            readvalue.AddValue(item);

            result.Add(readvalue);
            datatype = sDataType.QueryResponse;
            return result;

        }
    }

    /// <summary>
    /// DataInfo
    /// </summary>
    public class DecodeDataInfo : DecodeBase
    {
       

        protected override ArrayList TransFormData(sFrameData data, out sDataType datatype)
        {
            ArrayList result = new ArrayList();
            datatype = sDataType.None;
           
            
            decimal scale = 1.0M, offset = 0.0M;
           
            switch (data.DataTag)
            {

                #region D_080 温度

                case DataTypeDatalog.D_080:
                    GetScaleAndOffset("99", data.TU, out scale, out offset);
                    foreach (CollectData collectdata in data.CollectDatas)
                    {
                        sMeterData meterdata = new sMeterData();
                        meterdata.CollectDate = collectdata.CollectTime;
                        meterdata.Data = ((decimal)collectdata.Data.ConvertHexToFloat() * scale + offset).ToString();
                        meterdata.DataID = Common.ConvertFromDataType(data.DataTag);
                        result.Add(meterdata);
                    }
                    datatype = sDataType.MeasureData;

                    // item = new ParameterItem();
                    //item.Value = Convert.ToString(data.DataString.ConvertHexToFloat());
                    //item.ParameterName = parameternames[0];
                    //readvalue.AddValue(item);
                    break;
                #endregion
                #region D_150: 场强+‘-’+电池剩余次数

                case DataTypeDatalog.D_150: //场强+‘-’+电池剩余次数 - 连接次数


                    GetScaleAndOffset("98", data.TU, out scale, out offset);
                    decimal scale2 = 1m;
                    decimal offset2 = 0m;
                    GetScaleAndOffset("05", data.TU, out scale2, out offset2);
                    //数据信息
                    foreach (CollectData collectdata in data.CollectDatas)
                    {

                        string data1 = "0", data2 = "0", data3 = "0";
                        string[] getdatas = collectdata.Data.Split('-');
                        if (getdatas.Length > 2)
                        {
                            data1 = getdatas[0];
                            data2 = getdatas[1];
                            data3 = getdatas[2];
                        }

                        sMeterData meterdata1 = new sMeterData();
                        meterdata1.CollectDate = collectdata.CollectTime;
                        meterdata1.Data = (data1.ConvertFrom16() * scale + offset).ToString();
                        meterdata1.DataID = Common.ConvertFromDataType(data.DataTag);
                        result.Add(meterdata1);

                        sMeterData meterdata2 = new sMeterData();
                        meterdata2.CollectDate = collectdata.CollectTime;
                        meterdata2.Data = (data2.ConvertFrom16() * scale + offset).ToString();
                        meterdata2.DataID = Common.ConvertFromDataType(data.DataTag);

                        result.Add(meterdata2);

                    }
                    datatype = sDataType.MeasureData;   //检测量信息 
                    break;
                #endregion
                #region D_180:  压力数据
                
                case DataTypeDatalog.D_180:  //整型,压力数据
                

                      GetScaleAndOffset("01", data.TU, out scale, out offset);

                      scale = 1;
                    //数据信息
                    foreach (CollectData collectdata in data.CollectDatas)
                    {
                        sMeterData meterdata = new sMeterData();
                        meterdata.CollectDate = collectdata.CollectTime;
                        meterdata.Data = ((decimal)collectdata.Data.ConvertFrom62() * scale + offset).ToString();
                        meterdata.DataID = Common.ConvertFromDataType(data.DataTag);
                        result.Add(meterdata);
                    }
                    datatype = sDataType.MeasureData;   //检测量信息 
                    break;
               
               

                #endregion




                default:
                    break;
            }

            return result;
        }



     
    }


    /// <summary>
    /// AlertInfo
    /// </summary>
    public class DecodeAlertInfo : DecodeBase
    {
        protected override ArrayList TransFormData(sFrameData data, out sDataType datatype)
        {
            ArrayList result = new ArrayList();
           
            switch (data.DataTag)
            {

                case DataTypeDatalog.D_111:  //压力上限报警
                case DataTypeDatalog.D_113:  //压力下限报警
                case DataTypeDatalog.D_115:  //压力突升报警
                case DataTypeDatalog.D_117:  //压力突降报警
                case DataTypeDatalog.D_112: //压力上限消警
                case DataTypeDatalog.D_114: //压力下限消警
                    datatype = TransToDatatype(data.DataTag);
                    foreach (CollectData collectdata in data.CollectDatas)
                    {
                        sMeterData meterdata = new sMeterData();
                        meterdata.DataID = "01";
                        meterdata.CollectDate = collectdata.CollectTime;
                        meterdata.Data = collectdata.Data.ConvertHexToFloat().ToString("f3");
                        result.Add(meterdata);
                    }
                    break;
               
                
                default:
                    foreach (CollectData collectdata in data.CollectDatas)
                    {
                        sMeterData meterdata = new sMeterData();
                        meterdata.CollectDate = collectdata.CollectTime;
                        meterdata.Data = collectdata.Data.ConvertHexToFloat().ToString();
                        result.Add(meterdata);
                    }
                    datatype = sDataType.MeasureData;
                    break;
            }
            return result;
        }

        private sDataType TransToDatatype(DataTypeDatalog gettype)
        {
            sDataType result = sDataType.None;
            switch (gettype)
            {
                case DataTypeDatalog.D_111://压力上限报警
                    result=sDataType.AlarmBreakLimitUp;
                    break;
                case DataTypeDatalog.D_113://压力下限报警
                    result = sDataType.AlarmBreakLimitDown;
                    break;
                case DataTypeDatalog.D_115: //压力突升报警
                    result = sDataType.AlarmSuddenRise;
                    break;
                case DataTypeDatalog.D_117://压力突降报警
                    result = sDataType.AlarmSuddenBreak;
                    break;
                case DataTypeDatalog.D_112://压力上限消警
                    result = sDataType.AlarmBreakLimitUpBack;
                    break;
                case DataTypeDatalog.D_114://压力下限消警
                    result = sDataType.AlarmBreakLimitDownBack;
                    break;
               
            }
                 



            return result;
        }
    }

    /// <summary>
    /// 查询回复解包
    /// </summary>
    public class DecodeQueryResponse : DecodeBase
    {
        protected override ArrayList TransFormData(sFrameData data, out sDataType datatype)
        {
            ArrayList result = new ArrayList();

            ResponseReadValue readvalue;
            datatype = sDataType.QueryResponse;
            string[] parameternames;
            string[] parametervalues;
            ParameterItem item;

            readvalue = new ResponseReadValue();
            readvalue.DataID = Common.ConvertFromDataType(data.DataTag);
            readvalue.DataName = Common.ConvertFromDataTypetoName(data.DataTag);
            parameternames = Common.ConvertFromDataTypetoParameterNames(data.DataTag);

            switch (data.DataTag)
            {
                case DataTypeDatalog.D_000: //系统参数

                    parametervalues = data.DataString.Split('-');

                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i];

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }
                    }

                    break;
                case DataTypeDatalog.D_010:
     
                 
                    parametervalues = data.DataString.Split('-');

                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i];

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }

                    break;

                case DataTypeDatalog.D_030:
                
               
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i].ConvertFrom62().ToString();

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }
                    break;

                
                case DataTypeDatalog.D_150:   //时间+‘-’+场强+‘-’+电池电量消耗‘-’+终端连接次数
                     parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        item = new ParameterItem();
                        item.Value = parametervalues[0].ConvertToDateFrom62();
                        item.ParameterName = parameternames[0];
                        readvalue.AddValue(item);

                        for (int i = 1; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value =CommonMethod.ToInt( parametervalues[i]).ToString();//本次特殊约定，为直接int型

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }
                    break;
                    
                case DataTypeDatalog.D_250:
                case DataTypeDatalog.D_260:

                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();


                            item.Value = parametervalues[i].ConvertFrom16().ToString();

                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }

                    }
                    break;
                case DataTypeDatalog.D_230:

                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == 2 && parametervalues.Length == 1)
                    {
                        item = new ParameterItem();
                        item.Value = parametervalues[0].Substring(0, 1);
                        item.ParameterName = parameternames[0];
                        readvalue.AddValue(item);

                        readvalue.AddValue(new ParameterItem { Value = parametervalues[0].Substring(1, 1), ParameterName = parameternames[1] });

                    }
                    break;
                case DataTypeDatalog.D_240:  //上限报警有效 + 突变报警有效+‘-’+上限值+‘-’+下限值‘-’+突变变化值
                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length ==5 && parametervalues.Length==4)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {

                            item = new ParameterItem();
                            switch (i)
                            {
                            
                                case 0://报警有效
                                      string alertstr=  parametervalues[0].PadLeft(4, '0');
                                      item.Value = alertstr.Substring(0, 1);
                                      item.ParameterName = parameternames[0];
                                      readvalue.AddValue(item);

                                      item = new ParameterItem();
                                      item.Value = alertstr.Substring(1, 1);
                                      item.ParameterName = parameternames[1];
                                      readvalue.AddValue(item);
                                    break;
                                case 1:
                                case 2:
                                case 3:
                                    
                                    item.Value = parametervalues[i].ConvertHexToFloat().ToString("f3");

                                    item.ParameterName = parameternames[i];
                                    readvalue.AddValue(item);
                                    break;
                                case 4:
                                    item.Value = parametervalues[i].ConvertFrom16().ToString();

                                    item.ParameterName = parameternames[i];
                                    readvalue.AddValue(item);
                                    break;

                            }
                            
                                

                        }

                    }

                    break;
                case DataTypeDatalog.D_180: //压力
     

                    item = new ParameterItem();
                    item.Value =data.DataString.ConvertHexToFloat().ToString("f8");
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);

                    break;

                case DataTypeDatalog.D_200://系统运行状态
                
                    item = new ParameterItem();
                    item.Value = data.DataString;
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);

                    break;
                case DataTypeDatalog.D_220:
   
                    item = new ParameterItem();
                    item.Value = data.DataString;
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
                case DataTypeDatalog.D_050:  //时间

                    parametervalues = data.DataString.Split('-');
                    if (parameternames.Length == parametervalues.Length)
                    {
                        for (int i = 0; i < parametervalues.Length; i++)
                        {
                            item = new ParameterItem();
                            string datestr = "20{0}-{1}-{2} {3}:{4}:{5}";
                            if (parametervalues[i].Length == 6)
                            {
                                datestr = string.Format(datestr, parametervalues[i].Substring(0, 1).ConvertFrom62().ToString().PadLeft(2, '0'), parametervalues[i].Substring(1, 1).ConvertFrom62(), parametervalues[i].Substring(2, 1).ConvertFrom62(), parametervalues[i].Substring(3, 1).ConvertFrom62(), parametervalues[i].Substring(4, 1).ConvertFrom62(), parametervalues[i].Substring(5, 1).ConvertFrom62());
                            }
                            else
                            {
                                datestr = "";
                            }
                            item.Value = datestr;
                            item.ParameterName = parameternames[i];
                            readvalue.AddValue(item);
                        }
                    }

                    break;
              
                case DataTypeDatalog.D_080: //当前温度
                    item = new ParameterItem();
                    item.Value = Convert.ToString(data.DataString.ConvertHexToFloat());
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
                case DataTypeDatalog.D_090: //回差值
                    item = new ParameterItem();
                    item.Value = data.DataString.ConvertHexToFloat().ToString();
                    item.ParameterName = parameternames[0];
                    readvalue.AddValue(item);
                    break;
                default:
                    break;
            }

            item = new ParameterItem();
            item.Value = data.TU;
            item.ParameterName = "终端编号";
            readvalue.AddValue(item);

            result.Add(readvalue);
            datatype = sDataType.QueryResponse;
            return result;

        }
    }

    public class DecodeDebugInfo : DecodeBase
    {
        protected override ArrayList TransFormData(sFrameData sData, out sDataType datatype)
        {
            ArrayList result = new ArrayList();

            
            datatype = sDataType.None;
            //switch (sData.DataTag)
            //{ 
                
            //    case DataTypeDatalog.D_340:
                    
                    
                    
            //        break;

            //}



            return result;
        }

    }

}
