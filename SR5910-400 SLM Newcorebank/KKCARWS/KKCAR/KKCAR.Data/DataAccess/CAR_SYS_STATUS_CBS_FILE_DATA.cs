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
    
    public partial class CAR_SYS_STATUS_CBS_FILE_DATA
    {
        public decimal CAR_SYS_STATUS_CBS_FILE_DATAID { get; set; }
        public decimal CAR_SYS_STATUS_CBS_FILE_ID { get; set; }
        public string CAR_REFERENCE_CODE { get; set; }
        public string CAR_FILE_NAME { get; set; }
        public string CAR_CREATE_DATE { get; set; }
        public decimal CAR_CURRENT_SEQUENCE { get; set; }
        public decimal CAR_TOTAL_SEQUENCE { get; set; }
        public decimal CAR_TOTAL_RECORD { get; set; }
        public string CAR_SYSTEM_CODE { get; set; }
        public string CAR_REFERENCE_NO { get; set; }
        public string CAR_CHANNEL_ID { get; set; }
        public string CAR_STATUS_DATE_TIME { get; set; }
        public string CAR_SUBSCRIPTION_ID { get; set; }
        public string CAR_SUBSCRIPT_CUS_TYPE { get; set; }
        public string CAR_SUBSCRIPT_CARD_TYPE { get; set; }
        public string CAR_OWNER_SYSTEM_ID { get; set; }
        public string CAR_OWNER_SYSTEM_CODE { get; set; }
        public string CAR_REF_SYSTEM_ID { get; set; }
        public string CAR_REF_SYSTEM_CODE { get; set; }
        public string CAR_STATUS { get; set; }
        public string CAR_STATUS_NAME { get; set; }
    
        public virtual CAR_SYS_STATUS_CBS_FILE CAR_SYS_STATUS_CBS_FILE { get; set; }
    }
}