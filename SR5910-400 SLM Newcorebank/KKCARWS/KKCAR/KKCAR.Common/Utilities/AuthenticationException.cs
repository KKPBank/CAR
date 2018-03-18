using System;

///<summary>
/// Class Name : AuthenticationException
/// Purpose    : -
/// Author     : Neda Peyrone
///</summary>
///<remarks>
/// Change History:
/// Date            Author          Description
/// ------          --------        -----------
///</remarks>
namespace KKCAR.Common.Utilities
{
    [Serializable]
    public class AuthenticationException : Exception
    {
        public AuthenticationException()
        {
        }

        public AuthenticationException(string message) : base(message)
        {
        }

        public AuthenticationException(string format, params object[] args) : base(string.Format(format, args))
        {
        }

        public AuthenticationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AuthenticationException(string format, Exception innerException, params object[] args) : base(string.Format(format, args), innerException)
        {
        }
    }
}