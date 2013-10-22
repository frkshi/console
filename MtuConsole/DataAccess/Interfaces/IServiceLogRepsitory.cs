using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataEntity;

namespace DataAccess.Interfaces
{
    public interface IMtuLogRepsitory
    {
      

        DataTable LoadMtuLog(DateTime begintime,DateTime endtime,string actions,string levels);
        
        
    }
}
