using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;

namespace DataAccess.Interfaces
{
    public interface IRedoCollectionData
    {
        /// <summary>
        /// 获取源码文件信息
        /// </summary>
        /// <returns></returns>
        List<CollectionFile> LoadFileInfo();

        /// <summary>
        /// 获取某文件中未解码的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>未解码的记录</returns>
        List<CollectionData> LoadUnDecodeData(string fileName);

        /// <summary>
        /// 获取指定文件，指定时间，指定rtus的上下行数据
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="rtus"></param>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        List<SourceCodeData> LoadSourceCode( string[] rtus, DateTime begintime, DateTime endtime);
        /// <summary>
        /// 更新某文件中的记录
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="noteId">编号</param>
        /// <param name="status">状态</param>
        /// <returns>Bool型</returns>
        bool Update(string fileName, string noteId, string status);
    }
}
