using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using log4net;

///<summary>
/// Class Name : XMLTransformer
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Common.Utilities
{
    public static class XMLTransformer
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (XMLTransformer));

        public static XDocument GenerateXDocument(string source)
        {
            using (var tr = new StringReader(source))
            {
                XDocument xDoc = XDocument.Load(tr);
                return xDoc;
            }
        }

        public static string GetSingleNodeValue(XDocument xDoc, string singleNodeName)
        {
            var singleNode = xDoc.Descendants().FirstOrDefault(node => node.Name == singleNodeName);
            string nodeValue = null;
            if (singleNode != null && !singleNode.IsEmpty) { nodeValue = singleNode.Value; }
            return nodeValue;
        }

        public static dynamic GetDeserializeResult<T>(T srcObject, Type targetObject)
        {
            try
            {
                using (var data = new MemoryStream())
                {
                    XmlRootAttribute xRoot = new XmlRootAttribute();
                    xRoot.ElementName = typeof(T).Name;
                    xRoot.IsNullable = true;

                    XmlSerializer serializer = XmlSerializerCache.Create(typeof(T), xRoot);
                    serializer.Serialize(data, srcObject);

                    // USE FOR TRANSFORM STREAM TO BE READABLE
                    data.Position = 0;

                    data.Seek(0, SeekOrigin.Begin);

                    XmlSerializer deserializer = XmlSerializerCache.Create(targetObject, xRoot);
                    return deserializer.Deserialize(data);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                throw;
            }
        }

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
                    return textWriter.ToString();
                }
            }

            return "There is no response body.";
        }

        public static string ToXml(this DataSet ds)
        {
            StringWriter sw = new StringWriter();
            ds.WriteXml(sw);
            return sw.ToString();
        }
    }
}