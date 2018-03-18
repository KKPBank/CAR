using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace KKCAR.Common.Utilities
{
    public static class ApplicationHelpers
    {
        private const string ipAddress = "NoIPAddress";
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ApplicationHelpers));
        private static readonly ResourceManager rm = new ResourceManager("KKCAR.Common.Resources.Resource", typeof(KKCAR.Common.Resources.Resource).Assembly);

        public static bool IsSet<T>(T a, T b)
        {
            return ((int)(object)a & (int)(object)b) == (int)(object)b;
        }

        public static string GetMessage(string resourceKey)
        {
            string message;

            try
            {
                message = rm.GetString(resourceKey);
            }
            catch
            {
                message = "Could not find global resource.";
                Logger.DebugFormat("{0}, Could not find global resource.", resourceKey);
            }

            return message;
        }

        public static string GetMessage(string resourceKey, string cultureName)
        {
            string message;

            try
            {
                message = rm.GetString(resourceKey, GetCultureInfo(cultureName));
            }
            catch
            {
                message = "Could not find global resource.";
                Logger.Debug(string.Format("{0}, Could not find global resource.", resourceKey));
            }

            return message;
        }

        //public static string GetIPAddress()
        //{
        //    IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
        //    IPAddress ipAddress = ipHostInfo.AddressList[0];

        //    return ipAddress.ToString();
        //}

        /// <summary>
        /// Gets the client's IP address.
        /// This method takes into account the X-Forwarded-For header,
        /// in case the blog is hosted behind a load balancer or proxy.
        /// </summary>
        /// <returns>The client's IP address.</returns>
        public static string GetClientIP()
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                var request = context.Request;
                if (request != null)
                {
                    try
                    {
                        var userHostAddress = request.UserHostAddress;

                        // Attempt to parse.  If it fails, we catch below and return "0.0.0.0"
                        // Could use TryParse instead, but I wanted to catch all exceptions
                        IPAddress.Parse(userHostAddress);

                        var xForwardedFor = request.ServerVariables["X_FORWARDED_FOR"];

                        if (string.IsNullOrEmpty(xForwardedFor))
                            return userHostAddress;

                        // Get a list of public ip addresses in the X_FORWARDED_FOR variable
                        var publicForwardingIps = xForwardedFor.Split(',').Where(ip => !IsPrivateIpAddress(ip)).ToList();

                        // If we found any, return the last one, otherwise return the user host address
                        return publicForwardingIps.Any() ? publicForwardingIps.Last() : userHostAddress;
                    }
                    catch (Exception)
                    {
                        // Always return all zeroes for any failure (my calling code expects it)
                        return ipAddress;
                    }
                }
            }

            return ipAddress;
        }

        private static bool IsPrivateIpAddress(string ipAddress)
        {
            // http://en.wikipedia.org/wiki/Private_network
            // Private IP Addresses are: 
            //  24-bit block: 10.0.0.0 through 10.255.255.255
            //  20-bit block: 172.16.0.0 through 172.31.255.255
            //  16-bit block: 192.168.0.0 through 192.168.255.255
            //  Link-local addresses: 169.254.0.0 through 169.254.255.255 (http://en.wikipedia.org/wiki/Link-local_address)

            var ip = IPAddress.Parse(ipAddress);
            var octets = ip.GetAddressBytes();

            var is24BitBlock = octets[0] == 10;
            if (is24BitBlock) return true; // Return to prevent further processing

            var is20BitBlock = octets[0] == 172 && octets[1] >= 16 && octets[1] <= 31;
            if (is20BitBlock) return true; // Return to prevent further processing

            var is16BitBlock = octets[0] == 192 && octets[1] == 168;
            if (is16BitBlock) return true; // Return to prevent further processing

            var isLinkLocalAddress = octets[0] == 169 && octets[1] == 254;
            return isLinkLocalAddress;
        }

        public static string GetCurrentMethod(Int32 index = 2)
        {
            StackTrace st = new StackTrace();
            MethodBase method = st.GetFrame(index).GetMethod();
            string methodName = method.Name;
            //string className = method.ReflectedType.Name;
            //string fullMethodName = string.Format("{0}.{1}", className, methodName);
            //return fullMethodName;
            return methodName;
        }

        public static CultureInfo GetCultureInfo(string cultureName = Constants.KnownCulture.EnglishUS)
        {
            CultureInfo ciUI = new CultureInfo(cultureName);
            return ciUI;
        }

        public static string RemoveWhiteSpaces(string html)
        {
            string s = RemoveNewLine(html);

            if (!string.IsNullOrWhiteSpace(s))
            {
                return Regex.Replace(s, @"\s*(<[^>]+>)\s*", "$1", RegexOptions.Singleline);
            }

            return string.Empty;
        }

        private static string RemoveNewLine(string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return Regex.Replace(str, @"\t|\n|\r", string.Empty);
            }

            return string.Empty;
        }

        public static string GetMessageKey(string errorSystem, string errorCode)
        {
            string[] names = new string[2] { errorSystem.NullSafeTrim(), errorCode.NullSafeTrim() };

            if (names.Any(x => !string.IsNullOrEmpty(x)))
            {
                return names.Where(x => !string.IsNullOrEmpty(x)).Aggregate((i, j) => i + "_" + j);
            }

            return string.Empty;
        }

        public static string GetServiceRefNo()
        {
            return DateTime.Now.FormatDateTime("yyyyMMddhhmmssfff");
        }

        public static string GetServiceTXNDate()
        {
            return DateTime.Now.FormatDateTime("yyyyMMddhhmmss");
        }

        public static bool ValidateCardNo(string cardNo)
        {
            try
            {
                if (cardNo.Length != 13) return false;

                int sum = 0;
                for (int i = 0; i < 12; i++)
                {
                    sum += int.Parse(cardNo.ToArray().GetValue(i).ToString()) * (13 - i);
                }
                if ((11 - sum % 11) % 10 != (int.Parse(cardNo.ToArray().GetValue(12).ToString())))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static string GetHashString(string s)
        {
            using (var hashTool = new System.Security.Cryptography.SHA512Managed())
            {
                Byte[] PasswordAsByte = System.Text.Encoding.UTF8.GetBytes(s);
                Byte[] EncryptedBytes = hashTool.ComputeHash(PasswordAsByte);
                hashTool.Clear();
                return Convert.ToBase64String(EncryptedBytes);
            }
        }

        public static bool Authenticate(string username, string password)
        {
            var taskUsername = WebConfig.GetTaskUsername();
            var taskPassword = WebConfig.GetTaskPassword();
            var isValidTaskUser = taskUsername.Equals(username) && taskPassword.Equals(password);
            return isValidTaskUser;
        }

        public static string GetNotifyUrl(string systemCode, string serviceName, string dataDate, bool getResult, string path)
        {
            return string.Format(Constants.NotifyUrlPattern, systemCode, serviceName, dataDate, getResult, path);
        }
        
        public static string GetBatchUrl(string systemCode, string fileName)
        {
            return string.Format(Constants.BatchUrlPattern, systemCode, fileName);
        }

        public static bool ValidateRefNo(string s)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    string strDate = s.Substring(0, 8);
                    string seqNo = s.Substring(8, s.Length - 8);

                    int n;
                    bool isNumeric = int.TryParse(seqNo, out n);

                    Logger.DebugFormat("O:--Validate RefNo/{0}--:StrDate/{1}:SeqNo/{2}", s, strDate, strDate, seqNo);
                    return strDate.ParseDateTime("yyyyMMdd") != null && isNumeric;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
        }
        
        public static string GetBatchResponseFilePattern(string fiPrefix, string fiSuffix)
        {
            return string.Format(CultureInfo.InvariantCulture, Constants.BatchResponseFilePattern, fiPrefix, fiSuffix);
        }

        public static string GetBatchPathImport(string sysCode)
        {
            string batchPathImport = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", WebConfig.GetBatchPathImport(), sysCode);

            if (!Directory.Exists(batchPathImport))
            {
                Directory.CreateDirectory(batchPathImport);
                Logger.InfoFormat("O:--Directory does not exist, create it--:importPath/{0}", batchPathImport);
            }

            return batchPathImport;
        }

        public static string GetBatchPathExport(string sysCode)
        {
            string batchPathExport = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", WebConfig.GetBatchPathExport(), sysCode);

            if (!Directory.Exists(batchPathExport))
            {
                Directory.CreateDirectory(batchPathExport);
                Logger.InfoFormat("O:--Directory does not exist, create it--:exportPath/{0}", batchPathExport);
            }

            return batchPathExport;
        }

        public static string GetBatchPathSource(string sysCode)
        {
            string batchPathSource = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", WebConfig.GetBatchPathSource(), sysCode);

            if (!Directory.Exists(batchPathSource))
            {
                Directory.CreateDirectory(batchPathSource);
                Logger.InfoFormat("O:--Directory does not exist, create it--:sourcePath/{0}", batchPathSource);
            }

            return batchPathSource;
        }

        public static string GetBatchSshRemoteDirectory(string sysCode)
        {
            string remoteDirectory = WebConfig.GetBatchSshRemoteDir();
            return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", remoteDirectory, sysCode);
        }

        public static string GetBatchRequestDirectory(string sysCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}/Request", GetBatchSshRemoteDirectory(sysCode));
        }

        public static string GetBatchResponseDirectory(string sysCode)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}", GetBatchSshRemoteDirectory(sysCode));
        }

        #region "Get latest updated date of the DLL"

        public static string GetDisplayDllLastUpdatedDate()
        {
            DateTime date = GetDllLastUpdatedDate();
            string dateDisplay = date.FormatDateTime("yyyyMMdd HH:mm");

            string softwareVersion = String.Format("Ver{0}_{1}", WebConfig.GetSoftwareVersion(), dateDisplay);
            return softwareVersion;
        }

        private static DateTime GetDllLastUpdatedDate()
        {
            DateTime latestDate = new DateTime();
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/bin"));
            FileInfo[] arrFileInfo = dirInfo.GetFiles("*.dll");
            foreach (FileInfo fileInfo in arrFileInfo)
            {
                if (fileInfo.LastWriteTime > latestDate)
                {
                    latestDate = fileInfo.LastWriteTime;
                }
            }

            return latestDate;
        }

        #endregion

        #region "My Extensions"

        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrWhiteSpace(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFromInvariantString(s);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }
            return result;
        }

        public static string FormatDecimal<T>(this T obj)
        {
            string result = string.Empty;

            try
            {
                if (!object.Equals(obj, default(T)))
                {
                    result = String.Format("{0:#,##0.00}", obj);
                }
                else
                {
                    // Non decimal data cant be calculated
                }
            }
            catch (Exception ex)
            {
                Logger.Error("FormatException:\n", ex);
            }

            return result;
        }

        public static string FormatNumber<T>(this T obj)
        {
            string result = string.Empty;

            try
            {
                if (!object.Equals(obj, default(T)))
                {
                    result = String.Format("{0:#,##0}", obj);
                }
                else
                {
                    // Non decimal data cant be calculated
                }
            }
            catch (Exception ex)
            {
                Logger.Error("FormatException:\n", ex);
            }

            return result;
        }

        public static T CopyObject<T>(this object objSource)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, objSource);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        public static string FormatMobile(this string mobileNo)
        {
            if (string.IsNullOrEmpty(mobileNo) || mobileNo.Length < 6)
                return string.Empty;
            return string.Format("{0}XXXX{1}", mobileNo.Substring(0, 3), mobileNo.Substring(7));
        }
        
        public static string FormatAccount(this string accountNo)
        {
            if (string.IsNullOrEmpty(accountNo) || accountNo.Length < 12)
                return accountNo;
            return string.Format("{0}-{1}-{2}-{3}", accountNo.Substring(0, 3), accountNo.Substring(3, 3), accountNo.Substring(6, 3), accountNo.Substring(9));
        }

        public static string LimitTo(this string s, int length)
        {
            Logger.DebugFormat("O:--Limit To--:String Value/{0}", s);
            return (s == null || s.Length < length) ? s : s.Substring(0, length);
        }

        public static IDictionary<string, T> ToDictionary<T>(this T values)
        {
            var dict = new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

            if (!object.Equals(values, default(T)))
            {
                foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(values))
                {
                    T obj = (T)propertyDescriptor.GetValue(values);
                    dict.Add(propertyDescriptor.Name, obj);
                }
            }

            return dict;
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string MaskPrefix(this string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                IList<object> list = StringHelpers.ConvertStringToList(value, '_');
                if (list.Count == 3)
                {
                    list.RemoveAt(0);
                    return StringHelpers.ConvertListToString(list, "_");
                }
            }

            return value;
        }

        /// <summary>
        /// Converts the provided app-relative path into an absolute Url containing the 
        /// full host name
        /// </summary>
        /// <param name="relativeUrl">App-Relative path</param>
        /// <returns>Provided relativeUrl parameter as fully qualified Url</returns>
        /// <example>~/path/to/foo to http://www.web.com/path/to/foo</example>
        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            string relativeUrlNew = relativeUrl;
            if (string.IsNullOrEmpty(relativeUrlNew))
                return relativeUrlNew;

            if (HttpContext.Current == null)
                return relativeUrlNew;

            if (relativeUrlNew.StartsWith("/"))
                relativeUrlNew = relativeUrlNew.Insert(0, "~");
            if (!relativeUrlNew.StartsWith("~/"))
                relativeUrlNew = relativeUrlNew.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrlNew));
        }

        public static string ExtractProductCode(this string s)
        {
            string accountNo = s.NullSafeTrim();

            if (!string.IsNullOrWhiteSpace(accountNo) && s.Length > 4)
            {
                return accountNo.Substring(4, 3);
            }

            return s;
        }

        public static string ToErrorMessage(this AggregateException exception)
        {
            StringBuilder sb = new StringBuilder("");

            if (exception.InnerExceptions != null && exception.InnerExceptions.Count > 0)
            {
                foreach (Exception ex in exception.InnerExceptions)
                {
                    sb.AppendFormat(Constants.StackTraceError.InnerException, ex.Source, ex.Message, ex.StackTrace);
                }
            }

            if (exception.InnerException != null)
            {
                string innerException = sb.ToString();
                sb.Clear();
                sb.AppendFormat(Constants.StackTraceError.Exception, innerException);
            }

            return sb.ToString();
        }

        public static string ExtractNamePrefix(this string s)
        {
            var match = Regex.Match(s, @"(\w*).txt", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if (match.Success)
            {
                return match.Groups[1].Value.NullSafeTrim();
            }

            return string.Empty;
        }

        #endregion
    }
}
