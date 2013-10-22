using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataEntity;
using System.Threading;
using System.IO;
using DataAccess.Helpers;
using MtuConsole.Common;
using DataAccess.Sqlite;

namespace DataAccess
{
    public class MeasureDataExportManager
    {

        #region Private Fields

        private MtuLog _logger = new MtuLog();

        /// <summary>
        /// 遇到数据库异常时是否备份至本地文件
        /// </summary>
        public bool NeedBackup { get; private set; }

        /// <summary>
        /// 数据队列存储器
        /// </summary>
        private MeasureDataExportQueueSaver _saver;

        /// <summary>
        /// 数据库连接监视器
        /// </summary>
        private DbConnectionMonitor _monitor;

        /// <summary>
        /// 数据队列
        /// </summary>
        private List<MeasureData> _queue;

        /// <summary>
        /// 监视器开关
        /// </summary>
        public bool _monitorStarted = false;

        #endregion

        #region Properties

        internal IMeasureDataPersistenceContext TargetPersistenceContext { get; private set; }

        internal IMeasureDataPersistenceContext BackupPersistenceContext { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">远端sqlserver数据库存储参数</param>
        /// <param name="backup">本地sqlite备份存储参数</param>
        public MeasureDataExportManager(SqlServerMeasureDataExportPersistenceContext target, SqliteMeasureDataPersistenceContext backup)
        {
            _logger = new MtuLog();

            this.NeedBackup = true;

            this.TargetPersistenceContext = target;

            if (backup == null)
            {
                backup = new SqliteMeasureDataPersistenceContext(FileHelper.ReviseFilePath(AppDomain.CurrentDomain.BaseDirectory), FileSplitUnit.Day, UtilityParameters.DefaultFreeSpace);
            }

            SqliteMeasureDataPersistenceContext tmpContext = SqliteMeasureDataPersistenceContext.Copy(backup);
            tmpContext.FilePath += FolderNames.ExportFolderName + "\\" +  FolderNames.MeasureFolerName;
            FileHelper.EnsureFolderExist(tmpContext.FilePath);
            tmpContext.FilePath += "\\";
            this.BackupPersistenceContext = tmpContext;

            this.Initialize();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">本地sqlite存储参数</param>
        public MeasureDataExportManager(SqliteMeasureDataPersistenceContext target)
        {
            _logger = new MtuLog();

            if (target == null)
            {
                target = new SqliteMeasureDataPersistenceContext(FileHelper.ReviseFilePath(AppDomain.CurrentDomain.BaseDirectory), FileSplitUnit.Hour, UtilityParameters.DefaultFreeSpace);
            }

            SqliteMeasureDataPersistenceContext tmpContext = SqliteMeasureDataPersistenceContext.Copy(target);
            tmpContext.FilePath += FolderNames.DataFolerName + "\\" + FolderNames.ExportFolderName + "\\" +  FolderNames.MeasureFolerName;
            FileHelper.EnsureFolderExist(tmpContext.FilePath);
            tmpContext.FilePath += "\\";

            this.TargetPersistenceContext = tmpContext;
            this.Initialize();
        }

        #endregion

        #region Event Requires

        /// <summary>
        /// 事件处理
        /// </summary>
        public event EventHandler<DataPersistErrorEventArgs> Error;

        internal void OnError(DataPersistErrorEventArgs args)
        {
            EventHandler<DataPersistErrorEventArgs> temp = this.Error;

            if (temp != null)
            {
                temp(this, args);
            }
        }

        #endregion

        #region Internal Methods

        internal List<MeasureData> GetQueueData()
        {
            return _queue;
        }

        internal void StartMonitorConnection()
        {
            if (!_monitorStarted)
            {
                _monitor.StartDbConnectionDetecting();
                _monitorStarted = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 保存单个检测量
        /// </summary>
        /// <param name="entity">单个检测量</param>
        public void AddToWrite(MeasureData entity)
        {
            this._queue.Add(entity);
        }

        /// <summary>
        /// 保存多个检测量
        /// </summary>
        /// <param name="entities">检测量集合</param>
        public void AddToWrite(IEnumerable<MeasureData> entities)
        {
            this._queue.AddRange(entities);
        }

        /// <summary>
        /// 线程结束命令，调用该方法确保队列中的数据持久化到数据库
        /// </summary>
        public void EnsureQueueDataPersisted()
        {
            _saver.SetWorkFlag(false);
            _saver.GetWorkerThread().Join();
        }
        #endregion


        #region Private Method

        private void Initialize()
        {
            _queue = new List<MeasureData>();           
            _saver = new MeasureDataExportQueueSaver(this);

            ICheckConnection cc = this.TargetPersistenceContext.GetRepository() as ICheckConnection;
            if (BackupPersistenceContext != null)
            {
                if (!cc.CheckConnection())
                {
                    _saver._directlySaveToBackup = true;

                    _logger.Debug("DirectlySaveToBackup value changed, newvalue:" + _saver._directlySaveToBackup.ToString());
                    _logger.Debug("DataBase Error Message: 数据库无法连接！");
                }
            }
            _saver.StartWork();

            if (this.NeedBackup)
            {
                _monitor = new DbConnectionMonitor(cc);

                _monitor.ConnectionOk += delegate(object sender, EventArgs e)
                {
                    _saver.NotifyConnectionOk();
                };
            }
            if (_saver._directlySaveToBackup)
            {
                this.StartMonitorConnection();
                this.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.ConnectionError));
            }
        }

        #endregion

    }
}
