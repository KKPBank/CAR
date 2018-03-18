/*==============================================================*/
/* Table: CAR_SYS_STATUS_CBS_FILE                               */
/*==============================================================*/
create table CAR_SYS_STATUS_CBS_FILE (
   CAR_SYS_STATUS_CBS_FILE_ID NUMBER                not null,
   CAR_FILENAME         VARCHAR2(100)         not null,
   CAR_FILEPATH         VARCHAR2(1000)        not null,
   CAR_FILE_CREATE_DATE DATE                  not null,
   CAR_FILE_PROCESS_TIME DATE                  not null,
   CAR_PROCESS_STATUS   VARCHAR2(100)         not null,
   CAR_PROCESS_ERROR_STEP VARCHAR2(1000),
   constraint PK_CAR_SYS_STATUS_CBS_FILE primary key (CAR_SYS_STATUS_CBS_FILE_ID)
         using index tablespace CARTXNDATATBS,
   constraint AK_KEY_2_CAR_SYS_ unique (CAR_FILENAME)
         using index tablespace CARTXNDATATBS
)
   tablespace CARTXNDATATBS;



CREATE SEQUENCE SEQ_CAR_SYS_STATUS_CBS_FILE
  START WITH 1
  MAXVALUE 9999999999999999999999999999
  MINVALUE 0
  NOCYCLE
  NOCACHE
  NOORDER;

GRANT SELECT ON SEQ_CAR_SYS_STATUS_CBS_FILE TO CAR;