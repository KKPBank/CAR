alter table CAR.CAS_SEARCH_LEAD
add SUBSCRIPTION_TYPE_ID_ varchar2(100);

update CAR.CAS_SEARCH_LEAD
set SUBSCRIPTION_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

alter table CAR.CAS_SEARCH_LEAD
modify  SUBSCRIPTION_TYPE_ID_ varchar2(100) not null;




--Drop Current Primary Key
alter table CAR.CAS_SEARCH_LEAD
drop primary key cascade;


--Drop Column
alter table CAR.CAS_SEARCH_LEAD
drop column SUBSCRIPTION_TYPE_ID cascade constraints;

--Rename Temp Column
alter table CAR.CAS_SEARCH_LEAD
rename column SUBSCRIPTION_TYPE_ID_ to SUBSCRIPTION_TYPE_ID;



--Create Primary Key
ALTER TABLE CAR.CAS_SEARCH_LEAD
 ADD CONSTRAINT  CAS_SEARCH_LEAD_PK
 PRIMARY KEY
 (LEAD_ID, SUBSCRIPTION_ID, SUBSCRIPTION_TYPE_ID)
 USING INDEX tablespace  "CARMASDATATBS";