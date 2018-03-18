using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKCAR.Service.Messages.Batch
{
    public class CARInsertStatusTextfileDataList
    {
        public decimal ExtSysStatusCbsFileId { get; set; }
        public BulkInsertStatusRequest TextFileDataList { get; set; }
    }
}
