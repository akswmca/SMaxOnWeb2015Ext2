using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Models;
using System.Runtime.Serialization;
using System.Data;
namespace WebAPI.Models
{

    public class InItemList
    {
        public int ServiceId { get; set; }
        public int HospitalId { get; set; }
        public int SpokeId { get; set; }
        public string Source { get; set; }

    }

    public class OutItemList
    {
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int SpecializationId { get; set; }
        public int ServiceItemId { get; set; }
        public string ServiceItemName { get; set; }
        public int SpecimenId { get; set; }
        public string SpecimenName { get; set; }
        public decimal Amount { get; set; }
        public int DepartmentId { get; set; }
    }

    public class OutItemListSampleCollection
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public int ServiceId { get; set; }
        public decimal Price { get; set; }

    }

    public class OutItemListRet : Base
    {
        public List<OutItemList> ItemList { get; set; }
        public List<OutItemListSampleCollection> SampleCollectionCharges { get; set; }

        public void GetItemListAssemble(IDataReader objIDataReader)
        {
            ItemList = AssembleBECollection<OutItemList>(objIDataReader);
        }

        public void GetSampleCollectionCharges(IDataReader objIDataReader)
        {
            SampleCollectionCharges = AssembleBECollection<OutItemListSampleCollection>(objIDataReader);
        }
    }

    public class InDoctorsList
    {
        public string DocType { get; set; }
        public int HospitalID { get; set; }
        public string Source { get; set; }
    }

    public class OutDoctorsList
    {
        public int EmpId { get; set; }
        public int DocId { get; set; }
        public string DocName { get; set; }
        public int SpecialiseID { get; set; }
        public string Specialisation { get; set; }
        public int RefDoctorID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }

    public class OutDoctorsListRet : Base
    {
        public List<OutDoctorsList> DoctorList { get; set; }

        public void GetInternalExternalDoctorsListAssemble(IDataReader objIDataReader)
        {
            DoctorList = AssembleBECollection<OutDoctorsList>(objIDataReader);
        }
    }

    public class InLabOrder
    {
        public string MaxID { get; set; }
        public string PatientType { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }

    public class OutLabOrder
    {
        public string MAXID { get; set; }
        public string VisitID { get; set; }
        public string PatType { get; set; }
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int HospitalID { get; set; }
        public string TestName { get; set; }
        public int TestProfileID { get; set; }
        public int risAccessionid { get; set; }
        public string urlRad { get; set; }
        public string urlPath { get; set; }
        public int TestId { get; set; }
        public string TestProfileName { get; set; }
        public Int32 ProfileID { get; set; }
        public string ProfileName { get; set; }
    }

    public class OutLabOrderRet : Base
    {
        public List<OutLabOrder> TestIDs { get; set; }

        public void GetInternalExternalDoctorsListAssemble(IDataReader objIDataReader)
        {
            TestIDs = AssembleBECollection<OutLabOrder>(objIDataReader);
        }
    }

    public class InPatSearchByBed
    {
        public int BedID { get; set; }
        public string Source { get; set; }
    }

    public class PatSearchByBedBed
    {
        public string BedStatus { get; set; }
        public int Deleted { get; set; }
    }

    public class PatSearchByBedBedData : Base
    {
        public List<PatSearchByBedBed> Bed { get; set; }

        public void GetInternalExternalDoctorsListAssemble(IDataReader objIDataReader)
        {
            Bed = AssembleBECollection<PatSearchByBedBed>(objIDataReader);
        }
    }

    public class PatSearchByBedDetail
    {
        public string MaxID { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string LastName { get; set; }
        public string FullPatientName { get; set; }
        public int IPID { get; set; }
        public string AdmitDateTime { get; set; }
        public string EMail { get; set; }
        public string PhoneNo { get; set; }
        public string PrimDoctor { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public string DateOfBirth { get; set; }
        public int VIPFlag { get; set; }
        public string VIPReason { get; set; }
        public string Diabetic { get; set; }
        public string NonVeg { get; set; }
        public string Onion { get; set; }
        public string Garlic { get; set; }
        public string isDMG { get; set; }
    }

    public class PatSearchByBedData : Base
    {
        public List<PatSearchByBedDetail> Patients { get; set; }

        public void GetInternalExternalDoctorsListAssemble(IDataReader objIDataReader)
        {
            Patients = AssembleBECollection<PatSearchByBedDetail>(objIDataReader);
        }
    }

    public class OutPatSearchByBedRet : Base
    {
        public string BedStatus { get; set; }
        public string MaxID { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        //public string LastName { get; set; }
        //public string FullPatientName { get; set; }
        public int IPID { get; set; }
        public string AdmitDateTime { get; set; }
        //public string EMail { get; set; }
        public string PhoneNo { get; set; }
        public string PrimDoctor { get; set; }
        //public string Gender { get; set; }
        //public string Address { get; set; }
        //public string DateOfBirth { get; set; }
        public int VIPFlag { get; set; }
        public string VIPReason { get; set; }
        public string Diabetic { get; set; }
        public string NonVeg { get; set; }
        public string Onion { get; set; }
        public string Garlic { get; set; }
        public string isDMG { get; set; }

    }

    public class InDoctorDetailByBed
    {
        public int BedID { get; set; }
        public string Source { get; set; }
    }

    public class DoctorDetailByBedGroup
    {
        public Int32 ID { get; set; }
        public String DocName { get; set; }

    }

    public class DoctorDetailByBedStatus
    {
        public String BedStatus { get; set; }
        public Int32 Deleted { get; set; }
        public String BedName { get; set; }
        public String MaxID { get; set; }
        public Int32 PrimeDoctorID { get; set; }
        public String PrimeDoctorName { get; set; }
        public Int32 PrimeDoctor2ID { get; set; }
        public String PrimeDoctor2Name { get; set; }
        public Int32 SecDoctorID { get; set; }
        public String SecDoctorName { get; set; }
        public Int32 BedID { get; set; }
        public Int32 HSPLocationId { get; set; }


    }
    public class DoctorDetailByBedStatusArray : Base
    {
        public Int32 ManiDoctorID { get; set; }
        public Int32 ManiDoctorName { get; set; }
        public Int32 DoctorType { get; set; }
        public List<DoctorDetailByBedStatus> RetVal
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<DoctorDetailByBedStatus>(objIDataReader);
        }

    }

    public class DoctorDetailByBedMain : Base
    {
        public Int32 MainDoctorID { get; set; }
        public String MainDoctorName { get; set; }
        public String DoctorType { get; set; }
        public List<DoctorDetailByBedGroup> GroupDoctor
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            GroupDoctor = AssembleBECollection<DoctorDetailByBedGroup>(objIDataReader);
        }

    }
    public class OutDoctorDetailByBedRet : Base
    {
        public Int32 BedID { get; set; }
        public String BedName { get; set; }
        public Int32 LocationID { get; set; }
        public String MaxID { get; set; }
        public String BedStatus { get; set; }
        public List<DoctorDetailByBedMain> RetVal
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<DoctorDetailByBedMain>(objIDataReader);
        }

    }

    public class DoctorChargeIn
    {
        public string IAcode { get; set; }
        public int RegistrationNo { get; set; }
        public int DoctorID { get; set; }
        public int HospitalID { get; set; }
        public string Source { get; set; }
        public decimal SourceAmount { get; set; }
    }

    public class DoctorChargeOut
    {
        public int DiscountID { get; set; }
        public int CompanyID { get; set; }
        public string DiscName { get; set; }
        public decimal DiscPer { get; set; }
        public decimal DoctorCharge { get; set; }
        public decimal BasePrice { get; set; }
        public int followup { get; set; }
    }

    public class DoctorChargeRet : Base
    {
        public List<DoctorChargeOut> RetValue
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<DoctorChargeOut>(objIDataReader);
        }
    }

    public class DefaultDiscount
    {
        public int DiscountID { get; set; }
        public string DiscName { get; set; }
        public decimal DiscPer { get; set; }
    }


    public class BedsInHospitalIn
    {
        public int HospitalID { get; set; }
        public string Source { get; set; }
    }

    public class BedsInHospitalOut
    {
        public string BedName { get; set; }
        public string RoomName { get; set; }
        public string StationName { get; set; }
        public string FloorName { get; set; }
        public int BedId { get; set; }
        public int RoomId { get; set; }
        public int StationId { get; set; }
        public int FloorId { get; set; }
        public int Census { get; set; }
        public int IsCritical { get; set; }
    }

    public class BedsInHospitalRet : Base
    {
        public List<BedsInHospitalOut> RetValue
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<BedsInHospitalOut>(objIDataReader);
        }
    }

    public class InInsertDoctorVisit
    {
        public int BedID { get; set; }
        public int DoctorID { get; set; }
        public string DateTime { get; set; }
        public string Source { get; set; }
    }

    public class OutInsertDoctorVisitRet : Base
    {
        public string SSNo { get; set; }
        public string IACode { get; set; }
        public string MaxID { get; set; }

        public int RegistrationNo { get; set; }
        public int RetCode { get; set; }
        public int IPID { get; set; }
        public int HSPLocationId { get; set; }


    }

    public class InEPCountMPHRx
    {
        public String SDate { get; set; }
        public String Source { get; set; }
    }

    public class OutEPCountMPHRx
    {
        public Int32 NCount { get; set; }
        public String Response { get; set; }
        public String Code { get; set; }
    }

    public class OutEPCountMPHRxRet : Base
    {
        public Int32 NCount { get; set; }
        public List<OutEPCountMPHRx> RetValue
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutEPCountMPHRx>(objIDataReader);
        }
    }

    public class InInterfaceData90
    {
        public Int32 NoOfOrders { get; set; }
        public Int32 SampleCollected { get; set; }
        public Int32 LabReceived { get; set; }
        public Int32 Result { get; set; }
        public Int32 Cancelled { get; set; }
        public Int32 PDFAPICall { get; set; }
        public Int32 EPCount { get; set; }
        public String sTransDate { get; set; }
        public String AppName { get; set; }
        public String Source { get; set; }
    }

    public class InterfaceData90
    {
        public Int32 code { get; set; }
        public String msg { get; set; }

    }

    public class ArrInterfaceData90 : Base
    {
        public List<InterfaceData90> RetValue
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<InterfaceData90>(objIDataReader);
        }
    }

    public class OutInterfaceData90Ret : Base
    {

    }

    public class InOneSendSMSFromTMSDB90
    {
        public String RefId { get; set; }
        public String MobileNo { get; set; }
        public String Message { get; set; }

    }

    public class InSendSMSFromTMSDB90 : Base
    {
        public string FeedID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string JobName { get; set; }

        public String Source { get; set; }
        public List<InOneSendSMSFromTMSDB90> Detail
        {
            get;
            set;
        }

        public void Assemble(IDataReader objIDataReader)
        {
            Detail = AssembleBECollection<InOneSendSMSFromTMSDB90>(objIDataReader);
        }
    }

    public class OutSendSMSFromTMSDB90
    {
        public String RefId { get; set; }
        public Int32 iStatus { get; set; }
        public String sStatusText { get; set; }
        public String DBDetMessage { get; set; }
    }

    public class OutSendSMSFromTMSDB90Ret : Base
    {
        public List<OutSendSMSFromTMSDB90> RetVal
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<OutSendSMSFromTMSDB90>(objIDataReader);
        }
    }


    public class InQMSDisplayGetQueue
    {
        public Int32 DisplayID { get; set; }
        public String Source { get; set; }
    }

    public class OutQMSDisplayGetQueue
    {
        public Int32 QueueID { get; set; }
        public String SeriesSeqNo { get; set; }
        public Int32 CounterID { get; set; }
        public String CounterName { get; set; }
        public String AreaName { get; set; }
        public Int32 AreaID { get; set; }
        public Int32 SeqNo { get; set; }
    }

    public class OutQMSDisplayGetQueueRet : Base
    {
        public List<OutQMSDisplayGetQueue> RetVal
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<OutQMSDisplayGetQueue>(objIDataReader);
        }
    }

    public class InDietBarCodeKOTTAT
    {
        public string BarCode { get; set; }
        public string ScanDateTime { get; set; }
        public string TransType { get; set; }
        public string Source { get; set; }
    }

    public class OutDietBarCodeKOTTAT : Base
    {
        public string retID { get; set; }
        public string RetMessage { get; set; }
    }

    public class DietBarCodeKOTTATData
    {
        public string RetID { get; set; }
        public string RetMas { get; set; }
    }

    public class DietBarCodeKOTTAT : Base
    {
        public List<DietBarCodeKOTTATData> RetVal
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetVal = AssembleBECollection<DietBarCodeKOTTATData>(objIDataReader);
        }
    }

    public class InLogin
    {
        public String UserName { get; set; }
        public String OldPwd { get; set; }
        public String MacAddress { get; set; }
        public String Source { get; set; }
    }

    public class LoginDataDetail
    {
        public Int32 UserId { get; set; }
        public Int32 HospitalId { get; set; }
        public String HospitalName { get; set; }
        public Int32 SessionId { get; set; }
        public String MacAddress { get; set; }
        public String UserFullName { get; set; }
        public Int32 StationId { get; set; }
        public String StationName { get; set; }
    }

    public class LoginDataMain : Base
    {
        public List<LoginDataDetail> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<LoginDataDetail>(objIDataReader);
        }
    }

    public class OutLogin
    {
        public Int32 HospitalId { get; set; }
        public String HospitalName { get; set; }
        public Int32 StationId { get; set; }
        public String StationName { get; set; }
    }

    public class OutLoginRet : Base
    {
        public Int32 UserId { get; set; }
        public Int32 SessionId { get; set; }
        public String MacAddress { get; set; }
        public String UserFullName { get; set; }
        public Int32 DaysLeftForPwd { get; set; }
        public List<OutLogin> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutLogin>(objIDataReader);
        }
    }

    public class InSearchMedicine
    {
        public Int32 StationId { get; set; }
        public String SearchText { get; set; }
        public Int32 NoOfRows { get; set; }
        public String Source { get; set; }
    }

    public class OutSearchMedicine
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
    }

    public class OutSearchMedicineRet : Base
    {
        public List<OutSearchMedicine> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutSearchMedicine>(objIDataReader);
        }
    }

    public class InBatchWiseQuantity
    {
        public Int32 StationId { get; set; }
        public Int32 ItemID { get; set; }
        public String Source { get; set; }
    }

    public class OutBatchWiseQuantity
    {
        public Int32 BatchID { get; set; }
        public String BatchNo { get; set; }
        public Int32 Quantity { get; set; }
        public Decimal MRP { get; set; }
    }

    public class OutBatchWiseQuantityRet : Base
    {
        public List<OutBatchWiseQuantity> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutBatchWiseQuantity>(objIDataReader);
        }
    }


    public class InPharmacySaveCartOrderItems
    {
        public Int32 ItemID { get; set; }
        public Int32 BatchID { get; set; }
        public Int32 QuantityNew { get; set; }
        public Decimal MRP { get; set; }
        public String RemarksItem { get; set; }
    }

    public class InPharmacySaveCartOrderItemsEPUrl
    {
        public String Url { get; set; }
        public String UrlDesc { get; set; }
        public String SlNo { get; set; }
    }

    public class InPharmacySaveCartOrder : Base
    {
        public String Flag { get; set; }
        public String IACode { get; set; }
        public Int32 Registrationno { get; set; }
        public Int32 OrderByDocId { get; set; }
        public String PatFName { get; set; }
        public String PatLName { get; set; }
        public String PatMobNo { get; set; }
        public String PatAddress { get; set; }
        public String PatDOB { get; set; }
        public Int32 UserID { get; set; }
        public Int32 StationID { get; set; }
        public Int32 CartOrderID { get; set; }
        public String RemarksMain { get; set; }
        public String PaymentMode { get; set; }
        public Int32 DiscountID { get; set; }
        public String DeliveryType { get; set; }
        public String Source { get; set; }

        public List<InPharmacySaveCartOrderItems> Items { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            Items = AssembleBECollection<InPharmacySaveCartOrderItems>(objIDataReader);
        }

        public List<InPharmacySaveCartOrderItemsEPUrl> Urls { get; set; }

        public void Assemble1(IDataReader objIDataReader)
        {
            Urls = AssembleBECollection<InPharmacySaveCartOrderItemsEPUrl>(objIDataReader);
        }
    }

    //public class OutPharmacySaveCartOrder
    //{
    //    public Int32 BatchID { get; set; }
    //    public String BatchNo { get; set; }
    //    public Int32 Quantity { get; set; }
    //    public Decimal MRP { get; set; }
    //}

    public class OutPharmacySaveCartOrderRet : Base
    {
        public String DBMessage { get; set; }
        public Int32 CartOrderID { get; set; }
        public Int32 NoOfItems { get; set; }
        public Decimal TotalAmount { get; set; }
    }

    public class InGetEPrescriptionList
    {
        public String IACode { get; set; }
        public Int32 RegistrationNo { get; set; }
        public String VisitDateFrom { get; set; }
        public String VisitDateTo { get; set; }
        public Int32 HospitalID { get; set; }
        public String Source { get; set; }

    }

    public class OutGetEPrescriptionList
    {
        public Int32 visitID { get; set; }
        public String VisitDateTime { get; set; }
        public Int32 RefDoctorId { get; set; }
        public String RefDoctorName { get; set; }
        public Int32 HospitalID { get; set; }
        public String HospitalName { get; set; }
    }

    public class OutGetEPrescriptionListRet : Base
    {
        public List<OutGetEPrescriptionList> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutGetEPrescriptionList>(objIDataReader);
        }
    }

    public class InPharmacyCartOrderSelect
    {
        public Int32 StationId { get; set; }
        public Int32 CartOrderId { get; set; }
        public String IACode { get; set; }
        public Int32 Registrationno { get; set; }
        public String PatFName { get; set; }
        public String PatMobNo { get; set; }
        public String Source { get; set; }
    }

    public class PharmacyCartOrderSelectIsDataFound
    {
        public Int32 isDataFound { get; set; }
        public Int32 CartOrderID { get; set; }
        public String IACode { get; set; }
        public Int32 Registrationno { get; set; }
        public Int32 OrderByDocId { get; set; }
        public String PatFName { get; set; }
        public String PatLName { get; set; }
        public String PatMobNo { get; set; }
        public String PatAddress { get; set; }
    }
    public class PharmacyCartOrderSelectIsDataFoundArr : Base
    {
        public List<PharmacyCartOrderSelectIsDataFound> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<PharmacyCartOrderSelectIsDataFound>(objIDataReader);
        }
    }

    public class OutPharmacyCartOrderSelect
    {
        public Int32 DetID { get; set; }
        public Int32 ItemID { get; set; }
        public Int32 BatchID { get; set; }
        public Int32 Quantity { get; set; }
        public Decimal MRP { get; set; }
        public String ItemName { get; set; }
        public String BatchNo { get; set; }
    }

    public class OutPharmacyCartOrderSelectRet : Base
    {
        public Int32 CartOrderID { get; set; }
        public String IACode { get; set; }
        public Int32 Registrationno { get; set; }
        public Int32 OrderByDocId { get; set; }
        public String PatFName { get; set; }
        public String PatLName { get; set; }
        public String PatMobNo { get; set; }
        public String PatAddress { get; set; }

        public List<OutPharmacyCartOrderSelect> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPharmacyCartOrderSelect>(objIDataReader);
        }
    }


    public class InPharmacyCartOrderSearch
    {
        public String IACode { get; set; }
        public Int32 Registrationno { get; set; }
        public String PatMobNo { get; set; }
        public String PatFName { get; set; }
        public String SearchFlag { get; set; }
        public String DateFrom { get; set; }
        public String DateTo { get; set; }

        public String Source { get; set; }
    }

    public class OutPharmacyCartOrderSearch
    {
        public Int32 CartID { get; set; }
        public String MaxID { get; set; }
        public String PatFName { get; set; }
        public String PatMobNo { get; set; }
        public String IsActive { get; set; }
        public String isBilled { get; set; }
        public String isCancelled { get; set; }
        public String SavedDateTime { get; set; }
        public String LastUpdateDateTime { get; set; }

    }

    public class OutPharmacyCartOrderSearchRet : Base
    {

        public List<OutPharmacyCartOrderSearch> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPharmacyCartOrderSearch>(objIDataReader);
        }
    }


    public class InAppointmentCancel
    {
        public Int32 PaymentID { get; set; }
        public String BookingSource { get; set; }
        public String CancellationDate { get; set; }
        public String Source { get; set; }
    }

    public class MAppointmentCancel
    {
        public String retMsg { get; set; }

    }

    public class MAppointmentCancelArr : Base
    {
        public List<MAppointmentCancel> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<MAppointmentCancel>(objIDataReader);
        }
    }

    public class OutAppointmentCancelRet : Base
    {

    }



    public class InAppointmentUpdateRefund
    {
        public Int32 PaymentID { get; set; }
        public String BookingSource { get; set; }
        public Decimal RefundedAmount { get; set; }
        public String PayURefundID { get; set; }
        public String RefundDate { get; set; }
        public String Source { get; set; }
    }

    public class MAppointmentUpdateRefund
    {
        public String retMsg { get; set; }

    }

    public class MAppointmentUpdateRefundArr : Base
    {
        public List<MAppointmentUpdateRefund> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<MAppointmentUpdateRefund>(objIDataReader);
        }
    }

    public class OutAppointmentUpdateRefundRet : Base
    {

    }





    public class InPharmacyCartOrderSearchDetail
    {
        public Int32 CartID { get; set; }
        public String Source { get; set; }
    }

    //public class PharmacyCartOrderSearchDetail
    //{
    //    public Int32 CartOrderID { get; set; }
    //    public String MaxID { get; set; }
    //    public Int32 OrderByDocId { get; set; }
    //    public String docName { get; set; }
    //    public String PatFName { get; set; }
    //    public String PatLName { get; set; }
    //    public String PatMobNo { get; set; }
    //    public String PatAddress { get; set; }
    //    public String PatDOB { get; set; }
    //    public Int32 SaveUserID { get; set; }
    //    public Int32 StationID { get; set; }
    //    public String PaymentMode { get; set; }
    //    public String Remarks { get; set; }
    //    public String IsActive { get; set; }
    //    public String isBilled { get; set; }    
    //}

    public class PharmacyCartOrderSearchDetailArr : Base
    {

        public List<OutPharmacyCartOrderSearchDetailRet> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPharmacyCartOrderSearchDetailRet>(objIDataReader);
        }
    }


    public class OutPharmacyCartOrderSearchDetail
    {
        public Int32 ItemID { get; set; }
        public String itemName { get; set; }
        public Int32 BatchID { get; set; }
        public String BatchNo { get; set; }
        public Int32 Quantity { get; set; }
        public Decimal MRP { get; set; }
        public String Remarks { get; set; }
        public String IsDeleted { get; set; }
    }

    public class OutPharmacyCartOrderSearchDetailUrl
    {
        public Int32 SlNo { get; set; }
        public String UrlDesc { get; set; }
        public String UrlLink { get; set; }
        public String IsDeleted { get; set; }
    }


    public class OutPharmacyCartOrderSearchDetailRet : Base
    {
        public Int32 CartOrderID { get; set; }
        public String MaxID { get; set; }
        public Int32 OrderByDocId { get; set; }
        public String docName { get; set; }
        public String PatFName { get; set; }
        public String PatLName { get; set; }
        public String PatMobNo { get; set; }
        public String PatAddress { get; set; }
        public String PatDOB { get; set; }
        public Int32 SaveUserID { get; set; }
        public Int32 StationID { get; set; }
        public String PaymentMode { get; set; }
        public String Remarks { get; set; }
        public String IsActive { get; set; }
        public String isBilled { get; set; }
        public String isCancelled { get; set; }
        public Int32 UpdateUserID { get; set; }
        public Int32 IssuedOrRej { get; set; }
        public Int32 IssuedOrRejByUserID { get; set; }
        public String SavedDateTime { get; set; }
        public String LastUpdateDateTime { get; set; }
        public Int32 DiscountId { get; set; }
        public Int32 CancelledReasonId { get; set; }
        public String CancelledRemarks { get; set; }
        public String DeliveryType { get; set; }

        public List<OutPharmacyCartOrderSearchDetail> RetValue
        {
            get;
            set;
        }
        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPharmacyCartOrderSearchDetail>(objIDataReader);
        }

        public List<OutPharmacyCartOrderSearchDetailUrl> RetValueUrl
        {
            get;
            set;
        }
        public void Assemble1(IDataReader objIDataReader)
        {
            RetValueUrl = AssembleBECollection<OutPharmacyCartOrderSearchDetailUrl>(objIDataReader);
        }
    }

    public class InPatientInformation
    {
        public String MaxID { get; set; }
        public String BillNo { get; set; }
        public Int32 IPID { get; set; }
        public Int32 HospitalID { get; set; }
        public String Source { get; set; }
    }

    public class OutPatientInformation
    {
        public String PatientName { get; set; }
        public String Age { get; set; }
        public String Gender { get; set; }
        public String HospitalID { get; set; }
        public String HospitalName { get; set; }
        public String AdmitingDepartment { get; set; }
        public String AdmitingDoctor { get; set; }
        public String AdmitionDate { get; set; }
        public String DischargeDate { get; set; }
        public String IPOPNumber { get; set; }
        public String MobileNo { get; set; }
        public String EmailID { get; set; }
        public String Channel { get; set; }
        public String RoomCategory { get; set; }
        public String VIPType { get; set; }
        public String Speciality { get; set; }
        public String RoomNo { get; set; }
        public String BedNo { get; set; }
        public String IsDMG { get; set; }
    }

    public class OutPatientInformationRet : Base
    {
        public List<OutPatientInformation> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPatientInformation>(objIDataReader);
        }
    }

    public class InPatientDemographyByIDList : BaseOnly
    {
        public Int32 PatID { get; set; }
        public String Source { get; set; }

        public List<InPatientDemographyByID> PatList { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            PatList = AssembleBECollection<InPatientDemographyByID>(objIDataReader);
        }
    }

    public class InPatientDemographyByID
    {
        public Int32 PatID { get; set; }
    }

    public class OutPatientDemographyByID
    {
        public Int32 PatId { get; set; }
        public String PatientName { get; set; }
        public String Gender { get; set; }
        public String MobileNo { get; set; }
        public String EmailID { get; set; }
        public String DOB { get; set; }
    }

    public class OutPatientDemographyByIDArray : Base
    {
        public List<OutPatientDemographyByID> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutPatientDemographyByID>(objIDataReader);
        }
    }


    public class InMyHealthEPrescription
    {
        public String Bookingno { get; set; }
        public String Data1 { get; set; }
        public String Data2 { get; set; }
        public String Data3 { get; set; }
        public String Data4 { get; set; }
        public String Data5 { get; set; }
        public String Updatedatetime { get; set; }
        public String Savedatetime { get; set; }
        public String Source { get; set; }
    }

    public class OutMyHealthEPrescriptionRet : Base
    {
        public Int32 VisitID { get; set; }
    }
    public class OutMyHealthEPrescription : Base
    {
        public Int32 VisitID { get; set; }
        public String RetMessage { get; set; }
    }

    public class MyHealthEPrescriptionArray : Base
    {
        public List<OutMyHealthEPrescription> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutMyHealthEPrescription>(objIDataReader);
        }
    }

    public class InOPVisitCaseSummary
    {
        public string MaxId { get; set; }
        public int DoctorId { get; set; }
        public int CaseSummaryId { get; set; }
        public int OnlinePaymentId { get; set; }
        public int VisitId { get; set; }
        public string BookingNo { get; set; }
        public string BookingSource { get; set; }
        public string CaseSummaryText { get; set; }
        public string Source { get; set; }
    }



    public class OutOPVisitCaseSummary : Base
    {
        public List<OutOPVisitCaseSummaryRet> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutOPVisitCaseSummaryRet>(objIDataReader);
        }
    }


    public class OutOPVisitCaseSummaryRet : Base
    {
        public int CaseSummaryId { get; set; }
        public string DBMessage { get; set; }
        public int VisitId { get; set; }
        public int OnlinePaymentId { get; set; }
    }


    public class InPatientListDoctorwise
    {
        public int DoctorID { get; set; }
        public int HospitalID { get; set; }
        public string Source { get; set; }
    }

    public class OutPatientListDoctorwise
    {
        public int IPID { get; set; }
        public int HSPLocationId { get; set; }
        public string MaxId { get; set; }
        public string PatName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string PatType { get; set; }
        public string AdmitDate { get; set; }
        public int PrimDoctorId { get; set; }
        public string PrimDoctorName { get; set; }
        public int SecDoctorId { get; set; }
        public string SecDoctorName { get; set; }
        public int PrimDoctorId2 { get; set; }
        public string PrimDoctorName2 { get; set; }
        public int ReferralDocId { get; set; }
        public string RefralDoctorName { get; set; }
        public string AdmitStatus { get; set; }
    }

    public class OutPatientListDoctorwiseRet : Base
    {
        public List<OutPatientListDoctorwise> Patients
        { get; set; }
        public void Assemble(IDataReader objIDataReader)
        {
            Patients = AssembleBECollection<OutPatientListDoctorwise>(objIDataReader);
        }
    }

    public class InLabOrdersCreateURLItem
    {
        public string ItemId { get; set; }
    }

    public class InLabOrdersCreateURL
    {
        public string OrderId { get; set; }
        public string PatType { get; set; }
        public List<InLabOrdersCreateURLItem> Items { get; set; }

    }
    public class InLabOrdersCreateURLArr
    {
        public string Source { get; set; }
        public string UserId { get; set; }
        public List<InLabOrdersCreateURL> Orders { get; set; }
    }

    public class OutLabOrdersCreateURL : Base
    {
        public string urlPath { get; set; }
        public string OrderId { get; set; }
        public string PatType { get; set; }
    }

    public class OutLabOrdersCreateURLRet : Base
    {
        public List<OutLabOrdersCreateURL> urls { get; set; }
    }

    public class InOPBilltails
    {
        public int BillId { get; set; }
        public int NoOfRecords { get; set; }
        public string Source { get; set; }
    }
    public class OutOPBilltails
    {
        public string MaxID { get; set; }
        public string Company { get; set; }
        public string Patient { get; set; }
        public string Pphone { get; set; }
        public string PCellNo { get; set; }
        public string Email { get; set; }
        public string BillDate { get; set; }
        public string BillNo { get; set; }
        public decimal Amount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountReason { get; set; }
        public string Location { get; set; }
        public string LocalAddress { get; set; }
        public string CityName { get; set; }
        public string DoctorName { get; set; }
        public string SpecName { get; set; }
        public string PaymentType { get; set; }
        public string ServiceName { get; set; }
        public string ItemName { get; set; }
        public string Department { get; set; }
        public string Channel { get; set; }
        public string Age { get; set; }
        public string AdmissionDate { get; set; }
        public string DischargeDateTime { get; set; }
    }

    public class OutOPBilltailsRet : Base
    {
        public List<OutOPBilltails> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutOPBilltails>(objIDataReader);
        }
    }

    public class InEPDataSampark
    {
        public string MaxId { get; set; }
        public string Source { get; set; }
    }
    public class OutEPDataSampark
    {
        public string Diagnosis { get; set; }
        public string SurgeryName { get; set; }
        public string DMG { get; set; }
    }

    public class OutEPDataSamparkRet : Base
    {
        public List<OutEPDataSampark> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutEPDataSampark>(objIDataReader);
        }
    }

    public class InClabsiScreening
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class OutClabsiScreening
    {
        public int ipid { get; set; }
        public int patient_ssn { get; set; }
        public decimal age { get; set; }
        public string centralline { get; set; }
        public string bloodculture { get; set; }
        public string ADDRESS3 { get; set; }
        public string ORGANISM_NAME { get; set; }
        public string STATION_NAME { get; set; }
        public string SEX { get; set; }
        public string admissiondate { get; set; }
        public string centrallinedate { get; set; }
        public string bloodculturedate { get; set; }
        public string CARE_PROVIDER_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string PATIENT_NAME { get; set; }
        public string VITAL_TYPE { get; set; }
        public decimal VITAL_VALUE_NUMERIC { get; set; }
        public string vital_enter_date { get; set; }
        public string test_order_date { get; set; }
    }

    public class OutClabsiScreeningRet : Base
    {
        public List<OutClabsiScreening> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutClabsiScreening>(objIDataReader);
        }
    }
    public class InVapScreening
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class OutVapScreening
    {
        public int ipid { get; set; }
        public int patient_ssn { get; set; }
        public decimal age { get; set; }
        public string Ventilator_procedure { get; set; }
        public string culturename { get; set; }
        public string ADDRESS3 { get; set; }
        public string ORGANISM_NAME { get; set; }
        public string STATION_NAME { get; set; }
        public string SEX { get; set; }
        public string admissiondate { get; set; }
        public string Ventilatordate { get; set; }
        public string cultureResultdate { get; set; }
        public string CARE_PROVIDER_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string PATIENT_NAME { get; set; }
        public string VITAL_TYPE { get; set; }
        public decimal VITAL_VALUE_NUMERIC { get; set; }
        public string vital_enter_date { get; set; }
        public string test_order_date { get; set; }
        public string SPO2 { get; set; }
        public decimal SPO2_value { get; set; }
        public string spo2_date { get; set; }
    }

    public class OutVapScreeningRet : Base
    {
        public List<OutVapScreening> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutVapScreening>(objIDataReader);
        }
    }
    public class InSSIScreening
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class OutSSIScreening
    {
        public int ipid { get; set; }
        public string bill_type { get; set; }
        public string organismName { get; set; }
        public string test_result { get; set; }
        public string swabtestdate { get; set; }
        public string testnames { get; set; }
        public string address3 { get; set; }
        public int his_order_id { get; set; }
        public string itemname { get; set; }
        public int his_hospital_location_id { get; set; }
        public string surgeryorderday { get; set; }
        public int patient_ssn { get; set; }
        public string patient_name { get; set; }
        public string gender { get; set; }
        public int AGE { get; set; }
        public string service_name { get; set; }
        public string care_provider_name { get; set; }
    }

    public class OutSSIScreeningRet : Base
    {
        public List<OutSSIScreening> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutSSIScreening>(objIDataReader);
        }
    }
    public class InCautiScreening
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class OutCautiScreening
    {
        public int ipid { get; set; }
        public int patient_ssn { get; set; }
        public decimal patient_age { get; set; }
        public string culture_test { get; set; }
        public string folyscat { get; set; }
        public string ADDRESS3 { get; set; }
        public string STATION_NAME { get; set; }
        public string SEX { get; set; }
        public string admissiondate { get; set; }
        public string folyscathdate { get; set; }
        public string urinecultureorderdate { get; set; }
        public string CARE_PROVIDER_NAME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string PATIENT_NAME { get; set; }
        public string VITAL_TYPE { get; set; }
        public decimal VITAL_VALUE_NUMERIC { get; set; }
        public string vital_enter_date { get; set; }
        public string culturetestresultdate { get; set; }
        public string ORGANISM_NAME { get; set; }
        public string PARAMETER_NAME { get; set; }
        public decimal TEST_RESULT { get; set; }
    }
    public class OutCautiScreeningRet : Base
    {
        public List<OutCautiScreening> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutCautiScreening>(objIDataReader);
        }
    }
    public class InReAdmission
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Source { get; set; }
    }
    public class OutReAdmission
    {
        public int IDF_READMISSION { get; set; }
        public int PATIENT_SSN { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DISCHARGE_DATE { get; set; }
        public string ADMISSION_DATE { get; set; }
        public string address3 { get; set; }
        public string READMISSION_DEPARTMENT { get; set; }
        public string DISCHARGE_DEPARTMENT { get; set; }
        public string ADMISSION_CARE_PROVIDER { get; set; }
        public string DISCHARGE_CARE_PROVIDER { get; set; }
        public string DATEOFBIRTH { get; set; }
        public string SEX { get; set; }
        public int PATIENT_ID { get; set; }
        public string FULLDATE { get; set; }
        public int READMISSION_PROBLEM_COUNT { get; set; }
        public int DISCHARGE_PROBLEM_COUNT { get; set; }
    }
    public class OutReAdmissionRet : Base
    {
        public List<OutReAdmission> RetValue { get; set; }

        public void Assemble(IDataReader objIDataReader)
        {
            RetValue = AssembleBECollection<OutReAdmission>(objIDataReader);
        }
    }
}