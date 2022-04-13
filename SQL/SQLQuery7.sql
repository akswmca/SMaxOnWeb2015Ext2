
select top 100 o.id, p.IACode + '.' + Cast(p.Registrationno As Varchar(10)) As MaxID, cm.Name As Company, p.Title + ' ' + p.Firstname + ' ' + p.MiddleName + Case When IsNull(p.MiddleName,'')<>'' Then ' ' Else '' End + p.LastName As Patient, p.PPhone, p.Pcellno, p.PEMail As Email, Convert(Varchar(20), o.Datetime,105) As BillDate, o.BillNo, d.Amount As Amount, d.discountamount As DiscountAmount, di.Name As DiscountReason, l.Address3 As Location, p.Address1 + ' ' + p.Address2 + ' ' + p.Address3 As Localaddress, c.name As Cityname, rd.Name DoctorName, 
isnull((case when d.serviceid=25 then 
	(select top 1 s.name from m_specialisation s(nolock), L_Doctorspecialisation e(nolock)  where s.id=e.SpecialisationId and e.EmpId=d.itemid)
	else '' end),'') As SpecName,
'' As PaymentType, d.ServiceName, d.ItemName,
isnull((case when d.serviceid=25 then 
	(select top 1 dm.name from m_department dm(nolock), m_employee e(nolock)  where dm.id=e.DepartmentId and e.id=d.itemid)
	else '' end),'') As department,  
cc.Name, dbo.Fn_GetAge(p.DateOfBirth,default) As Age, Convert(Varchar(20), o.Datetime,105) As AdmissionDate, Convert(Varchar(20), o.Datetime,105) As DischargeDateTime

from d_opbill d(nolock) 
 join o_opbill o(nolock) on o.id=d.OPBillId
 join m_patient p(nolock) on p.iacode=o.iacode and p.registrationno=o.registrationno  
 join m_city c (nolock) on p.pcity=c.id 
 join M_organization l(nolock) on o.HSPLocationId=l.id
 left join M_ReferralDoctor rd(nolock) on o.RefDoctorId=rd.id
 left join m_specialisation sm(nolock) on rd.Specialisation=sm.id
 Left join m_company cm(nolock) on o.CompanyId=cm.Id  
 join m_companychannel cc(nolock) on cm.ChannelId=cc.id
 left join m_discountreasons di(nolock) on d.OldOPBillId=di.id
 --where o.id>@billId
 order by d.id desc




