using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DataEntity;
using DataAccess.Interfaces;
using DataAccess.Helpers;
using System.IO;
using MtuConsole.Common;

namespace DataAccess
{
    internal class MeasureDataExportQueueSaver
    {

        #region Private Fields

        private MtuLog _logger = null;

        /// <summary>
        /// 工作线程
        /// </summary>
        private Thread _workerThread;

        /// <summary>
        /// 运行标识
        /// </summary>
        private bool _workFlag = false;

        /// <summary>
        /// 线程运行间隔
        /// </summary>
        private int _workDuration = 2000;

        /// <summary>
        /// 是否保存至本地备份
        /// </summary>
        public bool _directlySaveToBackup = false;

        /// <summary>
        /// 是否存在备份文件
        /// </summary>
        private bool _hasBackupData = true;

        /// <summary>
        /// 检测量数据存储管理器
        /// </summary>
        private MeasureDataExportManager _manager;

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager">检测量数据存储管理器</param>
        public MeasureDataExportQueueSaver(MeasureDataExportManager manager)
        {
            _logger = new MtuLog();

            _manager = manager;

            _workerThread = new Thread(new ThreadStart(this.DoWork));

            _workFlag = true;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 数据库连接畅通
        /// </summary>
        public void NotifyConnectionOk()
        {
            _directlySaveToBackup = false;
            _manager._monitorStarted = false;
            _logger.Debug("DirectlySaveToBackup value changed, newvalue:" + _directlySaveToBackup.ToString());
        }

        /// <summary>
        /// 线程启动
        /// </summary>
        public void StartWork()
        {
            _workerThread.Start();
        }

        /// <summary>
        /// 获取当前工作线程
        /// </summary>
        /// <returns>线程</returns>
        public Thread GetWorkerThread()
        {
            return _workerThread;
        }

        /// <summary>
        /// 设置运行标识
        /// </summary>
        /// <param name="flag">运行标识</param>
        public void SetWorkFlag(bool flag)
        {
            _workFlag = flag;
        }

        #endregion

        #region Private Methods

        private void DoWork()
        {
            while (_workFlag)
            {
                this.DoDetailWork();

                Thread.Sleep(_workDuration);
            }

            //this call is used to ensure queue data persisted
            this.DoDetailWork();
        }

        private void DoDetailWork()
        {
           
            if (_manager.NeedBackup && !_directlySaveToBackup && _hasBackupData )
            {
                this.RestoreBackupData();
            }

            List<MeasureData> queue = _manager.GetQueueData();
            int queueCount = queue.Count;

            if (queueCount > 0)
            {
                MeasureData[] temp = new MeasureData[queueCount];
                queue.CopyTo(0, temp, 0, queueCount);
                queue.RemoveRange(0, queueCount);
                this.SaveData(temp);
            }
        }

        private void SaveData(MeasureData[] data)
        {
        
            if (_manager.NeedBackup)
            {
                if (_directlySaveToBackup)
                {
                    _manager.BackupPersistenceContext.GetRepository().BulkInsert(data);
                }
                else
                {
                    var repository = _manager.TargetPersistenceContext.GetRepository() as SqlServer.SqlServerMeasureDataExportRepository;
                    
                    if (!repository.BulkInsert(data))
                    {                        
                        SubDbMonitor.DBErrorCount++;
                        _manager.BackupPersistenceContext.GetRepository().BulkInsert(data);
                        _hasBackupData = true;

                        _directlySaveToBackup = true;

                        _logger.Debug("DirectlySaveToBackup value changed, newvalue:" + _directlySaveToBackup.ToString());

                        _manager.StartMonitorConnection();
                        _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                    }
                    else if (SubDbMonitor.DBErrorCount > 0)
                        SubDbMonitor.DBErrorCount = 0;
                }
            }
            else
            {
                if (!_manager.TargetPersistenceContext.GetRepository().BulkInsert(data))
                {
                    _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                }
            }
        }

        private void RestoreBackupData()
        {
            try
            {
                //we can only deal with sqlite backup db here now
                if (_manager.BackupPersistenceContext is SqliteMeasureDataPersistenceContext)
                {
                    SqliteMeasureDataPersistenceContext ctx = _manager.BackupPersistenceContext as SqliteMeasureDataPersistenceContext;
                    
                    string f = FileHelper.GetFirstCreationFile(ctx.FilePath);                    

                    while (!string.IsNullOrEmpty(f))
                    {
                        IMeasureDataBackupRepository repository = _manager.BackupPersistenceContext.GetRepository(f) as IMeasureDataBackupRepository;
                        IEnumerable<MeasureData> all = repository.LoadAll();
                        _manager.GetQueueData().AddRange(all);

                        File.Delete(f);
                        f = FileHelper.GetFirstCreationFile(ctx.FilePath);  
                    }
                    _hasBackupData = false;
                }

            }
            catch (Exception e)
            {
                _logger.Error ("MeasureData 数据恢复至DB出错，错误信息：" + e.Message.ToString(),e);
            }
            finally { }
        }

        #endregion

    }
}
