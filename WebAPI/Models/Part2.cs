using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;
using System.Runtime.Serialization;
using System.Data;
namespace WebAPI.Models
{
    
    public class InInPatientDetailTPA
    {
        public int IPID { get; set; }
        public int HospitalId { get; set; }
        public string Source { get; set; }
    }
    
    public class InPatientDetailTPADetail
    {
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string AdmitDateTime { get; set; }
        public string EstdDchgDateTime { get; set; }
        public Int32 bedid { get; set; }
        public string BedName { get; set; }
        public string BedTypeName { get; set; }
        public string PrimDocName { get; set; }
        public string SecDocName { get; set; }
        public string PrimDocDepartment { get; set; }
        public string SecDocDepartment { get; set; }
        public string Investigations { get; set; }
        public string MedicalManagement { get; set; }
        public Int32 FirstRoomRent { get; set; }
        public string ProcedureName { get; set; }
        public int ProcedureFee { get; set; }
        public string AdmissionType { get; set; }
        public string PrimDocMobNo { get; set; }
        public string PatMobileNo { get; set; }
        public string PatAddress { get; set; }

    }

    public class InPatientDetailTPADetailAll : Base
    {
        public List<InPatientDetailTPADetail> RetVal { get; set; }
        
        public void GetSampleCollectionCharges(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<InPatientDetailTPADetail>(objIDataReader);
        }
    }

    public class OutInPatientDetailTPARet : Base
    {
        public string PatientName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string DOA { get; set; }
        public string DOD { get; set; }
        public string RoomType { get; set; }
        public string TreatingDoctorName { get; set; }
        public string Department { get; set; }
        public string InvestigationDetails { get; set; }
        public string MedicalManagementDetails { get; set; }
        public Int32 RoomRent { get; set; }
        public string Procedure { get; set; }
        public int ProfessionalFees { get; set; }
        public string ReasonForHospitalization { get; set; }
        public string SurgicalManagement { get; set; }
        public string TypeOfAdmission { get; set; }
        public string PrimDocMobNo { get; set; }
        public string PatMobileNo { get; set; }
        public string PatAddress { get; set; }
    }



    public class InInPatientClaimBook
    {           
        public string PatientId { get; set; } // MaxID
        public int TreatmentId { get; set; } // IPID
        public int HospitalId { get; set; }
        public string Source { get; set; }
    }

