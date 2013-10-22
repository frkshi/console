using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using DataEntity;

using MtuConsole.Common;
using MtuConsole.TcpProcess;
using FunctionLib;

namespace Decode
{
    public class Encode : IEncode
    {
        #region field
        private RTUSetting _rtusetting;
        /// <summary>
        /// 通道1
        /// </summary>
        private CommunicationSetting _communicationsetting1;  
        /// <summary>
        /// 通道2
        /// </summary>
        private CommunicationSetting _communicationsetting2;
        /// <summary>
        /// 后门通道
        /// </summary>
        private CommunicationSetting _communicationsettingSecret;
        private DataTable _measuresetting;
        private int _addday, _addsecond;
        private MtuLog _logger;
        private DataTable _airpressure;
        private const decimal STANDARD_AIRPRESSURE = 0.101325m;
        
        #endregion

        #region public method

        public Encode()
        {
            _logger = new MtuLog();
        }

        public void SetRtuSetting(RTUSetting rs)
        {
            _rtusetting = rs;

        }
        
        public void SetCommunicationSetting(CommunicationSetting inputsetting)
        {
            _communicationsetting1 = inputsetting;
        }
        public void SetCommunicationSetting(CommunicationSetting inputsetting1,CommunicationSetting inputsetting2)
        {
            _communicationsetting1 = inputsetting1;
            _communicationsetting2 = inputsetting2;
        }
        public void SetMeasureSetting(DataTable inputsetting)
        {
            _measuresetting = inputsetting;
        }

        public void SetTimeOffSet(int addday, int addsecond)
        {
            _addday = addday;
            _addsecond = addsecond;
        }

        /// <summary>
        /// 通道1
        /// </summary>
        public CommunicationSetting Communicationsetting1
        {
            get
            {
                return _communicationsetting1;
            }
            set
            {
                _communicationsetting1 = value;
            }
        }

        /// <summary>
        /// 通道2
        /// </summary>
        public CommunicationSetting Communicationsetting2
        {
            get
            {
                return _communicationsetting2;
            }
            set
            {
                _communicationsetting2 = value;
            }
        }
        /// <summary>
        /// 后门通道
        /// </summary>
        public CommunicationSetting CommunicationsettingSecret
        {
            get
            {
                return _communicationsettingSecret;
            }
            set
            {
                _communicationsettingSecret = value;
            }

        }


        public List<string> EncodeDataList(RTUCommandType commandtype, CommandParameters cmdparameters)
        {
            List<string> result = new List<string>();
            List<string> dataidlist = GetDataIDbyCommandType(commandtype);
            foreach (string dataid in dataidlist)
            {
                string framebody = "";
                try
                {
                    framebody = cmdparameters.RtuID.PadLeft(10,'0') + dataid.PadLeft(3,'0') +
                        GetDatabyID(commandtype, dataid.PadLeft(3,'0'), cmdparameters.NewdlaAddition);
               
                    framebody = framebody.Length.ConvertTo62().PadLeft(2, '0') + framebody;
                    result.Add(ConvertToHeadChar(commandtype, dataid.PadLeft(3, '0')) + framebody + framebody.ConvertToRCC() + "#");
                }
                catch(Exception e)
                {
                    _logger.Error(e.Message, e);
                    result.Add(string.Empty);
                }
            }

            return result;
        }

        public string EncodeData(RTUCommandType commandtype, CommandParameters cmdparameters)
        {

            string result = "";
            string framebody = "";
            try
            {
                framebody = cmdparameters.RtuID.PadLeft(10, '0') + cmdparameters.PortID.PadLeft(3,'0') +
                    GetDatabyID(commandtype, cmdparameters.PortID.PadLeft(3,'0'), cmdparameters.NewdlaAddition);
                framebody = framebody.Length.ConvertTo62().PadLeft(2, '0') + framebody;
                result = ConvertToHeadChar(commandtype, cmdparameters.PortID.PadLeft(3, '0')) + framebody + framebody.ConvertToRCC() + "#";
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
                result = string.Empty;
            }
            

            return result;
        }
        
        #endregion

        #region private method

        private string ConvertToHeadChar(RTUCommandType commandtype,string dataid)
        {
            string result = "&";
            switch (commandtype)
            {
                case RTUCommandType.QueryData:
                    result = "?";
                    break;
                default:
                    if (dataid == "330" || dataid == "340"||dataid=="360" || dataid=="350")
                    {
                        result = "~";
                    }
                    else
                    {
                        result = "&";
                    }
                    break;
            }

            return result;
        }

