using Cas.Dal.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cas.Dal.BatchData
{

    #region BatchHeader
    public class BatchLogServiceHeader
    {
        public string ReferenceCode { get; set; }
        public string FileName { get; set; }
        public DateTime? CreateDate { get; set; }
        public decimal? CurrentSequence { get; set; }
        public decimal? TotalSequence { get; set; }
        public decimal? TotalRecord { get; set; }
        public string SystemCode { get; set; }
        public string SecurityKey { get; set; }
    }
    public class BatchLogServiceHeaderResponse
    {
        public string ReferenceCode { get; set; }
        public string FileName { get; set; }
        public string CreateDate { get; set; }
        public decimal? CurrentSequence { get; set; }
        public decimal? TotalSequence { get; set; }
        public decimal? TotalRecord { get; set; }
        public string SystemCode { get; set; }
        public string SecurityKey { get; set; }
    }
    #endregion

    #region Data
    public class BatchResponseData
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }
    #endregion

    #region Request
    public class BatchCreateActivityLogReqest
    {
        public BatchLogServiceHeader Header { get; set; }
        public List<CreateActivityLogData> Body { get; set; }
    }

    public class HTTPCreateActivityLogRequest
    {
        public string SystemCode { get; set; }
        public string ServiceName { get; set; }
        public string DataDate { get; set; }
        public string Path { get; set; }
        public bool GetResult { get; set; }
    }
    #endregion

    #region Response
    public class BatchCreateActivityLogBody
    {
        public string ReferenceNo { get; set; }
        public string ChannelId { get; set; }
        public BatchResponseData ResponseStatus { get; set; }
    }

    public class BatchCreateActivityLogResponse
    {
        public BatchLogServiceHeaderResponse Header { get; set; }
        public List<BatchCreateActivityLogBody> Body { get; set; }
    } 

    public class HttpCreateActivityLogResponse
    {
        BatchResponseData ResponseStatus { get; set; }
    }
    #endregion
}
