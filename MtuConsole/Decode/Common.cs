using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using FunctionLib;
namespace Decode
{
    /// <summary>
    /// 信息类型
    /// </summary>
    public enum InfoType
    { 
        /// <summary>
        /// 未知
        /// </summary>
        None,
        /// <summary>
        /// 数据信息
        /// </summary>
        Data,
        /// <summary>
        /// 报警信息
        /// </summary>
        Alert,
        /// <summary>
        /// 系统信息
        /// </summary>
        System,
        /// <summary>
        /// 参数设定
        /// </summary>
        ParameterSet,
        /// <summary>
        /// 参数查询
        /// </summary>
        ParameterQuery,
        /// <summary>
        /// 后门信息
        /// </summary>
        SecretDoor


    }

    /// <summary>
    /// 数据类型 
    /// </summary>
     public enum DataTypeDatalog
    {
        /// <summary>
        /// 未知
        /// </summary>
        None,

        /// <summary>
        ///系统信息
        /// </summary>
        D_000,

        /// <summary>
        ///GPRS信息
        /// </summary>
        D_010,

        /// <summary>
        ///周期设定
        /// </summary>
        D_030,

        /// <summary>
        ///系统时间
        /// </summary>
        D_050,
         /// <summary>
         /// 当前温度
         /// </summary>
         D_080,
         /// <summary>
         /// 回差值
         /// </summary>
         D_090,
         
        /// <summary>
        ///系统运行状态
        /// </summary>
        D_200,
        
        /// <summary>
        /// 硬件功能调试
        /// </summary>
        D_220,
        
        /// <summary>
        /// 报警使能位
        /// </summary>
        D_240,
        /// <summary>
        /// 上下限值，前4个字节为上限值，后4个字节是下限值
        /// </summary>
        D_250,
        /// <summary>
        /// 突升突降变化率值，前4个字节为突升变化率值，后4个字节是突降变化率值
         /// </summary>
        D_260,
        /// <summary>
        ///场强 + 电池余量
        /// </summary>
        D_150,
        /// <summary>
        ///压力值
        /// </summary>
        D_180,
     
        /// <summary>
        ///压力上限报警
        /// </summary>
        D_111,

        /// <summary>
        ///压力下限报警
        /// </summary>
        D_113,

        /// <summary>
        ///压力突升报警
        /// </summary>
        D_115,

        /// <summary>
        ///压力突降报警
        /// </summary>
        D_117,
         /// <summary>
         /// 压力上限消警
         /// </summary>
         D_112,
         /// <summary>
         /// 压力下限消警
         /// </summary>
         D_114,
         /// <summary>
         /// 压力突升消警
         /// </summary>
         D_116,
         
         /// <summary>
         /// 压力突降消警
         /// </summary>
         D_118,
         /// <summary>
         /// 模块log
         /// </summary>
         D_230
         
     
        


    }
     /// <summary>
     /// DLA-P10-2设备类型:001,002,003
     /// </summary>
     public enum ProductType
     {
         DLA_P10_2_030,
         DLA_P10_2_033,
         DLA_P10_2_032,
         DLA_P10_2_031,
         DLA_P10_2_035,
         DLA_P10_2_036,
         DLA_P10_2_037,
         DLA_P10_2_008,
         DLA_P10_2_009,
         None
     }

