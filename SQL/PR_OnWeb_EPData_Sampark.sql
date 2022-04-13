USE [MAXHIS_RESTORE]
GO
/****** Object:  StoredProcedure [dbo].[pr_OnWeb_EPData_Sampark]    Script Date: 13-04-2022 12:15:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- pr_OnWeb_EPData_Sampark 'SKCT.438725'
ALTER procedure [dbo].[pr_OnWeb_EPData_Sampark] @MaxId nvarchar(50)
as
DECLARE @Iacode VARCHAR(6)           
DECLARE @Regno INT         
SELECT @Iacode = SUBSTRING(@MaxId, 0, CHARINDEX('.', @MaxId)), @Regno = SUBSTRING(@MaxId, CHARINDEX('.', @MaxId) + 1, LEN(@MaxId) + 1)     

select Top 1 ProvisionalDiagnosis
from o_opvisit d 
left join M_Employee e on e.id=d.DoctorId 
left join m_department de on de.id=e.DepartmentId
where e.DepartmentId in (61,126,170) 
and d.IACode=@Iacode and d.RegistrationNo=@Regno
order by d.id desc



