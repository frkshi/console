using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Management;

namespace DataAccess.Helpers
{
    internal class FileDiskHelper
    {
        /// <summary>
        /// 确保足够的磁盘空间
        /// </summary>
        /// <param name="fullFileName">完整路径</param>
        public static void EnsureDiskSapceEnough(string fullFileName , int freeSpace)
        {
            string diskName = fullFileName.Substring(0, 2);
            string path = FileHelper.GetFilePath(fullFileName);
            if (ExistFile(path))
            {
                while (FileDiskHelper.GetFreeSpace(diskName) < (long)(freeSpace) * 1024 * 1024)
                {
                    FileDiskHelper.DeleteFirstCreatedFile(path);
                    if (!ExistFile(path))
                        break;
                }
            }
        }

        /// <summary>
        /// 返回可用磁盘空间字节数
        /// </summary>
        /// <param name="disk">磁盘名称D</param>
        /// <returns>磁盘空间字节数</returns>
        private static long GetFreeSpace(string diskName)
        {
            return new DriveInfo(diskName).AvailableFreeSpace;
            //ManagementObject disk = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}\"", diskName));
            //return (long)disk["FreeSpace"];
        }

        /// <summary>
        /// 删除第一个文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool型</returns>
        private static bool DeleteFirstCreatedFile(string filePath)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(filePath);
                FileInfo[] files = d.GetFiles();

                DateTime minTime = DateTime.Now;
                FileInfo firstFile = null;
                foreach (var f in files)
                {
                    if (f.CreationTime < minTime)
                    {
                        firstFile = f;
                        minTime = f.CreationTime;
                    }
                }
                firstFile.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 是否存在早于1个月以前的文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>bool型</returns>
        private static bool ExistFile(string filePath)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(filePath);
                FileInfo[] files = d.GetFiles();

                DateTime minTime = DateTime.Now.AddMonths(-1);
                foreach (var f in files)
                {
                    if (f.CreationTime < minTime)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
