using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Sqlite;
using DataEntity;
using System.Data;
using DataAccess.SqlServer;
using DataAccess.Helpers;

namespace DataAccess
{
    public class SpecialManager
    {
        private SqliteSettingPersistenceContext _sqliteContext;     //源路径
        private SqlServerSettingPersistenceContext _sqlContext;     //目标路径

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqliteContext">源路径</param>
        /// <param name="sqlContext">目标路径</param>
        public SpecialManager(SqliteSettingPersistenceContext sqliteContext, SqlServerSettingPersistenceContext sqlContext)
        {
            _sqliteContext = sqliteContext;
            _sqlContext = sqlContext;
        }

        #region 同步配置
        /// <summary>
        /// 同步配置
        /// </summary>
        /// <param name="sourceContext">源路径</param>
        /// <param name="destContext">目标路径</param>
        /// <returns>bool型</returns>
        public bool SyncSetting()
        {
            SqliteSettingRepository sqliteRepository = new SqliteSettingRepository(SqliteConnectionHelper.BuildConnectionString(_sqliteContext.FilePath + "DataLogConfig.db3"));
            List<CommunicationSetting> listComm = sqliteRepository.LoadCommunicationSettingList();
            List<RTUSetting> listRtu = sqliteRepository.LoadRTUSettingList();
            List<MeasureSetting> listMeasure = sqliteRepository.LoadMeasureSettingList();

            SqlServerSettingRepository sqlRepository = new SqlServerSettingRepository(_sqlContext.ConnectionString);
            sqlRepository.TruncateTable();

            if (sqlRepository.SyncCommunicationSetting(listComm) &&
                sqlRepository.SyncRTUSetting(listRtu) &&
                sqlRepository.SyncMeasureSetting(listMeasure))
                return true;
            else
                return false;

        }
        #endregion
    }
}