        private string GetDatabyID(RTUCommandType commandtype, string dataid, NewDLAAddition dlaaddition)
        {

            string result = "";
            DataRow[] measurerows;

            if (commandtype == RTUCommandType.QueryData)
            {
                return result;
            }

            switch (dataid)
            {
                case "000":  //系统参数
                    result = "{0}-{1}-{2}";
                    result = string.Format(result, dlaaddition.Producttype, dlaaddition.SoftwareVersion, dlaaddition.HardwareVersion);
                    break;
                case "010": //GPRS信息  APN+‘-’+IP1+‘-’+PORT1 +‘-’+IP2+‘-’+PORT2
                    result = "{0}-{1}-{2}-{3}-{4}";
                    result = string.Format(result, _communicationsetting1.APN, _communicationsetting1.IP, _communicationsetting1.ExTcp,_communicationsetting2.IP,_communicationsetting2.ExTcp);
                    break;
                case "020"://后门参数
                    result = "{0}-{1}";
                    if (_communicationsettingSecret != null)
                    {
                        result = string.Format(result, _communicationsettingSecret.IP, _communicationsettingSecret.ExTcp);
                    }
                    else
                    {
                        result = "-";
                    }
                    break;
                case "030"://周期设定
                    result = "{0}-{1}";  //保存周期1+ '-' + 发送周期1 

                    if (_rtusetting.SendCycle == 0 || _rtusetting.SaveCycle == 0 )
                    {
                        throw new Exception("invalid cycle");
                    }
                    result = string.Format(result, _rtusetting.SaveCycle.ConvertTo62().PadLeft(3, '0'), _rtusetting.SendCycle.ConvertTo62().PadLeft(3, '0'));

                    break;
                case "040"://保存周期
                    result = "{0}";

                    result = string.Format(result, _rtusetting.SaveCycle2.ConvertTo62().PadLeft(3,'0'));
                    break;
                case "050":// 系统时间 YMDhms
                    DateTime tmpdt = DateTime.Now;
                    int iYear = Convert.ToInt32(tmpdt.Year.ToString().Substring(2, 2));


                    result = iYear.ConvertTo62() + tmpdt.Month.ConvertTo62() + tmpdt.Day.ConvertTo62() + tmpdt.Hour.ConvertTo62() + tmpdt.Minute.ConvertTo62() + tmpdt.Second.ConvertTo62();

                    break;
                case "060"://清除电池数据
                    result = "";
                    break;
                case "070"://系统复位
                    result = "";
                    break;
                case "080"://温度                 
                    result = "";
                    break;
                case "090":   //回差
                    result = _rtusetting.BackValue.ConvertFloatToHex();
                    break;              
                case "200"://系统运行状态
                    result = dlaaddition.Systemstate.ToString();
                    break;
                case "210"://本地远程
                    result = dlaaddition.LocalRemote.ToString();
                    break;
                case "220"://硬件调试
                    result = dlaaddition.TestCode.ToString();
                    break;
                case "330"://AT上发
                    result = dlaaddition.ATcode.ToString();
                    break;
                case "340"://获得日志
                    result = dlaaddition.LogCode;
                    break;
                case "350"://上传一次
                    result = "1";
                    break;
        
                case "230":
                    measurerows = _measuresetting.Select("rtuid='" + _rtusetting.RTUId + "' and  datatype='01'");
                   string pressureisopen="0";
                    if (measurerows != null)
                    {
                        pressureisopen = measurerows[0]["isopen"].ToString() == "1" ? "1" : "0";
                    }
                    measurerows = _measuresetting.Select("rtuid='" + _rtusetting.RTUId + "' and  datatype='06'");
                    string waterflowisopen = "0";
                    if (measurerows != null)
                    {
                        waterflowisopen = measurerows[0]["isopen"].ToString() == "1" ? "1" : "0";
                    }
                    result = pressureisopen + waterflowisopen;

                    break;
                case "240"://报警有效+‘-’+上限值+‘-’+下限值‘-’+突变变化值

                    result = "{0}-{1}-{2}-{3}";
                    measurerows = _measuresetting.Select("rtuid='" + _rtusetting.RTUId + "' and  datatype='01'");
                    if (measurerows != null)
                    {
                        string alertstr=GetAlertStr(measurerows[0]["sendoutdata"].ToString(),measurerows[0]["sendchangedata"].ToString()) ;
                        decimal upperlimit = CommonMethod.IsNumeric(measurerows[0]["upperlimit"].ToString()) ? Convert.ToDecimal(measurerows[0]["upperlimit"].ToString()) : 0;
                        decimal lowerlimit = CommonMethod.IsNumeric(measurerows[0]["lowerlimit"].ToString()) ? Convert.ToDecimal(measurerows[0]["lowerlimit"].ToString()) : 0;
                        decimal increaseratio = CommonMethod.IsNumeric(measurerows[0]["ratio"].ToString()) ? Convert.ToDecimal(measurerows[0]["Ratio"].ToString()) : 0;
                      //  int increaseinterval= CommonMethod.IsNumeric(measurerows[0]["increaseinterval"].ToString())?Convert.ToInt32(measurerows[0]["increaseinterval"].ToString()):0;
                    
                        result = string.Format(result, alertstr, upperlimit.ConvertFloatToHex().PadLeft(8, '0'), lowerlimit.ConvertFloatToHex().PadLeft(8, '0'), increaseratio.ConvertFloatToHex().PadLeft(8, '0'));
                           
                    }
                  break;
                case "0a0": //大气压力偏移值
                  measurerows = _measuresetting.Select("datatype='01'");
                  decimal elevation = 0.0m;
                  if (measurerows != null)
                  {
                      foreach (DataRow row in measurerows)
                      {
                          if (row["rtuid"].ToString() == _rtusetting.RTUId || row["rtuid"].ToString()=="0000000000")
                          {
                              elevation = Convert.ToDecimal(row["elevation"].ToString());
                              break;
                          }
                      }

                  }
                  //1米水压=0.01mpa
                  
                  result = (GetAirPressure()-elevation*0.01m).ConvertFloatToHex();
                  break;
                case "0b0"://压力比例系数
                  measurerows = _measuresetting.Select("rtuid='" + _rtusetting.RTUId + "' and  datatype='01'");
                  if (measurerows != null)
                  {
                      result = CommonMethod.ConvertToDecimal(measurerows[0]["scale"].ToString(),8).ConvertFloatToHex();
                  }
                  break;
             

            }

            return result;
        }
        private string GetAlertStr(string outalert, string changealert)
        {
            string result ="{0}{1}00";
            //‘0000’：报警全无效
            //‘1000’：上下限有效
            //‘0100’：突变报警有效
            //‘1100’：报警全有效   
            result = string.Format(result, outalert == "0" ? "0" : "1", changealert == "0" ? "0" : "1");

            return result;
        }
        /// <summary>
        /// 以命令类型对应标号
        /// </summary>
        /// <param name="commandtype"></param>
        /// <returns></returns>
        private List<string> GetDataIDbyCommandType(RTUCommandType commandtype)
        {
            List<string> result = new List<string>();

            switch (commandtype)
            {

                case RTUCommandType.CommunicationConfig:
                    result.Add("010");
                    result.Add("020");
                    break;
                case RTUCommandType.GenernalConfig:
                    result.Add("030");
                    //result.Add("040");
                    result.Add("240");
                    result.Add("090");
                    result.Add("0b0");
                   
                    break;
                case RTUCommandType.SystemTimeSetting:
                    result.Add("050");
                    break;
                case RTUCommandType.HistoryDataRecovery:
                    break;
                case RTUCommandType.QueryData:
                    result.Add("010");
                    result.Add("020");
                    result.Add("030");
                    //result.Add("040");
                    result.Add("050");
                    result.Add("000");
                    result.Add("240");
                    result.Add("090");
                    result.Add("0a0");
                    break;
                case RTUCommandType.AirPressureSetting:
                    result.Add("0a0");
                    break;
                case RTUCommandType.RestGPRSSetting:
                    result.Add("060");
                    break;
                case RTUCommandType.GPRSSetting:
                    result.Add("010");
                    break;
                case RTUCommandType.CycleSetting:
                    result.Add("030");
                    break;
                case RTUCommandType.FlowSaveCycle:
                    result.Add("040");
                    break;
                case RTUCommandType.PressureAlertSetting:
                    result.Add("240");
                    break;
                case RTUCommandType.BackValueSetting:
                    result.Add("090");
                    break;
                case RTUCommandType.PressureScaleSetting:
                    result.Add("0b0");
                    break;
                
                default:
                    break;
            }

            return result;

        }
        /// <summary>
        /// 返回当前大气压
        /// </summary>
        /// <returns></returns>
        private decimal GetAirPressure()
        {
            decimal result = STANDARD_AIRPRESSURE;
            try
            {
                DataTable tmpdt = AirPressure.Copy();

                DataRow[] resultRow = tmpdt.Select("1>0", "collecttime");
                
                if (resultRow.Length > 0)
                {
                    result = Convert.ToDecimal(resultRow[0]["AirPressure"].ToString());
                }
                tmpdt.Dispose();
            }
            catch(Exception e)
            {
                _logger.Error(e.Message, e);
            }

            return result;
        }
        #endregion

        // private string Encode

        public void SetEnableAirPressure(bool enableairpressure)
        {
        }

        public DataTable AirPressure
        {
            get
            {
                return _airpressure;
            }
            set
            {
                _airpressure = value;
            }
        }
    }
}
