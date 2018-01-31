using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCTools.Logger;

namespace BCTools.Deom
{
    class Program
    {
        static void Main(string[] args)
        {
            ILooger _allLog = LoggerManager.GetLogger("ALL.ClientLibrary");

            string regOrderNo = "deom";
            string origQryId ="deom";
            string bussName = "deom";
            string inText = "deom";
            string outText = "deom";
            string message = "deom";
                        
            var unionpayContent = new LoggerContent.LogContent(regOrderNo, origQryId, bussName, inText, outText, message);
            _allLog.Info(unionpayContent);
            _allLog.Dispose(); //释放程序的占有

            Console.Write("生成完毕，请按下任何键退出");
            Console.ReadKey();
        }
    }
}
