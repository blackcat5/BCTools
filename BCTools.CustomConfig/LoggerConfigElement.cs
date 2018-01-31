using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BCTools.CustomConfig
{
    public class LoggerConfigElement : ConfigurationElement
    {
        private const string key_layoutName = "layoutName";

        /// <summary>
        /// 日志执行者_布局器名称
        /// </summary>        
        [ConfigurationProperty(key_layoutName, IsKey = true, IsRequired = true)]
        public string LayoutName
        {
            get { return this[key_layoutName] as string; }
            set { this[key_layoutName] = value; }
        }

        private const string key_level = "level";

        /// <summary>
        /// 日志执行者_级别
        /// </summary>        
        [ConfigurationProperty(key_level, IsKey = true)]
        public string Level
        {
            get { return this[key_level] as string; }
            set { this[key_level] = value; }
        }

        private const string key_filePath = "filePath";

        /// <summary>
        /// 日志执行者_路径
        /// </summary>        
        [ConfigurationProperty(key_filePath, IsKey = true)]
        public string FilePath
        {
            get { return this[key_filePath] as string; }
            set { this[key_filePath] = value; }
        }

        private const string key_staticLogFileName = "staticLogFileName";

        /// <summary>
        /// 日志执行者_日志的文件名是否静态
        /// </summary>        
        [ConfigurationProperty(key_staticLogFileName, IsKey = true)]
        public bool StaticLogFileName
        {
            get
            {
                string staticLogFileName = this[key_staticLogFileName] as string;

                bool isTrue = false; bool.TryParse(staticLogFileName, out isTrue);
                return isTrue;
            }
            set { this[key_staticLogFileName] = value; }
        }

        private const string key_datePattern = "datePattern";

        /// <summary>
        /// 日志执行者_日期的格式
        /// </summary>        
        [ConfigurationProperty(key_datePattern, IsKey = true)]
        public string DatePattern
        {
            get { return this[key_datePattern] as string; }
            set { this[key_datePattern] = value; }
        }

        private const string key_maximumFileSize = "maximumFileSize";

        /// <summary>
        /// 日志执行者_最大文件大小
        /// </summary>        
        [ConfigurationProperty(key_maximumFileSize, IsKey = true)]
        public string MaximumFileSize
        {
            get { return this[key_maximumFileSize] as string; }
            set { this[key_maximumFileSize] = value; }
        }

        private const string key_Text = "text";

        /// <summary>
        /// 日志执行者_模板文本
        /// </summary>        
        [ConfigurationProperty(key_Text, IsKey = true)]
        public string Text
        {
            get { return this[key_Text] as string; }
            set { this[key_Text] = value; }
        }				
    }
}
