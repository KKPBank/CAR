using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace Cas.Common
{
    public class XmlSerializerCache
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(XmlSerializerCache));
        private static readonly Dictionary<string, XmlSerializer> Cache = new Dictionary<string, XmlSerializer>();

        public static XmlSerializer Create(Type type, XmlRootAttribute root)
        {
            var key = String.Format(CultureInfo.InvariantCulture, "{0}:{1}", type, root.ElementName);

            if (!Cache.ContainsKey(key))
            {
                //Logger.DebugFormat("O:--Create New XmlSerializer--:Key/{0}", key);
                Cache.Add(key, new XmlSerializer(type, root));
            }
            //else
            //{
            //    Logger.DebugFormat("O:--Get Exists XmlSerializer--:Key/{0}", key);
            //}

            return Cache[key];
        }
    }
}
