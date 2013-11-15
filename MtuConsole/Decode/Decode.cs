using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataEntity;

using MtuConsole.TcpProcess;
using MtuConsole.Common;

namespace Decode
{
    public class Decode : IDecode
    {
        private MtuLog _logger;
        private DataTable _measuresetting;
        private DataTable _rtusetting;
        private RWDatabase _rwdatabase;
        private int _addday, _addsecond;

        public Decode()
        {
            _logger = new MtuLog();
            _measuresetting = null;
            _rwdatabase = null;

            // InitialTable();
        }

        public DataTable MeasureSetting
        {
            set { _measuresetting = value; }
            get { return _measuresetting; }
        }
        public DataTable RtuSetting
        {
            set { _rtusetting = value; }
            get { return _rtusetting; }
        }

        public RWDatabase RwDataBase
        {
            set { _rwdatabase = value; }
            get { return _rwdatabase; }
        }

        public void SetTimeOffSet(int addday, int addsecond)
        {
            _addday = addday;
            _addsecond = addsecond;
        }

        public ArrayList Trans2ArrayList(string sCode, out sDataType dataType, out string Rtuid)
        {
            ArrayList result = new ArrayList();
            
            InfoType infotype = Common.ConvertToInfoType(sCode.Substring(0, 1));
            dataType = sDataType.None;
            Rtuid = "";
            switch (infotype)
            {
                case InfoType.Alert:
                    DecodeAlertInfo decodealert = new DecodeAlertInfo();
                    decodealert.MeasureSetting = _measuresetting;
                    decodealert.RtuSetting = RtuSetting;
                    result = decodealert.Trans2ArrayList(sCode, out dataType, out Rtuid);

                    break;
                case InfoType.Data:
                    DecodeDataInfo decodedata = new DecodeDataInfo();
                    decodedata.MeasureSetting = _measuresetting;
                    decodedata.MsgRwDatabase = _rwdatabase;
                    decodedata.RtuSetting = RtuSetting;
                    
                    result = decodedata.Trans2ArrayList(sCode, out dataType, out Rtuid);
                    break;
                case InfoType.System:
                    DecodeSystemInfo decodedatasystem = new DecodeSystemInfo();
                     //decodedatasystem.MeasureSetting = _measuresetting;
                     //decodedatasystem.MsgRwDatabase = _rwdatabase;
                     result = decodedatasystem.Trans2ArrayList(sCode, out dataType, out Rtuid);
                    break;
                case InfoType.ParameterQuery:  //查询的回复
                    DecodeQueryResponse decodequeryresponse = new DecodeQueryResponse();
                    decodequeryresponse.MeasureSetting = _measuresetting;
                    result = decodequeryresponse.Trans2ArrayList(sCode, out dataType, out Rtuid);
                    break;
                case InfoType.SecretDoor:
                    DecodeDebugInfo debuginfo = new DecodeDebugInfo();
                    result = debuginfo.Trans2ArrayList(sCode, out dataType, out Rtuid);


                    break;
                default:

                    break;
            }

            return result;
        }

        public DataTable AirPressure
        {
            get { return null; }
            set { return; }
        }

        public void SetEnableAirPressure(bool enableairpressure)
        {
            return;
        }
    }
}
