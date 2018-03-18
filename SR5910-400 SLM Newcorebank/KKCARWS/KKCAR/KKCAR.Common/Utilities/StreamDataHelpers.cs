using System;
using System.IO;
using System.Text;
using System.Web.Hosting;
using log4net;

///<summary>
/// Class Name : StreamDataHelper
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
    public static class StreamDataHelpers
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(StreamDataHelpers));
        
        public static bool TryToDelete(string path)
        {
            try
            {
                File.Delete(path);
                return true;
            }
            catch (IOException ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
        }

        public static bool TryToCopy(string filePath, string newPath)
        {
            try
            {
                File.Copy(filePath, newPath, true);
                return true;
            }
            catch (IOException ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
        }

        public static string GetApplicationPath(string relativeUrl)
        {
            return HostingEnvironment.MapPath(relativeUrl);
        }

        public static string LoadJson(string fileName)
        {
            return File.ReadAllText(fileName, Encoding.UTF8);
            //using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            //{
            //    using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024))
            //    {
            //        return reader.ReadToEnd();
            //    }
            //}
        }
    }
}