   public static class Common
    {
       public static DateTime ConvertToDatetime(string data)
       {
           DateTime result = Convert.ToDateTime("1900-01-01 00:00:00");
           if (data.Length== 6)
           {
               string datestring;
               datestring = (2000 + (int)data.Substring(0, 1).ConvertFrom62()).ToString();
               datestring += "-" + data.Substring(1, 1).ConvertFrom62().ToString();
               datestring += "-" + data.Substring(2, 1).ConvertFrom62().ToString();
               datestring += " " + data.Substring(3, 1).ConvertFrom62().ToString();
               datestring += ":" + data.Substring(4, 1).ConvertFrom62().ToString();
               datestring += ":" + data.Substring(5, 1).ConvertFrom62().ToString();
               result = Convert.ToDateTime(datestring);



           }
            return result;
       }
       public static InfoType ConvertToInfoType(string data)
       {
           InfoType result = InfoType.None;
           switch (data)
           {
               case "*":
                   result = InfoType.Data;
                   break;
               case "$":
                   result = InfoType.System;
                   break;
               case "!":
                   result = InfoType.Alert;
                   break;
               case "&":
                   result = InfoType.ParameterSet;
                   break;
               case "?":
                   result = InfoType.ParameterQuery;
                   break;
               case "~":
                   result = InfoType.SecretDoor;
                   break;
               default:
                   break;


           }

           return result;
       
       }
       public static string ConvertFromInfoType(InfoType data)
       {
           string result = string.Empty;
           switch (data)
           { 
               case InfoType.None:
                   result = string.Empty;
                   break;
               case InfoType.Alert:
                   result = "!";
                   break;
               case InfoType.Data:
                   result = "*";
                   break;
               case InfoType.ParameterQuery:
                   result = "?";
                   break;
               case InfoType.ParameterSet:
                   result = "&";
                   break;
               case InfoType.System:
                   result = "$";
                   break;


           }


           return result;
       }
   
       public static DataTypeDatalog ConvertToDataType(string data)
       {
           DataTypeDatalog value = DataTypeDatalog.None;

           #region << switch >>

           switch (data)
           {
               case "000":	// 系统信息
                   value = DataTypeDatalog.D_000;
                   break;
               case "010":	// GPRS信息
                   value = DataTypeDatalog.D_010;
                   break;
               case "030":	// 周期设定
                   value = DataTypeDatalog.D_030;
                   break;
            
               case "050":	// 系统时间
                   value = DataTypeDatalog.D_050;
                   break;
               case "080":	// 温度读取和温度校正
                   value = DataTypeDatalog.D_080;
                   break;
               case "090":
                   value = DataTypeDatalog.D_090;
                   break;
               
          
               case "200":	// 系统运行状态
                   value = DataTypeDatalog.D_200;
                   break;
              
               case "220":
                   value = DataTypeDatalog.D_220;
                   break;
               case "150":	// 场强
                   value = DataTypeDatalog.D_150;
                   break;
               case "180":	// 压力值
                   value = DataTypeDatalog.D_180;
                   break;

               case "111": //压力越上限报警值
                   value = DataTypeDatalog.D_111;
                   break;
               case "112"://压力越上限消警值
                   value = DataTypeDatalog.D_112;
                   break;
               case "113"://压力越下限报警值
                   value = DataTypeDatalog.D_113;
                   break;
               case "114"://压力越下限消警值
                   value = DataTypeDatalog.D_114;
                   break;
               case "115"://压力突升报警值
                   value = DataTypeDatalog.D_115;
                   break;
               case "116":
                   value = DataTypeDatalog.D_116;
                   break;
               case "117"://压力突降报警值
                   value = DataTypeDatalog.D_117;
                   break;
               case "118"://压力突降消警
                   value = DataTypeDatalog.D_118;
                   break;
               case "230":
                   value = DataTypeDatalog.D_230;
                   break;
               case "250":
                   value = DataTypeDatalog.D_250;
                   break;
               case "260":
                   value = DataTypeDatalog.D_260;
                   break;
             
               default:
                   value = DataTypeDatalog.None;
                   break;
           }

           #endregion

           return value;
       }

