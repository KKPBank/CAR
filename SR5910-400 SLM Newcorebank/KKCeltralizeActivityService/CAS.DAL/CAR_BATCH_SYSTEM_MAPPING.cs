//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cas.Dal
{
    using System;
    using System.Collections.Generic;
    
    public partial class CAR_BATCH_SYSTEM_MAPPING
    {
        public decimal MAPPING_ID { get; set; }
        public string SYSTEM_ID { get; set; }
        public decimal BATCH_ID { get; set; }
        public string RESPONSE_URL { get; set; }
        public bool IS_BATCH_RESULT { get; set; }
    
        public virtual CAR_BATCH CAR_BATCH { get; set; }
        public virtual CAS_SYSTEM CAS_SYSTEM { get; set; }
    }
}
