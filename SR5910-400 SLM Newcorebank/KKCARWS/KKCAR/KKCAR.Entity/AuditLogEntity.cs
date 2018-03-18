using System;
using System.Collections.Generic;

///<summary>
/// Class Name : AuditLogEntity
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Entity
{
    public class AuditLogEntity
    {
        public string BatchDate { get; set; }
        public string BatchCode { get; set; }
        public string Filename { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string LogDetail { get; set; }
        public List<LogDetailEntity> LogDetailList { get; set; }
        public LogStatus Status { get; set; }
        public decimal? NumOfTotal { get; set; }
        public decimal? NumOfComplete { get; set; }
        public decimal? NumOfError { get; set; }
        public string RerunPath { get; set; }
        public DateTime TransactionDate { get; set; }
        public string SystemCode { get; set; }
        public string ServiceName { get; set; }
        public string ReferenceCode { get; set; }

        public string StatusDisplay
        {
            get { return Status.ToShortString(); }
        }
    }

    public class LogDetailEntity
    {
        public string ReferenceNo { get; set; }
        public string ChannelId { get; set; }
        public string LogCode { get; set; }
        public string LogDetail { get; set; }
    }

    public enum LogStatus
    {
        Fail = 0,
        Success = 1
    }

    public static class LogStatusExtension
    {
        public static string ToShortString(this LogStatus flag)
        {
            switch (flag)
            {
                case LogStatus.Fail:
                    return "Fail";
                case LogStatus.Success:
                    return "Success";
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public class TraceLogEntity
    {
        public string SystemCode { get; set; }
        public string ServiceName { get; set; }
        public string ReferenceCode { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string RequestUrl { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string IpAddress { get; set; }
        public string XmlRequest { get; set; }
        public string XmlResponse { get; set; }
    }
}
