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
    
    public partial class CAR_BATCH_LOG
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CAR_BATCH_LOG()
        {
            this.CAR_BATCH_LOG_DETAIL = new HashSet<CAR_BATCH_LOG_DETAIL>();
        }
    
        public decimal BATCH_LOG_ID { get; set; }
        public decimal BATCH_ID { get; set; }
        public string BATCH_DATE { get; set; }
        public Nullable<decimal> BATCH_ROUND { get; set; }
        public string SYSTEM_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string REFERENCE_CODE { get; set; }
        public Nullable<System.DateTime> TRANSACTION_DATE { get; set; }
        public string FILE_NAME { get; set; }
        public System.DateTime START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }
        public Nullable<decimal> TOTAL_HEADER { get; set; }
        public Nullable<decimal> TOTAL_DETAIL { get; set; }
        public Nullable<decimal> TOTAL_COMPLETE { get; set; }
        public Nullable<decimal> TOTAL_FAIL { get; set; }
        public string STATUS { get; set; }
        public string ERROR_DETAIL { get; set; }
        public string RERUN_PATH { get; set; }
    
        public virtual CAR_BATCH CAR_BATCH { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAR_BATCH_LOG_DETAIL> CAR_BATCH_LOG_DETAIL { get; set; }
    }
}
