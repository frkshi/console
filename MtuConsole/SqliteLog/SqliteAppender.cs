using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqliteLog 
{
    public class SqliteAppender : log4net.Appender.AdoNetAppender
    {
        int count = 0;
		#region Public Instance Constructors
        public SqliteAppender()
            : base()
        {

        }

        #endregion 
		
        #region Override AdoNetAppender
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            if (String.IsNullOrEmpty(this.m_tableName) || String.IsNullOrEmpty(this.m_tableDefine)) return;

            using (System.Data.IDbCommand dbCommand = this.Connection.CreateCommand())
            {
                Console.WriteLine(String.Format("delete data : {0}", count));
                count++;
                String existsScript = String.Format("SELECT COUNT(*) AS CNT FROM SQLITE_MASTER WHERE TYPE=\'table\' AND UPPER(TBL_NAME)=\'{0}\'", this.m_tableName.ToUpper());
                dbCommand.CommandType = System.Data.CommandType.Text;
                dbCommand.CommandText = existsScript;
                if (int.Parse(dbCommand.ExecuteScalar().ToString())== 0)
                {
                    dbCommand.CommandText = this.m_tableDefine;
                    dbCommand.ExecuteNonQuery();
                }
            }    
        }
        protected override void SendBuffer(System.Data.IDbTransaction dbTran, log4net.Core.LoggingEvent[] events)
        {
            base.SendBuffer(dbTran, events);
            if (this.m_savingDays > 0 && this.Connection != null && this.Connection.State == System.Data.ConnectionState.Open)
            {

                String delCommandText = String.Format("DELETE FROM {0} WHERE [DATE]<= SUBSTR(DATE('NOW', '-{1} day')||' 23:59:59', 1, 19)", this.m_tableName, this.m_savingDays);
                using (System.Data.IDbCommand dbCommand = this.Connection.CreateCommand())
                {
                    dbCommand.CommandType = System.Data.CommandType.Text;
                    dbCommand.CommandText = delCommandText;
                    dbCommand.ExecuteNonQuery();
                }                 
            }
        }

        #endregion

        #region Public Instance Properties
        /// <summary>
        /// 保留日志时间(单位：天)
        /// </summary>
        public int SavingDays
        {
            get { return this.m_savingDays; }
            set { this.m_savingDays = value; }
        }

        /// <summary>
        /// logtable名称
        /// </summary>
        public String TableName
        {
            get { return this.m_tableName; }
            set { this.m_tableName = value; }
        }

        /// <summary>
        /// create table logTable 脚本
        /// </summary>
        public String TableDefine 
        {
            get { return this.m_tableDefine; }
            set { this.m_tableDefine = value; }
        }

        #endregion
        
        #region Private Instance Fields
        //默认保留日志15天
        private int m_savingDays = 15;

        private String m_tableDefine = "";
        private String m_tableName = "";
        #endregion
    }
}
