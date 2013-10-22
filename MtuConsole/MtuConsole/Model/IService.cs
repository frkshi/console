using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;

namespace MtuConsole
{
    /// <summary>
    /// 带提醒的集合
    /// </summary>
    public interface IChangeReminderCollection
    {
        event EventHandler<ChangeReminderEventArgs> Changed;
    }

    public enum CollectionChangedType
    {
        Add,
        Replace,
        Remove,
        Clear,
        Batch
    }

    public class ChangeReminderEventArgs : EventArgs
    {
        public ChangeReminderEventArgs() { }

        public ChangeReminderEventArgs(CollectionChangedType action)
        {
            Action = action;
        }

        public CollectionChangedType Action { get; set; }
    }
    public interface IBasicDataService : IDisposable
    {
        #region << Cache Data >>

        /// <summary>
        /// Rtu list
        /// </summary>
        IList<RtuItem> Rtus { get; }

        #endregion

     

        #region << Events >>

        /// <summary>
        /// cache data, Rtu List Changed
        /// </summary>
        event EventHandler<ChangeReminderEventArgs> RtusChanged;


        #endregion
    }
}