    public class InPatientClaimBookDetail
    {
        public string PolicyID { get; set; }
        public Int32 TPAID { get; set; }
        public string MRNNo { get; set; }
        public string PatientID { get; set; }
        public Int32 TreatmentID { get; set; }
        public string PatientName { get; set; }
        public Int32 HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }
        public string DischargeDate { get; set; }
        public string DischargeTime { get; set; }
        public Int32 InsurerID { get; set; }
        public string InsurerName { get; set; }
    }

    public class InPatientClaimBookAll : Base
    {
        public List<InPatientClaimBookDetail> RetVal { get; set; }

        public void GetSampleCollectionCharges(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<InPatientClaimBookDetail>(objIDataReader);
        }
    }

    public class OutInPatientClaimBookRet : Base
    {
        public string PolicyID { get; set; }
        public Int32 TPAID { get; set; }
        public string MRNNo { get; set; }
        public string PatientID { get; set; }
        public Int32 TreatmentID { get; set; }
        public string PatientName { get; set; }
        public Int32 HospitalID { get; set; }
        public string HospitalName { get; set; }
        public string City { get; set; }
        public string PIN { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }
        public string DischargeDate { get; set; }
        public string DischargeTime { get; set; }
        public Int32 InsurerID { get; set; }
        public string InsurerName { get; set; }
    }












    public class InVNUAPIPatient
    {
        public int TreatmentId { get; set; } // IPID
        public int HospitalId { get; set; }
        public String ReqInfo { get; set; }
        public string Source { get; set; }
    }

    public class OutVNUAPIPatientDiagnosisInfo
    {
        public string Diagnosis { get; set; }
        public string Code { get; set; }
        public string IsFinal { get; set; }
        public string Comment { get; set; }
        public string DiagnosisDate { get; set; }
        public string DiagnosisBy { get; set; }
    }

    public class OutVNUAPIPatientComplaintsInfo
    {
        public string ChiefComplaint { get; set; }
        public string DurationofComplaint { get; set; }
    }

    public class OutVNUAPIPatientPastMedicalHistory
    {
        public string DM { get; set; }
        public string IHD { get; set; }
        public string Asthma { get; set; }
        public string HT { get; set; }
        public string COPD { get; set; }
        public string Other { get; set; }
    }

    public class OutVNUAPIPatientRelevantclinicalfinding
    {
        public string ClinicalFindings { get; set; }
        public string DATE { get; set; }
    }

    public class OutVNUAPIPatientIfAccident
    {
        public string InjuryDate { get; set; }
        public string ReportedToPolice { get; set; }
        public string PoliceStation { get; set; }
        public string MLCNo { get; set; }
        public string InjuryOrDiseasebySubstanceAbuse { get; set; }
    }

    public class OutVNUAPIPatientFamilyPhysician
    {
        public string Name { get; set; }
        public string ContactNo { get; set; }
    }

    public class OutVNUAPIPatientContactAttendingrelative
    {
        public string Name { get; set; }
        public string Relation { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
    }

    public class OutVNUAPIPatientEstimatInfo
    {
        public string Date { get; set; }
        public string BillTemplate { get; set; }
        public string PreparedBy { get; set; }
        public string TreatingDoctor { get; set; }
        public string BedNo { get; set; }
        public string RefDoctorName { get; set; }
        public string ApproximateDays { get; set; }
        public string EstAmt { get; set; }
        public string Description { get; set; }
        public string Rate { get; set; }
    }

    public class OutVNUAPIPatientPaymentDetails
    {
        public string ApprovedAmount { get; set; }
        public string Discount { get; set; }
        public string PaidByPatient { get; set; }
        public string SettlementAmount { get; set; }
        public string TDS { get; set; }
        public string SettlementDate { get; set; }
        public string CreditCardBank { get; set; }
        public string UTRno { get; set; }
    }

    public class OutVNUAPIPatientRet : Base
    {
        public string HospitalID { get; set; }
        public string MRNNo { get; set; }
        public string PatientID { get; set; }
        public string TreatmentID { get; set; }
        public string TreatingDoctor { get; set; }
        public string DoctorContactNumber { get; set; }
        public string Qualification { get; set; }
        public string DocRegNoWithStateCode { get; set; }
        public string Department { get; set; }
        public string Speciality { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string ContactNumber { get; set; }
        public string IDCardName { get; set; }
        public string IDCard { get; set; }
        public string InsurerName { get; set; }
        public string PolicyNumber { get; set; }
        public string TPAID { get; set; }
        public string EMPID { get; set; }
        public string CorporateName { get; set; }
        public string EmailID { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }
        public string DischargeDate { get; set; }
        public string DischargeTime { get; set; }
        public string AdmissionNote { get; set; }
        public string BedNo { get; set; }
        public string RoomType { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string SurgicalDetail { get; set; }
        public string MedicalManagementDetail { get; set; }
        public string Investigations { get; set; }
        public string EmergencyOrPlanned { get; set; }
        public string ExpectedStayinDays { get; set; }

        public OutVNUAPIPatientIfAccident IfAccident { get; set; }
        public List<String> AdditionalDiagnosis { get; set; }
        public List<string> OtherInsurance { get; set; }


        public List<OutVNUAPIPatientDiagnosisInfo> DiagnosisInfo { get; set; }
        public void GetDiagnosisInfo(IDataReader objIDataReader)
        {
            DiagnosisInfo = AssembleBECollection<OutVNUAPIPatientDiagnosisInfo>(objIDataReader);
        }

        public List<OutVNUAPIPatientComplaintsInfo> ComplaintsInfo { get; set; }
        public void GetComplaintsInfo(IDataReader objIDataReader)
        {
            ComplaintsInfo = AssembleBECollection<OutVNUAPIPatientComplaintsInfo>(objIDataReader);
        }

        public List<OutVNUAPIPatientPastMedicalHistory> PastMedicalHistory { get; set; }
        public void GetPastMedicalHistory(IDataReader objIDataReader)
        {
            PastMedicalHistory = AssembleBECollection<OutVNUAPIPatientPastMedicalHistory>(objIDataReader);
        }

        public List<OutVNUAPIPatientRelevantclinicalfinding> RelevantClinicalfinding { get; set; }
        public void GetRelevantclinicalfinding(IDataReader objIDataReader)
        {
            RelevantClinicalfinding = AssembleBECollection<OutVNUAPIPatientRelevantclinicalfinding>(objIDataReader);
        }

        public List<OutVNUAPIPatientFamilyPhysician> FamilyPhysician { get; set; }
        public void GetFamilyPhysician(IDataReader objIDataReader)
        {
            FamilyPhysician = AssembleBECollection<OutVNUAPIPatientFamilyPhysician>(objIDataReader);
        }

        public List<OutVNUAPIPatientContactAttendingrelative> ContactAttendingrelative { get; set; }
        public void GetContactAttendingrelative(IDataReader objIDataReader)
        {
            ContactAttendingrelative = AssembleBECollection<OutVNUAPIPatientContactAttendingrelative>(objIDataReader);
        }

        public List<OutVNUAPIPatientEstimatInfo> EstimatInfo { get; set; }
        public void GetEstimatInfo(IDataReader objIDataReader)
        {
            EstimatInfo = AssembleBECollection<OutVNUAPIPatientEstimatInfo>(objIDataReader);
        }

        public List<OutVNUAPIPatientPaymentDetails> PaymentDetails { get; set; }
        public void GetPaymentDetails(IDataReader objIDataReader)
        {
            PaymentDetails = AssembleBECollection<OutVNUAPIPatientPaymentDetails>(objIDataReader);
        }
    }

    public class VNUAPIPatientMain
    {
        public string HospitalID { get; set; }
        public string MRNNo { get; set; }
        public string PatientID { get; set; }
        public string TreatmentID { get; set; }
        public string TreatingDoctor { get; set; }
        public string DoctorContactNumber { get; set; }
        public string Qualification { get; set; }
        public string DocRegNoWithStateCode { get; set; }
        public string Department { get; set; }
        public string Speciality { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public string Weight { get; set; }
        public string Height { get; set; }
        public string ContactNumber { get; set; }
        public string IDCardName { get; set; }
        public string IDCard { get; set; }
        public string InsurerName { get; set; }
        public string PolicyNumber { get; set; }
        public string TPAID { get; set; }
        public string EMPID { get; set; }
        public string CorporateName { get; set; }
        public string EmailID { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmissionTime { get; set; }
        public string DischargeDate { get; set; }
        public string DischargeTime { get; set; }
        public string AdmissionNote { get; set; }
        public string BedNo { get; set; }
        public string RoomType { get; set; }
        public string HistoryOfPresentIllness { get; set; }
        public string SurgicalDetail { get; set; }
        public string MedicalManagementDetail { get; set; }
        public string Investigations { get; set; }
        public string EmergencyOrPlanned { get; set; }
        public string ExpectedStayinDays { get; set; }
    }

    public class VNUAPIPatientMainArr : Base
    {
        public List<VNUAPIPatientMain> retVal { get; set; }
        public void GetDiagnosisInfo(IDataReader objIDataReader)
        {
            retVal = AssembleBECollection<VNUAPIPatientMain>(objIDataReader);
        }
    }

    public class VNUAPIPatientIfAccident
    {
        public string InjuryDate { get; set; }
        public string ReportedToPolice { get; set; }
        public string PoliceStation { get; set; }
        public string MLCNo { get; set; }
        public string InjuryOrDiseasebySubstanceAbuse { get; set; }
    }

    public class VNUAPIPatientIfAccidentArr : Base
    {
        public List<VNUAPIPatientIfAccident> retVal { get; set; }
        public void GetDiagnosisInfo(IDataReader objIDataReader)
        {
            retVal = AssembleBECollection<VNUAPIPatientIfAccident>(objIDataReader);
        }
    }

    


    

}