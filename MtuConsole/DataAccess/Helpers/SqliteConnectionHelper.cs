using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace DataAccess.Helpers
{
    internal static class SqliteConnectionHelper
    {
        /// <summary>
        /// 构建sqlite连接字符串
        /// </summary>
        /// <param name="dbFile">文件路径</param>
        /// <returns>sqlite连接字符串</returns>
        public static string BuildConnectionString(string dbFile)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder();
            sb.DataSource = dbFile;
            sb.Version = 3;
            sb.DefaultTimeout = 120;
            return sb.ToString();
        }

        public static string BuildCustomerConnString(string filename, string connstring)
        {
            SQLiteConnectionStringBuilder sb = new SQLiteConnectionStringBuilder(connstring);
            SQLiteConnectionStringBuilder sbReturn = new SQLiteConnectionStringBuilder();
            sbReturn.DataSource = sb.DataSource.Substring(0, sb.DataSource.LastIndexOf('\\') + 1) + "\\" + filename + ".db3";
            sbReturn.Version = sb.Version;
            sbReturn.DefaultTimeout = sb.DefaultTimeout;
            return sbReturn.ToString();
        }
    }
}
