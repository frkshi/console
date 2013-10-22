
using System;
using System.Collections.Generic;
using System.Text;
using DataAccess;
using DataEntity;
using MtuConsole.Common;

namespace MtuConsole.TcpProcess
{

    /// <summary>
    /// 各类数据组件打包
    /// </summary>
    public class RWDatabase
    {
        /// <summary>
        /// logger
        /// </summary>
        private MtuLog _logger;

        private bool _hasremotedb;
        /// <summary>
        /// 标示是否有远端数据库
        /// </summary>
        public bool HasRemoteDB
        {
            get { return _hasremotedb; }
            set { _hasremotedb = value; }
        }

        private MeasureDataManager _remotemeasuredatamanager;
        /// <summary>
        /// 远端measure data
        /// </summary>
        public MeasureDataManager RemoteMeasureDataManager
        {
            get { return _remotemeasuredatamanager; }
            set { _remotemeasuredatamanager = value; }
        }


        private SettingManager _remotesettingmanager;

        /// <summary>
        /// 远端remote data
        /// </summary>
        public SettingManager RemoteSettingManager
        {
            get { return _remotesettingmanager; }
            set { _remotesettingmanager = value; }
        }

        private AlertDataManager _remotealertdatamanger;
        /// <summary>
        /// 远端alert data
        /// </summary>
        public AlertDataManager RemoteAlertDataManager
        {
            get { return _remotealertdatamanger; }
            set { _remotealertdatamanger = value; }
        }

        private MeasureDataManager _localmeasuredatamanager;
        /// <summary>
        /// 本地 measure data
        /// </summary>
        public MeasureDataManager LocalMeasureDataManager
        {
            get { return _localmeasuredatamanager; }
            set { _localmeasuredatamanager = value; }
        }

        private SettingManager _localsettingmanager;
        /// <summary>
        /// 本地setting data
        /// </summary>
        public SettingManager LocalSettingManager
        {
            get { return _localsettingmanager; }
            set { _localsettingmanager = value; }
        }

        private CollectionDataManager _localcollectiondatamanager;
        /// <summary>
        /// 本地collection data
        /// </summary>
        public CollectionDataManager LocalCollectionDataManager
        {
            set { _localcollectiondatamanager = value; }
            get { return _localcollectiondatamanager; }


        }

        private AlertDataManager _localalertdatamanger;
        /// <summary>
        /// 本地alert data
        /// </summary>
        public AlertDataManager LocalAlertDataManager
        {
            set { _localalertdatamanger = value; }
            get { return _localalertdatamanger; }

        }

        private bool _hasoutside;
        /// <summary>
        /// 外部数据库标示
        /// </summary>
        public bool HasOutSide
        {
            get { return _hasoutside; }
            set { _hasoutside = value; }
        }


        private MeasureDataExportManager _outsidemeasuredatamanager;
        /// <summary>
        /// 外部检测量数据库
        /// </summary>
        public MeasureDataExportManager OutSideMeasureDataManager
        {
            get { return _outsidemeasuredatamanager; }
            set { _outsidemeasuredatamanager = value; }
        }


        private AlertDataExportManager _outsidealertmanager;
        /// <summary>
        /// 外部报警数据库
        /// </summary>
        public AlertDataExportManager OutSideAlertManager
        {
            get { return _outsidealertmanager; }
            set { _outsidealertmanager = value; }
        }

        public MtuLogManager MtuLogManager
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RWDatabase()
        {
            _logger = new MtuLog();
        }

        #region 公共数据库读写方法

