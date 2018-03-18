using KKCAR.Common.Utilities;
using KKCAR.Service.Messages;
using KKCAR.Service.Messages.Batch;
using KKCAR.Service.Messages.Common;
using log4net;
using Newtonsoft.Json;
using OfficeOpenXml;
using Renci.SshNet;
using Renci.SshNet.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using KKCAR.Data.DataAccess;

///<summary>
/// Class Name : FileFacade
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Business
{
    public class FileFacade : IFileFacade
    {
        private KKCARContextContainer _context;
        private LogMessageBuilder _logMsg = new LogMessageBuilder();
        private static readonly ILog Logger = LogManager.GetLogger(typeof(FileFacade));

        public IEnumerable<string> GetFileList(string localPath, string fiPrefix)
        {
            try
            {
                IEnumerable<string> localFiles = Directory.EnumerateFiles(localPath, string.Format(CultureInfo.InvariantCulture, "{0}*.txt", fiPrefix));
                return localFiles;
            }
            catch (IOException ex)
            {
                Logger.Error("Exception occur:\n", ex);
                throw new CustomException("{0}: {1}", fiPrefix, ex.Message);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                throw;
            }
        }

        public T LoadJsonObject<T>(string filePath)
        {
            try
            {
                string jsonString = StreamDataHelpers.LoadJson(filePath);
                T request = JsonConvert.DeserializeObject<T>(jsonString);
                return request;
            }
            catch (JsonSerializationException)
            {
                var cex = new CustomException("Invalid JSON format");
                cex.Data.Add(Constants.ErrorCode.KKCAR_ERR402, "Invalid JSON format");
                Logger.Debug("O:--FAILED--:Request Data--:jsonString/" + cex.Message);
                throw cex;
            }
        }

        

        public List<InsertStatusRequest> GetExampleData(string path)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                var tmpFolder = HostingEnvironment.MapPath(path);

                using (var stream = File.OpenRead(tmpFolder))
                {
                    pck.Load(stream);
                }

                var ws = pck.Workbook.Worksheets["Test_Data"];
                return ReadExcelData(ws);
            }
        }

        public bool GenerateJsonRequest(string path)
        {
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    var tmpFolder = HostingEnvironment.MapPath(path);
                    string batchImportPath = WebConfig.GetBatchPathImport();

                    using (var stream = File.OpenRead(tmpFolder))
                    {
                        pck.Load(stream);
                    }

                    foreach (var ws in pck.Workbook.Worksheets.Skip(1))
                    {
                        int pageNumber = 1;
                        string fiPrefix = WebConfig.GetBatchFilePrefix();
                        int numberOfObjectsPerPage = WebConfig.GetNumberOfObjectsPerPage();

                        var today = DateTime.Now;
                        var reqList = ReadExcelData(ws);
                        var totalPage = (reqList.Count + numberOfObjectsPerPage - 1) / numberOfObjectsPerPage;
                        IEnumerable<InsertStatusRequest> queryResultPage = reqList.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);

                        while (queryResultPage.Any())
                        {
                            var firstRecord = queryResultPage.ElementAt(0);

                            string importPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", batchImportPath, ws.Name);
                            var fiRequest = string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}.txt", fiPrefix, !string.IsNullOrWhiteSpace(fiPrefix) ? "_" : "", today.FormatDateTime(Constants.DateTimeFormat.FileName));

                            BulkInsertStatusRequest req = new BulkInsertStatusRequest();

                            req.Header = new Service.Messages.Common.BatchHeader
                            {
                                CreateDate = firstRecord.Header.TransactionDateTime,
                                CurrentSequence = pageNumber.ConvertToString(),
                                Filename = fiRequest,
                                ReferenceNo = firstRecord.Header.ReferenceNo,
                                SecurityKey = firstRecord.Header.SecurityKey,
                                SystemCode = firstRecord.Header.SystemCode,
                                TotalRecord = queryResultPage.Count().ConvertToString(),
                                TotalSequence = totalPage.ConvertToString()
                            };

                            var results = (from x in queryResultPage
                                           select new BatchRequestBody
                                           {
                                               PDMCampaignDesc = x.PDMCampaignDesc,
                                               PDMCampaignId = x.PDMCampaignID,
                                               ChannelId = x.Header.ChannelID,
                                               OwnerSystemCode = x.OwnerSystemCode,
                                               OwnerSystemId = x.OwnerSystemId,
                                               PDMProductDesc = x.PDMProductDesc,
                                               PDMProductGroupDesc = x.PDMProductGroupDesc,
                                               PDMProductGroupId = x.PDMProductGroupID,
                                               PDMProductId = x.PDMProductID,
                                               PDMProductSubGroupDesc = x.PDMProductSubGroupDesc,
                                               PDMProductSubGroupId = x.PDMProductSubGroupID,
                                               CMTProductID = x.CMTProductID,
                                               CMTProductDesc = x.CMTProductDesc,
                                               CMTProductGroupID = x.CMTProductGroupID,
                                               CMTProductGroupDesc = x.CMTProductGroupDesc,
                                               CMTCampaignID = x.CMTCampaignID,
                                               CMTCampaignDesc = x.CMTCampaignDesc,
                                               ReferenceNo = x.Header.ReferenceNo,
                                               RefSystemCode = x.RefSystemCode,
                                               RefSystemId = x.RefSystemId,
                                               Status = x.Status,
                                               StatusDateTime = x.StatusDateTime,
                                               StatusName = x.StatusName,
                                               SubscriptionCardType = x.SubscriptionCardType,
                                               SubscriptionCusType = x.SubscriptionCusType,
                                               SubscriptionId = x.SubscriptionID,
                                               SubStatus = x.SubStatus,
                                               SubStatusName = x.SubStatusName,
                                               UpdatedBranch = x.UpdatedBRANCH,
                                               UpdatedId = x.UpdatedID,
                                               UpdatedName = x.UpdatedName,
                                               UpdatedPosition = x.UpdatedPosition,
                                               UpdatedTeam = x.UpdatedTeam
                                           });

                            req.Body = results.Any() ? results.ToArray() : null;

                            string jsonString = JsonConvert.SerializeObject(req);
                            ExportBatchResponse(importPath, fiRequest, jsonString);

                            pageNumber++;
                            queryResultPage = reqList.Skip(numberOfObjectsPerPage * pageNumber).Take(numberOfObjectsPerPage);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
        }

        public bool ExportBatchResponse(string filePath, string fileName, string jsonString)
        {
            try
            {
                string targetFile = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", filePath, fileName);
                using (var sw = new StreamWriter(targetFile, false, Encoding.UTF8))
                {
                    sw.WriteLine(jsonString);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
            }

            return false;
        }

        #region "Sftp"

        /// <summary>
        /// This will list the contents of the current directory.
        /// </summary>
        public bool DownloadFilesViaFTP(string localPath, string sysCode, string fiPrefix)
        {
            try
            {
                // Delete exist files
                IEnumerable<string> localFiles = Directory.EnumerateFiles(localPath, string.Format(CultureInfo.InvariantCulture, "{0}*.txt", fiPrefix)); // lazy file system lookup

                foreach (var localFile in localFiles)
                {
                    if (StreamDataHelpers.TryToDelete(localFile))
                    {
                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete exist local file").Add("FileName", localFile).ToSuccessLogString());
                    }
                    else
                    {
                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete exist local file").Add("FileName", localFile).ToFailLogString());
                    }
                }

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("LocalPath", localPath).ToInputLogString());

                bool isFileFound;
                string host = WebConfig.GetBatchSshServer();
                int port = WebConfig.GetBatchSshPort();
                string username = WebConfig.GetBatchSshUsername();
                string password = WebConfig.GetBatchSshPassword();
                string ignoreFiPattern = WebConfig.GetBatchResponseSuffix();

                // . always refers to the current directory.
                string remoteDirectory = string.IsNullOrWhiteSpace(fiPrefix) ? ApplicationHelpers.GetBatchSshRemoteDirectory(sysCode) : ApplicationHelpers.GetBatchRequestDirectory(sysCode);

                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();

                    if (IsDirectoryExists(sftp, remoteDirectory))
                    {
                        var files = sftp.ListDirectory(remoteDirectory).Where(x => !x.IsDirectory && !x.IsSymbolicLink && x.Name != "." && x.Name != ".." && !x.Name.ToUpperInvariant().Contains(ignoreFiPattern.ToUpperInvariant()));
                        isFileFound = files.Any();

                        if (isFileFound)
                        {
                            // Download file to local via SFTP
                            foreach (var file in files)
                            {
                                DownloadFile(sftp, file, localPath);
                            }

                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").ToSuccessLogString());
                        }
                        else
                        {
                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("Error Message", "File Not Found").ToFailLogString());
                        }
                    }

                    sftp.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("Error Message", ex.Message).ToInputLogString());
            }

            return false;
        }


        public List<string> DownloadBatchFilesViaFTP(string localPath,  string fiPrefix, int prefixLength)
        {
            List<string> ret = new List<string>();
            try
            {
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("LocalPath", localPath).ToInputLogString());

                bool isFileFound;
                string host = WebConfig.GetBatchCARInsertStatusSSHServer();
                int port = WebConfig.GetBatchCARInsertStatusSSHPort();
                string username = WebConfig.GetBatchCARInsertStatusSSHUsername();
                string password = WebConfig.GetBatchCARInsertStatusSSHPassword();

                // . always refers to the current directory.
                string remoteDirectory = WebConfig.GetBatchCARInsertStatusSSHRemoteDir();

                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();

                    if (IsDirectoryExists(sftp, remoteDirectory))
                    {
                        var files = sftp.ListDirectory(remoteDirectory)
                                .Where(x => !x.IsDirectory && !x.IsSymbolicLink && x.Name != "." && x.Name != ".."
                                && x.Name.ToUpperInvariant().StartsWith(fiPrefix.ToUpperInvariant()));

                        isFileFound = files.Any();
                        if (isFileFound)
                        {
                            List<string> filterFileList = new List<string>();
                            DateTime startDate = DateTime.Now.AddDays(0 - WebConfig.GetBatchCARInsertStatusIntervalDay());
                            DateTime endDate = DateTime.Now.AddDays(WebConfig.GetBatchCARInsertStatusIntervalDay());
                            DateTime currDate = startDate;
                            do
                            {
                                filterFileList.Add((fiPrefix + currDate.ToString("yyyyMMdd")).ToUpperInvariant());
                                currDate = currDate.AddDays(1);
                            } while (currDate <= endDate);

                            // Download file to local via SFTP
                            foreach (var file in files)
                            {
                                if (filterFileList.Contains(file.Name.ToUpperInvariant().Substring(0, prefixLength)))
                                {
                                    string f = DownloadBatchFile(sftp, file, localPath);
                                    if (f != null)
                                        ret.Add(f);
                                }
                            }

                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").ToSuccessLogString());
                        }
                        else
                        {
                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("Error Message", "File Not Found").ToFailLogString());
                        }
                    }
                    sftp.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download Files Via FTP").Add("Error Message", ex.Message).ToInputLogString());
            }

            return ret;
        }

        public bool DeleteFilesViaFTP(string sysCode, string fiPrefix)
        {
            try
            {
                // Delete exist files
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Files Via FTP").ToInputLogString());

                bool isFileFound = false;
                string host = WebConfig.GetBatchSshServer();
                int port = WebConfig.GetBatchSshPort();
                string username = WebConfig.GetBatchSshUsername();
                string password = WebConfig.GetBatchSshPassword();
                string remoteDirectory = ApplicationHelpers.GetBatchRequestDirectory(sysCode); // . always refers to the current directory.
                string ignoreFiPattern = WebConfig.GetBatchResponseSuffix();

                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();

                    if (IsDirectoryExists(sftp, remoteDirectory))
                    {
                        var files = sftp.ListDirectory(remoteDirectory).Where(x => !x.IsDirectory && !x.IsSymbolicLink && x.Name != "." && x.Name != ".." && !x.Name.ToUpperInvariant().Contains(ignoreFiPattern.ToUpperInvariant()));
                        isFileFound = files.Any();

                        if (isFileFound)
                        {
                            // Delete file to local via SFTP
                            foreach (var file in files)
                            {
                                DeleteFile(sftp, file.FullName);
                            }

                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Files Via FTP").ToSuccessLogString());
                        }
                        else
                        {
                            Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Files Via FTP").Add("Error Message", "File Not Found").ToFailLogString());
                        }
                    }

                    sftp.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Files Via FTP").Add("Error Message", ex.Message).ToInputLogString());
            }

            return false;
        }

        public bool UploadExportFilesViaFTP(string localPath, string sysCode, string fiPrefix, string fiSuffix)
        {
            try
            {
                string host = WebConfig.GetBatchSshServer();
                int port = WebConfig.GetBatchSshPort();
                string username = WebConfig.GetBatchSshUsername();
                string password = WebConfig.GetBatchSshPassword();
                string remoteDirectory = ApplicationHelpers.GetBatchResponseDirectory(sysCode); // . always refers to the current directory.

                var files = Directory.GetFiles(localPath);

                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();

                    if (IsDirectoryExists(sftp, remoteDirectory))
                    {
                        // Upload file to local via SFTP
                        foreach (var file in files)
                        {
                            UploadFile(sftp, remoteDirectory, file);
                        }

                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Upload Files Via FTP").ToSuccessLogString());
                    }

                    sftp.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Upload Files Via FTP").Add("Error Message", ex.Message).ToInputLogString());
            }

            return false;
        }

        public bool MoveFileViaFTP(string sysCode, string fiPrefix)
        {
            try
            {
                bool isFileFound;
                string host = WebConfig.GetBatchSshServer();
                int port = WebConfig.GetBatchSshPort();
                string username = WebConfig.GetBatchSshUsername();
                string password = WebConfig.GetBatchSshPassword();
                string remoteDirectory = ApplicationHelpers.GetBatchSshRemoteDirectory(sysCode); // . always refers to the current directory.
                string targetDirectory = ApplicationHelpers.GetBatchRequestDirectory(sysCode);
                string ignoreFiPattern = WebConfig.GetBatchResponseSuffix();

                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();

                    if (IsDirectoryExists(sftp, remoteDirectory))
                    {
                        var files = sftp.ListDirectory(remoteDirectory).Where(x => !x.IsDirectory && !x.IsSymbolicLink && x.Name != "." && x.Name != ".." && !x.Name.ToUpperInvariant().Contains(ignoreFiPattern.ToUpperInvariant()));
                        isFileFound = files.Any();

                        if (isFileFound)
                        {
                            // Move file to local via SFTP
                            foreach (var file in files)
                            {
                                string newFile = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", targetDirectory, file.Name);
                                file.MoveTo(newFile);
                            }
                        }

                        Logger.Info(_logMsg.Clear().SetPrefixMsg("Move Files Via FTP").ToSuccessLogString());
                    }

                    sftp.Disconnect();
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Move Files Via FTP").Add("Error Message", ex.Message).ToInputLogString());
            }

            return false;
        }

        #endregion

        #region "Functions"

        private List<InsertStatusRequest> ReadExcelData(ExcelWorksheet ws)
        {
            const int startRow = 2;
            int iRow = startRow;
            List<InsertStatusRequest> reqList = new List<InsertStatusRequest>();
            string refNo = ws.Cells[iRow, 1].Value.ConvertToString();

            while (!string.IsNullOrWhiteSpace(refNo))
            {
                InsertStatusRequest req = new InsertStatusRequest();

                var header = new Service.Messages.Common.Header();
                header.ReferenceNo = refNo;
                header.TransactionDateTime = ws.Cells[iRow, 2].Value.ConvertToString().ParseDateTime().Value;
                header.ServiceName = ws.Cells[iRow, 3].Value.ConvertToString();
                header.SystemCode = ws.Cells[iRow, 4].Value.ConvertToString();
                header.SecurityKey = ws.Cells[iRow, 5].Value.ConvertToString();
                header.ChannelID = ws.Cells[iRow, 6].Value.ConvertToString();
                req.Header = header;

                req.StatusDateTime = ws.Cells[iRow, 7].Value.ConvertToString().ParseDateTime().Value;
                req.SubscriptionID = ws.Cells[iRow, 8].Value.ConvertToString();
                req.SubscriptionCusType = ws.Cells[iRow, 9].Value.ConvertToString();
                req.SubscriptionCardType = ws.Cells[iRow, 10].Value.ConvertToString();
                req.PDMProductGroupID = ws.Cells[iRow, 11].Value.ConvertToString();
                req.PDMProductGroupDesc = ws.Cells[iRow, 12].Value.ConvertToString();
                req.PDMProductSubGroupID = ws.Cells[iRow, 13].Value.ConvertToString();
                req.PDMProductSubGroupDesc = ws.Cells[iRow, 14].Value.ConvertToString();
                req.PDMProductID = ws.Cells[iRow, 15].Value.ConvertToString();
                req.PDMProductDesc = ws.Cells[iRow, 16].Value.ConvertToString();
                req.PDMCampaignID = ws.Cells[iRow, 17].Value.ConvertToString();
                req.PDMCampaignDesc = ws.Cells[iRow, 18].Value.ConvertToString();
                req.CMTProductGroupID = ws.Cells[iRow, 19].Value.ConvertToString();
                req.CMTProductGroupDesc = ws.Cells[iRow, 20].Value.ConvertToString();
                req.CMTProductID = ws.Cells[iRow, 21].Value.ConvertToString();
                req.CMTProductDesc = ws.Cells[iRow, 22].Value.ConvertToString();
                req.CMTCampaignID = ws.Cells[iRow, 23].Value.ConvertToString();
                req.CMTCampaignDesc = ws.Cells[iRow, 24].Value.ConvertToString();
                req.OwnerSystemId = ws.Cells[iRow, 25].Value.ConvertToString();
                req.OwnerSystemCode = ws.Cells[iRow, 26].Value.ConvertToString();
                req.RefSystemId = ws.Cells[iRow, 27].Value.ConvertToString();
                req.RefSystemCode = ws.Cells[iRow, 28].Value.ConvertToString();
                req.Status = ws.Cells[iRow, 29].Value.ConvertToString();
                req.StatusName = ws.Cells[iRow, 30].Value.ConvertToString();
                req.SubStatus = ws.Cells[iRow, 31].Value.ConvertToString();
                req.SubStatusName = ws.Cells[iRow, 32].Value.ConvertToString();
                req.UpdatedBRANCH = ws.Cells[iRow, 33].Value.ConvertToString();
                req.UpdatedTeam = ws.Cells[iRow, 34].Value.ConvertToString();
                req.UpdatedID = ws.Cells[iRow, 35].Value.ConvertToString();
                req.UpdatedName = ws.Cells[iRow, 36].Value.ConvertToString();
                req.UpdatedPosition = ws.Cells[iRow, 37].Value.ConvertToString();
                reqList.Insert(iRow - startRow, req);

                iRow++;
                refNo = ws.Cells[iRow, 1].Value.ConvertToString();
            }

            return reqList;
        }

        private void DeleteFile(SftpClient client, string remoteFile)
        {
            try
            {
                client.DeleteFile(remoteFile);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Remote File").Add("FileName", remoteFile).ToSuccessLogString());
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete Remote File").Add("Error Message", ex.Message).ToInputLogString());
                throw;
            }
        }

        private void UploadFile(SftpClient client, string remoteDirectory, string uploadFile)
        {
            try
            {
                string fileName;

                using (var fs = new FileStream(uploadFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    fileName = Path.GetFileName(fs.Name);
                    string uploadPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", remoteDirectory, fileName);
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Upload File").Add("FileName", fileName).ToInputLogString());
                    client.BufferSize = 4 * 1024;
                    client.UploadFile(fs, uploadPath, null);
                }

                if (StreamDataHelpers.TryToDelete(uploadFile))
                {
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete exist export file").Add("FileName", uploadFile).ToSuccessLogString());
                }
                else
                {
                    Logger.Info(_logMsg.Clear().SetPrefixMsg("Delete exist export file").Add("FileName", uploadFile).ToFailLogString());
                }

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Upload File").Add("FileName", fileName).ToSuccessLogString());
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Upload File").Add("Error Message", ex.Message).ToFailLogString());
                throw;
            }
        }


        private string DownloadBatchFile(SftpClient client, Renci.SshNet.Sftp.SftpFile file, string directory)
        {
            string ret = "";
            try
            {
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").Add("FileName", file.FullName).ToInputLogString());

                string PathFile = Path.Combine(directory, file.Name);
                using (var fileStream = File.OpenWrite(PathFile))
                {
                    client.DownloadFile(file.FullName, fileStream);
                    if (File.Exists(PathFile) == true)
                        ret = PathFile;
                }

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").ToSuccessLogString());
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").Add("Error Message", ex.Message).ToInputLogString());
                throw;
            }

            return ret;
        }

        private void DownloadFile(SftpClient client, Renci.SshNet.Sftp.SftpFile file, string directory)
        {
            try
            {
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").Add("FileName", file.FullName).ToInputLogString());

                using (var fileStream = File.OpenWrite(Path.Combine(directory, file.Name)))
                {
                    client.DownloadFile(file.FullName, fileStream);
                }

                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").ToSuccessLogString());
            }
            catch (Exception ex)
            {
                Logger.Error("Exception occur:\n", ex);
                Logger.Info(_logMsg.Clear().SetPrefixMsg("Download File").Add("Error Message", ex.Message).ToInputLogString());
                throw;
            }
        }


        private bool IsDirectoryExists(SftpClient client, string path)
        {
            bool isDirectoryExist = false;

            try
            {
                client.ChangeDirectory(path);
                isDirectoryExist = true;
            }
            catch (SftpPathNotFoundException)
            {
                return false;
            }

            return isDirectoryExist;
        }

        #endregion
    }
}
