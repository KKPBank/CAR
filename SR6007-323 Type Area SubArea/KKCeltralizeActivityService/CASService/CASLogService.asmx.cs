using Cas.Biz;
using Cas.Common;
using Cas.Dal.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml.Serialization;

namespace CASService
{
    [WebService(Namespace = "http://www.kiatnakinbank.com/services/CAS/CASLogService", Name = "CASLogService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CASLogService : System.Web.Services.WebService
    {
        #region Member
        private readonly ILog _logger;
        #endregion

        #region Constructor
        public CASLogService()
        {
            try
            {
                _logger = LogManager.GetLogger(typeof(CASLogService));
            }
            catch (Exception ex)
            {
                _logger.Error("E:--Exception occur:--\n", ex);
            }
        }
        #endregion

        [WebMethod]
        public CreateActivityLogResponse CreateActivityLog(LogServiceHeader Header, CreateActivityLogData CreateActivityLog)
        {
            _logger.Info("I:--CAS-CreateActivityLog--Start--");
            _logger.Debug(Header.SerializeObject());
            _logger.Debug(CreateActivityLog.SerializeObject());

            string err = "";
            decimal refid = 0;
            CreateActivityLogResponse ret = new CreateActivityLogResponse();
            try {
                ret.ResponseStatus = new ResponseData();
                if(Header == null)
                {
                    ret.ResponseStatus.ResponseCode = "CAS-E-101";
                    ret.ResponseStatus.ResponseMessage = "Data required: Header";
                }
                else if (Header.ServiceName != "CreateActivityLog")
                {
                    if (String.IsNullOrEmpty(Header.ServiceName))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-101";
                        ret.ResponseStatus.ResponseMessage = "Data required: ServiceName";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-203";
                        ret.ResponseStatus.ResponseMessage = "Invalid Service Name";
                    }
                }
                else
                {
                    // Check Auth
                    AuthenBiz aBiz = new AuthenBiz();
                    if (!aBiz.CheckAuth(Header, out err))
                        ret.ResponseStatus.ResponseMessage = aBiz.ErrorMessage;


                    if (string.IsNullOrEmpty(err))
                    {
                        ServiceLogBiz bz = new ServiceLogBiz();
                        if (!bz.CreateLog(CreateActivityLog, Header, out err, out refid))
                            ret.ResponseStatus.ResponseMessage = bz.ErrorMessage;
                    }

                    if (string.IsNullOrEmpty(err))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-I-000";
                        ret.ResponseStatus.ResponseMessage = "Success";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = err;
                    }
                }

                // return result
                ret.Header = new LogServiceHeader()
                {
                    ReferenceNo = Convert.ToString(refid),
                    ServiceName = "CreateActivityLog",
                    SystemCode = Header.SystemCode,
                    TransactionDateTime = DateTime.Now
                };

                if (ret.ResponseStatus.ResponseMessage != "Success" || System.Configuration.ConfigurationManager.AppSettings["CreateServiceActivityLogDB"].ToString() == "Y")
                {
                    var ist = HttpContext.Current.Request.InputStream;
                    using (StreamReader sr = new StreamReader(ist, Encoding.UTF8))
                    {
                        ist.Position = 0;
                        XmlSerializer xs = new XmlSerializer(typeof(CreateActivityLogResponse));
                        using (StringWriter sw = new StringWriter())
                        {
                            xs.Serialize(sw, ret);

                            string error;
                            ActivityLogBiz.InsertServiceActivityLog(new Cas.Dal.CAS_SERVICE_ACTIVITYLOG()
                            {
                                SYSTEM_CODE = Header.SystemCode,
                                RESPONSE_CODE = ret.ResponseStatus.ResponseCode,
                                RESPONSE_MESSAGE = ret.ResponseStatus.ResponseMessage,
                                SERVICE_NAME = ret.Header.ServiceName,
                                REQUEST_IPADDRESS = Context.Request.UserHostAddress,
                                REQUEST_URL = Context.Request.Url.AbsoluteUri,
                                REFERENCE_NO = Header.ReferenceNo,
                                XML_REQUEST = sr.ReadToEnd(),
                                XML_RESPONSE = sw.ToString()

                            }, out error);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                _logger.Error("E:--CAS-CreateActivityLog--", ex);
            }
            _logger.Debug(ret.SerializeObject());
            _logger.Info("O:--CAS-CreateActivityLog--End--");
            return ret;
        }
        
        [WebMethod]
        public InquiryActivityLogResponse InquiryActivityLog(LogServiceHeader Header, InquiryActivityLogData InquiryActivityLog)
        {
            _logger.Info("I:--CAS-InquiryActivityLog--Start--");
            _logger.Debug(Header.SerializeObject());
            _logger.Debug(InquiryActivityLog.SerializeObject());

            string servicename = "InquiryActivityLog";
            var ret = new InquiryActivityLogResponse();
            try {
                ret.ResponseStatus = new ResponseData();
                List<ActivityDataItem> output = null;
                string err = "";

                if(Header == null)
                {
                    ret.ResponseStatus.ResponseCode = "CAS-E-101";
                    ret.ResponseStatus.ResponseMessage = "Data required: Header";
                }
                else if (Header.ServiceName != servicename)
                {
                    if (String.IsNullOrEmpty(Header.ServiceName))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-101";
                        ret.ResponseStatus.ResponseMessage = "Data required: ServiceName";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-203";
                        ret.ResponseStatus.ResponseMessage = "Invalid Service Name";
                    }
                }
                else
                {
                    AuthenBiz aBiz = new AuthenBiz();
                    if (!aBiz.CheckAuth(Header, out err))
                        ret.ResponseStatus.ResponseMessage = aBiz.ErrorMessage;

                    if (string.IsNullOrEmpty(err))
                    {
                        ServiceLogBiz bz = new ServiceLogBiz();
                        if (!bz.InquiryLog(InquiryActivityLog, Header, out err, out output, true))
                            ret.ResponseStatus.ResponseMessage = bz.ErrorMessage;
                    }

                    if (string.IsNullOrEmpty(err))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-I-000";
                        ret.ResponseStatus.ResponseMessage = "Success";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = err;
                    }
                }
                ret.Header = new LogServiceHeader()
                {
                    ReferenceNo = Header.ReferenceNo,
                    ServiceName = servicename,
                    SystemCode = Header.SystemCode,
                    TransactionDateTime = DateTime.Now
                };
                ret.InquiryActivityDataList = output;

                if (ret.ResponseStatus.ResponseMessage != "Success" || System.Configuration.ConfigurationManager.AppSettings["CreateServiceActivityLogDB"].ToString() == "Y")
                {
                    var ist = HttpContext.Current.Request.InputStream;
                    using (StreamReader sr = new StreamReader(ist, Encoding.UTF8))
                    {
                        ist.Position = 0;
                        XmlSerializer xs = new XmlSerializer(typeof(InquiryActivityLogResponse));
                        StringWriter sw = new StringWriter();
                        xs.Serialize(sw, ret);

                        string error;
                        ActivityLogBiz.InsertServiceActivityLog(new Cas.Dal.CAS_SERVICE_ACTIVITYLOG()
                        {
                            SYSTEM_CODE = Header.SystemCode,
                            RESPONSE_CODE = ret.ResponseStatus.ResponseCode,
                            RESPONSE_MESSAGE = ret.ResponseStatus.ResponseMessage,
                            SERVICE_NAME = ret.Header.ServiceName,
                            REQUEST_IPADDRESS = Context.Request.UserHostAddress,
                            REQUEST_URL = Context.Request.Url.AbsoluteUri,
                            REFERENCE_NO = Header.ReferenceNo,
                            XML_REQUEST = sr.ReadToEnd(),
                            XML_RESPONSE = sw.ToString()
                        }, out error);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("E:--CAS-InquiryActivityLog--", ex);
            }
            _logger.Debug(ret.SerializeObject());
            _logger.Info("O:--CAS-InquiryActivityLog--End--");
            return ret;
        }
        
        [WebMethod]
        public InquiryActivityLogResponse InquiryActivityLogByID(LogServiceHeader Header, InquiryActivityLogData InquiryActivityLog)
        {
            _logger.Info("I:--CAS-InquiryActivityLogByID--Start--");
            _logger.Debug(Header.SerializeObject());
            _logger.Debug(InquiryActivityLog.SerializeObject());

            string servicename = "InquiryActivityLogByID";
            var ret = new InquiryActivityLogResponse();
            try
            {
                ret.ResponseStatus = new ResponseData();
                List<ActivityDataItem> output = null;
                string err = "";

                if(Header == null)
                {
                    ret.ResponseStatus.ResponseCode = "CAS-E-101";
                    ret.ResponseStatus.ResponseMessage = "Data required: Header";
                }
                else if (Header.ServiceName != servicename)
                {
                    if (String.IsNullOrEmpty(Header.ServiceName))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-101";
                        ret.ResponseStatus.ResponseMessage = "Data required: ServiceName";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-E-203";
                        ret.ResponseStatus.ResponseMessage = "Invalid Service Name";
                    }
                }
                else
                {
                    AuthenBiz aBiz = new AuthenBiz();
                    if (!aBiz.CheckAuth(Header, out err))
                        ret.ResponseStatus.ResponseMessage = aBiz.ErrorMessage;

                    if (string.IsNullOrEmpty(err))
                    {
                        ServiceLogBiz bz = new ServiceLogBiz();
                        if (!bz.InquiryLog(InquiryActivityLog, Header, out err, out output))
                            ret.ResponseStatus.ResponseMessage = bz.ErrorMessage;
                    }

                    if (string.IsNullOrEmpty(err))
                    {
                        ret.ResponseStatus.ResponseCode = "CAS-I-000";
                        ret.ResponseStatus.ResponseMessage = "Success";
                    }
                    else
                    {
                        ret.ResponseStatus.ResponseCode = err;
                    }
                }
                ret.Header = new LogServiceHeader()
                {
                    ReferenceNo = Header.ReferenceNo,
                    ServiceName = servicename,
                    SystemCode = Header.SystemCode,
                    TransactionDateTime = DateTime.Now
                };
                ret.InquiryActivityDataList = output;

                if (ret.ResponseStatus.ResponseMessage != "Success" || System.Configuration.ConfigurationManager.AppSettings["CreateServiceActivityLogDB"].ToString() == "Y")
                {
                    var ist = HttpContext.Current.Request.InputStream;
                    using (StreamReader sr = new StreamReader(ist, Encoding.UTF8))
                    {
                        ist.Position = 0;
                        XmlSerializer xs = new XmlSerializer(typeof(InquiryActivityLogResponse));
                        using (StringWriter sw = new StringWriter())
                        {
                            xs.Serialize(sw, ret);
                            string error;
                            ActivityLogBiz.InsertServiceActivityLog(new Cas.Dal.CAS_SERVICE_ACTIVITYLOG()
                            {
                                SYSTEM_CODE = Header.SystemCode,
                                RESPONSE_CODE = ret.ResponseStatus.ResponseCode,
                                RESPONSE_MESSAGE = ret.ResponseStatus.ResponseMessage,
                                SERVICE_NAME = ret.Header.ServiceName,
                                REQUEST_IPADDRESS = Context.Request.UserHostAddress,
                                REQUEST_URL = Context.Request.Url.AbsoluteUri,
                                REFERENCE_NO = Header.ReferenceNo,
                                XML_REQUEST = sr.ReadToEnd(),
                                XML_RESPONSE = sw.ToString()
                            }, out error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("E:--CAS-InquiryActivityLogByID----", ex);
            }
            _logger.Debug(ret.SerializeObject());
            _logger.Info("O:--CAS-InquiryActivityLogByID--End--");
            return ret;

        }
    }
}
