--Alter Column SUBSCRIPT_TYPE_ID In CAS_SEARCH_NON_CUSTOMER
alter table CAR.CAS_SEARCH_NON_CUSTOMER
add SUBSCRIPT_TYPE_ID_ varchar2(25);

update CAR.CAS_SEARCH_NON_CUSTOMER
set SUBSCRIPT_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

alter table CAR.CAS_SEARCH_TICKET
modify  SUBSCRIPT_TYPE_ID_ varchar2(100) not null;


--Drop Current Constraints
alter table CAR.CAS_SEARCH_NON_CUSTOMER
drop primary key cascade;

--Drop Column
alter table CAR.CAS_SEARCH_NON_CUSTOMER
drop column SUBSCRIPTION_TYPE_ID cascade constraints;


--Rename Temp Column
alter table CAR.CAS_SEARCH_NON_CUSTOMER
rename column SUBSCRIPT_TYPE_ID_ to SUBSCRIPTION_TYPE_ID;

--Create Primary Key
ALTER TABLE CAR.CAS_SEARCH_NON_CUSTOMER
 ADD CONSTRAINT CAS_SEARCH_NON_CUSTOMER_PK
 PRIMARY KEY
 (NON_CUSTOMER_ID, SUBSCRIPTION_ID, SUBSCRIPTION_TYPE_ID)
 USING INDEX tablespace  "CARMASDATATBS";