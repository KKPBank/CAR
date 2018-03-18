using System.IO;
using System.Xml.Serialization;

namespace Cas.Common
{
    public static class XMLTransformer
    {        
        public static string SerializeObject<T>(this T toSerialize)
        {
            if (!object.Equals(toSerialize, default(T)))
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = typeof(T).Name;
                xRoot.IsNullable = true;

                XmlSerializer xmlSerializer = XmlSerializerCache.Create(typeof(T), xRoot);
                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString().Replace(" xmlns=\"http://www.kiatnakinbank.com/services/CAS/CASLogService\"", "");
                }
            }

            return "There is no response body.";
        }
        
    }
}
