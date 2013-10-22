//********************************************************
//创建日期：2009-11-20
//创建作者：Frank.Shi shijianping@shanghai3h.com
//功能说明: decode,encode接口
//********************************************************
//********************************************************
//修改日期：<修改日期，格式：YYYY-MM-DD>
//修改作者：<姓名，邮箱>
//修改说明:  XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
//********************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using DataEntity;

using MtuConsole.Common;

namespace MtuConsole.TcpProcess
{
   
    public enum sDataType
    {
        /*
        //============DLA==============
        //01	实时数据
        //02	上下限报警

        //03	突变报警
        //04	招测数据
        //05	历史数据


        //============DLW===========
        //01	实时数据
        //02	上下限报警

        //03	突降报警
        //04	gpsOne数据
        //05	历史数据
        //06	脉冲转向报警
        //07	终端报警
        //08	终端向主端传送参数

        //09	终端向主端传送参数

        //0B	流向突变报警
        //0C	强磁干扰报警
        //0D	终端申请对时
        //0E	水表终端版本号
        */
        /// <summary>
        /// DLA :01,05 ;
        /// </summary>
        MeasureData,
        
        /// <summary>
        /// DLA  上下限报警
        /// </summary>
        AlarmBreakLimit,
        

        /// <summary>
        /// 流量计数据
        /// </summary>
        FlowMeterData,
        /// <summary>
        /// 突降报警
        /// </summary>
        AlarmSuddenBreak,

        /// <summary>
        ///  设备报警
        /// </summary>
        AlarmEquipment,
        
        /// <summary>
        /// 查询回复
        /// </summary>
        QueryResponse,

        
        /// <summary>
        ///压力突升
        /// </summary>
        AlarmSuddenRise,
        /// <summary>
        /// 压力越上限
        /// </summary>
        AlarmBreakLimitUp,
        /// <summary>
        /// 压力越下限
        /// </summary>
        AlarmBreakLimitDown,
        /// <summary>
        /// 压力越上限恢复
        /// </summary>
        AlarmBreakLimitUpBack,
        /// <summary>
        /// 压力越下限恢复
        /// </summary>
        AlarmBreakLimitDownBack,
       
        /// <summary>
        /// 未定义类型
        /// </summary>
        None
    }

    public enum sCommandType
    {
        //数据设置
        DataSet = 0,
        //GPRS/CDMA 的TCP/IP参数设置
        ConfigSet = 1,
        //招测数据
        GetData = 2,
        //采集参数设置
        CollectSet = 3,
    }

    /// <summary>
    /// new dla 附加属性 ,非数据库属性
    /// </summary>
    public struct NewDLAAddition
    {
        public string Producttype;
        public string SoftwareVersion;
        public string SerialNum;
        public string HardwareVersion;

        public int Systemstate;
        public int LocalRemote;
        public int TestCode;
        public int ATcode;
        public string LogCode;

    }


    public struct sMeterData
    {
        public DateTime CollectDate;
        public string Data;
        public string DataID;  //数据标号
        public string Flag;  //越界标记
        public string Temperature;  //温度
        public string Signal;     //信号强度
        public string CheckFlag;  //校验 

    }
    public struct CommandParameters
    {
        public string RtuID;
        public int LineNumber;
        public DateTime StartDate;
        public DateTime EndDate;
        public string PortID;
    
        public NewDLAAddition NewdlaAddition;

    }

    /// <summary>
    /// 参数结构
    /// </summary>
    public struct ParameterItem
    {   //参数名
        public string ParameterName;
        //参数值
        public string Value;

    }
    /// <summary>
    /// 查询回复参数结构 ,DLE,NewDLA
    /// </summary>
    public class ResponseReadValue
    {
        /// <summary>
        /// 数标
        /// </summary>
        private string _dataID;
        /// <summary>
        /// 数标名
        /// </summary>
        private string _dataName;
        /// <summary>
        /// 值
        /// </summary>
        private List<ParameterItem> _values;

        public ResponseReadValue()
        {
            _values = new List<ParameterItem>();
        }
        public string DataID
        {
            set { _dataID = value; }
            get { return _dataID; }
        }

        public string DataName
        {
            set { _dataName = value; }
            get { return _dataName; }
        }

        public List<ParameterItem> Values
        {
            get { return _values; }

        }

        public void AddValue(ParameterItem item)
        {
            _values.Add(item);
        }

        public override string ToString()
        {
            string result = string.Empty;
            result = "DataID={0};DataName={1};Values:{2}";
            result = string.Format(result, DataID, DataName, ValuesString());
            return result;
        }

        private string ValuesString()
        {
            string result = string.Empty;
            foreach (ParameterItem item in Values)
            {
                result = result + "[" + item.ParameterName + "]=" + item.Value + ";";
            }

            return result;
        }
    }



    public interface IDecode
    {
        DataTable MeasureSetting { get; set; }
        DataTable AirPressure { get; set; }
        DataTable RtuSetting { get; set; }
        RWDatabase RwDataBase { get; set; }
        void SetTimeOffSet(int addday, int addsecond);
        void SetEnableAirPressure(bool enableairpressure);

        ArrayList Trans2ArrayList(string sCode, out sDataType dataType, out string Rtuid);


    }
    public interface IEncode
    {


        void SetRtuSetting(RTUSetting inputsetting);
        void SetCommunicationSetting(CommunicationSetting inputsetting);
        void SetCommunicationSetting(CommunicationSetting inputsetting1, CommunicationSetting inputsetting2);
        void SetMeasureSetting(DataTable intputsetting);
        void SetTimeOffSet(int addday, int addsecond);
        string EncodeData(RTUCommandType CommandType, CommandParameters cmdparameters);
        List<string> EncodeDataList(RTUCommandType commandtype, CommandParameters cmdparameters);
        void SetEnableAirPressure(bool enableairpressure);
        DataTable AirPressure { get; set; }
    }




    public class FactoryDecodeEncode
    {
        public IDecode CreateDecode(string sDllName, string sObjectName)
        {


            return (IDecode)CommonMethod.CreatePkgObject(sDllName, sObjectName);

        }

        public IEncode CreateEncode(string sDllName, string sObjectName)
        {
            return (IEncode)CommonMethod.CreatePkgObject(sDllName, sObjectName);
        }

        public IResponseMessage CreateResponseMessage(string sDllName, string sObjectName)
        {
            return (IResponseMessage)CommonMethod.CreatePkgObject(sDllName, sObjectName);

        }


    }

}
