using System;

///<summary>
/// Class Name : Header
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date         Author           Description
/// ----         ------           -----------
///</remarks>
namespace KKCAR.Service.Messages.Common
{
    public class Header
    {
        public string ReferenceNo { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ServiceName { get; set; }
        public string SystemCode { get; set; }
        public string SecurityKey { get; set; }
        public string ChannelID { get; set; }
    }
}
