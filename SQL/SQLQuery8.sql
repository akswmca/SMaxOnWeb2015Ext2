

Select * From Company_Channel_Master Where CompanyId=4794


Select * From m_city

Select * From m_company

Select top 1000 * From d_opbill where opbillid=2 order by id desc

Select top 100  * From o_opbill

Select * From m_service

--Select top 10 * From vw_Item
Select top 10 * From M_employee where id=13250 [ if service id=25]
Select * From M_procedure where id in (62502,13250) [service id other than 25]

Select * From M_ReferralDoctor

Select * From M_organization

Select * From m_specialisation

Select * From m_department

Select * From m_mop


Select top 10* From m_companychannel

select * from sysobjects where name like '%channel%'
