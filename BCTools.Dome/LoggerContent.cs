using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCTools.Deom
{
    public class LoggerContent
    {
        public class LogContent
        {
            public LogContent(string regOrderNo, string origQryId, string bussName, string inText, string outText, string message)
            {
                this._regOrderNo = regOrderNo;
                this._origQryId = origQryId;
                this._bussName = bussName;
                this._message = message;
                this._inText = inText;
                this._outText = outText;
                this._logId = Guid.NewGuid();
            }

            private Guid _logId;
            public Guid LogId
            {
                get { return _logId; }
                set { _logId = value; }
            }

            private string _regOrderNo;
            public string RegOrderNo
            {
                get { return _regOrderNo; }
                set { _regOrderNo = value; }
            }

            private string _origQryId;
            public string OrigQryId
            {
                get { return _origQryId; }
                set { _origQryId = value; }
            }

            private string _bussName;
            public string BussName
            {
                get { return _bussName; }
                set { _bussName = value; }
            }

            private string _inText;
            public string InText
            {
                get { return _inText; }
                set { _inText = value; }
            }

            private string _outText;
            public string OutText
            {
                get { return _outText; }
                set { _outText = value; }
            }

            private string _message;
            public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
        }
    }
}
