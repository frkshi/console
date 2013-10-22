using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// 存储错误类型
    /// </summary>
    public enum DataPersistErrorType
    {
        ConnectionError = 1,
        DBError = 2
    }
    
    public class DataPersistErrorEventArgs : EventArgs
    {
        /// <summary>
        /// 存储错误类型
        /// </summary>
        public DataPersistErrorType ErrorType { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="errorType">存储错误类型</param>
        public DataPersistErrorEventArgs(DataPersistErrorType errorType)
        {
            this.ErrorType = errorType;
        }

    }

}
