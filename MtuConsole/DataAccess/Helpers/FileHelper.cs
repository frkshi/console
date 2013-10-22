using System;
using System.Collections.Generic;
using System.Text;
using DataEntity;
using System.IO;

namespace DataAccess.Helpers
{
    internal static class FileHelper
    {
        /// <summary>
        /// 根据拆分单元获取完整文件路径
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="extension">后缀</param>
        /// <param name="splitUnit">拆分单元</param>
        /// <returns>完整文件路径</returns>
        public static string GetFullFileNameBySplitUnit(string path, string extension, FileSplitUnit splitUnit)
        {
            string fileName = string.Empty;

            switch (splitUnit)
            {
                case FileSplitUnit.Month:
                    fileName = DateTime.Now.ToString("yyyyMM");
                    break;
                case FileSplitUnit.Day:
                    fileName = DateTime.Now.ToString("yyyyMMdd");
                    break;
                case FileSplitUnit.Hour:
                    fileName = DateTime.Now.ToString("yyyyMMddHH");
                    break;
                default:
                    break;
            }
            return path + fileName + "." + extension;
        }


        /// <summary>
        /// 返回日期范围内的文件名集合，以天分格
        /// </summary>
        /// <param name="begintime"></param>
        /// <param name="endtime"></param>
        /// <returns></returns>
        public static List<string> GetFileNameByDateRange(DateTime begintime, DateTime endtime)
        {
            List<string> result = new List<string>();
            string filename = string.Empty;

            DateTime filedate = begintime;

            while (filedate.Date <= endtime.Date)
            {
                result.Add(filedate.ToString("yyyyMMdd"));

               filedate= filedate.AddDays(1);
            }



            return result;
        }
        /// <summary>
        /// 获取最早文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>最早文件名(包括完整路径和后缀)</returns>
        public static string GetFirstCreationFile(string filePath)
        {
            string result = string.Empty;

            DirectoryInfo di = new DirectoryInfo(filePath);

            FileInfo[] files = di.GetFiles();

            if (files.Length > 0)
            {
                FileInfo candidate = files[0];

                foreach (var f in files)
                {
                    if (f.CreationTime < candidate.CreationTime)
                    {
                        candidate = f;
                    }
                }
                result = candidate.FullName;
            }

            return result;
        }
        
        /// <summary>
        /// 获取文件路径
        /// </summary>
        /// <param name="fullFileName">完整文件名</param>
        /// <returns>文件路径</returns>
        public static string GetFilePath(string fullFileName)
        {
            if (string.IsNullOrEmpty(fullFileName) || fullFileName.IndexOf("\\") == -1)
            {
                throw new ArgumentException("Invalid full file name!");
            }

            int pos = fullFileName.LastIndexOf("\\");
            string path = fullFileName.Substring(0, pos + 1);
            return path;
        }

        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        /// <param name="folderFullPath">文件夹路径</param>
        public static void EnsureFolderExist(string folderFullPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderFullPath);
            if (!di.Exists)
            {
                di.Create();
            }
        }

        /// <summary>
        /// 修正文件路径
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static string ReviseFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || filePath.IndexOf("\\") == -1)
            {
                throw new ArgumentException("Invalid full filePath!");
            }
            if (!filePath.EndsWith("\\"))
            {
                filePath += "\\";
            }
            return filePath;
        }

        /// <summary>
        /// 从connectstring 获取file path
        /// </summary>
        /// <param name="conncetstr"></param>
        /// <returns></returns>
        public static string GetFilePathFromConnectStr(string conncetstr)
        {
            string result="";

            //data source=D:\testsqlite\DLADLE_V2\CollectionData\20110401.db3;version=3;default timeout=120

            try
            {
                result = conncetstr.Substring(conncetstr.IndexOf('=')+1, conncetstr.IndexOf(';') - conncetstr.IndexOf('='));
                result=result.Substring(0,result.LastIndexOf('\\')+1);
                char[] c=new char[] {'"'};
                result = result.TrimStart(c);
            }
            catch
            { 
            
            }
            return result;
        }
        /// <summary>
        /// 根据文件筛选选择文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileMonthFilter">文件筛选</param>
        /// <returns></returns>
        public static List<string> GetFileName(string filePath, int fileMonthFilter)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] files = di.GetFiles("*.db3");

            DateTime now = DateTime.Now;

            List<string> fileRange = new List<string>();
            for (int i = 0; i < fileMonthFilter; i++)
            {
                fileRange.Add(now.AddMonths(-fileMonthFilter).ToString("yyyyMM"));
            }

            List<string> filename = new List<string>();
            for (int j = 0; j < files.Length; j++ )
            {
                if (fileRange.Exists(delegate(string name) { return name == files[j].Name.Substring(0, 6); }))
                {
                    filename.Add(files[j].Name);
                }
            }
            return filename;
        }

        /// <summary>
        /// 根据条件筛选选择文件名
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="filteExpression">文件名起始字符筛选</param>
        /// <returns></returns>
        public static List<string> GetFileName(string filePath, string filteExpression)
        {
            DirectoryInfo di = new DirectoryInfo(filePath);
            FileInfo[] files = di.GetFiles("*.db3");
            List<string> filename = new List<string>();
            for (int i = 0; i < files.Length; i++)
            {
                if (files[i].Name.StartsWith(filteExpression))
                {
                    filename.Add(files[i].Name);
                }
            }
            return filename;
        }
    }
}
