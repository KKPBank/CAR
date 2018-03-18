using System;

///<summary>
/// Class Name : CustomException
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
    [Serializable]
    public class CustomException : Exception
    {
        public CustomException()
        {
        }

        public CustomException(string message) : base(message)
        {
        }

        public CustomException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public CustomException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException)
        {
        }
    }
}