using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace MtuConsole.Common
{
    /// <summary>
    /// 测试用
    /// </summary>
    public static class CommonMemory
    {
        private static int _count;
        public static void AddCount(int i)
        {
            _count = _count + i;
        }
        public static int Count
        {
            get
            {
                return _count;
            }
        }
    }
    /// <summary>
    /// 公用方法
    /// </summary>
    public static class CommonMethod
    {


        #region << Hex To String >>

        /// <summary>
        /// Hex To String
        /// </summary>
        /// <param name="mHex"></param>
        /// <returns></returns>
        public static string HexToString(String source)
        {
            try
            {
                source = source.Replace(" ", "");
                if (source.Length <= 0) return "";
                byte[] vBytes = new byte[source.Length / 2];
                for (int i = 0; i < source.Length; i += 2)
                    if (!byte.TryParse(source.Substring(i, 2), NumberStyles.HexNumber, null, out vBytes[i / 2]))
                        vBytes[i / 2] = 0;
                return ASCIIEncoding.Default.GetString(vBytes);
            }
            catch { return ""; }
        }
        #endregion

        /// <summary>
        /// 转string to int，无法转的返0
        /// </summary>
        /// <param name="inputStr"></param>
        /// <returns></returns>
        public static int ToInt(string inputStr)
        {
            int result;
            try
            {
                result = Convert.ToInt32(inputStr);
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public static bool ToBool(string inputStr)
        {
            if (inputStr.ToLower() == "false" || inputStr == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// convert to bool
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IntStrToBool(string str)
        {

            try
            {
                return Convert.ToBoolean(Convert.ToInt16(str));


            }
            catch
            {
                return false;
            }

        }



        /// <summary>
        /// 判断是否Int
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isInt(string str)
        {
            try
            {
                Convert.ToInt32(str);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Check is Datetime
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsDateTime(string str)
        {
            try
            {
                Convert.ToDateTime(str);
            }
            catch
            {
                return false;
            }
            return true;
        }


        public static bool IsNumeric(string str)
        {
            bool result = false;
            try
            {
                Convert.ToDouble(str);
                result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public static decimal ConvertToDecimal(string inputstr, int inputprecision)
        {
            decimal result;
            try
            {
                result = Convert.ToDecimal(Convert.ToDouble(inputstr));
                result = decimal.Round(result, inputprecision);

            }
            catch
            {
                result = 0;
            }

            return result;

        }
        /// <summary>
        /// 根据dll名和类名返回对象
        /// </summary>
        /// <param name="sDllName">dll名</param>
        /// <param name="sObjectName">类名</param>
        /// <returns></returns>
        public static object CreatePkgObject(string sDllName, string sObjectName)
        {
            // 获得package
            Assembly assmble;
            try
            {

                assmble = System.Reflection.Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory.ToString() + sDllName);

            }
            catch (Exception e)
            {

                return null;
            }

            // 创建对象
            Object ret = null;
            try
            {

               // Type[] type = assmble.GetTypes();
                Type t = assmble.GetType(sObjectName);
                ret = assmble.CreateInstance(sObjectName);
                //object o = Activator.CreateInstance(type[1]);
            }
            catch(Exception e)
            {
                ret = null;
            }

            if (ret == null)
            {
                //error log
            }

            return ret;
        }
    }
}
