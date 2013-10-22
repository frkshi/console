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
    internal class AlertDataQueueSaver
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
        /// 报警数据存储管理器
        /// </summary>
        private AlertDataManager _manager;             

        #endregion

        #region Constructors

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager">报警数据存储管理器</param>
        public AlertDataQueueSaver(AlertDataManager manager)
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

        /// <summary>
        /// 线程处理
        /// </summary>
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

        /// <summary>
        /// 处理内容
        /// </summary>
        private void DoDetailWork()
        {
          
            if (_manager.NeedBackup && !_directlySaveToBackup && _hasBackupData )
            {
                this.RestoreBackupData();
            }

            List<AlertData> queue = _manager.GetQueueData();
            int queueCount = queue.Count;

            if (queueCount > 0)
            {
                AlertData[] temp = new AlertData[queueCount];
                queue.CopyTo(0, temp, 0, queueCount);
                queue.RemoveRange(0, queueCount);
                this.SaveData(temp);
            }
            List<AlertDataDetail> queuealertdetail = _manager.GetQueueAlertDetail();
            queueCount = queuealertdetail.Count;
            if (queueCount > 0)
            {
                AlertDataDetail[] tempdetail = new AlertDataDetail[queueCount];
                queuealertdetail.CopyTo(0, tempdetail, 0, queueCount);
                queuealertdetail.RemoveRange(0, queueCount);
                SaveAlertDetail(tempdetail);

            }

            List<SecreatDoor> queueSecretdoor = _manager.GetQueueSecretDoor();
            queueCount = queueSecretdoor.Count;
            if (queueCount > 0)
            {
                SecreatDoor[] tempsecretdoor = new SecreatDoor[queueCount];
                queueSecretdoor.CopyTo(0, tempsecretdoor, 0, queueCount);
                queueSecretdoor.RemoveRange(0, queueCount);
                SaveSecretDoor(tempsecretdoor);

            }



        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data">报警数据</param>
        private void SaveData(AlertData[] data)
        {
            if (_manager.NeedBackup)
            {
                
                if (_directlySaveToBackup)
                {
                    _manager.BackupPersistenceContext.GetRepository().BulkInsert(data); 
                }
                else
                {
                    var repository = _manager.TargetPersistenceContext.GetRepository() as SqlServer.SqlServerAlertDataRepository;

                    try
                    {
                        if (!repository.BulkInsert(data))
                        {
                            SubDbMonitor.DBErrorCount++;
                            _manager.BackupPersistenceContext.GetRepository().BulkInsert(data);
                            _hasBackupData = true;

                            _directlySaveToBackup = true;
                            _manager.StartMonitorConnection();
                            _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                        }
                        else if (SubDbMonitor.DBErrorCount > 0)
                            SubDbMonitor.DBErrorCount = 0;
                    }
                    catch (Exception e)
                    {
                        _logger.Debug("AlertData 存入SqlServer出错，错误信息：" + e.Message.ToString());
                    }
                }
            }
            else
            {
                if(!_manager.TargetPersistenceContext.GetRepository().BulkInsert(data))
                {
                    _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                }
            }
        }

        /// <summary>
        /// 保存报警数据，含60条详细信息
        /// </summary>
        /// <param name="data"></param>
        private void SaveAlertDetail(AlertDataDetail[] datas)
        {
           
                if (_manager.NeedBackup)
                {

                    if (_directlySaveToBackup)
                    {

                        _manager.BackupPersistenceContext.GetRepository().InsertAlertDetail(datas);

                    }
                    else
                    {
                        var repository = _manager.TargetPersistenceContext.GetRepository() as SqlServer.SqlServerAlertDataRepository;

                        try
                        {
                            List<AlertDataDetail> leftalertdatas = new List<AlertDataDetail>();

                            if (!repository.InsertAlertDetail(datas,out leftalertdatas))
                            {
                                SubDbMonitor.DBErrorCount++;
                                AlertDataDetail[] exceptiondatas = new AlertDataDetail[leftalertdatas.Count];
                                
                                leftalertdatas.CopyTo(exceptiondatas);
                                _manager.BackupPersistenceContext.GetRepository().InsertAlertDetail(exceptiondatas);
                                _hasBackupData = true;

                                _directlySaveToBackup = true;
                                _manager.StartMonitorConnection();
                                _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                            }
                            else if (SubDbMonitor.DBErrorCount > 0)
                                SubDbMonitor.DBErrorCount = 0;
                        }
                        catch (Exception e)
                        {
                            _logger.Debug("AlertDataDetail 存入SqlServer出错，错误信息：" + e.Message.ToString());
                        }

                    }
                }
                else
                {

                    if (!_manager.TargetPersistenceContext.GetRepository().InsertAlertDetail(datas))
                    {
                        _manager.OnError(new DataPersistErrorEventArgs(DataPersistErrorType.DBError));
                    }
                }
            
        }

        private void SaveSecretDoor(SecreatDoor[] datas)
        {
            if (_manager.NeedBackup)
            {

                    var repository = _manager.TargetPersistenceContext.GetRepository() as SqlServer.SqlServerAlertDataRepository;

                    try
                    {
                        repository.InsertSecretDoor(datas);
                      
                        
                    }
                    catch (Exception e)
                    {
                        _logger.Debug("secretdoor 存入SqlServer出错，错误信息：" + e.Message.ToString());
                    }

                }
           
        }
        /// <summary>
        /// 恢复备份数据
        /// </summary>
        private void RestoreBackupData()
        {
            try
            {
                //we can only deal with sqlite backup db here now
                if (_manager.BackupPersistenceContext is SqliteAlertDataPersistenceContext)
                {
                    SqliteAlertDataPersistenceContext ctx = _manager.BackupPersistenceContext as SqliteAlertDataPersistenceContext;
                    
                    string f = FileHelper.GetFirstCreationFile(ctx.FilePath);                    

                    while (!string.IsNullOrEmpty(f))
                    {
                        IAlertDataBackupRepository repository = _manager.BackupPersistenceContext.GetRepository(f) as IAlertDataBackupRepository;
                        IEnumerable<AlertData> all = repository.LoadAll();
                        _manager.GetQueueData().AddRange(all);

                        IEnumerable<AlertDataDetail> details=repository.LoadAllDetail();
                        _manager.GetQueueAlertDetail().AddRange(details);

                        File.Delete(f);
                        f = FileHelper.GetFirstCreationFile(ctx.FilePath);  
                    }
                    _hasBackupData = false;
                }

            }
            catch (Exception e)
            {
                _logger.Debug("AlertData 数据恢复至DB出错，错误信息：" + e.Message.ToString());
            }
            finally { }
        }

        #endregion

    }
}