        /// <summary>
        /// rtusetting update
        /// </summary>
        /// <param name="rs"></param>
        public void UpdateRTUSetting(RTUSetting rs)
        {
            try
            {
                LocalSettingManager.UpdateRTUSetting(rs);

                // 更新远端数据库
                if (HasRemoteDB)
                    RemoteSettingManager.UpdateRTUSetting(rs);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);

            }



        }
        /// <summary>
        /// update measuresetting
        /// </summary>
        /// <param name="ms"></param>
        public void UpdateMeasureSetting(MeasureSetting ms)
        {
            if (ms == null)
            {
                return;
            }
            try
            {
                LocalSettingManager.UpdateMeasureSetting(ms);
                if (HasRemoteDB)
                {
                    RemoteSettingManager.UpdateMeasureSetting(ms);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }

        /// <summary>
        /// 检测量写入
        /// </summary>
        /// <param name="data"></param>
        public void AddToWrite(MeasureData data)
        {
            try
            {
                LocalMeasureDataManager.AddToWrite(data);
                if (HasRemoteDB)
                {

                    RemoteMeasureDataManager.AddToWrite(data);
                }
                if (HasOutSide)
                {

                    OutSideMeasureDataManager.AddToWrite(data);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }


        }

        /// <summary>
        /// insert measure datas
        /// </summary>
        /// <param name="datas"></param>
        public void AddToWrite(List<MeasureData> datas)
        {
            try
            {
                LocalMeasureDataManager.AddToWrite(datas);
                if (HasRemoteDB)
                {
                    RemoteMeasureDataManager.AddToWrite(datas);
                }

                if (HasOutSide)
                {
                    OutSideMeasureDataManager.AddToWrite(datas);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }
        /// <summary>
        /// 报警数据写入
        /// </summary>
        /// <param name="data"></param>
        public void AddToWrite(AlertData data)
        {
            try
            {
                LocalAlertDataManager.AddToWrite(data);
                if (HasRemoteDB)
                {
                    RemoteAlertDataManager.AddToWrite(data);
                }
                if (HasOutSide)
                {
                    OutSideAlertManager.AddToWrite(data);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }


        public void AddToWrite(CollectionData data)
        {
            try
            {
                LocalCollectionDataManager.AddToWrite(data);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }
        }

        /// <summary>
        /// 插入dla 2代定义报警信息，望alertdata 插入一条，往alertdetial 插60个详细数据
        /// </summary>
        /// <param name="datas"></param>
        public void AddToWriteAlertDetail(AlertDataDetail data)
        {
            try
            {
                LocalAlertDataManager.AddToWriteAlertDetail(data);
                if (HasRemoteDB)
                {
                    RemoteAlertDataManager.AddToWriteAlertDetail(data);
                }
                if (HasOutSide)
                {
                    OutSideAlertManager.AddToWriteAlertDetail(data);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }


        }
        public void AddToWriteSecretDoor(SecreatDoor data)
        {
            try
            {
                if (HasRemoteDB)
                {
                    RemoteAlertDataManager.AddToWrite(data);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message, e);
            }

        }

        public bool InsertUnconfiguredRTU(UnConfiguredRTU entity)
        {
            return LocalSettingManager.InsertUnconfiguredRtu(entity);
        }


        public bool InsertRtusetting_Device(Rtusetting_Device entity)
        {
            return LocalSettingManager.InsertRtuSetting_Device(entity);
        }



        #endregion
        /// <summary>
        /// 释放连接,关闭线程
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (HasRemoteDB)
                {
                    if (RemoteMeasureDataManager != null)
                    {
                        RemoteMeasureDataManager.EnsureQueueDataPersisted();
                    }
                    if (RemoteAlertDataManager != null)
                    {
                        RemoteAlertDataManager.EnsureQueueDataPersisted();
                    }

                }

                if (HasOutSide)
                {
                    if (OutSideMeasureDataManager != null)
                    {
                        OutSideMeasureDataManager.EnsureQueueDataPersisted();
                    }

                    if (OutSideAlertManager != null)
                    {
                        OutSideAlertManager.EnsureQueueDataPersisted();
                    }
                }
                LocalCollectionDataManager.EnsureQueueDataPersisted();
                LocalAlertDataManager.EnsureQueueDataPersisted();
                LocalMeasureDataManager.EnsureQueueDataPersisted();
                LocalSettingManager.EnsureQueueDataPersisted();
            }
            catch
            { }

        }

    }
}
