USE [max_bi]
GO
/****** Object:  StoredProcedure [dbo].[PR_OnWeb_CautiScreening]    Script Date: 19-04-2022 17:08:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  Dhirendra K Singh  
-- Create date: 19-April-2022  
-- Description: OnWeb_CautiScreening
-- =============================================  
ALTER procedure [dbo].[PR_OnWeb_CautiScreening]
 @FromDate Varchar(50),
 @ToDate Varchar(50)  
as  
-- PR_OnWeb_CautiScreening '01-Jan-2022','10-Jan-2022'  
begin  
SET NOCOUNT ON; 
select a.ipid,a.patient_ssn,a.patient_age,pr.PROCEDURE_NAME as culture_test,pr1.item_NAME as folyscat,hl.ADDRESS3
,st.STATION_NAME,se.SEX,d1.FULLDATE as admissiondate,d2.FULLDATE as folyscathdate
,d3.FULLDATE as urinecultureorderdate,cp.CARE_PROVIDER_NAME,dep.DEPARTMENT_NAME,pd.PATIENT_NAME,vt.VITAL_TYPE,vv.VITAL_VALUE_NUMERIC,
d4.FULLDATE as vital_enter_date,d5.FULLDATE as culturetestresultdate,R.ORGANISM_NAME,pp.PARAMETER_NAME,A.TEST_RESULT
 from f_cauti_report_catheter a 
  left join D_PROCEDURE pr on (a.test_name_id=pr.IDD_PROCEDURE)
 left join D_item_cauti pr1 on (a.item_ID=pr1.IDD_item_cauti)
left join d_his_hospital_location hl on (hl.IDD_HIS_HOSPITAL_LOCATION=a.HIS_HOSPITAL_LOCATION_ID)
--left join D_ORGANISM o on (o.IDD_ORGANISM=a.ORGANISM_ID)
left join d_station st on (st.IDD_STATION=a.STATION_ID)
left join d_sex se on (se.IDD_SEX=a.SEX_ID)
left join d_date d1 on (d1.IDD_DATE=a.ADMISSION_DATE_ID)
left join d_date d2 on (d2.IDD_DATE=a.ITEM_ORDER_DATE_ID)
left join d_date d3 on (d3.IDD_DATE=a.TEST_ORDER_DATE_ID)
left join d_date d5 on (d5.IDD_DATE=a.test_result_date_id)
left join d_care_providers cp on (cp.IDD_CARE_PROVIDERS=a.CARE_PROVIDER_ID)
left join d_department dep on (dep.IDD_DEPARTMENT=a.DEPARTMENT_ID)
left join d_patient_details pd on (pd.IDD_PATIENT_DETAILS = a.PATIENT_DETAIL_ID)
left join D_organism R ON (R.idd_organism=A.TEST_RESULT_ID)
--left join f_result rr on (rr.IPID=a.IPID and rr.HIS_HOSPITAL_LOCATION_ID=a.HIS_HOSPITAL_LOCATION_ID and rr.PARAMETER_ID in (10057,7012,3787))
--left join d_test_result tr on (tr.IDD_TEST_RESULT=rr.TEST_RESULT_ID)
 left join D_PARAMETER pp on (pp.IDD_PARAMETER=A.PARAMETER_ID)
 left join d_vital_type vt on (vt.IDD_VITAL_TYPE=a.VITAL_TYPE_ID)
 left join d_vital_value vv on (vv.IDD_VITAL_VALUE=a.VITAL_VALUE_ID)
 left join d_date d4 on (d4.IDD_DATE=a.VITAL_ENTERED_DATE_ID)
where 
a.TEST_ORDER_DATE_ID>=a.ITEM_ORDER_DATE_ID+2 and a.TEST_RESULT_DATE_ID>=a.TEST_ORDER_DATE_ID
and d3.fulldate >=Convert(DateTime,@FromDate)
and d3.fulldate <=Convert(DateTime,@ToDate)
and lower(pr.PROCEDURE_NAME) like '%urine%'
group by
a.ipid,a.patient_ssn,a.patient_age,pr.PROCEDURE_NAME ,pr1.item_NAME ,hl.ADDRESS3
,st.STATION_NAME,se.SEX,d1.FULLDATE ,d2.FULLDATE 
,d3.FULLDATE ,cp.CARE_PROVIDER_NAME,dep.DEPARTMENT_NAME,pd.PATIENT_NAME,vt.VITAL_TYPE,vv.VITAL_VALUE_NUMERIC,
d4.FULLDATE ,d5.FULLDATE ,R.ORGANISM_NAME,pp.PARAMETER_NAME,A.TEST_RESULT

End