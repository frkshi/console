
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class ConvertData
    {
        #region ����ת�����������ʮλ����+26λСд��ĸ��
        /// <summary>
        /// ����ת�����������ʮλ����+26λСд��ĸ��
        /// </summary>
        /// <param name="num">����</param>
        /// <param name="minlen">��������λ��</param>
        /// <param name="maxlen">�������λ��</param>
        /// <returns>����</returns>
        public static string ConvertCode36(int num,int minlen,int maxlen)
        {
            string sreturn="";
            int max = 1;
            for (int i = 1; i <= maxlen; i++)
            {
                max = 36 * max;
            }
            while (num >= max)
            { //���num�����������ֵ���ͽ�ȡnumֵ�����磺4569 ��ȡ 456
                num = int.Parse(num.ToString().Substring(0,num.ToString().Length - 2));
            }
            //ת������
            sreturn = ConvertCode36(num);
            if (sreturn.Length < minlen)
            {//���벻��λ���Ĳ���
                int i = minlen - sreturn.Length;
                for (int j = 1; j <= i; j++)
                {
                    sreturn = "0" + sreturn;
                }
            }
            return sreturn;
        }

        /// <summary>
        /// ����ת�����������ʮλ����+26λСд��ĸ��
        /// </summary>
        /// <param name="num">����</param>
        /// <returns>����</returns>
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
                    i = m + 48;//48-0 (0��ASCII��Ϊ48��
                }
                else
                {
                    i = m + 87;//97-10 (a��ASCII��Ϊ97��
                }
                c = (char)i;
                sreturn = c + sreturn;
            }
            return sreturn;
        }
        #endregion

        #region ����ת�����������ʮλ����+26λСд��ĸ+26λ��д��ĸ��
        
        /// <summary>
        /// ����ת�����������ʮλ����+26λСд��ĸ+26λ��д��ĸ��
        /// </summary>
        /// <param name="num">����</param>
        /// <returns>����</returns>
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
                //    i = m + 48;//48-0 (0��ASCII��Ϊ48��
                //}
                //else if (m <= 35)
                //{
                //    i = m + 87;//97-10 (a��ASCII��Ϊ97��
                //}
                //else
                //{
                //    i = m + 29;//65-36 (Z��ASCII��Ϊ65��
                //}
                //ע���޸ģ�Ϊdla�������⴦�� 16����
                m = num % 16;
                num = num / 16;
                if (m <= 9)
                {
                    i = m + 48;//48-0 (0��ASCII��Ϊ48��
                }
                else
                {
                    i = m + 87;//97-10 (a��ASCII��Ϊ97��
                }

                c = (char)i;
                sreturn = c + sreturn;
            }
            return sreturn;
        }
        #endregion


    }
}
