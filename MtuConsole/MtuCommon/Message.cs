using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MtuConsole.Common
{

    
    /// <summary>
    /// 接收通道抛出消息事件
    /// </summary>
    /// <param name="objMessage"></param>
    public delegate void MtuMessageHandler(Message objMessage);

    public class ServerHost
    {

        public event MtuMessageHandler SendMsg;

        public ServerHost()
        {}
        /// <summary>
        /// 管理事件触发，界面注册
        /// </summary>
        /// <param name="msg"></param>
        public void Send(Message msg)
        {
            SendMsg(msg);
        }
        
    }

    /// <summary>
    /// Enumeration that represents the type of a message
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// 错误消息
        /// </summary>
        Error,

        /// <summary>
        /// 成功发送设置参数后，通知客户端
        /// </summary>
        Notice,

        /// <summary>
        /// 解码后数据包（检测量信息
        /// </summary>
        Final,

        /// <summary>
        /// 原始包
        /// </summary>
        Original,
        /// <summary>
        /// 服务器记录
        /// </summary>
        ServiceNote
    }
   public class Message
    {

        #region << Properties >>
        /// <summary>
        /// The body of the message. This contains the message text.
        /// </summary>
       public MessageBody Body
       {
           get;
           set;
       }

        /// <summary>
        /// message type 
        /// </summary>
       public MessageType Type
       {
           get;
           set;
       }
       public string OriginString
       {
           get;
           set;
       }
       public override string ToString()
       {

          string result=string.Empty;

           result=this.Type.ToString() + this.Body.ToString();

           return result;
       }
        #endregion
    }

   public  class MessageBody 
   {
       public MessageBody()
       {
          
       }

       /// Communication Id        
       public string CommunicationId
       {
           get;
           set;
       }

       /// Port 
       public string Port
       {
           get;
           set;
       }

       public string IP
       {
           get;
           set;
       }
       public override string ToString()
       {
           string result = "";
           Type type = this.GetType();
           foreach (System.Reflection.PropertyInfo PInfo in type.GetProperties())
           {
               //用PInfo.GetValue获得值 
               string val = Convert.ToString(PInfo.GetValue(this, null));
               //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作 
               string name = PInfo.Name;

               result += name + "=" + val + ";" + Environment.NewLine;
           }

           return result;
       }
   }

   /// <summary>
   /// 数据包消息
   /// </summary>
   public  class DataMessageBody : MessageBody
   {
       public DataMessageBody() { }

       /// Rtu Id
       public string RtuId
       {
           get;
           set;
       }

       /// Rtu Name
       public string RtuName
       {
           get;
           set;
       }
       public override string ToString()
       {
           string result = "";
           Type type = this.GetType();
           foreach (System.Reflection.PropertyInfo PInfo in type.GetProperties())
           {
               //用PInfo.GetValue获得值 
               string val = Convert.ToString(PInfo.GetValue(this, null));
               //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作 
               string name = PInfo.Name;

               result += name + "=" + val + ";" + Environment.NewLine;
           }

           return result;
       }
   }

    /// <summary>
    /// 原始包数据消息体
    /// </summary>
   public class OriginalMessageBody : DataMessageBody
   {
       public OriginalMessageBody()
       {
          
       }

       /// <summary>
       /// 
       /// </summary>
       public string Data
       {
           get;
           set;
       }

       public DateTime Datetime
       {
           get;
           set;
       }
       public override string ToString()
       {
           string result = "";
           Type type = this.GetType();
           foreach (System.Reflection.PropertyInfo PInfo in type.GetProperties())
           {
               //用PInfo.GetValue获得值 
               string val = Convert.ToString(PInfo.GetValue(this, null));
               //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作 
               string name = PInfo.Name;

               result += name + "=" + val + ";" + Environment.NewLine;
           }

           return result;
       }
   }




   /// <summary>
   /// 解码后的数据消息
   /// </summary>
   public  class FinalMessageBody : DataMessageBody { }

   /// <summary>
   /// 检测量数据消息
   /// </summary>
   public class MeasureMessageBody : FinalMessageBody
   {
       public MeasureMessageBody()
       {
          
       }

       public string DataType
       {
           get;
           set;
       }
       public string MeasureId
       {
           get;
           set;
       }

       public string MeasureName
       {
           get;
           set;
       }

       public MeasureValueCollection Items
       {
           get;
           set;
       }
       public override string ToString()
       {
           string result = "";
           Type type = this.GetType();
           foreach (System.Reflection.PropertyInfo PInfo in type.GetProperties())
           {
               //用PInfo.GetValue获得值 
               string val = Convert.ToString(PInfo.GetValue(this, null));
               //获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作 
               string name = PInfo.Name;

               if (name == "Items")
               {
                   result = result + "Items=";
                   foreach (MeasureValue v in Items.GetAllItems())
                   {
                       result = result + v.Datetime.ToString() + " " + v.OriginalValue + " ";
                   }

               }
               else
               {
                   result += name + "=" + val + ";" + Environment.NewLine;
               }
           }

           return result;
       }
   }

   /// <summary>
   /// Measure Message content collection
   /// </summary>
   public class MeasureValueCollection 
   {
       private List<MeasureValue> _collection;
       public MeasureValueCollection()
       {
           _collection = new List<MeasureValue>();
       }

       public List<MeasureValue> GetAllItems()
       {
           return _collection;
       }

       public void AddItem(MeasureValue itemType)
       {
           _collection.Add(itemType);
       }
   }

   /// <summary>
   /// Measure Message content
   /// </summary>
   public class MeasureValue 
   {
       public MeasureValue()
       {
          
       }

       public DateTime Datetime
       {
           get;
           set;
       }

       /// <summary>
       /// 原始值1
       /// </summary>
       public string OriginalValue
       {
           get;
           set;
       }
   }



   ///// <summary>
   ///// 上下限报警数据消息
   ///// </summary>
   public class LimitAlertMessageBody : MeasureAlertMessageBody
   {


       /// <summary>
       /// 原始值
       /// </summary>
       public string OriginalValue
       {
           get;
           set;
       }
   }

   /// <summary>
   /// 警告数据消息
   /// </summary>
   public  class AlertMessageBody : FinalMessageBody
   {


       /// <summary>
       /// 发生时间
       /// </summary>
       public DateTime Datetime
       {
           get;
           set;
       }
   }
   /// <summary>
   /// 检测量报警数据消息
   /// </summary>
   public  class MeasureAlertMessageBody : AlertMessageBody
   {
       private List<AlertItem> _collection;
      public  MeasureAlertMessageBody()
       {
       _collection=new List<AlertItem>();
       }

       /// <summary>
       /// 检测量名称
       /// </summary>
       public string MeasureName
       {
         get;set;

       }


       ///// <summary>
       ///// 原始值1
       ///// </summary>
       //public string OriginalValue1
       //{
       //    get;
       //    set;
       //}

       ///// <summary>
       ///// 原始值2
       ///// </summary>
       //public string OriginalValue2
       //{
       //    get;
       //    set;
       //}
       public List<AlertItem> GetAllAlertValues()
       {
          return _collection;
       }

       public void AddAlertValue(AlertItem item)
       {
           _collection.Add(item);
       }
   }

   public class AlertItem 
   {
       public AlertItem()
       {
           
       }

       public DateTime Time
       {
           get;
           set;
       }

       public string Value
       {
           get;
           set;
       }
   }

   /// <summary>
   /// 突变报警数据消息
   /// </summary>
   public  class MutationAlertMessageBody : MeasureAlertMessageBody
   {

       public MutationAlertMessageBody()
       { }
       /// <summary>
       /// 原始值1
       /// </summary>
       public string OriginalValue1
       {
           get;
           set;
       }

       /// <summary>
       /// 原始值2
       /// </summary>
       public string OriginalValue2
       {
           get;
           set;
       }
   }

   public  class RTUCommand 
   {
       public RTUCommand()
       {
           
       }

       /// <summary>
       /// RtuId
       /// </summary>
       public string Id
       {
           get;
           set;
       }

       /// <summary>
       /// RTU 参数设置分类
       /// </summary>
       public RTUCommandType Type
       {
           get;
           set;
       }


       /// <summary>
       /// 命令对应的编码
       /// </summary>
       public string CommandEncodeString
       {
           get;
           set;
       }
   }

   public enum RTUCommandType
   {
       /// <summary>
       /// 常规
       /// </summary>

       GenernalConfig = 1,
       /// <summary>
       /// 通信
       /// </summary>

       CommunicationConfig = 2,
       /// <summary>
       /// 脉冲量昨日数据恢复
       /// </summary>

       PulseVolumeYesterdayDataRecovery = 3,
       /// <summary>
       /// 脉冲量今日数据恢复
       /// </summary>

       PulseVolumeTodayDataRecovery = 4,
       /// <summary>
       /// 系统数据恢复
       /// </summary>

       HistoryDataRecovery = 5,
       /// <summary>
       /// 系统时间设定
       /// </summary>

       SystemTimeSetting = 6,
       /// <summary>
       /// 数据查询
       /// </summary>

       QueryData = 7,
       /// <summary>
       /// 当前气压值下发
       /// </summary>

       AirPressureSetting = 8,

       /// <summary>
       /// 重置GPRS通讯时间参数
       /// </summary>

       RestGPRSSetting = 9,


       /// <summary>
       /// dla2代，010
       /// </summary>

       GPRSSetting = 10,
       /// <summary>
       /// 030
       /// </summary>

       CycleSetting = 11,
       /// <summary>
       /// 090
       /// </summary>

       BackValueSetting = 12,
       /// <summary>
       /// 0b0
       /// </summary>

       PressureScaleSetting = 13,

       /// <summary>
       /// 240
       /// </summary>

       PressureAlertSetting = 14,
       /// <summary>
       /// 流量保存周期040
       /// </summary>

       FlowSaveCycle = 15

   }
}
