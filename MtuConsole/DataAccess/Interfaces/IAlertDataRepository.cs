using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    public interface IAlertDataRepository
    {
        /// <summary>
        /// 新增单个实体
        /// </summary>
        /// <param name="entity">报警数据实体</param>
        /// <returns>bool型</returns>
        bool Insert(AlertData entity);

        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entities">报警数据实体集合</param>
        /// <returns>bool型</returns>
        bool BulkInsert(IEnumerable<AlertData> entities);
        /// <summary>
        /// 插入alertdetail 数据，望alertdata表和alertdetail表分别插入主从数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        bool InsertAlertDetail(AlertDataDetail[] datas);

        bool InsertAlertDetail(AlertDataDetail[] datas, out List<AlertDataDetail> leftdatas);
        /// <summary>
        /// 后门信息
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        bool InsertSecretDoor(SecreatDoor[] datas);
    }
}
