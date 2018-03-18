using System;
using System.Collections;
using System.IO;
using log4net;

///<summary>
/// Class Name : TextGenerator
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Common.Mail
{
    public class TextGenerator
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(TextGenerator));
        private Hashtable _hashData;

        private void SetHashData(Hashtable hashData)
        {
            _hashData = hashData;
        }

        private void GenText(string pathName, StringWriter sw)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                fs = new FileStream(pathName, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(fs);
                string line = sr.ReadLine();

                while (line != null)
                {
                    if (line.IndexOf("[",StringComparison.InvariantCultureIgnoreCase) >= 0 && line.IndexOf("]", StringComparison.InvariantCultureIgnoreCase) >= 0)
                    {
                        foreach (string key in _hashData.Keys)
                        {
                            try
                            {
                                line = line.Replace("[" + key + "]", (string)_hashData[key]);
                            }
                            catch (Exception)
                            {
                                Logger.Debug("Missing Message Key[" + key + "]");
                            }
                        }
                    }

                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
            finally
            {
                
            }
        }

        public string GenText(string pathName, Hashtable hData)
        {
            StringWriter sw = null;

            try
            {
                sw = new StringWriter();
                SetHashData(hData);
                GenText(pathName, sw);
                sw.Flush();
                return sw.ToString();
            }
            finally
            {
                if (sw != null)
                {
                    sw.Dispose();
                    sw = null;
                }
            }
        }
    }
}