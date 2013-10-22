using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Helpers;
using DataEntity;

namespace DataAccess
{
    public class TextCollectionDataPersistenceContext : ICollectionDataPersistenceContext
    {
        public string FilePath { get; set; }                            //文件路径
        public FileSplitUnit FileSplitUnit { get; private set; }        //拆分单元
        public int FreeSpace { get; set; }                              //磁盘可用空间(M)

        /// <summary>
        /// 报警量数据文本文件持久化参数构造函数
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="splitUnit">拆分单元</param>
        /// <param name="freeSpace">磁盘可用空间(M)</param>
        public TextCollectionDataPersistenceContext(string filePath, FileSplitUnit splitUnit, int freeSpace)
        {
            this.FilePath = FileHelper.ReviseFilePath(filePath);
            this.FileSplitUnit = splitUnit;
            this.FreeSpace = freeSpace;
        }

        #region ICollectionDataPersistenceContext Members

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <returns>存储策略</returns>
        public ICollectionDataRepository GetRepository()
        {
            string fullFileName = FileHelper.GetFullFileNameBySplitUnit(this.FilePath, "txt", this.FileSplitUnit);

            FileDiskHelper.EnsureDiskSapceEnough(fullFileName, this.FreeSpace);

            return this.GetRepository(fullFileName);
        }

        /// <summary>
        /// 获取存储策略
        /// </summary>
        /// <param name="prerequisite">DB 文件完整路径</param>
        /// <returns>存储策略</returns>
        public ICollectionDataRepository GetRepository(string prerequisite)
        {
            return new Text.TextCollectionDataRepository(prerequisite);
        }

        #endregion
    }
}
