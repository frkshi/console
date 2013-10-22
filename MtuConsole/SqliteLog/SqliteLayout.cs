using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqliteLog
{
    public class SqliteLayout : log4net.Layout.PatternLayout
    {
        public SqliteLayout()
        {
            this.AddConverter("property", typeof(ReflectionPatternConverter));
        }
    }
}
