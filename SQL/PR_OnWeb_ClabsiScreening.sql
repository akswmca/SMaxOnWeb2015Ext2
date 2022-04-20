USE [max_bi]
GO
/****** Object:  StoredProcedure [dbo].[PR_OnWeb_ClabsiScreening]    Script Date: 19-04-2022 17:07:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  Dhirendra K Singh  
-- Create date: 19-April-2022  
-- Description: OnWeb_ClabsiScreening  
-- =============================================  
ALTER procedure [dbo].[PR_OnWeb_ClabsiScreening]
 @FromDate Varchar(50),
 @ToDate Varchar(50)  
as  
-- PR_OnWeb_ClabsiScreening '01/01/2022','01/10/2022'  
begin  
SET NOCOUNT ON;  

select a.ipid,a.patient_ssn,a.age,pr.PROCEDURE_NAME as centralline,pr1.PROCEDURE_NAME as bloodculture,hl.ADDRESS3
,o.ORGANISM_NAME,st.STATION_NAME,se.SEX,d1.FULLDATE as admissiondate,d2.FULLDATE as centrallinedate
,d3.FULLDATE as bloodculturedate,cp.CARE_PROVIDER_NAME,dep.DEPARTMENT_NAME,pd.PATIENT_NAME,vt.VITAL_TYPE,vv.VITAL_VALUE_NUMERIC,
d4.FULLDATE as vital_enter_date,d5.FULLDATE as test_order_date
 from F_CLABSI_REPORT a join 
D_PROCEDURE pr on (a.C_PROCEDURE_ID=pr.IDD_PROCEDURE)
join D_PROCEDURE pr1 on (a.CULTURE_TEST_ID=pr1.IDD_PROCEDURE)
join d_his_hospital_location hl on (hl.IDD_HIS_HOSPITAL_LOCATION=a.HIS_HOSPITAL_LOCATION_ID)
join D_ORGANISM o on (o.IDD_ORGANISM=a.ORGANISM_ID)
join d_station st on (st.IDD_STATION=a.STATION_ID)
join d_sex se on (se.IDD_SEX=a.SEX_ID)
join d_date d1 on (d1.IDD_DATE=a.ADMISSION_DATE_ID)
join d_date d2 on (d2.IDD_DATE=a.PROCEDURE_DATE_ID)
join d_date d3 on (d3.IDD_DATE=a.TEST_DATE_ID)
join d_date d5 on (d5.IDD_DATE=a.test_order_date_id)
join d_care_providers cp on (cp.IDD_CARE_PROVIDERS=a.CARE_PROVIDER_ID)
join d_department dep on (dep.IDD_DEPARTMENT=a.DEPARTMENT_ID)
join d_patient_details pd on (pd.IDD_PATIENT_DETAILS = a.PATIENT_DETAIL_ID)
left join d_vital_type vt on (vt.IDD_VITAL_TYPE=a.VITAL_TYPE_ID)
left join d_vital_value vv on (vv.IDD_VITAL_VALUE=a.VITAL_VALUE_ID)
left join d_date d4 on (d4.IDD_DATE=a.VITAL_ENTERED_DATE_ID)
where 
a.test_order_date_id>=a.procedure_date_id+2 and
a.test_date_id>=a.test_order_date_id and
d5.fulldate >=Convert(DateTime,@FromDate)
and d5.fulldate <=Convert(DateTime,@ToDate)
group by
a.ipid,a.patient_ssn,a.age,pr.PROCEDURE_NAME ,pr1.PROCEDURE_NAME ,hl.ADDRESS3
,o.ORGANISM_NAME,st.STATION_NAME,se.SEX,d1.FULLDATE ,d2.FULLDATE 
,d3.FULLDATE ,cp.CARE_PROVIDER_NAME,dep.DEPARTMENT_NAME,pd.PATIENT_NAME,vt.VITAL_TYPE,vv.VITAL_VALUE_NUMERIC,
d4.FULLDATE ,d5.FULLDATE 
End