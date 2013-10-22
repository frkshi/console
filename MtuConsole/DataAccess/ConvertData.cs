
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ConvertData
    {
        #region 编码转换（编码规则：十位数字+26位小写字母）
        /// <summary>
        /// 编码转换（编码规则：十位数字+26位小写字母）
        /// </summary>
        /// <param name="num">数字</param>
        /// <param name="minlen">编码最少位数</param>
        /// <param name="maxlen">编码最大位数</param>
        /// <returns>编码</returns>
        public static string ConvertCode36(int num,int minlen,int maxlen)
        {
            string sreturn="";
            int max = 1;
            for (int i = 1; i <= maxlen; i++)
            {
                max = 36 * max;
            }
            while (num >= max)
            { //如果num超过编码最大值，就截取num值。例如：4569 截取 456
                num = int.Parse(num.ToString().Substring(0,num.ToString().Length - 2));
            }
            //转换代码
            sreturn = ConvertCode36(num);
            if (sreturn.Length < minlen)
            {//编码不足位数的补零
                int i = minlen - sreturn.Length;
                for (int j = 1; j <= i; j++)
                {
                    sreturn = "0" + sreturn;
                }
            }
            return sreturn;
        }

        /// <summary>
        /// 编码转换（编码规则：十位数字+26位小写字母）
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>编码</returns>
        public static string ConvertCode36(int num)
        {
            char c;
            int i, m;
            string sreturn = "";
            while (num > 0)
            {
                m = num % 36;
                num = num / 36;
                if (m <= 9)
                {
                    i = m + 48;//48-0 (0的ASCII码为48）
                }
                else
                {
                    i = m + 87;//97-10 (a的ASCII码为97）
                }
                c = (char)i;
                sreturn = c + sreturn;
            }
            return sreturn;
        }
        #endregion

        #region 编码转换（编码规则：十位数字+26位小写字母+26位大写字母）
        
        /// <summary>
        /// 编码转换（编码规则：十位数字+26位小写字母+26位大写字母）
        /// </summary>
        /// <param name="num">数字</param>
        /// <returns>编码</returns>
        public static string ConvertCode62(int num)
        {
            char c;
            int i, m;
            string sreturn = "";
            while (num > 0)
            {
                //m = num % 62;
                //num = num / 62;
                //if (m <= 9)
                //{
                //    i = m + 48;//48-0 (0的ASCII码为48）
                //}
                //else if (m <= 35)
                //{
                //    i = m + 87;//97-10 (a的ASCII码为97）
                //}
                //else
                //{
                //    i = m + 29;//65-36 (Z的ASCII码为65）
                //}
                //注意修改：为dla做的特殊处理， 16进制
                m = num % 16;
                num = num / 16;
                if (m <= 9)
                {
                    i = m + 48;//48-0 (0的ASCII码为48）
                }
                else
                {
                    i = m + 87;//97-10 (a的ASCII码为97）
                }

                c = (char)i;
                sreturn = c + sreturn;
            }
            return sreturn;
        }
        #endregion


    }
}
