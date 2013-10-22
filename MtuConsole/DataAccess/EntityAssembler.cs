using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using System.Data;

namespace DataAccess
{
    public static class EntityAssembler
    {
        #region Create Setting Entity

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>通讯配置实体</returns>
        public static CommunicationSetting CreateCommSettingEntity(IDataReader dr)
        {
            CommunicationSetting entity = new CommunicationSetting();
            if (!dr.IsDBNull(0))
            {
                entity.CommunicationId = dr.GetInt32(0);
            }
            entity.CommunicationName = dr[1] as string;
            entity.MobileNo = dr[2] as string;
            entity.Note = dr[3] as string;
            entity.APN = dr[4] as string;
            entity.Dns1 = dr[5] as string;
            entity.Dns2 = dr[6] as string;
            entity.IP = dr[7] as string;
            entity.ExTcp = dr[8] as string;
            entity.InTcp = dr[9] as string;
            entity.CommunicationTypeId = dr[10] as string;
            if (!dr.IsDBNull(11))
            {
                entity.Tag = dr.GetByte(11);
            }
            if (!dr.IsDBNull(12))
            {
                entity.InsertTime = dr.GetDateTime(12);
            }
            if (!dr.IsDBNull(13))
            {
                entity.UpdateTime = dr.GetDateTime(13);
            }
            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>终端配置实体</returns>
        public static RTUSetting CreateRTUSettingEntity(IDataReader dr)
        {

            RTUSetting entity = new RTUSetting();
            entity.RTUId = dr[0] as string;
            entity.RTUName = dr[1] as string;
            entity.RTUSName = dr[2] as string;
            entity.ProductTypeId = dr[3] as string;
            entity.Manu = dr[4] as string;
            if (!dr.IsDBNull(5))
            {
                entity.InstallDate = dr.GetDateTime(5);
            }
            entity.InstallLoca = dr[6] as string;
            entity.InstallAddr = dr[7] as string;
            if (!dr.IsDBNull(8))
            {
                entity.Analog = dr.GetInt32(8);
            }
            if (!dr.IsDBNull(9))
            {
                entity.Digit = dr.GetInt32(9);
            }
            if (!dr.IsDBNull(10))
            {
                entity.VoltAlert = dr.GetDecimal(10);
            }
            if (!dr.IsDBNull(11))
            {
                entity.VoltClose = dr.GetDecimal(11);
            }
            if (!dr.IsDBNull(12))
            {
                entity.CollCycle = dr.GetInt32(12);
            }
            if (!dr.IsDBNull(13))
            {
                entity.SendCycle = dr.GetInt32(13);
            }
            if (!dr.IsDBNull(14))
            {
                entity.SaveCycle = dr.GetInt32(14);
            }
            entity.Note = dr[15] as string;
            entity.MobileNo = dr[16] as string;
            if (!dr.IsDBNull(17))
            {
                entity.CommunicationId = dr.GetInt32(17);
            }
            if (!dr.IsDBNull(18))
            {
                entity.CommunicationId1 = dr.GetInt32(18);
            }
            if (!dr.IsDBNull(19))
            {
                entity.CommunicationId2 = dr.GetInt32(19);
            }
            if (!dr.IsDBNull(20))
            {
                entity.IsOpen = dr.GetBoolean(20);
            }
            entity.OpenTime1 = dr[21] as string;
            entity.OpenTime2 = dr[22] as string;
            entity.OpenTime3 = dr[23] as string;
            entity.OpenTime4 = dr[24] as string;
            entity.OpenTime5 = dr[25] as string;
            entity.ServiceCenter = dr[26] as string;
            entity.TableName = dr[27] as string;
            if (!dr.IsDBNull(28))
            {
                entity.RegionId = dr.GetInt32(28);
            }
            if (!dr.IsDBNull(29))
            {
                entity.PowerSupply = dr.GetInt32(29);
            }
            if (!dr.IsDBNull(30))
            {
                entity.Tag = dr.GetByte(30);
            }
            entity.CommunicationTypeId = dr[31] as string;
            entity.Caliber = dr[32] as string;
            if (!dr.IsDBNull(33))
            {
                entity.CaliberType = dr.GetInt32(33);
            }
            if (!dr.IsDBNull(34))
            {
                entity.SendExceptionAlert = dr.GetBoolean(34);
            }
            if (!dr.IsDBNull(35))
            {
                entity.SendVoltAlert = dr.GetBoolean(35);
            }

            //[CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],
            //[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber]

            if (!dr.IsDBNull(36))
            {
                entity.CollCycle2 = dr.GetInt32(36);
            }

            if (!dr.IsDBNull(37))
            {
                entity.SendCycle2 = dr.GetInt32(37);
            }

            if (!dr.IsDBNull(38))
            {
                entity.SaveCycle2 = dr.GetInt32(38);
            }

            if (!dr.IsDBNull(39))
            {
                entity.CollCycle3 = dr.GetInt32(39);
            }

            if (!dr.IsDBNull(40))
            {
                entity.SendCycle3 = dr.GetInt32(40);
            }

            if (!dr.IsDBNull(41))
            {
                entity.SaveCycle3 = dr.GetInt32(41);
            }

            if (!dr.IsDBNull(42))
            {
                entity.SafetyLevel = dr.GetDecimal(42);
            }

            if (!dr.IsDBNull(43))
            {
                entity.AlertLevel = dr.GetDecimal(43);
            }

            if (!dr.IsDBNull(44))
            {
                entity.SystemVoltage = dr.GetDecimal(44);
            }

            if (!dr.IsDBNull(45))
            {
                entity.UltrasonicVoltage = dr.GetDecimal(45);
            }

            if (!dr.IsDBNull(46))
            {
                entity.LocationDevication = dr.GetDecimal(46);
            }

            if (!dr.IsDBNull(47))
            {
                entity.WellDeep = dr.GetDecimal(47);
            }

            if (!dr.IsDBNull(48))
            {
                entity.WellElevation = dr.GetDecimal(48);
            }

            if (!dr.IsDBNull(49))
            {
                entity.PipeElevation = dr.GetDecimal(49);
            }

            if (!dr.IsDBNull(50))
            {
                entity.PipeCaliber = dr.GetDecimal(50);
            }

            if (!dr.IsDBNull(51))
            {
                entity.BackValue = dr.GetDecimal(51);
            }

            if (!dr.IsDBNull(54))
            {
                entity.InsertTime = dr.GetDateTime(54);
            }
            if (!dr.IsDBNull(55))
            {
                entity.UpdateTime = dr.GetDateTime(55);
            }

            entity.DeviceStatus = Convert.ToInt32(dr[52]);
            entity.CurrentCycle = Convert.ToInt32(dr[53]);
            
            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>终端配置，终端版实体</returns>
        public static Rtusetting_Device CreateRtuSettingDeviceEntity(IDataReader dr)
        {

    //        [RtuID] [varchar](50) NULL,
    //[ProductType] [nchar](50) NULL,
    //[SoftwareVersion] [nchar](50) NULL,
    //[HardwareVersion] [nchar](50) NULL,
    //[APN] [nchar](10) NULL,
    //[IP1] [nchar](15) NULL,
    //[IP2] [nchar](15) NULL,
    //[Port1] [nchar](10) NULL,
    //[Port2] [nchar](10) NULL,
    //[SecretIP] [nchar](15) NULL,
    //[SecretPort] [nchar](10) NULL,
    //[Savecycle1] [int] NOT NULL,
    //[Sendcycle1] [int] NOT NULL,
    //[SaveCycle2] [int] NOT NULL,
    //[Sendcycle2] [int] NOT NULL,
    //[BackValue] [numeric](18, 0) NOT NULL,
    //[AirPressure] [numeric](18, 0) NOT NULL,
    //[Scale] [decimal](18, 0) NOT NULL

            Rtusetting_Device entity = new Rtusetting_Device();
            entity.RTUId = dr[0] as string;
            if(!dr.IsDBNull(1))
            {
              entity.ProductType=dr[1] as string;
            }
            if (!dr.IsDBNull(2))
            {
                entity.SoftwareVersion = dr[2] as string;
            }
            if (!dr.IsDBNull(3))
            {
                entity.HardwareVersion = dr[3] as string;
            }
            if (!dr.IsDBNull(4))
            {
                entity.APN = dr[4] as string;
            }
            if (!dr.IsDBNull(5))
            {
                entity.IP1 = dr[5] as string;
            }
            if (!dr.IsDBNull(6))
            {
                entity.IP2 = dr[6] as string;
            }
            if (!dr.IsDBNull(7))
            {
                entity.Port1 = dr[7] as string;
            }
            if (!dr.IsDBNull(8))
            {
                entity.Port2 = dr[8] as string;
            }
            if (!dr.IsDBNull(9))
            {
                entity.SecretIP = dr[9] as string;
            }
            if (!dr.IsDBNull(10))
            {
                entity.SecretPort = dr[10] as string;
            }
            if (!dr.IsDBNull(11))
            {
                entity.Savecycle1 = dr.GetInt32(11);
            }
            if (!dr.IsDBNull(12))
            {
                entity.Sendcycle1 = dr.GetInt32(12);
            }
            if (!dr.IsDBNull(13))
            {
                entity.SaveCycle2 = dr.GetInt32(13);
            }
            if (!dr.IsDBNull(14))
            {
                entity.Sendcycle2 = dr.GetInt32(14);
            }

            if (!dr.IsDBNull(15))
            {
                entity.BackValue = dr.GetDecimal(15);
            }

            if (!dr.IsDBNull(16))
            {
                entity.AirPressure = dr.GetDecimal(16);
            }
            if (!dr.IsDBNull(17))
            {
                entity.Scale = dr.GetDecimal(17);
            }

            return entity;
        
        }
        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>检测量配置实体</returns>
        public static MeasureSetting CreateMeasureSettingEntity(IDataReader dr)
        {
            MeasureSetting entity = new MeasureSetting();
            if (!dr.IsDBNull(0))
            {
                entity.MeasureId = dr.GetInt32(0);
            }
            entity.MeasureName = dr[1] as string;
            entity.RTUId = dr[2] as string;
            entity.SignalType = dr[3] as string;
            entity.DataType = dr[4] as string;
            entity.PortId = dr[5] as string;
            entity.Unit = dr[6] as string;
            if (!dr.IsDBNull(7))
            {
                entity.Range = dr.GetDecimal(7);
            }
            if (!dr.IsDBNull(8))
            {
                entity.Elevation = dr.GetDecimal(8);
            }
            if (!dr.IsDBNull(9))
            {
                entity.Precision = dr.GetDecimal(9);
            }
            if (!dr.IsDBNull(10))
            {
                entity.DecimalDigits = dr.GetDecimal(10);
            }
            if (!dr.IsDBNull(11))
            {
                entity.Scale = dr.GetDecimal(11);
            }
            if (!dr.IsDBNull(12))
            {
                entity.Offset = dr.GetDecimal(12);
            }
            if (!dr.IsDBNull(13))
            {
                entity.Revise = dr.GetDecimal(13);
            }
            if (!dr.IsDBNull(14))
            {
                entity.UpperLimit = dr.GetDecimal(14);
            }
            if (!dr.IsDBNull(15))
            {
                entity.LowerLimit = dr.GetDecimal(15);
            }
            if (!dr.IsDBNull(16))
            {
                entity.Ratio = dr.GetDecimal(16);
            }
            entity.Note = dr[17] as string;
            if (!dr.IsDBNull(18))
            {
                entity.Type = dr.GetInt32(18);
            }
            if (!dr.IsDBNull(19))
            {
                entity.IsOpen = dr.GetBoolean(19);
            }
            if (!dr.IsDBNull(20))
            {
                entity.IsSet = dr.GetBoolean(20);
            }
            if (!dr.IsDBNull(21))
            {
                entity.SendOutData = dr.GetBoolean(21);
            }
            if (!dr.IsDBNull(22))
            {
                entity.SendChangeData = dr.GetBoolean(22);
            }
            if (!dr.IsDBNull(23))
            {
                entity.DataStatus = dr.GetBoolean(23);
            }
            if (!dr.IsDBNull(24))
            {
                entity.SwitchType = dr.GetBoolean(24);
            }
            if (!dr.IsDBNull(25))
            {
                entity.SendRedirectionAlert = dr.GetBoolean(25);
            }
            if (!dr.IsDBNull(26))
            {
                entity.IncreaseInterval = dr.GetInt32(26);
            }
         
            if (!dr.IsDBNull(27))
            {
                entity.InsertTime = dr.GetDateTime(27);
            }
            if (!dr.IsDBNull(28))
            {
                entity.UpdateTime = dr.GetDateTime(28);
            }
            return entity;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>检测量模板实体</returns>
        public static MeasureTemplet CreateMeasureTempletEntity(IDataReader dr)
        {
            MeasureTemplet entity = new MeasureTemplet();
            if (!dr.IsDBNull(0))
            {
                entity.MTempletId = dr.GetInt32(0);
            }
            entity.MTempletName = dr[1] as string;
            entity.ProductTypeId = dr[2] as string;
            entity.Manu = dr[3] as string;
            entity.SignalType = dr[4] as string;
            entity.Unit = dr[5] as string;
            if (!dr.IsDBNull(6))
            {
                entity.Range = dr.GetDecimal(6);
            }
            if (!dr.IsDBNull(7))
            {
                entity.Precision = dr.GetDecimal(7);
            }
            if (!dr.IsDBNull(8))
            {
                entity.DecimalDigits = dr.GetDecimal(8);
            }
            if (!dr.IsDBNull(9))
            {
                entity.Scale = dr.GetDecimal(9);
            }
            if (!dr.IsDBNull(10))
            {
                entity.Offset = dr.GetDecimal(10);
            }
            if (!dr.IsDBNull(11))
            {
                entity.UpperLimit = dr.GetDecimal(11);
            }
            if (!dr.IsDBNull(12))
            {
                entity.LowerLimit = dr.GetDecimal(12);
            }
            if (!dr.IsDBNull(13))
            {
                entity.Ratio = dr.GetDecimal(13);
            }
            entity.Note = dr[14] as string;
            entity.DataType = dr[15] as string;
            entity.PortId = dr[16] as string;
            if (!dr.IsDBNull(17))
            {
                entity.IsSet = dr.GetBoolean(17);
            }
            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>终端模板实体</returns>
        public static RTUTemplet CreateRtuTempletEntity(IDataReader dr)
        {
            RTUTemplet entity = new RTUTemplet();
            entity.ProductTypeId = dr[0] as string;
            entity.Manu = dr[1] as string;
            if (!dr.IsDBNull(2))
            {
                entity.Analog = dr.GetInt32(2);
            }
            if (!dr.IsDBNull(3))
            {
                entity.Digit = dr.GetInt32(3);
            }
            if (!dr.IsDBNull(4))
            {
                entity.VoltAlert = dr.GetDecimal(4);
            }
            if (!dr.IsDBNull(5))
            {
                entity.VoltClose = dr.GetDecimal(5);
            }
            if (!dr.IsDBNull(6))
            {
                entity.CollCycle = dr.GetInt32(6);
            }
            if (!dr.IsDBNull(7))
            {
                entity.SendCycle = dr.GetInt32(7);
            }
            if (!dr.IsDBNull(8))
            {
                entity.SaveCycle = dr.GetInt32(8);
            }
            entity.Note = dr[9] as string;
            entity.ServiceCenter = dr[10] as string;
            entity.CommunicationTypeId = dr[11] as string;
            if (!dr.IsDBNull(12))
            {
                entity.Tag = dr.GetByte(12);
            }
            entity.Caliber = dr[13] as string;
            if (!dr.IsDBNull(14))
            {
                entity.CaliberType = dr.GetInt32(14);
            }
            // CollCycle2 , SendCycle2 , SaveCycle2  ,CollCycle3 , SendCycle3 , SaveCycle3,  
            if (!dr.IsDBNull(15))
            {
                entity.CollCycle2 = dr.GetInt32(15);
            }

            if (!dr.IsDBNull(16))
            {
                entity.SendCycle2 = dr.GetInt32(16);
            }


            if (!dr.IsDBNull(17))
            {
                entity.SaveCycle2 = dr.GetInt32(17);
            }
            if (!dr.IsDBNull(18))
            {
                entity.CollCycle3 = dr.GetInt32(18);
            }
            if (!dr.IsDBNull(19))
            {
                entity.SendCycle3 = dr.GetInt32(19);
            }
            if (!dr.IsDBNull(20))
            {
                entity.SaveCycle3 = dr.GetInt32(20);
            }
            //SafetyLevel,AlertLevel,SystemVoltage,UltrasonicVoltage ,LocationDevication,WellDeep,WellElevation,PipeElevation,PipeCaliber
            if (!dr.IsDBNull(21))
            {
                entity.SafetyLevel = dr.GetDecimal(21);
            }
            if (!dr.IsDBNull(22))
            {
                entity.AlertLevel = dr.GetDecimal(22);
            }
            if (!dr.IsDBNull(23))
            {
                entity.SystemVoltage = dr.GetDecimal(23);
            }
            if (!dr.IsDBNull(24))
            {
                entity.UltrasonicVoltage = dr.GetDecimal(24);
            }
            if (!dr.IsDBNull(25))
            {
                entity.LocationDevication = dr.GetDecimal(25);
            }
            if (!dr.IsDBNull(26))
            {
                entity.WellDeep = dr.GetDecimal(26);
            }
            if (!dr.IsDBNull(27))
            {
                entity.WellElevation = dr.GetDecimal(27);
            }


            if (!dr.IsDBNull(28))
            {
                entity.PipeElevation = dr.GetDecimal(28);
            }
            if (!dr.IsDBNull(29))
            {
                entity.PipeCaliber = dr.GetDecimal(29);
            }
            if (!dr.IsDBNull(30))
            {
                entity.BackValue = dr.GetDecimal(30);
            }
            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>检测量实体</returns>
        public static MeasureData CreateMeasureDataEntity(IDataReader dr)
        {
            MeasureData entity = new MeasureData();
            if (!dr.IsDBNull(0))
            {
                entity.MeasureId = dr.GetInt32(0);
            }
            if (!dr.IsDBNull(1))
            {
                entity.CollDatetime = dr.GetDateTime(1);
            }
            if (!dr.IsDBNull(2))
            {
                entity.CollNum = dr.GetDecimal(2);
            }
            if (!dr.IsDBNull(3))
            {
                entity.Tag = dr.GetInt32(3);
            }
            if (!dr.IsDBNull(4))
            {
                entity.Sign = dr.GetByte(4);
            }
            if (dr.FieldCount == 6)
            {
                entity.RTUId = dr[5] as string;
            }
            return entity;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>报警数据实体</returns>
        public static AlertData CreateAlertDataEntity(IDataReader dr)
        {
            AlertData entity = new AlertData();
            if (!dr.IsDBNull(0))
            {
                entity.MeasureId = dr.GetInt32(0);
            }
            if (!dr.IsDBNull(1))
            {
                entity.CollStartTime = dr.GetDateTime(1);
            }
            if (!dr.IsDBNull(2))
            {
                entity.StartNum = dr.GetDecimal(2);
            }
            if (!dr.IsDBNull(3))
            {
                entity.CollEndTime = dr.GetDateTime(3);
            }
            if (!dr.IsDBNull(4))
            {
                entity.EndNum = dr.GetDecimal(4);
            }
            if (!dr.IsDBNull(5))
            {
                entity.Ratio = dr.GetDecimal(5);
            }
            if (!dr.IsDBNull(6))
            {
                entity.AlertTypeId = dr.GetInt32(6);
            }
            if (!dr.IsDBNull(7))
            {
                entity.Tag = dr.GetByte(7);
            }
            if (!dr.IsDBNull(8))
            {
                entity.Sign = dr.GetInt32(8);
            }
            entity.RTUId = dr[9] as string;
            return entity;
        }


        public static AlertDataDetail CreateAlertDataDetailEntity(IDataReader dr)
        {
            //"MeasureId,RtuId,CollDateTimes,CollNums,AlertTypeID"
            AlertDataDetail entity = new AlertDataDetail();
            if (!dr.IsDBNull(0))
            {
                entity.MeasureId = dr.GetInt32(0);
            }
            if (!dr.IsDBNull(1))
            {
                entity.RTUId = dr[1] as string;
            }
            if (!dr.IsDBNull(2))
            {
                entity.CollTimes = dr[2] as string;
            }
            if (!dr.IsDBNull(3))
            {
                entity.CollNums = dr[3] as string;
            }
            if (!dr.IsDBNull(4))
            {
                entity.AlertTypeId = dr.GetInt32(4);
                    
            }



            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>通讯协议类型实体</returns>
        public static CommunicationType CreateCommunicationTypeEntity(IDataReader dr)
        {
            CommunicationType entity = new CommunicationType();
            entity.CommunicationTypeId = dr[0] as string;
            entity.CTName = dr[1] as string;
            return entity;
        }

        /// <summary>
        /// 构造实体
        /// </summary>
        /// <param name="dr">DataReader</param>
        /// <returns>报警类型实体</returns>
        public static AlertType CreateAlertTypeEntity(IDataReader dr)
        {
            AlertType entity = new AlertType();
            if (!dr.IsDBNull(0))
            {
                entity.AlertTypeId = dr.GetInt32(0);
            }
            entity.AlertName = dr[1] as string;
            if (!dr.IsDBNull(2))
            {
                entity.ValidTime = dr.GetInt32(2);
            }
            entity.Remark = dr[3] as string;
            entity.StaticImageUrl = dr[4] as string;
            entity.DynamicImageUrl = dr[5] as string;
            if (!dr.IsDBNull(6))
            {
                entity.GroupType = dr.GetInt32(6);
            }
            return entity;
        }

        public static DataTypeConfig CreateDataTypeEntity(IDataReader dr)
        {
            DataTypeConfig entity = new DataTypeConfig();
            entity.Id = dr[0] as string;
            entity.Name = dr[1] as string;
            entity.Type = dr[2] as string;
            entity.Class = dr[3] as string;
            if (!dr.IsDBNull(4))
            {
                entity.Precision = dr.GetInt32(4);
            }
            return entity;
        }

        public static CollectionData CreateCollectionDataEntity(IDataReader dr)
        {
            CollectionData entity = new CollectionData();
            //NoteId,PhoneNumber,MessageCenterNo,SendTime,MessageContent,status,BccResult,FrameMark,TransformMark,InsertTime
            if (!dr.IsDBNull(0))
            {
                entity.NoteId = dr.GetInt32(0);
            }
            entity.PhoneNumber = dr[1] as string;
            entity.MessageCenterNo = dr[2] as string;
            if (!dr.IsDBNull(3))
            {
                entity.SendTime = dr.GetDateTime(3);
            }
            entity.MessageContent = dr[4] as string;
            if (!dr.IsDBNull(5))
            {
                entity.Status = dr.GetInt32(5);
            }
            if (!dr.IsDBNull(6))
            {
                entity.BccResult = dr.GetInt32(6);
            }
            if (!dr.IsDBNull(7))
            {
                entity.FrameMark = dr.GetDecimal(7);
            }
            if (!dr.IsDBNull(8))
            {
                entity.TransformMark = dr.GetInt32(8);
            }
            return entity;
        }


        public static SourceCodeData CreateSourceCodeData(IDataReader dr)
        {

            // rtuid,remoteip,sendtime,messagecontent,'receive' as direction
            SourceCodeData result = new SourceCodeData();
            if (!dr.IsDBNull(0))
            {
                result.RtuID= dr[0] as string;
            }
            if (!dr.IsDBNull(1))
            {
                result.RemoteIP = dr[1] as string;
            }
            if (!dr.IsDBNull(2))
            {
                result.MessageTime = dr.GetDateTime(2);
            }
            if (!dr.IsDBNull(3))
            {
                result.MessageContent = dr[3] as string;
            }
            result.Direction = dr[4].ToString().ToLower() == "receive" ? MessageDirection.Receive : MessageDirection.Send;
            return result;
        }
        public static UnConfiguredRTU CreateUnconfiguredRtu(IDataReader dr)
        {
            UnConfiguredRTU result = new UnConfiguredRTU();
            if (!dr.IsDBNull(0))
            {
                result.RtuID = dr[0] as string;
            }
            ///rtuid,ProductTypeId,InsertTime,Tag
            if (!dr.IsDBNull(1))
            {
                result.ProductTypeID = dr[1] as string;
            }

            if (!dr.IsDBNull(2))
            {
                result.InsertTime = dr.GetDateTime(2);
            }
            if (!dr.IsDBNull(3))
            {
                result.Tag = dr.GetInt16(3);
            }
            return result;
        }

        /// <summary>
        /// 转换string数组至in 所用字符串
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static string ConvertToSqlInnerstr(string[] filters)
        {
            string result="";
            foreach (string str in filters)
            {
                result += "'" + str + "',";

            }
            if (result.Length > 0)
            {
                result = result.Substring(0, result.Length - 1);  
            }


            return result;

        }
        #endregion
    }
}
