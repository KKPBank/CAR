
/*==============================================================*/
/* Table: CAR_SYS_STATUS_CBS_FILE_DATA                          */
/*==============================================================*/
create table CAR_SYS_STATUS_CBS_FILE_DATA (
   CAR_SYS_STATUS_CBS_FILE_DATAID NUMBER                not null,
   CAR_SYS_STATUS_CBS_FILE_ID NUMBER                not null,
   CAR_REFERENCE_CODE   VARCHAR2(50)          not null,
   CAR_FILE_NAME        VARCHAR2(100)         not null,
   CAR_CREATE_DATE      VARCHAR2(50)          not null,
   CAR_CURRENT_SEQUENCE NUMBER                not null,
   CAR_TOTAL_SEQUENCE   NUMBER                not null,
   CAR_TOTAL_RECORD     NUMBER                not null,
   CAR_SYSTEM_CODE      VARCHAR2(50)          not null,
   CAR_REFERENCE_NO     VARCHAR2(50)          not null,
   CAR_CHANNEL_ID       VARCHAR2(50)          not null,
   CAR_STATUS_DATE_TIME VARCHAR2(50)          not null,
   CAR_SUBSCRIPTION_ID  VARCHAR2(100),
   CAR_SUBSCRIPT_CUS_TYPE VARCHAR2(50),
   CAR_SUBSCRIPT_CARD_TYPE VARCHAR2(50),
   CAR_OWNER_SYSTEM_ID  VARCHAR2(100)         not null,
   CAR_OWNER_SYSTEM_CODE VARCHAR2(50)          not null,
   CAR_REF_SYSTEM_ID    VARCHAR2(100),
   CAR_REF_SYSTEM_CODE  VARCHAR2(100),
   CAR_STATUS           VARCHAR2(50)          not null,
   CAR_STATUS_NAME      VARCHAR2(100)         not null,
   constraint PK_CAR_SYS_STATUS_CBS_FILE_DAT primary key (CAR_SYS_STATUS_CBS_FILE_DATAID)
         using index tablespace CARTXNDATATBS
)
   tablespace CARTXNDATATBS;

alter table CAR_SYS_STATUS_CBS_FILE_DATA
   add constraint FK_CAR_SYS__REFERENCE_CAR_SYS_ foreign key (CAR_SYS_STATUS_CBS_FILE_ID)
      references CAR_SYS_STATUS_CBS_FILE (CAR_SYS_STATUS_CBS_FILE_ID);
      
      
      
      



CREATE SEQUENCE SEQ_CAR_SYS_STATUSCBSFILEDATA
  START WITH 1
  MAXVALUE 9999999999999999999999999999
  MINVALUE 0
  NOCYCLE
  NOCACHE
  NOORDER;
  
  GRANT SELECT ON SEQ_CAR_SYS_STATUSCBSFILEDATA TO CAR;