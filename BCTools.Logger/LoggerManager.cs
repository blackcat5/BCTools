using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BCTools.CustomConfig;
using System.Reflection;
using System.Threading;

namespace BCTools.Logger
{
    public enum Level : byte
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// 致命错误
        /// </summary>
        Fatal = 1,
        /// <summary>
        /// 一般错误
        /// </summary>
        Error = 2,
        /// <summary>
        /// 警告
        /// </summary>
        Warn = 3,
        /// <summary>
        /// 调试
        /// </summary>
        Debug = 4,
        /// <summary>
        /// 信息
        /// </summary>
        Info = 5,
        /// <summary>
        /// 全部
        /// </summary>
        ALL = 6
    }

    public class Looger : ILooger
    {
        /// <summary>
        /// 格式_时间
        /// </summary>
        private const string Format_Date = "ForDate";
        /// <summary>
        /// 格式_线程Id
        /// </summary>
        private const string Format_Thread = "ForThread";
        /// <summary>
        /// 格式_级别
        /// </summary>
        private const string Format_Level = "ForLevel";

        public Looger(Level level, LoggerConfigElement element)
        {
            if (element == null)
            {
                _isFatalEnabled =
                _isErrorEnabled =
                _isWarnEnabled =
                _isDebugEnabled =
                _isInfoEnabled = false;
                return;
            }

            _isFatalEnabled = level >= Level.Fatal;
            _isErrorEnabled = level >= Level.Error;
            _isWarnEnabled = level >= Level.Warn;
            _isDebugEnabled = level >= Level.Debug;
            _isInfoEnabled = level >= Level.Info;
            _element = element;

            TaxtKeyValue = new Dictionary<string, string>();
            TaxtKeyValue.Add("{newline}", Environment.NewLine);
            TaxtKeyValue.Add("{date}", Format_Date);
            TaxtKeyValue.Add("{thread}", Format_Thread);
            TaxtKeyValue.Add("{level}", Format_Level);

            _currDate = DateTime.Now;

            string fileName = _element.DatePattern;
            if (_element.StaticLogFileName == false)
            {
                fileName = _currDate.ToString(fileName);
            }

            string newPath = Path.Combine(_element.FilePath, fileName);
            _currFileInfo = new FileInfo(newPath);

            if (_currFileInfo.Directory.Exists == false) { _currFileInfo.Directory.Create(); }
            if (_currFileInfo.Exists == false) { _currFileInfo.Create().Dispose(); }

            _currFileStream = _currFileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
        }

        private LoggerConfigElement _element { get; set; }
        public LoggerConfigElement Element { get; set; }

        private bool _isFatalEnabled { get; set; }
        public bool IsFatalEnabled
        {
            get { return _isFatalEnabled; }
        }

        private bool _isErrorEnabled { get; set; }
        public bool IsErrorEnabled
        {
            get { return _isErrorEnabled; }
        }

        private bool _isWarnEnabled { get; set; }
        public bool IsWarnEnabled
        {
            get { return _isWarnEnabled; }
        }

        private bool _isDebugEnabled { get; set; }
        public bool IsDebugEnabled
        {
            get { return _isDebugEnabled; }
        }

        private bool _isInfoEnabled { get; set; }
        public bool IsInfoEnabled
        {
            get { return _isInfoEnabled; }
        }

        public void Fatal(object obj)
        {
            if (_isFatalEnabled == true && _element != null)
            {
                ChangeTemplate(obj, Level.Fatal);
            }
        }

        public void Error(object obj)
        {
            if (_isErrorEnabled == true && _element != null)
            {
                ChangeTemplate(obj, Level.Error);
            }
        }

        public void Warn(object obj)
        {
            if (_isWarnEnabled == true && _element != null)
            {
                ChangeTemplate(obj, Level.Warn);
            }
        }

        public void Debug(object obj)
        {
            if (_isDebugEnabled == true && _element != null)
            {
                ChangeTemplate(obj, Level.Debug);
            }
        }

        private Dictionary<string, string> TaxtKeyValue { get; set; }

        public void Info(object obj)
        {
            if (_isInfoEnabled == true && _element != null)
            {
                ChangeTemplate(obj, Level.Info);
            }
        }

        private void ChangeTemplate(object obj, Level level)
        {
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();

            if (string.IsNullOrEmpty(_element.Text)) return;

            string newText = _element.Text;

            foreach (string key in TaxtKeyValue.Keys)
            {
                if (newText.Contains(key))
                {
                    string val = TaxtKeyValue[key];
                    switch (val)
                    {
                        case Format_Date:
                            newText = newText.Replace(key, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff"));
                            break;
                        case Format_Thread:
                            if (Thread.CurrentThread != null)
                                newText = newText.Replace(key, Thread.CurrentThread.ManagedThreadId.ToString());
                            else
                                newText = newText.Replace(key, string.Empty);
                            break;
                        case Format_Level:
                            newText = newText.Replace(key, level.ToString());

                            break;
                        default:
                            newText = newText.Replace(key, TaxtKeyValue[key]);
                            break;
                    }
                }
            }

            string name;
            object findval;
            foreach (PropertyInfo item in propertyInfos)
            {
                name = "{" + item.Name + "}";
                if (newText.Contains(name))
                {
                    findval = item.GetValue(obj, null);
                    if (findval == null) findval = string.Empty;
                    newText = newText.Replace(name, findval.ToString());
                }
            }

            BeginWrite(newText);
        }

        private DateTime _currDate { get; set; }

        private FileInfo _currFileInfo { get; set; }

        private FileStream _currFileStream { get; set; }

        private readonly object lockKey = new object();

        private void BeginWrite(string newText)
        {
            try
            {
                lock (lockKey)
                {
                    if (_currDate.Date != DateTime.Now.Date)
                    {
                        _currDate = DateTime.Now;
                        _currFileStream.Dispose();

                        string fileName = _element.DatePattern;
                        if (_element.StaticLogFileName == false)
                        {
                            fileName = _currDate.ToString(fileName);
                        }
                        string newPath = Path.Combine(_element.FilePath, fileName);

                        _currFileInfo = new FileInfo(newPath);

                        if (_currFileInfo.Directory.Exists == false) { _currFileInfo.Directory.Create(); }
                        if (_currFileInfo.Exists == false) { _currFileInfo.Create().Dispose(); }

                        _currFileStream = _currFileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
                    }

                    if (string.IsNullOrEmpty(_element.MaximumFileSize) == false)
                    {
                        string maximumFileSize = _element.MaximumFileSize.ToLower();
                        double maxLen = Convert.ToDouble(maximumFileSize.Substring(0, maximumFileSize.Length - 2));
                        string sub = maximumFileSize.Substring(maximumFileSize.Length - 2, 2);
                        double fileLen = _currFileStream.Length / 1024.0; //转换kb
                        bool isRollBackups = false;

                        switch (sub)
                        {
                            case "kb":
                                if (maxLen <= fileLen)
                                {
                                    isRollBackups = true;
                                }

                                break;
                            case "mb":
                                if (maxLen <= fileLen / 1024.0)
                                {
                                    isRollBackups = true;
                                }

                                break;
                            case "gb":
                                if (maxLen <= fileLen / 1024.0 / 1024.0)
                                {
                                    isRollBackups = true;
                                }

                                break;
                        }

                        if (isRollBackups)
                        {
                            _currFileStream.Dispose();

                            FileInfo[] fileNames = _currFileInfo.Directory.GetFiles(_currFileInfo.Name + "*", SearchOption.TopDirectoryOnly);

                            string filename = _currFileInfo.FullName;

                            _currFileInfo.MoveTo(filename + fileNames.Length);
                            _currFileInfo = new FileInfo(filename);
                            _currFileStream = _currFileInfo.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
                        }
                    }

                    byte[] rows = System.Text.Encoding.ASCII.GetBytes(Environment.NewLine);
                    _currFileStream.Write(rows, 0, rows.Length);
                    _currFileStream.Flush();

                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(newText);
                    _currFileStream.Write(bytes, 0, bytes.Length);
                    _currFileStream.Flush();
                }
            }
            catch { }
        }

        public void Dispose()
        {
            _currFileStream.Dispose();
        }
    }

    public class LoggerManager
    {
        public static ILooger GetLogger(string layoutName)
        {
            Level level = Level.None;

            LoggerConfigSection getsection = CustomConfigManager.LoggerConfigSection();
            if (getsection == null)
                return new Looger(level, null);

            LoggerConfigElement findelement = getsection.Loggers[layoutName];
            if (findelement == null)
                return new Looger(level, null);

            Enum.TryParse(findelement.Level, out level);

            return new Looger(level, findelement);
        }
    }
}
