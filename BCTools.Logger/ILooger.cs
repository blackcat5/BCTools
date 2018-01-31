using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BCTools.Logger
{
    public interface ILooger
    {
        bool IsDebugEnabled { get; }

        bool IsErrorEnabled { get; }

        bool IsFatalEnabled { get; }

        bool IsInfoEnabled { get; }

        bool IsWarnEnabled { get; }

        void Fatal(object obj);

        void Error(object obj);

        void Warn(object obj);

        void Debug(object obj);

        void Info(object obj);

        void Dispose();
    }
}
