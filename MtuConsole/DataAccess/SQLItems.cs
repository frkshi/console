using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using System.Data.SqlClient;

namespace DataAccess
{

    public static class SQLItems
    {

        #region Sql Parts

        /// <summary>
        /// CommunicationSetting Fields
        /// </summary>
        public static readonly string DefaultCommSettingFields = @"CommunicationId, CommunicationName, MobileNo, Note, APN, Dns1, Dns2, IP, ExTcp, InTcp, CommunicationTypeId, Tag";

        /// <summary>
        /// CommunicationSetting Fields without identity field
        /// </summary>
        public static readonly string CommSettingFields = @"CommunicationName, MobileNo, Note, APN, Dns1, Dns2, IP, ExTcp, InTcp, CommunicationTypeId, Tag";


        /// <summary>
        /// RtuSetting Fields
        /// </summary>
        public static readonly string DefaultRtuSettingFields = @"RTUId, RTUName, RTUSName, ProductTypeId, Manu, InstallDate, InstallLoca, InstallAddr, Analog, Digit, VoltAlert, VoltClose, CollCycle, SendCycle,
                                                            SaveCycle, Note, MobileNo, CommunicationId, CommunicationId1, CommunicationId2, IsOpen, OpenTime1, OpenTime2, OpenTime3, OpenTime4, OpenTime5, 
                                                            ServiceCenter, TableName, RegionId, PowerSupply, Tag, CommunicationTypeId, Caliber, CaliberType, SendExceptionAlert, SendVoltAlert,
                                                            [CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],
                                                            [LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber],[BackValue],[DeviceStatus],[CurrentCycle]";
        public static readonly string DefaultRtuSettingDeviceFields = @"RtuID,ProductType,SoftwareVersion,HardwareVersion,APN,IP1,IP2,Port1,Port2,SecretIP,SecretPort,Savecycle1,Sendcycle1,SaveCycle2,Sendcycle2,BackValue,AirPressure,Scale";
        /// <summary>
        /// MeasureSetting Fields
        /// </summary>
        public static readonly string DefaultMeasureSettingFields = @"MeasureId, MeasureName, RTUId, SignalType, DataType, PortId, Unit, [Range], Elevation, [Precision], DecimalDigits, Scale, Offset, Revise, 
                                                            UpperLimit, LowerLimit, Ratio, Note, Type, IsOpen, IsSet, SendOutData, SendChangeData, DataStatus, SwitchType, SendRedirectionAlert,increaseinterval";
        /// <summary>
        /// MeasureSetting Fields without identity field
        /// </summary>
        public static readonly string MeasureSettingFields = @"MeasureName, RTUId, SignalType, DataType, PortId, Unit, Range, Elevation, [Precision], DecimalDigits, Scale, Offset, Revise, 
                                                            UpperLimit, LowerLimit, Ratio, Note, Type, IsOpen, IsSet, SendOutData, SendChangeData, DataStatus, SwitchType, SendRedirectionAlert";

        /// <summary>
        /// MeasureTemplet Fields
        /// </summary>
        public static readonly string DefalutMeasureTempletFields = @"MTempletId, MTempletName, ProductTypeId, Manu, SignalType, Unit, [Range], [Precision], DecimalDigits, Scale, Offset, UpperLimit, LowerLimit,
                                                            Ratio, Note, DataType, PortId, IsSet";
        /// <summary>
        /// RtuTemplet Fields
        /// </summary>
        public static readonly string DefaultRtuTempletFields = @"ProductTypeId, Manu, Analog, Digit, VoltAlert, VoltClose, CollCycle, SendCycle, SaveCycle, Note, ServiceCenter, CommunicationTypeId, Tag, Caliber, CaliberType,
                                                                 [CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],[SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber],[Backvalue]"; 

        /// <summary>
        /// MeasureData Fields
        /// </summary>
        public static readonly string DefaultMeasureDataFields = "MeasureId, CollDatetime, CollNum,Tag, Sign";

        /// <summary>
        /// AlertData Fields
        /// </summary>
        public static readonly string DefaultAlertDataFields = "MeasureId,CollStartTime, StartNum, CollEndTime, EndNum, Ratio,AlertTypeId, Tag, Sign, RTUId ";



        public static readonly string DefaultAlertDataDetailFields = "MeasureId,RtuId,CollDateTimes,CollNums,AlertTypeID";
        /// <summary>
        /// ConfigTrack Fields
        /// </summary>
        public static readonly string DefaultConfigTrackFields = "OPTime, OPType, OPCode, OPKeyID, OPRemark";

        /// <summary>
        /// AlertType Fields
        /// </summary>
        public static readonly string DefaultAlertTypeFields = "AlertTypeId, AlertName, ValidTime, Remark, StaticImageUrl, DynamicImageUrl, GroupType";

        /// <summary>
        /// DataTypeConfig Fields
        /// </summary>
        public static readonly string DefaultDataTypeFields = "ID, Name, Type, Class, [Precision]";

        /// <summary>
        /// CommunicationType Fields
        /// </summary>
        public static readonly string DefaultCommunicationTypeFields = "CommunicationTypeId,CTName";

        /// <summary>
        /// collectiondata fields
        /// </summary>
        public static readonly string DefaultCollectionDataFields = " NoteId,PhoneNumber,MessageCenterNo,SendTime,MessageContent,status,BccResult,FrameMark,TransformMark";


