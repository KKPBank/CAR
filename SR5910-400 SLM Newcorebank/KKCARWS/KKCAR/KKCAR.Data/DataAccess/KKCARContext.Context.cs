﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class KKCARContextContainer : DbContext
    {
        public KKCARContextContainer()
            : base("name=KKCARContextContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CAR_BATCH> CAR_BATCH { get; set; }
        public virtual DbSet<CAR_BATCH_LOG> CAR_BATCH_LOG { get; set; }
        public virtual DbSet<CAR_BATCH_LOG_DETAIL> CAR_BATCH_LOG_DETAIL { get; set; }
        public virtual DbSet<CAR_STATUS_HISTORY> CAR_STATUS_HISTORY { get; set; }
        public virtual DbSet<CAR_STATUS_MASTER> CAR_STATUS_MASTER { get; set; }
        public virtual DbSet<CAR_STATUS_REFFERENCE> CAR_STATUS_REFFERENCE { get; set; }
        public virtual DbSet<CAS_ACTIVITY_DETAIL> CAS_ACTIVITY_DETAIL { get; set; }
        public virtual DbSet<CAS_ACTIVITY_TYPE> CAS_ACTIVITY_TYPE { get; set; }
        public virtual DbSet<CAS_AREA> CAS_AREA { get; set; }
        public virtual DbSet<CAS_CAMPAIGN> CAS_CAMPAIGN { get; set; }
        public virtual DbSet<CAS_CHANNEL> CAS_CHANNEL { get; set; }
        public virtual DbSet<CAS_PRODUCT> CAS_PRODUCT { get; set; }
        public virtual DbSet<CAS_PRODUCT_GROUP> CAS_PRODUCT_GROUP { get; set; }
        public virtual DbSet<CAS_ROLE> CAS_ROLE { get; set; }
        public virtual DbSet<CAS_SERVICE_ACTIVITYLOG> CAS_SERVICE_ACTIVITYLOG { get; set; }
        public virtual DbSet<CAS_SUBAREA> CAS_SUBAREA { get; set; }
        public virtual DbSet<CAS_SUBSCRIPTION_TYPE> CAS_SUBSCRIPTION_TYPE { get; set; }
        public virtual DbSet<CAS_SYSTEM> CAS_SYSTEM { get; set; }
        public virtual DbSet<CAS_TYPE> CAS_TYPE { get; set; }
        public virtual DbSet<CAS_SUBSCRIPTION_CARDTYPE> CAS_SUBSCRIPTION_CARDTYPE { get; set; }
        public virtual DbSet<CAR_SYS_STATUS_CBS_FILE> CAR_SYS_STATUS_CBS_FILE { get; set; }
        public virtual DbSet<CAR_SYS_STATUS_CBS_FILE_DATA> CAR_SYS_STATUS_CBS_FILE_DATA { get; set; }
        public virtual DbSet<CAR_SEARCH_REFAPP> CAR_SEARCH_REFAPP { get; set; }
        public virtual DbSet<CAS_SEARCH_CONTRACT> CAS_SEARCH_CONTRACT { get; set; }
        public virtual DbSet<CAS_SEARCH_LEAD> CAS_SEARCH_LEAD { get; set; }
        public virtual DbSet<CAS_SEARCH_NON_CUSTOMER> CAS_SEARCH_NON_CUSTOMER { get; set; }
        public virtual DbSet<CAS_SEARCH_SR> CAS_SEARCH_SR { get; set; }
        public virtual DbSet<CAS_SEARCH_SUBSCRIPTION> CAS_SEARCH_SUBSCRIPTION { get; set; }
        public virtual DbSet<CAS_SEARCH_TICKET> CAS_SEARCH_TICKET { get; set; }
        public virtual DbSet<CAS_ACTIVITY_HEADER> CAS_ACTIVITY_HEADER { get; set; }
        public virtual DbSet<CAR_BATCH_SYSTEM_MAPPING> CAR_BATCH_SYSTEM_MAPPING { get; set; }
        public virtual DbSet<CAR_SYS_CBS_FILE_MAPPING> CAR_SYS_CBS_FILE_MAPPING { get; set; }
    }
}