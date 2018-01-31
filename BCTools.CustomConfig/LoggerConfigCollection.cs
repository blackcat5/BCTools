using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace BCTools.CustomConfig
{
    public class LoggerConfigCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new LoggerConfigElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((LoggerConfigElement)element).LayoutName;
        }

        protected override string ElementName
        {
            get { return "logger"; }
        }

        public new int Count
        {
            get { return base.Count; }
        }

        public LoggerConfigElement this[int index]
        {
            get
            {
                return (LoggerConfigElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public LoggerConfigElement this[string Name]
        {
            get
            {
                return (LoggerConfigElement)BaseGet(Name);
            }
        }

        public int IndexOf(LoggerConfigElement element)
        {
            return BaseIndexOf(element);
        }

        public void Add(LoggerConfigElement element)
        {
            BaseAdd(element);
        }

        public void Remove(LoggerConfigElement element)
        {
            if (BaseIndexOf(element) >= 0)
                BaseRemove(element.LayoutName);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string client)
        {
            BaseRemove(client);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}
