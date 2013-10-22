using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqliteLog
{
    public class ReflectionPatternConverter : log4net.Layout.Pattern.PatternLayoutConverter
    {
        protected override void Convert(System.IO.TextWriter writer, log4net.Core.LoggingEvent loggingEvent)
        {
            if (Option != null)
            { 
                 log4net.Layout.Pattern.PatternLayoutConverter.WriteObject(writer, loggingEvent.Repository, FindProperty(Option, loggingEvent));
            }
            else
            { 
                log4net.Layout.Pattern.PatternLayoutConverter.WriteDictionary(writer,loggingEvent.Repository, loggingEvent.GetProperties());
            }
        }
 
        /// <summary>
        /// 日志对象属性值
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private object FindProperty(string property, log4net.Core.LoggingEvent loggingEvent)
        {
            object propertyValue = string.Empty; 
            System.Reflection.PropertyInfo propertyInfo = loggingEvent.MessageObject.GetType().GetProperty(property);
            if (propertyInfo != null)
            {
                propertyValue = propertyInfo.GetValue(loggingEvent.MessageObject, null);
            }
            return propertyValue;
        }
    }  
}
