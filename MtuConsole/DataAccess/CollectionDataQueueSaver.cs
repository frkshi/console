using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DataEntity;

namespace DataAccess
{
    internal class CollectionDataQueueSaver
    {
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
        /// 采集数据存储管理器
        /// </summary>
        private CollectionDataManager _manager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="manager">采集数据存储管理器</param>
        public CollectionDataQueueSaver(CollectionDataManager manager)
        {
            _manager = manager;

            _workerThread = new Thread(new ThreadStart(this.DoWork));

            _workFlag = true;
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

        /// <summary>
        /// 线程处理
        /// </summary>
        private void DoWork()
        {
            while (_workFlag)
            {
                try
                {
                    List<CollectionData> queue = _manager.GetQueueData();
                    int queueCount = queue.Count;

                    if (queueCount > 0)
                    {
                        CollectionData[] temp = new CollectionData[queueCount];
                        queue.CopyTo(0, temp, 0, queueCount);
                        queue.RemoveRange(0, queueCount);
                        this.SaveCollectionData(temp);
                    }

                    List<SendData> sendQueue = _manager.GetSendQueueData();
                    queueCount = sendQueue.Count;
                    if (queueCount > 0)
                    {
                        SendData[] tempsend = new SendData[queueCount];
                        sendQueue.CopyTo(0, tempsend, 0, queueCount);
                        sendQueue.RemoveRange(0, queueCount);
                        this.SaveSendData(tempsend);

                    }

                    Thread.Sleep(_workDuration);
                }
                catch (Exception e)
                { 
                    
                }
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="items">采集数据</param>
        private void SaveCollectionData(IEnumerable<CollectionData> items)
        {
            _manager.Context.GetRepository().BulkInsert(items);

             
        }

        private void SaveSendData(IEnumerable<SendData> items)
        {
            _manager.Context.GetRepository().BulkInsert(items);
        }
        

    }
}
