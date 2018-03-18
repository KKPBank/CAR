--Add Temp Column
alter table CAR.CAS_ACTIVITY_HEADER
add SUBSCRIPTION_TYPE_ID_ varchar2(100);

--Copy Data to Temp column
update CAR.CAS_ACTIVITY_HEADER
set SUBSCRIPTION_TYPE_ID_ = SUBSCRIPTION_TYPE_ID;

commit;

--Update To null
update CAR.CAS_ACTIVITY_HEADER
set SUBSCRIPTION_TYPE_ID=null;

commit;


--Change Data Type
alter table CAR.CAS_ACTIVITY_HEADER
modify SUBSCRIPTION_TYPE_ID varchar2(100);


--Copy Data from Temp To SUBSCRIPTION_TYPE_ID
update CAR.CAS_ACTIVITY_HEADER
set SUBSCRIPTION_TYPE_ID = SUBSCRIPTION_TYPE_ID_;

commit;

--Drop Temp Column
alter table CAR.CAS_ACTIVITY_HEADER
drop column SUBSCRIPTION_TYPE_ID_ cascade constraints;



