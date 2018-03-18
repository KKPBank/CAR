
alter table CAR.CAR_SEARCH_REFAPP
add SUBSCRIPTION_TYPE_ID_ varchar2(100);

update CAR.CAR_SEARCH_REFAPP
set SUBSCRIPTION_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

alter table CAR.CAR_SEARCH_REFAPP
modify SUBSCRIPTION_TYPE_ID_ varchar2(100) not null;


--Drop Current Primary Key
alter table CAR.CAR_SEARCH_REFAPP
drop primary key cascade;


--Drop Column
alter table CAR.CAR_SEARCH_REFAPP
drop column SUBSCRIPTION_TYPE_ID cascade constraints;

--Rename Temp Column
alter table CAR.CAR_SEARCH_REFAPP
rename column SUBSCRIPTION_TYPE_ID_ to SUBSCRIPTION_TYPE_ID;


--Create Primary Key
ALTER TABLE CAR.CAR_SEARCH_REFAPP
 ADD CONSTRAINT CAR_SEARCH_REFAPP_PK
 PRIMARY KEY
 (REFERENCE_APP_ID, SUBSCRIPTION_ID, SUBSCRIPTION_TYPE_ID)
 USING INDEX tablespace  "CARMASDATATBS";