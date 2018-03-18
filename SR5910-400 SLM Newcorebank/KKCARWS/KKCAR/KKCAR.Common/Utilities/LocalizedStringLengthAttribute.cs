using System.ComponentModel.DataAnnotations;

///<summary>
/// Class Name : LocalizedStringLengthAttribute
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
    public sealed class LocalizedStringLengthAttribute : StringLengthAttribute
    {
        private string _displayName;

        public LocalizedStringLengthAttribute(int maximumLength) : base(maximumLength)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            _displayName = validationContext.DisplayName;
            return base.IsValid(value, validationContext);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, _displayName, MaximumLength);
        }
    }
}