       public static string ConvertFromDataType(DataTypeDatalog data)
       {
           string value = string.Empty;

           #region << switch >>

           switch (data)
           {
               case DataTypeDatalog.D_000: // 系统信息
                   value = "000";
                   break;
               case DataTypeDatalog.D_010: // GPRS信息
                   value = "010";
                   break;
            
              
               case DataTypeDatalog.D_030: // 周期设定
                   value = "030";
                   break;
            
               case DataTypeDatalog.D_050: // 系统时间
                   value = "050";
                   break;
             
              
               case DataTypeDatalog.D_080: // 温度读取和温度校正
                   value = "080";
                   break;
               case DataTypeDatalog.D_090://回差值
                   value = "090";
                   break;
             
              
               case DataTypeDatalog.D_200: // 系统运行状态
                   value = "200";
                   break;
              
               case DataTypeDatalog.D_220://硬件功能
                   value = "220";
                   break;
               case DataTypeDatalog.D_150: // 场强
                   value = "98";
                   break;
               case DataTypeDatalog.D_180: // 压力值
                   value = "01";
                   break;
            
              
               case DataTypeDatalog.D_230: //数据有效
                   value = "230";
                   break;

              

               default:
                   value = string.Empty;
                   break;
           }

           #endregion

           return value;
       }
       public static string ConvertFromDataTypetoName(DataTypeDatalog data)
       {
           string value = string.Empty;

           #region << switch >>

           switch (data)
           {
               case DataTypeDatalog.D_000: // 系统信息
                   value = "系统信息";
                   break;
               case DataTypeDatalog.D_010: // GPRS信息
                   value = "GPRS信息";
                   break;
               case DataTypeDatalog.D_030: // 周期设定
                   value = "DLA周期设定";
                   break;
               case DataTypeDatalog.D_050: // 系统时间
                   value = "系统时间";
                   break;
               case DataTypeDatalog.D_080: // 温度读取和温度校正
                   value = "温度";
                   break;
               case DataTypeDatalog.D_090:
                   value = "回差值";
                   break;
               case DataTypeDatalog.D_200: // 系统运行状态
                   value = "系统运行状态";
                   break;
               case DataTypeDatalog.D_220:
                   value = "硬件功能调试";
                   break;
               case DataTypeDatalog.D_240:
                   value = "报警设置";
                   break;
               case DataTypeDatalog.D_250:
                   value = "上下限值";
                   break;
               case DataTypeDatalog.D_260:
                   value = "流量板工作状态";
                   break;
               case DataTypeDatalog.D_150: // 场强&电池余量
                   value = "场强&电池余量";
                   break;
               case DataTypeDatalog.D_180: // 压力值
                   value = "压力值";
                   break;
               case DataTypeDatalog.D_230:
                   value = "模块log";
                   break;
               default:
                   value = string.Empty;
                   break;
           }

           #endregion

           return value;
       }

