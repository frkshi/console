using System;
using System.Collections.Generic;
using System.Text;

namespace DataEntity
{
    /// <summary>
    /// 配置操作痕迹
    /// </summary>
    [Serializable]
    public class ConfigTrack
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OpTime
        {
            get;
            set;
        }

        /// <summary>
        /// 操作类型
        /// </summary>
        public OPType OpType
        {
            get;
            set;
        }

        /// <summary>
        /// 操作编码
        /// </summary>
        public OPCode OpCode
        {
            get;
            set;
        }

        /// <summary>
        /// 操作键值
        /// </summary>
        public OPKeyId OpKeyId
        {
            get;
            set;
        }

        /// <summary>
        /// 操作描述
        /// </summary>
        public string OpRemark
        {
            get;
            set;
        }
    }   

}