        /// <summary>
        /// AirPress table name
        /// </summary>
        public static readonly string DefaultAirPressureTableName = "AirPressure";

        /// <summary>
        /// CommunicationSetting TableName
        /// </summary>
        public static readonly string DefaultCommSettingTableName = "CommunicationSetting";

        /// <summary>
        /// RtuSetting TableName
        /// </summary>
        public static readonly string DefaultRtuSettingTableName = "RTUSetting";

        /// <summary>
        /// MeasureSetting TableName
        /// </summary>
        public static readonly string DefaultMeasureSettingTableName = "MeasureSetting";

        /// <summary>
        /// MeasureTemplet TableName
        /// </summary>
        public static readonly string DefaultMeasureTempletTableName = "MeasureTemplet";

        /// <summary>
        /// RtuTemplet TableName
        /// </summary>
        public static readonly string DefaultRtuTempletTableName = "RTUTemplet";

        /// <summary>
        /// MeasureData TableName
        /// </summary>
        public static readonly string DefaultMeasureDataTableName = "MeasureData";

        /// <summary>
        /// AlertData TableName
        /// </summary>
        public static readonly string DefaultAlertDataTableName = "AlertData";

        /// <summary>
        /// ConfigTrack TableName
        /// </summary>
        public static readonly string DefaultConfigTrackTableName = "ConfigTrack";

        /// <summary>
        /// CommunicationType TableName
        /// </summary>
        public static readonly string DefaultCommunicationTypeTableName = "CommunicationType";

        /// <summary>
        /// AlertType TableName
        /// </summary>
        public static readonly string DefaultAlertTypeTableName = "AlertType";

        /// <summary>
        /// DataType TableName
        /// </summary>
        public static readonly string DefaultDataTypeTableName = "DataTypeConfig";

        /// <summary>
        /// CollectionData TableName
        /// </summary>
        public static readonly string DefaultCollectionDataTableName = "CollectionData";

        /// <summary>
        /// device tablename
        /// </summary>
        public static readonly string DefaultDeviceTableName = "DeviceSetting";
        /// <summary>
        /// device table fields
        /// </summary>
        public static readonly string DefaultDeviceFields = "deviceid,rtuid,devicename,commandcount";

        /// <summary>
        /// datablock tablename
        /// </summary>
        public static readonly string DefaultDatablockTableName = "DatablockSetting";

        /// <summary>
        /// datablock fields
        /// </summary>
        public static readonly string DefaultDatablockFields = "BlockID,MeasureId,DeviceId,DataTypeId,Swap,Start,Length";
        
        /// <summary>
        /// dlgdatatype tablename
        /// </summary>
        public static readonly string  DefaultDLGDatatypeTableName="DLGDataType";

        /// <summary>
        /// dlgdatatype fields
        /// </summary>
        public static readonly string DefaultDLGDatatypeFields = "Id,Name,Digest";
             



        #endregion

        #region Join Table

        public static readonly string AllTempletFields = @"a.ProductTypeId, a.Manu as RManu, Analog, Digit, VoltAlert, VoltClose, CollCycle, SendCycle, SaveCycle,
                                                            a.Note as RNote, ServiceCenter, CommunicationTypeId, Tag, Caliber, CaliberType,
                                                            [CollCycle2],[SendCycle2],[SaveCycle2],[CollCycle3],[SendCycle3],[SaveCycle3],[SafetyLevel],[AlertLevel],
                                                            [SystemVoltage],[UltrasonicVoltage],[LocationDevication],[WellDeep],[WellElevation],[PipeElevation],[PipeCaliber], 
                                                            MTempletId, MTempletName, b.Manu as MManu , SignalType, Unit, [Range], [Precision], DecimalDigits,
                                                            Scale, Offset, UpperLimit, LowerLimit, Ratio, b.Note as MNote, DataType, PortId, IsSet,a.BackValue";
        public static readonly string AllTempletTables = @" RtuTemplet a LEFT JOIN MeasureTemplet b ON a.ProductTypeId = b.ProductTypeId ";

        #endregion

        #region truncate
        public static readonly string TruncateTableSql = @"truncate table communicationsetting 
                                                           truncate table measuresetting 
                                                           truncate table rtusetting";
        #endregion

        #region Create Select Sql

        /// <summary>
        /// 构建SQL 的查询语句
        /// </summary>
        /// <param name="selectItem">选项</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns>SQL查询 语句</returns>
        public static string CreateSelectSql(string selectItem, string tableName, string condition, string orderBy)
        {
            if (string.IsNullOrEmpty(selectItem) || string.IsNullOrEmpty(tableName))
                return string.Empty;
            else
            {
                return " SELECT " + selectItem + " FROM " + tableName
                        + (string.IsNullOrEmpty(condition) ? "" : " WHERE " + condition)
                        + (string.IsNullOrEmpty(orderBy) ? "" : " ORDER By " + orderBy);
            }
        }

        /// <summary>
        /// 构建删除sql语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="condition">条件</param>
        /// <returns>删除sql语句</returns>
        public static string CreateDeleteSql(string tableName, string condition)
        {
            if (string.IsNullOrEmpty(tableName))
                return string.Empty;
            else
            {
                return "DELETE FROM " + tableName + (string.IsNullOrEmpty(condition) ? "" : " WHERE " + condition);
            }
        }

        #endregion

    }
}