       public static string[] ConvertFromDataTypetoParameterNames(DataTypeDatalog data)
       {
          string[] result={""};
           string value = string.Empty;

           #region << switch >>

           switch (data)
           {
               case DataTypeDatalog.D_000: // 系统信息
                   result = new string[] { "产品型号", "软件版本", "硬件版本"};

                   break;
               case DataTypeDatalog.D_010: // GPRS信息
                   //APN+‘-’+IP1+‘-’+PORT1
                   result = new string[] { "APN", "IP1", "PORT1" };
                   
                   break;
             
               case DataTypeDatalog.D_030: // DLA周期设定
                 
                   result = new string[] { "DLA保存周期", "DLA发送周期" };
                   break;
            
               case DataTypeDatalog.D_050: // 系统时间
                   result = new string[] { "年月日时分秒" };
                   break;

           
             

               case DataTypeDatalog.D_090:
                   result = new string[] { "回差值" };
                   break;
             
             
               case DataTypeDatalog.D_200: //系统运行状态 ‘0’：飞行模式‘1’：全能模式
                   result = new string[] { "系统运行状态" };
                   break;
              
          
               case DataTypeDatalog.D_240: //使能位
                   result = new string[] { "压力","上限报警有效","下限报警有效", "突升报警有效", "突降报警有效" };
                   break;

               case DataTypeDatalog.D_250: //上下限值
                   result = new string[] {"上限值","下限值" };
                   break;
               case DataTypeDatalog.D_260: //突升突降变化率
                   result = new string[] { "突升变化率","突降变化率" };
                   break;
               case DataTypeDatalog.D_150: //场强+‘-’+电池剩余次数
                   result = new string[] { "场强", "电池耗时"};
                   break;
               case DataTypeDatalog.D_180: // 压力值
                   result = new string[] { "压力值" };
                   break;
              
               case DataTypeDatalog.D_230: //log信息
                   result = new string[] {"时间","内容" };
                   break;
            

               default:
                   result = new string[] { "未知参数" };
                   break;
           }

           #endregion

           return result;
       }
       

      
       
    
    
      
        /// <summary>
        /// convert binary to decimal
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private static string Convert2To10(string Data)
        {
            int length = Data.Length;
            double result = 0;
            for (int i = 0; i < length; i++)
            {
                //
                result += Convert.ToInt64(Data[i].ToString()) * Math.Pow(2, (length - (i + 1)));
            }
            return result.ToString();
        }
        /// <summary>
        /// convert hex to binary
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        private static string Convert16To2(string Data)
        {
            string tmpStr = String.Empty;
            string resultStr = String.Empty;
            int length = Data.Length;
            for (int i = 0; i < length; i++)
            {
                switch (Data[i].ToString().Trim().ToLower())
                {
                    case "0":
                        tmpStr = "0000";
                        break;
                    case "1":
                        tmpStr = "0001";
                        break;
                    case "2":
                        tmpStr = "0010";
                        break;
                    case "3":
                        tmpStr = "0011";
                        break;
                    case "4":
                        tmpStr = "0100";
                        break;
                    case "5":
                        tmpStr = "0101";
                        break;
                    case "6":
                        tmpStr = "0110";
                        break;
                    case "7":
                        tmpStr = "0111";
                        break;
                    case "8":
                        tmpStr = "1000";
                        break;
                    case "9":
                        tmpStr = "1001";
                        break;
                    case "a":
                        tmpStr = "1010";
                        break;
                    case "b":
                        tmpStr = "1011";
                        break;
                    case "c":
                        tmpStr = "1100";
                        break;
                    case "d":
                        tmpStr = "1101";
                        break;
                    case "e":
                        tmpStr = "1110";
                        break;
                    case "f":
                        tmpStr = "1111";
                        break;
                }
                resultStr += tmpStr;
            }
            return resultStr;
        }
        /// <summary>
        /// convert string to BCC code
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static string ConvertToBcc(string Data)
        {
            int dataLength = Data.Length;
            int sum = 0;
            string tmpStr = String.Empty;
            char tmpChar;
            int tmpInt;
            for (int i = 0; i < dataLength; i++)
            {
                tmpStr = Data.Substring(i, 1);
                tmpChar = Convert.ToChar(tmpStr);
                tmpInt = Convert.ToInt32(tmpChar);
                sum += tmpInt;
            }
            string sum16Str = Convert.ToString(sum, 16);
            if (sum > 255)
            {
                sum16Str = sum16Str.Substring(sum16Str.Length - 2);
            }
            string sum2Str = Convert16To2(sum16Str);
            //把2进制取反
            string reverseStr = String.Empty;
            for (int i = 0; i < sum2Str.Length; i++)
            {
                reverseStr += Convert.ToString(int.Parse(sum2Str[i].ToString()) ^ 1);
            }
            //转化成10进制
            string sum10Str = Convert2To10(reverseStr);
            long sumLong = Convert.ToInt64(sum10Str) + 1;
            sum16Str = Convert.ToString(sumLong, 16);
            if (sum16Str.Length == 1)
                sum16Str = "0" + sum16Str;
            sum16Str = sum16Str.Substring(sum16Str.Length - 2, 2);
            return sum16Str;
        }

        /// <summary>
        /// check the stirng is numeric
        /// </summary>
        /// <param name="CheckData"></param>
        /// <returns></returns>
        public static bool IsNumeric(string CheckData)
        {
            string numers = "0123456789.-";
            int postion = 0;
            for (int i = 0; i < CheckData.Length; i++)
            {
                postion = numers.IndexOf(CheckData[i].ToString());
                if (postion == -1)
                {
                    return false;
                }
            }
            return true;
        }


        public static string SupplyToLength( string source, int len)
        {
            string result=source;
            if (source.Length > len)
            {
                result = source.Substring(len);
            }
            else if (source.Length < len)
            {
                result = source.PadRight(len, '0');
            }

            return result;
        }
       
    }
}
