using KKCAR.Service.Messages.Common;
using System;
using System.Collections.Generic;
using System.Text;

///<summary>
/// Class Name : TaskInsertStatusResponse
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages.Batch
{
    public class TaskInsertStatusResponse
    {
        public DateTime SchedDateTime { get; set; }
        public long ElapsedTime { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
        public List<TaskInsertStatusResult> TaskResults { get; set; }

        public TaskInsertStatusResponse()
        {
        }

        public TaskInsertStatusResponse(DateTime schedDateTime)
        {
            this.SchedDateTime = schedDateTime;
            this.ResponseStatus = new ResponseStatus();
            this.TaskResults = new List<TaskInsertStatusResult>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(string.Format("Scheduled DateTime= {0}\n", SchedDateTime));
            sb.Append(string.Format("Response Code= {0}\n", ResponseStatus.ResponseCode));
            sb.Append(string.Format("Response Message= {0}\n", ResponseStatus.ResponseMessage));

            if (TaskResults != null && TaskResults.Count > 0)
            {
                for (int i = 0; i < TaskResults.Count; i++)
                {
                    var result = TaskResults[i];
                    sb.Append(string.Format("Row Index= {0}\n", i));
                    sb.Append(result.ToString());
                }
            }
                        
            return sb.ToString();
        }
    }

    public class TaskInsertStatusResult
    {
        public string FileName { get; set; }
        public int TotalRecords { get; set; }
        public int NumOfComplete { get; set; }
        public int NumOfError { get; set; }
        public string ExportFileName { get; set; }
        public ResponseStatus ResponseStatus { get; set; }

        public TaskInsertStatusResult()
        {
            this.ResponseStatus = new ResponseStatus();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("");
            sb.Append(string.Format("Reading file = {0}\n", FileName));
            sb.Append(string.Format("Export file = {0}\n", ExportFileName));
            sb.Append(string.Format("Total Records = {0} records\n", TotalRecords));
            sb.Append(string.Format("Total complete records = {0} records\n", NumOfComplete));
            sb.Append(string.Format("Total error records = {0} records\n", NumOfError));
            sb.Append(string.Format("Error Message = {0}\n", ResponseStatus.ResponseMessage));
            return sb.ToString();
        }
    }
}
