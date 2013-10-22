using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionLib
{
    public static class CheckDigitHelper
    {
        /// <summary>
        /// convert string to BCC code
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string ConvertToBcc(this string data)
        {
            int dataLength = data.Length;
            int sum = 0;
            string tmpStr = String.Empty;
            char tmpChar;
            int tmpInt;
            for (int i = 0; i < dataLength; i++)
            {
                tmpStr = data.Substring(i, 1);
                tmpChar = Convert.ToChar(tmpStr);
                tmpInt = Convert.ToInt32(tmpChar);
                sum += tmpInt;
            }
            string sum16Str = Convert.ToString(sum, 16);
            if (sum > 255)
            {
                sum16Str = sum16Str.Substring(sum16Str.Length - 2);
            }

            string sum2Str = sum16Str.ConvertFrom16To2();
            //把2进制取反
            string reverseStr = String.Empty;
            for (int i = 0; i < sum2Str.Length; i++)
            {
                reverseStr += Convert.ToString(int.Parse(sum2Str[i].ToString()) ^ 1);
            }

            //转化成10进制
            string sum10Str = reverseStr.ConvertFrom2().ToString();
            long sumLong = Convert.ToInt64(sum10Str) + 1;
            sum16Str = Convert.ToString(sumLong, 16);
            if (sum16Str.Length == 1)
                sum16Str = "0" + sum16Str;
            sum16Str = sum16Str.Substring(sum16Str.Length - 2, 2);
            return sum16Str;
        }

        public static string ConvertToRCC(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return "00";

            int sum = 0;
            foreach (char c in data)
            {
                sum += (int)c;
            }

            string sSum = sum.ConvertTo16().ToString().ToUpper();

            return sSum.Substring(sSum.Length - 2, 2);
        }
    }
}
