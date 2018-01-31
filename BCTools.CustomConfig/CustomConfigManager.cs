using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BCTools.CustomConfig
{
    public class CustomConfigManager
    {
        static CustomConfigManager() { }

        /// <summary>
        /// 根据配置节的路径和名称获取单个配置文件中的节
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configName">配置节的路径和名称</param>
        /// <returns></returns>
        public static T GetSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            return (T)ConfigurationManager.GetSection(sectionName);
        }

        public static T GetSection<T>(string sectionName, string
 configFilePath) where T : ConfigurationSection
        {
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = configFilePath;

            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);

            return (T)configuration.GetSection(sectionName);
        }

        /// <summary>
        /// 取得银行发行机构的配置节
        /// </summary>
        public static LoggerConfigSection LoggerConfigSection()
        {
            string _configFileName = "Logger.config";

            return GetSection<LoggerConfigSection>("loggerInfo", System.IO.Path.Combine(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, _configFileName));
        }
    }
}
