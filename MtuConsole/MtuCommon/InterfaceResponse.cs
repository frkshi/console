using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MtuConsole.Common
{

    /// <summary>
    /// 接收到的数据的实体结构
    /// </summary>
    public struct sReceiveEntity
    {


        public int AllNum;
        public DateTime LastReceiveTime;
        public string Head;

        /// <summary>
        /// 已收集的桢标号
        /// </summary>
        public List<int> CollectNums;
    }
    public interface IResponseMessage
    {

        string CreateResponeString(string getstr);
        bool VerfyData(string Data);
        string GetCheckTimeString(string rtuid, int addday, int addsecond);
        string GetMultiDataMissResponse(string rtuid, List<int> missnums);
        string GetMultiDataMissResponse(string head, string rtuid, List<int> missnums);
        sReceiveEntity GetMultidataStruct(string content);
        string GetFullDataResponse(string head, string rtuid);
    }
   
}
