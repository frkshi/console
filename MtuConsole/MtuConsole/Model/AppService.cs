using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;//.Tasks;
using System.ComponentModel;

namespace MtuConsole
{
    public class BasicDataServiceImp : IBasicDataService
    {
        #region << Private Fields >>

     

        private EventHandlerList _eventHandlers = new EventHandlerList();
      

        private readonly object _rtuChanged = new object();

        private readonly object _noticeMessageChanged = new object();


        #endregion

        #region << ctor >>

        public BasicDataServiceImp()
        {
      

            InitCacheData();

            InitChangedEventHandler();

          
        }

        private void InitCacheData()
        {
            //_channels = new ChangeReminderList<Model.Channel>();
            //_communicationTypes = new ChangeReminderList<Model.CommunicationType>();
            //_decodeLibrarys = new ChangeReminderList<Model.DecodeLibrary>();

            //_rtuTypes = new ChangeReminderList<Model.RtuType>();
            //_rtus = new ChangeReminderList<Model.RtuItem>();

            //_rtuTemplets = new ChangeReminderList<Model.RtuTemplet>();
            //_measureDataTypes = new ChangeReminderList<Model.MeasureDataType>();

            //_dlgDataTypes = new ChangeReminderList<Model.DLGDataType>();

            //_noticeMessages = new ChangeReminderList<Message>();
        }

        private void InitChangedEventHandler()
        {
           // ((IChangeReminderCollection)_rtus).Changed += new EventHandler<ChangeReminderEventArgs>(RTU_Changed);
          
          
        }

        #endregion

        #region << Cache Data >>

       

        private IList<RtuItem> _rtus;
        public IList<RtuItem> Rtus
        {
            get { return _rtus; }
            private set { _rtus = value; }
        }

      
        #endregion

        #region << Sync Method >>

        public void SyncAll()
        {


            //TODO:同步
           
 

        
        }


      

     
        
        #endregion

        #region << Events >>

       

        /// <summary>
        /// Rtu List Changed
        /// </summary>
        public event EventHandler<ChangeReminderEventArgs> RtusChanged
        {
            add { _eventHandlers.AddHandler(_rtuChanged, value); }
            remove { _eventHandlers.RemoveHandler(_rtuChanged, value); }
        }

    

        /// <summary>
        /// Fire cache data changed event
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="action"></param>
        private void FireCacheDataChanged(object eventObject, ChangeReminderEventArgs args)
        {
            EventHandler<ChangeReminderEventArgs> handler = _eventHandlers[eventObject] as EventHandler<ChangeReminderEventArgs>;

            if (handler != null)
                handler(this, args);
        }

        /// <summary>
        /// RTU List Changed event handler
        /// </summary>
        /// <param name="sender">event source</param>
        /// <param name="e">args</param>
        private void RTU_Changed(object sender, ChangeReminderEventArgs e)
        {
            FireCacheDataChanged(_rtuChanged, e);
        }


        private void NoticeMessage_Changed(object sender, ChangeReminderEventArgs e)
        {
            FireCacheDataChanged(_noticeMessageChanged, e);
        }

        #endregion

        #region << CommunicationService Event Handler >>

        void CommunicationService_OnAuth(object sender)
        {
            SyncAll();  // first init
        }

       

   

     


       #endregion

        



        #region << IDisposable Members >>

        public void Dispose()
        {
           
        }

        #endregion

    }
}
