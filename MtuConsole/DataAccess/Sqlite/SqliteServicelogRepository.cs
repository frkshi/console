using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataEntity;
using DataAccess.Interfaces;
using System.Data.SQLite;
using DataAccess.Helpers;

namespace DataAccess.Sqlite
{
    public class SqliteMtuLogRepository : SqliteRepositoryBase,IMtuLogRepsitory
    {
        /// <summary>
        /// 构造函数     
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        public SqliteMtuLogRepository(string connectionString) : base(connectionString) { }

        #region IMtuLogRepsitory 成员

     

        public DataTable LoadMtuLog(DateTime begintime,DateTime endtime,string actions,string levels)
        {

            DataTable dt = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(this.ConnectionString))
            {
                SQLiteCommand cmd = new SQLiteCommand(conn);
                string strsql = "select * from MtuLog where date between '{0}' and '{1}' ";
                
                strsql = string.Format(strsql, begintime.ToString("yyyy-MM-dd HH:mm:ss"), endtime.ToString("yyyy-MM-dd HH:mm:ss"));
                if (actions.Length > 1)
                {
                    strsql = strsql + " and action in (" + actions + ") ";
                }
                if (levels.Length > 1)
                {
                    strsql = strsql + " and level in (" + levels + ")";
                }
                cmd.CommandText = strsql;

                conn.Open();

                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                adapter.Fill(dt);
            }
            return dt;
        }

        
        public DataTable LoadMtuLog(string logtype)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
