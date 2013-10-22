using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataEntity;
using System.Data;
using MtuConsole.Common;
 

namespace DataAccess
{
    public class MtuLogManager
    {
        private IMtuLogPersistenceContext _context;
        
        private MtuLog _logger;
     

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context">SQLite文件存储参数</param>
        public MtuLogManager(SqliteMtuLogPersisitenceContext context)
        {
            _logger = new MtuLog();
            _context = context;
        }
    

   

        /// <summary>
        /// 读取service log
        /// </summary>
        /// <returns></returns>
        public DataTable LoadMtuLog(DateTime begintime,DateTime endtime,string actions,string levels)
        {
            DataTable result = _context.GetRepository().LoadMtuLog(begintime ,endtime ,actions,levels);

            return result;
        }

     


    }
}
