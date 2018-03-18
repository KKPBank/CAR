
alter table CAR.CAS_SEARCH_TICKET
add SUBSCRIPTION_TYPE_ID_ varchar2(100);

update CAR.CAS_SEARCH_TICKET
set SUBSCRIPTION_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

alter table CAR.CAS_SEARCH_TICKET
modify  SUBSCRIPTION_TYPE_ID_ varchar2(100) not null;


--Drop Current Primary Key
alter table CAR.CAS_SEARCH_TICKET
drop primary key cascade;


--Drop Column
alter table CAR.CAS_SEARCH_TICKET
drop column SUBSCRIPTION_TYPE_ID cascade constraints;

--Rename Temp Column
alter table CAR.CAS_SEARCH_TICKET
rename column SUBSCRIPTION_TYPE_ID_ to SUBSCRIPTION_TYPE_ID;



--Create Primary Key
ALTER TABLE CAR.CAS_SEARCH_TICKET
 ADD CONSTRAINT  CAS_SEARCH_TICKET_PK
 PRIMARY KEY
 (TICKET_ID, SUBSCRIPTION_ID, SUBSCRIPTION_TYPE_ID)
 USING INDEX tablespace  "CARMASDATATBS";