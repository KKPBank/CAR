using System;

///<summary>
/// Class Name : EmailAddress
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
    [Serializable]
    public sealed class EmailAddress
    {
        public EmailAddress()
        {
        }

        public EmailAddress(string address, string name)
        {
            Address = address;
            Name = name;
        }

        public string Address { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}