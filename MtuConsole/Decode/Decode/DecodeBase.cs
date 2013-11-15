using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataEntity;
using MtuConsole.TcpProcess;
using FunctionLib;
namespace Decode
{

    /// <summary>
    /// 采集数据
    /// </summary>
    public class CollectData
    {
        /// <summary>
        /// 采集时间
        /// </summary>
        public DateTime CollectTime;

        public string Data;

    }

    public struct sFrameData
    {

        /// <summary>
        /// 设备地址
        /// </summary>
        public string TU;
        /// <summary>
        /// 帧头
        /// </summary>
        public string BeginTag;
        /// <summary>
        /// 帧尾
        /// </summary>
        public string EndTag;

        /// <summary>
        /// 数标
        /// </summary>
        public DataTypeDatalog DataTag;

  
        /// <summary>
        /// 帧长度
        /// </summary>
        public int FrameLength;

        /// <summary>
        /// 校验位
        /// </summary>
        public string BccCode;

        /// <summary>
        /// 总帧数
        /// </summary>
        public int TotalFrameCount;

        /// <summary>
        /// 当前帧数
        /// </summary>
        public int ThisFrameCount;

        /// <summary>
        /// 数据周期
        /// </summary>
        public int DataCycle;

        /// <summary>
        /// 信息类型
        /// </summary>
        public InfoType Infotype;


        /// <summary>
        /// 数据字符串
        /// </summary>
        public List<CollectData> CollectDatas;

        public string DataString;


    }


    public abstract class DecodeBase
   {
       #region field
        public string teststring;
        public DataTable _measuresetting;
        private DataTable _rtusetting;

        private InfoType _infotype;
        private RWDatabase _msrwdatabase;

       #endregion

       #region property
        public DataTable MeasureSetting
        {
            set { _measuresetting = value; }
            get { return _measuresetting; }
        }
      
      
        public RWDatabase MsgRwDatabase
        {
            set { _msrwdatabase = value; }
            get { return _msrwdatabase; }
        }
        public DataTable RtuSetting
        {
            set { _rtusetting = value; }
            get { return _rtusetting; }
        }

        public InfoType MsgInfoType
        {
            set { _infotype = value; }
            get { return _infotype; }
        }
      
       #endregion

       #region abstract method
       // public abstract ArrayList Trans2ArrayList(string sCode, out sDataType dataType, out string Rtuid);
       

        protected abstract ArrayList TransFormData(sFrameData sData, out sDataType datatype);
       #endregion
        #region protected method

      
        protected void GetScaleAndOffset(string portid, string rtuid, out decimal scale, out  decimal offset)
        {
            scale = 1m;
            offset = 0m;
            try
            {
                DataRow[]  dr = MeasureSetting.Select("PortId = '" + portid + "' and RTUID = '" + rtuid + "'");
                if (dr.Length > 0)
                {
                    scale = Convert.ToDecimal(dr[0]["Scale"]);
                    offset = Convert.ToDecimal(dr[0]["Offset"]);
                }
            }
            catch
            { }
        }
       
        #endregion

        #region public method

       

        /// <summary>
        /// 验证数据的正确性,包括校验码和帧类别
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="UseData"></param>
        /// <returns></returns>
        private bool VerifyData(string data)
        {
            bool result=false;
            if (data.Length < 5)
            { return false; }

            string valuedata = data.Substring(1, data.Length - 4);
            string bccstr = data.Substring(data.Length - 3, 2);

            if (valuedata.ConvertToRCC() == bccstr)
            {
                result = true;
            }

            
          

            return result;
        }

        public  ArrayList Trans2ArrayList(string sCode, out sDataType dataType, out string Rtuid)
        {
            //throw new System.NotImplementedException();
            ArrayList resuleArray = new ArrayList();
            dataType = sDataType.None;
            Rtuid = "";
            bool verify = false;
            verify = VerifyData(sCode);
            verify = true;  //临时调试用
                if (verify)//verfy
                {
                    sFrameData sData = TransactData(sCode);
                    Rtuid = sData.TU;
                    resuleArray = TransFormData(sData, out dataType);
                }
            return resuleArray;
        }



