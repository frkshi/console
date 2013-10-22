using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using DataAccess.Interfaces;

namespace DataAccess
{
    public class CollectionDataManager
    {
        /// <summary>
        /// 采集数据队列
        /// </summary>
        private List<CollectionData> _collectionqueue;

        /// <summary>
        /// 发送数据队列
        /// </summary>
        private List<SendData> _sendQueue;


        /// <summary>
        /// 数据队列存储器
        /// </summary>
        private CollectionDataQueueSaver _saver;

        internal ICollectionDataPersistenceContext Context { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="target">本地sqlite存储参数</param>
        public CollectionDataManager(SqliteCollectionDataPersistenceContext context)
        {
            if (context == null)
            {
                this.Context = new SqliteCollectionDataPersistenceContext(AppDomain.CurrentDomain.BaseDirectory, FileSplitUnit.Hour, UtilityParameters.DefaultFreeSpace);                    
            }
            else
            {
                this.Context = context;
            }

            _collectionqueue = new List<CollectionData>();
            _sendQueue = new List<SendData>();

            _saver = new CollectionDataQueueSaver(this);
            _saver.StartWork();
        }

        /// <summary>
        /// 保存单个采集量
        /// </summary>
        /// <param name="entity">单个采集量</param>
        public void AddToWrite(CollectionData entity)
        {
            _collectionqueue.Add(entity);
        }
        /// <summary>
        /// 保存单个发送数据
        /// </summary>
        /// <param name="entity"></param>
        public void AddToWrite(SendData entity)
        {
            _sendQueue.Add(entity);
        }


        /// <summary>
        /// 保存多个采集量
        /// </summary>
        /// <param name="entities">采集量集合</param>
        public void AddToWrite(IEnumerable<CollectionData> entities)
        {
            _collectionqueue.AddRange(entities);
        }
        /// <summary>
        /// 保存多个发送数据
        /// </summary>
        /// <param name="entities"></param>
        public void AddToWrite(IEnumerable<SendData> entities)
        {
            _sendQueue.AddRange(entities);
        }
        /// <summary>
        /// 获取源码文件信息
        /// </summary>
        /// <returns></returns>
        public List<CollectionFile> LoadFileInfo()
        {
            IRedoCollectionData  redoCD =  this.Context.GetRepository() as IRedoCollectionData;
            List<CollectionFile> cfList = new List<CollectionFile>();
            if (redoCD != null)
            {
                cfList = redoCD.LoadFileInfo();
            }
            return cfList;
        }

        /// <summary>
        /// 获取某文件中未解码的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>未解码的记录</returns>
        public List<CollectionData> LoadUnDecodeData(string fileName)
        {
            IRedoCollectionData redoCD = this.Context.GetRepository() as IRedoCollectionData;
            List<CollectionData> cdList = new List<CollectionData>();
            if (redoCD != null)
            {
                cdList = redoCD.LoadUnDecodeData(fileName);
            }
            return cdList;
        }


        /// <summary>
        /// 获取时间范围内，rtu列表下，上下行源包数据，按时间排列
        /// </summary>
        /// <param name="rtus"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public List<SourceCodeData> LoadSourceCode(string[] rtus, DateTime begintime, DateTime endtime)
        {
            List<SourceCodeData> result = new List<SourceCodeData>();
            
            IRedoCollectionData redoCD = this.Context.GetRepository() as IRedoCollectionData;
            result = redoCD.LoadSourceCode(rtus, begintime, endtime);
            
            return result;
        }
        /// <summary>
        /// 更新某文件中的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="noteId">编号</param>
        /// <param name="status">状态</param>
        /// <returns>Bool型</returns>
        public bool Update(string fileName, string noteId, string status)
        {
            IRedoCollectionData redoCD = this.Context.GetRepository() as IRedoCollectionData;
            return redoCD.Update(fileName, noteId, status);
        }

        /// <summary>
        /// 线程结束命令，调用该方法确保队列中的数据持久化到数据库
        /// </summary>
        public void EnsureQueueDataPersisted()
        {
            _saver.SetWorkFlag(false);
            _saver.GetWorkerThread().Join();
        }

        internal List<CollectionData> GetQueueData()
        {
            return _collectionqueue;
        }
        internal List<SendData> GetSendQueueData()
        {
            return _sendQueue;
        }
    }
}
