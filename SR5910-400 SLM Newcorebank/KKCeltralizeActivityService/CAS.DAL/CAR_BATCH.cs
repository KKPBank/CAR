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
    
    public partial class CAR_BATCH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CAR_BATCH()
        {
            this.CAR_BATCH_SYSTEM_MAPPING = new HashSet<CAR_BATCH_SYSTEM_MAPPING>();
            this.CAR_BATCH_LOG = new HashSet<CAR_BATCH_LOG>();
        }
    
        public decimal BATCH_ID { get; set; }
        public string BATCH_CODE { get; set; }
        public string BATCH_NAME { get; set; }
        public Nullable<System.DateTime> START_TIME { get; set; }
        public Nullable<System.DateTime> END_TIME { get; set; }
        public string STATUS { get; set; }
        public decimal IS_DELETE { get; set; }
        public decimal IS_RERUN { get; set; }
        public string MAIN_URL { get; set; }
        public string MAIN_RERUN_URL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAR_BATCH_SYSTEM_MAPPING> CAR_BATCH_SYSTEM_MAPPING { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CAR_BATCH_LOG> CAR_BATCH_LOG { get; set; }
    }
}