        /// <summary>
        /// 截取字符串，将字符串解释为各类属性
        /// </summary>
        /// <param name="framestring"></param>
        /// <returns></returns>
        public sFrameData TransactData(string framestring)
        {

            
            sFrameData result = new sFrameData();
            result.BeginTag = framestring.Substring(0, 1);
            result.EndTag = framestring.Substring(framestring.Length-1, 1);
            result.FrameLength =(int) framestring.Substring(1, 2).ConvertFrom62();
            result.TU = framestring.Substring(3, 10);
            result.Infotype = Common.ConvertToInfoType(result.BeginTag);
             result.BccCode = framestring.Substring(framestring.Length - 3, 2);
            switch (result.Infotype)
            { 
                case InfoType.Alert:
                    result.DataTag = Common.ConvertToDataType(framestring.Substring(13, 3));
                    result.CollectDatas = GetAlertDatas(result.TU, result.DataTag,framestring.Substring(16, framestring.Length - 16 - 3));
                    break;
                case InfoType.Data:
                    result.DataTag=Common.ConvertToDataType(framestring.Substring(18,3));
                    result.TotalFrameCount = (int)framestring.Substring(13, 1).ConvertFrom62();
                    result.ThisFrameCount = (int)framestring.Substring(14, 1).ConvertFrom62();
                    result.DataCycle =(int)framestring.Substring(15, 3).ConvertFrom62();
                    result.CollectDatas = GetCollectData(framestring.Substring(21, framestring.Length - 21 - 3),result.DataCycle,result.DataTag);
                    break;

                case InfoType.ParameterQuery:
                case InfoType.ParameterSet:
                case InfoType.SecretDoor:
                case InfoType.System:
                case InfoType.None:
                    result.DataTag = Common.ConvertToDataType(framestring.Substring(13, 3));
                    result.DataString = framestring.Substring(16, framestring.Length - 16 - 3);
                    break;
                default:
                    result.DataTag = DataTypeDatalog.None;
                    break;
            }
            return result;
        }

        private List<CollectData> GetAlertDatas(string rtuid,DataTypeDatalog datatag, string datastring)
        {
            List<CollectData> result = new List<CollectData>();
            switch (datatag)
            { 
                case DataTypeDatalog.D_111:
                case DataTypeDatalog.D_112:
                case DataTypeDatalog.D_113:
                case DataTypeDatalog.D_114:
                case DataTypeDatalog.D_115:
                case DataTypeDatalog.D_116:
                case DataTypeDatalog.D_117:
                case DataTypeDatalog.D_118:
                
                     int collcycle = -30;  //采集周期固定为10s
                    
                    DataRow[] drs= RtuSetting.Select("rtuid='" + rtuid + "'");
                    if (drs.Length > 0)
                    {
                        collcycle =Convert.ToInt32( drs[0]["collcycle"].ToString());
                    }


                    result = GetFixLengthDatas(datastring,collcycle,4);
                    break;
                default:
                    result = GetCollectData(datastring, 0);
                    break;

            }

            return result;
        }
        /// <summary>
        /// 分割定长数据
        /// </summary>
        /// <param name="datas">数据字符长</param>
        /// <param name="datascyle"></param>
        /// <param name="datalength"></param>
        /// <returns></returns>
        private List<CollectData> GetFixLengthDatas(string datas, int datascyle, int datalength)
        {
            List<CollectData> result = new List<CollectData>();
            string timestr=datas.Substring(0,6);
            DateTime collecttime = Common.ConvertToDatetime(timestr);
                
            datas=datas.Substring(6);
            if ((datas.Length % datalength) != 0)
            {
                return result;
            }

            for (int i = 0; i < datas.Length / datalength; i++)
            {

                CollectData item = new CollectData();
                item.CollectTime = collecttime .AddSeconds(i*datascyle);
                item.Data = datas.Substring(i * datalength, datalength);
                result.Add(item);

            }
                
            return result;
        }
        /// <summary>
        /// 从string获取采集数据集
        /// </summary>
        /// <param name="data">数据区字符串</param>
        /// <returns></returns>
        private List<CollectData> GetCollectData(string data,int datacycle)
        {
            List<CollectData> result = new List<CollectData>();

            string[] datas = data.Split('_');
            DateTime collecttime = Common.ConvertToDatetime(datas[0].Substring(0, 6));
            datas[0] = datas[0].Substring(6);
            for (int i = 0; i < datas.Length; i++)
            {
                CollectData item = new CollectData();
                item.CollectTime = collecttime;
                item.Data = datas[i];
                result.Add(item);
                collecttime = collecttime.AddSeconds(datacycle);
            }
          
            return result;
        }

        private List<CollectData> GetCollectData(string data, int datacycle, DataTypeDatalog datatype)
        {
            List<CollectData> result = new List<CollectData>();
            string[] datas;
            DateTime collecttime;
            switch (datatype)
            { 
              
                default:
                    
                    datas = data.Split('-');
                    collecttime = Common.ConvertToDatetime(datas[0].Substring(0, 6));
                    datas[0] = datas[0].Substring(6);
                    for (int i = 0; i < datas.Length; i++)
                    {
                        CollectData item = new CollectData();
                        item.CollectTime = collecttime;
                        item.Data = datas[i];
                        result.Add(item);
                        collecttime = collecttime.AddSeconds(datacycle);
                    }
                    break;

            }
            return result;
        }
        public DecodeBase()
        {

        }
        #endregion
   }
}
