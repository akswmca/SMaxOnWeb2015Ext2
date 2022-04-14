USE [MAXHIS_RESTORE]
GO
/****** Object:  StoredProcedure [dbo].[PR_OnWeb_EPData_Sampark]    Script Date: 13-04-2022 18:05:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================  
-- Author:  Dhirendra K Singh  
-- Create date: 08-March-2022  
-- Description: OnWeb_OPBillDetail  
-- =============================================  
ALTER procedure [dbo].[PR_OnWeb_EPData_Sampark] @MaxId nvarchar(50)  
as  
-- PR_OnWeb_EPData_Sampark 'SKCT.438725'  
begin  
SET NOCOUNT ON;  
DECLARE @Iacode VARCHAR(6)             
DECLARE @Regno INT           
SELECT @Iacode = SUBSTRING(@MaxId, 0, CHARINDEX('.', @MaxId)), @Regno = SUBSTRING(@MaxId, CHARINDEX('.', @MaxId) + 1, LEN(@MaxId) + 1)       
  
DECLARE @DMG nvarchar(100)
set @DMG=''  
exec PR_TMS_GetDMGForEP @regNo=@Regno,@Iacode=@Iacode,@ReturnValue=@DMG output    

select Top 1 ProvisionalDiagnosis As Diagnosis, '' As SurgeryName, @DMG As DMG  
from o_opvisit d   
left join M_Employee e on e.id=d.DoctorId   
left join m_department de on de.id=e.DepartmentId  
where e.DepartmentId in (61,126,170)   
and d.IACode=@Iacode and d.RegistrationNo=@Regno  
order by d.id desc  
  
end  
  