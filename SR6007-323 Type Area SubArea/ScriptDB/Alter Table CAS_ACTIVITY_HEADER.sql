alter table CAS_ACTIVITY_HEADER
add  TYPE_NAME varchar2(100);

alter table CAS_ACTIVITY_HEADER
add AREA_NAME varchar2(100);

alter table CAS_ACTIVITY_HEADER
add SUBAREA_NAME varchar2(100);

alter table CAS_ACTIVITY_HEADER
add ACTIVITY_TYPE_NAME varchar2(100);


update CAS_ACTIVITY_HEADER h
set h.TYPE_NAME=(select t.type_name from cas_type t where t.type_id=h.type_id and rownum=1  );


update CAS_ACTIVITY_HEADER h
set h.AREA_NAME=(select a.area_name from CAS_AREA a where a.area_id=h.area_id and rownum=1  );

update CAS_ACTIVITY_HEADER h
set h.SUBAREA_NAME=(select s.subarea_name from CAS_SUBAREA s where s.subarea_id=h.subarea_id and rownum=1  );

update CAS_ACTIVITY_HEADER h
set h.ACTIVITY_TYPE_NAME=(select ac.activity_type_name from CAS_ACTIVITY_TYPE ac where ac.activity_type_id=h.activity_type_id and rownum=1  );

commit;







select activity_id,type_id,type_name
from cas_activity_header
where type_id<>6




select * from cas_type