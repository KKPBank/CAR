//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KKCAR.Data.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAR_SEARCH_REFAPP
    {
        public string REFERENCE_APP_ID { get; set; }
        public string SUBSCRIPTION_ID { get; set; }
        public decimal SUBSCRIPTION_TYPE_ID { get; set; }
    
        public virtual CAS_SEARCH_SUBSCRIPTION CAS_SEARCH_SUBSCRIPTION { get; set; }
    }
}
