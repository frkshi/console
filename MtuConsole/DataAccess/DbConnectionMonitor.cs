using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using DataAccess.Interfaces;

namespace DataAccess
{
    internal class DbConnectionMonitor
    {

        #region Private Fields

        /// <summary>
        /// 检测间隔
        /// </summary>
        private int _dbConnectionDetectingPeriod;

        #endregion

        #region Public Properties
        
        /// <summary>
        /// 监测开关
        /// </summary>
        public bool DbConnectionDetectingStarted { get; private set; }

        /// <summary>
        /// 是否继续监测
        /// </summary>
        public bool ContinueDbConnectionDetecting { get; private set; }

        internal ICheckConnection Target
        {
            get;
            set;
        }

        #endregion

        #region Event Requires

        /// <summary>
        /// 事件处理
        /// </summary>
        public event EventHandler<EventArgs> ConnectionOk;

        protected virtual void OnConnectionOk(EventArgs args)
        {
            EventHandler<EventArgs> temp = this.ConnectionOk;
            if (temp != null)
            {
                temp(this, args);
            }
        }

        #endregion

        #region Constructors

       /// <summary>
       /// 构造函数
       /// </summary>
       /// <param name="repository">存储策略</param>
        public DbConnectionMonitor(ICheckConnection repository)
        {
            this.Target = repository;

            this.DbConnectionDetectingStarted = false;
            this.ContinueDbConnectionDetecting = false;
            _dbConnectionDetectingPeriod = SubDbMonitor.DBDetectingPeriod;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 启动数据库连接监测
        /// </summary>
        public void StartDbConnectionDetecting()
        {
            if (!this.DbConnectionDetectingStarted)
            {
                _dbConnectionDetectingPeriod = SubDbMonitor.DBDetectingPeriod;

                this.ContinueDbConnectionDetecting = true;

                ThreadPool.QueueUserWorkItem(new WaitCallback(this.DetectDbConnection));

                this.DbConnectionDetectingStarted = true;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 检测数据库连接情况
        /// </summary>
        /// <param name="state">数据库连接状态</param>
        private void DetectDbConnection(object state)
        {
            while (this.ContinueDbConnectionDetecting)
            {
                Thread.Sleep(_dbConnectionDetectingPeriod);
                //System.Diagnostics.EventLog.WriteEntry("Detecting" + "," + SubDbMonitor.DBErrorCount + "," + _dbConnectionDetectingPeriod, string.Empty);

                if (this.Target.CheckConnection())
                {
                    this.OnConnectionOk(EventArgs.Empty);

                    //System.Diagnostics.EventLog.WriteEntry("ConnectionOk", string.Empty);

                    this.ContinueDbConnectionDetecting = false;
                    this.DbConnectionDetectingStarted = false;

                }

            }
        }

        #endregion

    }

    internal static class SubDbMonitor {
        
        private static int _dbErrorCount = 0;
        private static int _dbDetectingPeriod = 1000 * 60 * 1;//1分


        public static int DBErrorCount
        {
            get { return _dbErrorCount; }
            set
            {
                if (_dbErrorCount >= 3)
                    _dbDetectingPeriod = 1000 * 60 * 15; //15分
                else
                    _dbDetectingPeriod = 1000 * 60 * 1; // 1分
                _dbErrorCount = value;
            }
        }

        public static int DBDetectingPeriod
        {
            get { return _dbDetectingPeriod; }
        }
    }
}
