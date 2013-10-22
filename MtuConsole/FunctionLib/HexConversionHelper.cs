using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionLib
{
    /// <summary>
    /// 针对Int32，string之间提供快捷的进制之间转换的扩展方法
    /// </summary>
    public static class HexConversionHelper
    {
        #region << 2 to x >>

        /// <summary>
        /// 2 to 16
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertFrom2To16(this string source)
        {
            return source.ConvertFrom2().ConvertTo16();
        }

        /// <summary>
        /// 2 to 10
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ConvertFrom2To10(this string source)
        {
            return Convert.ToInt32(source, 2);
        }

        /// <summary>
        /// 2 to 8
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertFrom2To8(this string source)
        {
            return source.ConvertFrom2().ConvertTo8();
        }

        #endregion

        #region << Int32, 10 to x >>

        /// <summary>
        /// 10 to 2
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo2(this Int32 source)
        {
            return Convert.ToString(source, 2);
        }

        /// <summary>
        /// 10 to 8
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo8(this Int32 source)
        {
            return Convert.ToString(source, 8);
        }

        /// <summary>
        /// 10 to 16
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo16(this Int32 source)
        {
            return Convert.ToString(source, 16);
        }

        /// <summary>
        /// 10 to 62
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo62(this Int32 source)
        {
            return new SixtyTwoScale(source).ToString();
        }

        public static string ConvertToDateFrom62(this string source)
        {
            string result = string.Empty;
            if (source.Length == 6)
            {
                string stime = "20{0}-{1}-{2} {3}:{4}:{5}";

                result = string.Format(stime, source.Substring(0, 1).ConvertFrom62().ToString().PadLeft(2, '0'), source.Substring(1, 1).ConvertFrom62().ToString().PadLeft(2, '0'), source.Substring(2, 1).ConvertFrom62().ToString().PadLeft(2, '0'), source.Substring(3, 1).ConvertFrom62().ToString().PadLeft(2, '0'), source.Substring(4, 1).ConvertFrom62().ToString().PadLeft(2, '0'), source.Substring(5, 1).ConvertFrom62().ToString().PadLeft(2, '0'));
            }

            return result;
        }

        public static string FormatTimeTo62(DateTime TheTime)
        {
            string Tmpstr = "";
            string result = "";
            try
            {
                Tmpstr = TheTime.ToString("yy");
                result += int.Parse(Tmpstr).ConvertTo62();
                Tmpstr = TheTime.ToString("MM");
                result += int.Parse(Tmpstr).ConvertTo62();
                Tmpstr = TheTime.ToString("dd");
                result += int.Parse(Tmpstr).ConvertTo62();
                Tmpstr = TheTime.ToString("HH");
                result += int.Parse(Tmpstr).ConvertTo62();
                Tmpstr = TheTime.ToString("mm");
                result += int.Parse(Tmpstr).ConvertTo62();
                Tmpstr = TheTime.ToString("ss");
                result += int.Parse(Tmpstr).ConvertTo62();
            }
            catch (Exception)
            {
                result = "000000";
            }
            return result;
        }
        /// <summary>
        /// float to 16
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertFloatToHex(this decimal source)
        {
            string result = "";

            try
            {
                uint uintValue = BitConverter.ToUInt32(BitConverter.GetBytes((float)source), 0);
                byte[] byteValue = BitConverter.GetBytes(uintValue);
                Array.Reverse(byteValue);
                result = BitConverter.ToString(byteValue).Replace("-", "");
            }
            catch
            {
                result = "FFFFFFFF";
            }

            return result;
        }
        #endregion

        #region << Int32, 10 to x >>

        /// <summary>
        /// 10 to 2
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo2(this Int16 source)
        {
            return Convert.ToString(source, 2);
        }

        /// <summary>
        /// 10 to 8
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo8(this Int16 source)
        {
            return Convert.ToString(source, 8);
        }

        /// <summary>
        /// 10 to 16
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertTo16(this Int16 source)
        {
            return Convert.ToString(source, 16);
        }

        #endregion

        /// <summary>
        /// 62 to 10
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Int64 ConvertFrom62(this string source)
        {
            return new SixtyTwoScale(source).ToInt64();
        }

        #region << Int32 x to 10 >>

        /// <summary>
        /// 2 to 10
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ConvertFrom2(this string source)
        {
            return Convert.ToInt32(source, 2);
        }

        /// <summary>
        /// 8 to 10
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ConvertFrom8(this string source)
        {
            return Convert.ToInt32(source, 8);
        }

        /// <summary>
        /// 16 to 10
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int ConvertFrom16(this string source)
        {
            try
            {
                return Convert.ToInt32(source, 16);
            }
            catch
            {
                return -1;
            }
        }

        #endregion

        #region << Int16 x to 10 >>

        public static Int16 From2ForInt16(this string source)
        {
            return Convert.ToInt16(source, 2);
        }

        public static Int16 From8ForInt16(this string source)
        {
            return Convert.ToInt16(source, 8);
        }

        public static Int16 From16ForInt16(this string source)
        {
            return Convert.ToInt16(source, 16);
        }

        #endregion

        #region << 16 to x >>

        /// <summary>
        /// 16 to 2
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ConvertFrom16To2(this string source)
        {
            return source.ConvertFrom16().ConvertTo2();
        }

        #endregion

        /// <summary>
        /// 日期转换为62进制字符串
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string ConvertFromDataTime(this DateTime data)
        {
            string format = "{0}{1}{2}{3}{4}{5}";
            string year = (data.Year - 2000) > 0 ? (data.Year - 2000).ConvertTo62() : "0";

            return string.Format(format, year, data.Month.ConvertTo62(),
                data.Day.ConvertTo62(), data.Hour.ConvertTo62(),
                data.Minute.ConvertTo62(), data.Second.ConvertTo62());
        }

        /// <summary>
        /// Hex to Float,for ex. 3ED48D3B
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float ConvertHexToFloat(this string source)
        {
            try
            {
                uint num = uint.Parse(source, System.Globalization.NumberStyles.AllowHexSpecifier);
                byte[] floatVals = BitConverter.GetBytes(num);
                return BitConverter.ToSingle(floatVals, 0);
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// Hex to Double, for ex. 00000000409AB800
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double ConvertHexToDouble(this string source)
        {

            Int64 num = Int64.Parse(source, System.Globalization.NumberStyles.AllowHexSpecifier);
            byte[] byteVals = BitConverter.GetBytes(num);

            return BitConverter.ToDouble(byteVals, 0);
        }

    }
}
