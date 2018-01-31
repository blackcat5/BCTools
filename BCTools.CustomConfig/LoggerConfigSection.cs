using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BCTools.CustomConfig
{
    public class LoggerConfigSection : ConfigurationSection
    {
        public const string key_bankproxy = "loggers";

        [ConfigurationProperty(key_bankproxy, IsDefaultCollection = true)]
        public LoggerConfigCollection Loggers
        {
            get
            {
                return (LoggerConfigCollection)this[key_bankproxy];
            }
        }
    }
}
