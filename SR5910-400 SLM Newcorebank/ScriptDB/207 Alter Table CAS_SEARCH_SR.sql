alter table CAR.CAS_SEARCH_SR
add SUBSCRIPTION_TYPE_ID_ varchar2(100);

update CAR.CAS_SEARCH_SR
set SUBSCRIPTION_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

alter table CAR.CAS_SEARCH_SR
modify  SUBSCRIPTION_TYPE_ID_ varchar2(100) not null;



--Drop Current Primary Key
alter table CAR.CAS_SEARCH_SR
drop primary key cascade;


--Drop Column
alter table CAR.CAS_SEARCH_SR
drop column SUBSCRIPTION_TYPE_ID cascade constraints;

--Rename Temp Column
alter table CAR.CAS_SEARCH_SR
rename column SUBSCRIPTION_TYPE_ID_ to SUBSCRIPTION_TYPE_ID;




--Create Primary Key
ALTER TABLE CAR.CAS_SEARCH_SR
 ADD CONSTRAINT  CAS_SEARCH_SR_PK
 PRIMARY KEY
 (SR_ID, SUBSCRIPTION_ID, SUBSCRIPTION_TYPE_ID)
 USING INDEX tablespace  "CARMASDATATBS";