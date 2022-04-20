USE [max_bi]
GO
/****** Object:  StoredProcedure [dbo].[PR_OnWeb_SSIScreening]    Script Date: 19-04-2022 17:08:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  Dhirendra K Singh  
-- Create date: 19-April-2022  
-- Description: OnWeb_SSIScreening
-- =============================================  
ALTER procedure [dbo].[PR_OnWeb_SSIScreening]
 @FromDate Varchar(50),
 @ToDate Varchar(50) 
as  
-- PR_OnWeb_SSIScreening '01-Jan-2022','10-Jan-2022'  
begin  
SET NOCOUNT ON;  

Select distinct ipid,bill_type,organismName,testresult as test_result,swabtestdate,testnames,address3,
   his_order_id,itemname,his_hospital_location_id,surgeryorderday,patient_ssn,patient_name,gender,AGE,service_name,
   care_provider_name 
  from(

		Select distinct  OrganismName,testresult,procedure_date as swabtestdate
		,PROCEDURE_NAME as TestNames
		,dhhl.ADDRESS3,di.HIS_ORDER_ID,di.IPID as ipid,dii.ITEM_NAME as itemname,di.HIS_HOSPITAL_LOCATION_ID
		,(dd.FULLDATE+' '+dt.FULLTIME_CHAR) as surgeryorderday,fp.PATIENT_SSN,dpd.PATIENT_NAME,dss.SEX as gender,
		CONVERT(int,ROUND(DATEDIFF(hour,DB.FULLDATE,GETDATE())/8766.0,0)) AS Age
		,ds.SERVICE_NAME,dc.CARE_PROVIDER_NAME,dbt.BILL_TYPE


		from F_BILL_DETAILS(nolock) di 
		JOIN d_date(nolock) dd ON ( dd.IDD_DATE = di.ORDER_DATE_ID )
		join d_time(nolock) dt on (dt.IDD_TIME=di.ORDER_TIME_ID)
		join D_SERVICES(nolock) ds on di.SERVICE_ID=ds.IDD_SERVICES
		join f_patient(nolock) fp on di.PATIENT_ID = fp.IDF_PATIENT
		left join D_BILL_TYPE(nolock) dbt on di.BILL_TYPE_ID=dbt.IDD_BILL_TYPE
		left join d_care_providers(nolock) dc on di.CARE_PROVIDER_ID=dc.IDD_CARE_PROVIDERS
		left join d_sex(nolock) dss on fp.SEX_ID=dss.IDD_SEX
		join d_patient_details(nolock) dpd on fp.PATIENT_DETAIL_ID=dpd.IDD_PATIENT_DETAILS
		left join d_item(nolock) dii on di.ITEM_ID=dii.IDD_ITEM
		left join d_his_hospital_location(nolock) dhhl on di.HIS_HOSPITAL_LOCATION_ID=dhhl.IDD_HIS_HOSPITAL_LOCATION
		left join d_date  DB(NOLOCK) on (fp.DATE_OF_BIRTH_ID =   DB.IDD_DATE)
		join (
			Select  distinct   fr.result as testresult,case when di.IPID =0 then 0 else a.IPID end as procedure_ipid,
			di.HIS_ORDER_ID,(dd.FULLDATE+' '+dt.FULLTIME_CHAR) as procedure_date,
			( SELECT TOP 1 ORGANISM_NAME FROM  d_Organism (NOLOCK) WHERE IDD_ORGANISM = (case when di.IPID =0 then b.ORGANISM_ID else a.ORGANISM_ID end)
			) AS OrganismName ,di.patient_id,dp.PROCEDURE_NAME

			from F_BILL_DETAILS(nolock) di 
			JOIN d_date(nolock) dd ON ( dd.IDD_DATE = di.BILL_DATE_ID )
			join d_time(nolock) dt on (dt.IDD_TIME=di.BILL_TIME_ID)
			join  D_PROCEDURE(nolock) dp  on di.PROCEDURE_ID=dp.IDD_PROCEDURE
			LEFT JOIN F_IPLABCULTURE(nolock) A ON di.HIS_ORDER_ID = a.his_order_id AND di.ipid = a.ipid AND di.HIS_HOSPITAL_LOCATION_ID = a.his_hospital_location_id
			left join F_OPLABCULTURE(nolock) B on di.HIS_ORDER_ID = b.his_order_id AND di.ipid = 0 AND di.HIS_HOSPITAL_LOCATION_ID = b.his_hospital_location_id
			left join f_OP_IP_TEST_RESULTS(nolock) fr on di.HIS_ORDER_ID=fr.orderid
			where 
			dp.PROCEDURE_NAME in ('Wound Swab - Culture & Sensitivity','Pus - AFB Culture','Pus - Fungus Culture','Pus - Culture & Sensitivity')
		
		)pro on pro.patient_id =di.patient_id
		where  ds.SERVICE_NAME in ('Surgery','Cath Procedures','Maternity','OT Procedures','Surgery Package')
		and ( (DATEDIFF(day, dd.fulldate, procedure_date) +1)  between 2 and 30)
		and dd.fulldate <> procedure_date
		and  dd.fulldate < procedure_date
		and di.DELETED = 0
		
)main
   where swabtestdate >= Convert(DateTime,@FromDate)
   and swabtestdate <= Convert(DateTime,@ToDate)
   

   group by
   ipid,bill_type,organismName,testresult,swabtestdate,testnames,address3,
   his_order_id,itemname,his_hospital_location_id,surgeryorderday,patient_ssn,patient_name,gender,AGE,service_name,
   care_provider_name 
End