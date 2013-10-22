
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Diagnostics;
using MtuConsole.Common;
using FunctionLib;
using MtuConsole.TcpProcess;

namespace Decode
{
  public class ResponseMessage : IResponseMessage
    {
      private MtuLog _logger = new MtuLog();

      /// <summary>
        /// *0O000000000a7100k180b9139k00000000-00000000-00000000D9# *0O000000000a7200k180b913ak00000000-00000000-3B8007F844# 
      /// </summary>
      /// <param name="getdatas"></param>
      /// <returns></returns>
        public string CreateMultiResponesStr(string getdatas)
        {
            string result = string.Empty;
            string[] dataarray = getdatas.Split('#');
            int totalcount = 0;
            string  rtuid="";
                 
            List<int> collectednums = new List<int>();
            foreach (string data in dataarray)
            {
               
                if (data.Length > 13)
                {
                    string content = "";
                    content = data.Trim()+ "#";
                    if (totalcount == 0)
                    {
                        totalcount = content.Substring(13, 1).ConvertFrom16();
                        rtuid=content.Substring(3,10);
                    }
                    collectednums.Add(content.Substring(14, 1).ConvertFrom16());    

                }
                
            }
            List<int> missednums = new List<int>();

            for (int i = 1; i < totalcount; i++)
            {
                if (!collectednums.Contains(i))
                {
                    missednums.Add(i);
                }
            }
            string missedstr = "";
            if (missednums.Count > 0)
            { 
                missednums.ForEach(x=>{missedstr+=x.ToString();});
            }
            else
            {
                missedstr = "0";
            }
            string framebody = rtuid+missedstr;
            framebody = framebody.Length.ConvertTo62().PadLeft(2,'0') + framebody;
            result = "*" + framebody + framebody.ConvertToRCC() + "#";
                return result;
        }

        public string CreateResponeString(string getstr)
        {
            string result = "";
            string content = getstr.TrimStart().TrimEnd();
            string head = content.Substring(0, 1);
            string framebody;
            if (content.Length > 13)
            {
                switch (head)
                {
                    case "*"://数据类
                        if (content.Substring(13, 1) == "1")
                        {
                            framebody = "0b" + content.Substring(3, 10) + "0";
                            result = "*" + framebody + framebody.ConvertToRCC() + "#";
                        }
                        break;
                    case "!"://报警类
                        if (_logger != null)
                            _logger.Debug(string.Format("收到警报指令待处理, {0}", content));

                        if (content.Substring(13, 1) =="1")
                        {
                            framebody = framebody = "0b" + content.Substring(3, 10) + "0";
                            result = "!" + framebody + framebody.ConvertToRCC() + "#";
                        }
                       
                        // 如果是200停机或开机指令则立即生成对应的设置指令
                        //if (content.Substring(13, 3).Equals("200"))
                        //{
                        //    if (content.Substring(22, 1) == "0")  // 仅停机时下发该指令。
                        //    {
                        //        if (_logger != null)
                        //            _logger.Debug("->>200指令");
                        //        framebody = "0e" + content.Substring(3, 10) + content.Substring(13, 3) + content.Substring(22, 1);
                        //        result += "&" + framebody + framebody.ConvertToRCC() + "#";
                        //    }
                        //}
                        break;
                    case "$"://系统信息类
                        //if (content.Substring(13, 1) == "1")
                        //{
                            //framebody = "0b" + content.Substring(3, 10) + "0";
                            //result = "$" + framebody + framebody.ConvertToRCC() + "#";
                        //}
                        result = string.Empty;
                        break;
                    case "?":
                        result = string.Empty;
                        break;
                        
                }
            }

            return result;
            //throw new System.NotImplementedException();
        }

        public bool VerfyData(string Data)
        {
            return true;
            //throw new System.NotImplementedException();
        }
        public string GetCheckTimeString(string rtuid, int addday, int addsecond)
        {
            string result = "";
            Encode encodeobj = new Encode();
            try
            {
                result= encodeobj.EncodeData(RTUCommandType.SystemTimeSetting,new CommandParameters{ RtuID=rtuid, PortID="050"});
             }
            catch
            {
            
            }
            
            return result;

        }

        public string GetMultiDataMissResponse(string rtuid, List<int> missnums)
        {

            string result = "";

            try
            {
                string framebody = "";

                missnums.Sort();
                foreach (int i in missnums)
                {
                    framebody += i.ConvertTo62();
                }
                if (framebody.Length == 0)
                {
                    framebody = "0";
                }
                else
                {
                    framebody = Common.SupplyToLength(rtuid, 10) + framebody;
                }
                framebody = framebody.Length.ConvertTo62() + framebody;
                result = "*" + framebody + framebody.ConvertToRCC() + "#";
            }
            catch
            {

            }
            return result;
            //throw new System.NotImplementedException();
        }

        public string GetMultiDataMissResponse(string head, string rtuid, List<int> missnums)
        {

            string result = "";

            try
            {
                string framebody = "";

                missnums.Sort();
                foreach (int i in missnums)
                {
                    framebody += i.ConvertTo62();
                }
                if (framebody.Length == 0)
                {
                    framebody = "0";
                }
                else
                {
                    framebody = Common.SupplyToLength(rtuid, 10) + framebody;
                }

                framebody = framebody.Length.ConvertTo62().PadLeft(2, '0') + framebody;

                result = head + framebody + framebody.ConvertToRCC() + "#";
            }
            catch
            {

            }
            return result;
            //throw new System.NotImplementedException();
        }
        public sReceiveEntity GetMultidataStruct(string content)
        {
            sReceiveEntity result = new sReceiveEntity();

            result.AllNum = (int)content.Substring(13, 1).ConvertFrom62();
            result.Head = content.Substring(0, 1);
            List<int> nums = new List<int>();
            nums.Add((int)content.Substring(14, 1).ConvertFrom62());
            //  nums.Add(Convert16To10(content.Substring(9, 2)));
            result.CollectNums = nums;
            result.LastReceiveTime = DateTime.Now;
            return result;
            // throw new System.NotImplementedException();
        }
        public string GetFullDataResponse(string head, string rtuid)
        {
            string result = "";

            string framebody = "0b" + Common.SupplyToLength(rtuid, 10) + "0";
            result = head + framebody + framebody.ConvertToRCC() + "#";

            return result;

            //throw new System.NotImplementedException();
        }

    }
}
