
/*==============================================================*/
/* Table: CAR_SYS_CBS_FILE_MAPPING                              */
/*==============================================================*/
create table CAR_SYS_CBS_FILE_MAPPING (
   CAR_SYS_CBS_ID       number                not null,
   SYS_CBS              varchar2(50),
   SYS_CAR              varchar2(50),
   constraint PK_CAR_SYS_CBS_FILE_MAPPING primary key (CAR_SYS_CBS_ID)
)
   tablespace CARTXNDATATBS;

comment on table CAR_SYS_CBS_FILE_MAPPING is
'������ Mapping �����к������ҧ CBS �Ѻ CAR';

comment on column CAR_SYS_CBS_FILE_MAPPING.CAR_SYS_CBS_ID is
'Primary Key';

comment on column CAR_SYS_CBS_FILE_MAPPING.SYS_CBS is
'�����к�����Ҩҡ Text file �ͧ CBS';

comment on column CAR_SYS_CBS_FILE_MAPPING.SYS_CAR is
'�����к����������к� CAR';

/*==============================================================*/
/* Index: IDX_CAR_SYS_CBS_FILE_MAPPING1                         */
/*==============================================================*/
create index IDX_CAR_SYS_CBS_FILE_MAPPING1 on CAR_SYS_CBS_FILE_MAPPING (
   SYS_CBS ASC
)
tablespace CARTXNDATATBS;




CREATE SEQUENCE CAR.SEQ_CAR_SYS_CBS_FILE_MAPPING
  START WITH 1
  MAXVALUE 9999999999999999999999999999
  MINVALUE 0
  NOCYCLE
  NOCACHE
  NOORDER;