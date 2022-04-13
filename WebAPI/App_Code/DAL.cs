using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Data.SqlClient;
using WebAPI.Models;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Text.RegularExpressions;
using Sand64.Cryptography;
using WebAPI.ADID;
using System.Text;
using System.Security.Cryptography;

namespace WebAPI.Controllers
{
    public class DAL
    {
        //////////// Patient type /////
        //          OP=1             //
        //          IP=9             // 
        //          PR/ER=4          //
        //          PHP=2            //
        ///////////////////////////////

        SqlConnection theCon;
        SqlConnection theConReadOnly;

        SqlDatabase db = null;
        SqlDatabase dbReadOnly = null;

        public DAL()
        {
            String strCS = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
            String strCSRO = ConfigurationManager.ConnectionStrings["DBConnectionStringReadOnly"].ToString();

            strCS = Decryptor.DecryptString(strCS, "3w8motherw4fdcj7");
            strCSRO = Decryptor.DecryptString(strCSRO, "3w8motherw4fdcj7");

            db = new SqlDatabase(strCS);
            dbReadOnly = new SqlDatabase(strCSRO);

            theCon = new SqlConnection(strCS);
            theConReadOnly = new SqlConnection(strCSRO);

        }

        public String GetAPISalt(String EnvironmentName, String ClientKey, String MethodName, ref Int32 RetVal)
        {
            String strReturn = "";
            DataSet dsToken;
            SqlCommand theCommand = null;
            theConReadOnly.Open();
            try
            {
                theCommand = new SqlCommand("PR_OnWeb_GetAPISaltFromKey", theConReadOnly);
                theCommand.CommandType = CommandType.StoredProcedure;
                theCommand.Parameters.Add("@ClientKey", SqlDbType.NVarChar, 50);
                theCommand.Parameters["@ClientKey"].Value = ClientKey;

                theCommand.Parameters.Add("@EnvironmentName", SqlDbType.NVarChar, 100);
                theCommand.Parameters["@EnvironmentName"].Value = EnvironmentName;

                theCommand.Parameters.Add("@MethodName", SqlDbType.NVarChar, 100);
                theCommand.Parameters["@MethodName"].Value = MethodName;

                theCommand.CommandTimeout = theCon.ConnectionTimeout;
                SqlDataAdapter theDA = new SqlDataAdapter(theCommand);
                dsToken = new DataSet();
                theDA.Fill(dsToken);
                if ((dsToken.Tables.Count > 0) && (dsToken.Tables[0].Rows.Count > 0))
                {
                    strReturn = dsToken.Tables[0].Rows[0]["ClientSalt"].ToString();
                    RetVal = Convert.ToInt32(dsToken.Tables[0].Rows[0]["RetVal"].ToString());
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    strReturn = "Database server not found or not accessible.";
                else
                    strReturn = ex.Message;
                return "";

            }
            finally
            {
                if (theCommand != null)
                    theCommand.Dispose();

                if ((theConReadOnly != null) && (theConReadOnly.State == ConnectionState.Open))
                {
                    theConReadOnly.Close();
                    theConReadOnly.Dispose();
                }
            }
        }

        public OutItemListRet GetItemList(InItemList objInput)
        {
            OutItemListRet objOutputAll = new OutItemListRet();
            DbCommand cmd = null;
            DbCommand cmdSC = null;
            try
            {


                cmd = db.GetStoredProcCommand("Pr_Digicare_GetInvestigation_MAx"); //ajay
                db.AddInParameter(cmd, "@HspLocaId", SqlDbType.Int, objInput.HospitalId);
                db.AddInParameter(cmd, "@ServiceId", SqlDbType.Int, objInput.ServiceId);
                db.AddInParameter(cmd, "@SpokeId", SqlDbType.Int, objInput.SpokeId);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {

                    objOutputAll.ItemList = dr.AssembleBECollection<OutItemList>();
                    if (objOutputAll.ItemList.Count > 0)
                        objOutputAll.Message = "List of Investigation ";
                    else
                        objOutputAll.Message = "No Investigation found";
                }

                // Sample collection charges
                cmdSC = db.GetStoredProcCommand("PR_Digi_SampleCollectionCharges_MAX"); //ajay
                db.AddInParameter(cmdSC, "@Hsplocationid", DbType.Int16, objInput.ServiceId);

                using (IDataReader dr = db.ExecuteReader(cmdSC))
                {

                    objOutputAll.SampleCollectionCharges = dr.AssembleBECollection<OutItemListSampleCollection>();
                    if (objOutputAll.SampleCollectionCharges.Count > 0)
                        objOutputAll.Message = objOutputAll.Message + " & List of Sample collection charges";
                    else
                        objOutputAll.Message = objOutputAll.Message + " & Sample collection charges not found";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutDoctorsListRet GetDoctorList(InDoctorsList objInput)
        {
            OutDoctorsListRet objOutput = new OutDoctorsListRet();
            DbCommand cmd = null;

            try
            {

                cmd = db.GetStoredProcCommand("Pr_Digicare_GetInternalExternalDoctorsList_MAx"); //ajay
                db.AddInParameter(cmd, "@DocType", DbType.String, objInput.DocType);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {

                    objOutput.DoctorList = dr.AssembleBECollection<OutDoctorsList>();
                    if (objOutput.DoctorList.Count > 0)
                        objOutput.Message = "List of doctors";
                    else
                        objOutput.Message = "No doctors found";

                }
                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutLabOrderRet GetLabOrders(InLabOrder objInput)
        {
            OutLabOrderRet objOutput = new OutLabOrderRet();
            DbCommand cmd = null;

            try
            {
                String[] aMaxID = objInput.MaxID.Split('.');
                if (aMaxID.Length > 1)
                {
                    cmd = db.GetStoredProcCommand("PR_OnWeb_GetLabOrder"); //ajay
                    db.AddInParameter(cmd, "@Iacode", DbType.String, aMaxID[0]);
                    db.AddInParameter(cmd, "@Registrationno", SqlDbType.Int, Convert.ToInt32(aMaxID[1]));
                    db.AddInParameter(cmd, "@PatientType", DbType.String, objInput.PatientType);
                    db.AddInParameter(cmd, "@sFromDate", DbType.String, objInput.FromDate);
                    db.AddInParameter(cmd, "@sToDate", DbType.String, objInput.ToDate);

                    using (IDataReader dr = db.ExecuteReader(cmd))
                    {

                        objOutput.TestIDs = dr.AssembleBECollection<OutLabOrder>();
                        if (objOutput.TestIDs.Count > 0)
                            objOutput.Message = "List of the test done.";
                        else
                            objOutput.Message = "No test found";

                    }
                }
                if ((objOutput != null) && (objOutput.TestIDs.Count > 0))
                {
                    for (Int32 i = 0; i < objOutput.TestIDs.Count; i++)
                    {
                        if (objOutput.TestIDs[i].risAccessionid > 0)
                        {
                            objOutput.TestIDs[i].urlRad = "http://radimage/ZFP?mode=Inbound&lights=on#accessionNumber=" + objOutput.TestIDs[i].risAccessionid + "&un=CPRS&pw=PqZaWaecz9X3oU7YraHtx%2buQyscoHGV8rih%2b2IXx%2f%2b8%3d";
                            objOutput.TestIDs[i].urlPath = "";
                        }
                        else
                        {
                            string encryptestid = Encrypt(objOutput.TestIDs[i].TestProfileID.ToString());
                            string encryporderid = Encrypt(objOutput.TestIDs[i].PatType + objOutput.TestIDs[i].OrderID.ToString());
                            objOutput.TestIDs[i].urlPath = "https://lims.maxlab.co.in/maxlab/design/lab/getReportHIS.aspx?LabNo=" + encryporderid + "&TestCode=" + encryptestid;
                            objOutput.TestIDs[i].urlRad = "";
                        }
                    }
                }
                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutPatSearchByBedRet PatSearchByBed(InPatSearchByBed objInput)
        {
            OutPatSearchByBedRet objOutput = new OutPatSearchByBedRet();
            PatSearchByBedData objPatData = new PatSearchByBedData();
            PatSearchByBedBedData objBedData = new PatSearchByBedBedData();
            DbCommand cmd = null;

            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_GetPatientByBed"); //ajay
                dbReadOnly.AddInParameter(cmd, "@BedID", SqlDbType.Int, objInput.BedID);
                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objBedData.Bed = dr.AssembleBECollection<PatSearchByBedBed>();
                    if (objBedData.Bed.Count > 0)
                    {
                        objOutput.BedStatus = objBedData.Bed[0].BedStatus;
                        if (objBedData.Bed[0].BedStatus == null)
                        {
                            objOutput.Message = "No bed found.";
                        }
                        else if (objBedData.Bed[0].Deleted == 0)
                        {
                            dr.NextResult();
                            objPatData.Patients = dr.AssembleBECollection<PatSearchByBedDetail>();
                            if (objPatData.Patients.Count > 0)
                            {
                                //objOutput.Address = objPatData.Patients[0].Address;
                                objOutput.AdmitDateTime = objPatData.Patients[0].AdmitDateTime;
                                //objOutput.DateOfBirth = objPatData.Patients[0].DateOfBirth;
                                //objOutput.EMail = objPatData.Patients[0].EMail;
                                objOutput.Firstname = objPatData.Patients[0].Firstname;
                                //objOutput.FullPatientName = objPatData.Patients[0].FullPatientName;
                                //if(objPatData.Patients[0].Sex==1)
                                //    objOutput.Gender = "Male";
                                //else if (objPatData.Patients[0].Sex == 2)
                                //    objOutput.Gender = "Female";
                                //else
                                //    objOutput.Gender = "Other";
                                objOutput.IPID = objPatData.Patients[0].IPID;
                                //objOutput.LastName = objPatData.Patients[0].LastName;
                                objOutput.MaxID = objPatData.Patients[0].MaxID;
                                objOutput.PhoneNo = objPatData.Patients[0].PhoneNo;
                                objOutput.PrimDoctor = objPatData.Patients[0].PrimDoctor;
                                objOutput.Title = objPatData.Patients[0].Title;
                                objOutput.VIPFlag = objPatData.Patients[0].VIPFlag;
                                objOutput.VIPReason = objPatData.Patients[0].VIPReason;
                                objOutput.Diabetic = objPatData.Patients[0].Diabetic;
                                objOutput.NonVeg = objPatData.Patients[0].NonVeg;
                                objOutput.Onion = objPatData.Patients[0].Onion;
                                objOutput.Garlic = objPatData.Patients[0].Garlic;
                                objOutput.isDMG = objPatData.Patients[0].isDMG;

                                objOutput.Message = "Patient detail.";
                            }
                            else
                                objOutput.Message = "No patient found";
                        }
                        else
                        {
                            objOutput.Message = "Deleted Bed.";
                        }

                    }


                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutDoctorDetailByBedRet DoctorDetailByBed(InDoctorDetailByBed objInput)
        {
            OutDoctorDetailByBedRet objOutput = new OutDoctorDetailByBedRet();
            DoctorDetailByBedMain oDoctor;

            DbCommand cmd = null;
            DoctorDetailByBedStatusArray oBedStatus = new DoctorDetailByBedStatusArray();
            try
            {

                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_DoctorDetailByBed"); //ajay
                dbReadOnly.AddInParameter(cmd, "@BedID", SqlDbType.Int, objInput.BedID);
                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    oBedStatus.RetVal = dr.AssembleBECollection<DoctorDetailByBedStatus>();
                    if (oBedStatus.RetVal.Count > 0)
                    {
                        objOutput.BedID = oBedStatus.RetVal[0].BedID;
                        objOutput.LocationID = oBedStatus.RetVal[0].HSPLocationId;
                        objOutput.MaxID = oBedStatus.RetVal[0].MaxID;
                        objOutput.BedName = oBedStatus.RetVal[0].BedName;
                        objOutput.BedStatus = oBedStatus.RetVal[0].BedStatus;


                        objOutput.RetVal = new List<DoctorDetailByBedMain>();
                        Int32 LocationID = oBedStatus.RetVal[0].HSPLocationId;

                        oDoctor = new DoctorDetailByBedMain();
                        if (oBedStatus.RetVal[0].PrimeDoctorID > 0)
                        {
                            oDoctor = getGroupDoctor(oBedStatus.RetVal[0].PrimeDoctorID, LocationID);
                            oDoctor.MainDoctorID = oBedStatus.RetVal[0].PrimeDoctorID;
                            oDoctor.MainDoctorName = oBedStatus.RetVal[0].PrimeDoctorName;
                            oDoctor.DoctorType = "PC1";
                            oDoctor.Message = "Primary Consultant 1";
                        }
                        else
                        {
                            oDoctor.Message = "Not Found Primary Consultant 1";
                        }
                        objOutput.RetVal.Add(oDoctor);

                        oDoctor = new DoctorDetailByBedMain();
                        if (oBedStatus.RetVal[0].PrimeDoctor2ID > 0)
                        {
                            oDoctor = getGroupDoctor(oBedStatus.RetVal[0].PrimeDoctor2ID, LocationID);
                            oDoctor.MainDoctorID = oBedStatus.RetVal[0].PrimeDoctor2ID;
                            oDoctor.MainDoctorName = oBedStatus.RetVal[0].PrimeDoctor2Name;
                            oDoctor.DoctorType = "PC2";
                            oDoctor.Message = "Primary Consultant 2";
                        }
                        else
                        {
                            oDoctor.Message = "Not Found Primary Consultant 2";
                        }
                        objOutput.RetVal.Add(oDoctor);

                        oDoctor = new DoctorDetailByBedMain();
                        if (oBedStatus.RetVal[0].SecDoctorID > 0)
                        {
                            oDoctor = getGroupDoctor(oBedStatus.RetVal[0].SecDoctorID, LocationID);
                            oDoctor.MainDoctorID = oBedStatus.RetVal[0].SecDoctorID;
                            oDoctor.MainDoctorName = oBedStatus.RetVal[0].SecDoctorName;
                            oDoctor.DoctorType = "SC";
                            oDoctor.Message = "Secondary Consultant";
                        }
                        else
                        {
                            oDoctor.Message = "Not Found Secondary Consultant";
                        }
                        objOutput.RetVal.Add(oDoctor);
                    }


                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        private DoctorDetailByBedMain getGroupDoctor(Int32 DoctorID, Int32 HospitalID)
        {
            DoctorDetailByBedMain objOutputAll = new DoctorDetailByBedMain();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_GetGroupDoctor");
                dbReadOnly.AddInParameter(cmd, "@PrimDocId", DbType.String, DoctorID);
                dbReadOnly.AddInParameter(cmd, "@locationID", DbType.String, HospitalID);
                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.GroupDoctor = dr.AssembleBECollection<DoctorDetailByBedGroup>();
                    if (objOutputAll.GroupDoctor.Count > 0)
                    {
                        objOutputAll.Message = "Group Doctor";
                    }
                    else
                    {
                        objOutputAll.Message = "No any Group Doctor found.";
                    }
                    return objOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public DoctorChargeRet DoctorCharge(DoctorChargeIn objInput)
        {
            DoctorChargeRet objOutputAll = new DoctorChargeRet();

            DbCommand cmd = null;
            try
            {
                if (objInput.DoctorID == 43387)
                {
                    objOutputAll.RetValue = new List<DoctorChargeOut>();
                    DoctorChargeOut oDCCovid = new DoctorChargeOut();
                    oDCCovid.BasePrice = 250;
                    oDCCovid.CompanyID = 0;
                    oDCCovid.DiscName = "";
                    oDCCovid.DiscountID = 0;
                    oDCCovid.DiscPer = 0;
                    oDCCovid.DoctorCharge = 250;
                    oDCCovid.followup = 0;
                    objOutputAll.Message = "Doctor Charge";
                    objOutputAll.RetValue.Add(oDCCovid);
                }
                else
                {
                    cmd = dbReadOnly.GetStoredProcCommand("Pr_GetDoctorPriceForPracto_max");
                    dbReadOnly.AddInParameter(cmd, "@iacode", DbType.String, objInput.IAcode);
                    dbReadOnly.AddInParameter(cmd, "@registrationno", SqlDbType.Int, objInput.RegistrationNo);
                    dbReadOnly.AddInParameter(cmd, "@doctoID", DbType.String, objInput.DoctorID);
                    dbReadOnly.AddInParameter(cmd, "@locationID", DbType.String, objInput.HospitalID);

                    using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                    {
                        objOutputAll.RetValue = dr.AssembleBECollection<DoctorChargeOut>();
                        if (objOutputAll.RetValue.Count > 0)
                        {
                            if (objOutputAll.RetValue[0].DiscountID == 0)
                            {
                                DefaultDiscount oDefaultDiscount = getDefaultDiscount(objInput.HospitalID);
                                objOutputAll.RetValue[0].DiscountID = oDefaultDiscount.DiscountID;
                                objOutputAll.RetValue[0].DiscName = oDefaultDiscount.DiscName;
                                objOutputAll.RetValue[0].DiscPer = oDefaultDiscount.DiscPer;
                            }
                            objOutputAll.Message = "Doctor Charge";
                            // only in-case of new appointment for followup/re-visit take value from HIS.
                            if (objOutputAll.RetValue[0].followup == 0)
                            {
                                if (objInput.SourceAmount == 0)
                                    objInput.SourceAmount = objOutputAll.RetValue[0].BasePrice;

                                if (objOutputAll.RetValue[0].CompanyID > 0) // Credit
                                {
                                    if ((objOutputAll.RetValue[0].BasePrice == objInput.SourceAmount))
                                    {
                                        if (objOutputAll.RetValue[0].DoctorCharge > Convert.ToInt32((objInput.SourceAmount * 9) / 10))
                                        {
                                            objOutputAll.RetValue[0].DoctorCharge = Convert.ToInt32((objInput.SourceAmount * 9) / 10);
                                        }
                                        objOutputAll.RetValue[0].DiscName = "";
                                        objOutputAll.RetValue[0].DiscountID = 0;
                                        objOutputAll.RetValue[0].DiscPer = 0;
                                    }
                                    else if (objInput.SourceAmount > 0)
                                    {
                                        objOutputAll.RetValue[0].DiscPer = 10;
                                        objOutputAll.RetValue[0].DoctorCharge = Convert.ToInt32(objInput.SourceAmount * (100 - objOutputAll.RetValue[0].DiscPer) / 100);
                                    }

                                }
                                else //Cash
                                {
                                    if (objInput.SourceAmount > 0)
                                    {
                                        objOutputAll.RetValue[0].BasePrice = objInput.SourceAmount;
                                        if (objOutputAll.RetValue[0].DiscPer < 10)
                                            objOutputAll.RetValue[0].DiscPer = 10;
                                        objOutputAll.RetValue[0].DoctorCharge = Convert.ToInt32(objInput.SourceAmount * (100 - objOutputAll.RetValue[0].DiscPer) / 100);
                                    }
                                    else
                                    {
                                        objOutputAll.RetValue[0].DoctorCharge
                                        = Convert.ToInt32(objOutputAll.RetValue[0].BasePrice * (100 - objOutputAll.RetValue[0].DiscPer) / 100);
                                    }

                                }/////
                            }
                            else // in case of follow-up
                            {
                                DefaultDiscount oDefaultDiscount = getDefaultDiscount(objInput.HospitalID);
                                objOutputAll.RetValue[0].DiscountID = oDefaultDiscount.DiscountID;
                                objOutputAll.RetValue[0].DiscName = oDefaultDiscount.DiscName;
                                objOutputAll.RetValue[0].DiscPer = oDefaultDiscount.DiscPer;
                                objOutputAll.RetValue[0].DoctorCharge = Convert.ToInt32(objOutputAll.RetValue[0].BasePrice * (100 - objOutputAll.RetValue[0].DiscPer) / 100);

                            }
                        }
                        else
                        {
                            objOutputAll.Message = "Doctor Charge not found";
                        }


                    }
                }
                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public BedsInHospitalRet BedOfUnit(BedsInHospitalIn objInput)
        {
            BedsInHospitalRet objOutputAll = new BedsInHospitalRet();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_GetBedMaster");
                dbReadOnly.AddInParameter(cmd, "@HSPLocationId", DbType.String, objInput.HospitalID);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<BedsInHospitalOut>();
                    if (objOutputAll.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "Bed detail.";
                    }
                    else
                    {
                        objOutputAll.Message = "No any bed found.";
                    }
                    return objOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutInsertDoctorVisitRet PatientByBedTimeStamp(InInsertDoctorVisit objInput)
        {
            OutInsertDoctorVisitRet objOutput = new OutInsertDoctorVisitRet();

            DbCommand cmd = null;

            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_PatientByBedTimeStamp"); //ajay
                db.AddInParameter(cmd, "@BedID", SqlDbType.Int, objInput.BedID);
                db.AddInParameter(cmd, "@DocID", SqlDbType.Int, objInput.DoctorID);
                db.AddInParameter(cmd, "@sTimeStamp", DbType.String, objInput.DateTime);

                db.AddOutParameter(cmd, "@SSNo", DbType.String, 20);
                db.AddOutParameter(cmd, "@IACode", DbType.String, 20);
                db.AddOutParameter(cmd, "@RegistrationNo", SqlDbType.Int, 8);
                db.AddOutParameter(cmd, "@RetMsg", DbType.String, 200);
                db.AddOutParameter(cmd, "@RetCode", SqlDbType.Int, 8);
                db.AddOutParameter(cmd, "@IPID", SqlDbType.Int, 8);
                db.AddOutParameter(cmd, "@HSPLocationId", SqlDbType.Int, 8);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    objOutput.SSNo = cmd.Parameters["@SSNo"].Value.ToString();
                    objOutput.IACode = cmd.Parameters["@IACode"].Value.ToString();
                    objOutput.RegistrationNo = Convert.ToInt32(cmd.Parameters["@RegistrationNo"].Value.ToString());
                    objOutput.Message = cmd.Parameters["@RetMsg"].Value.ToString();
                    objOutput.RetCode = Convert.ToInt32(cmd.Parameters["@RetCode"].Value.ToString());
                    objOutput.IPID = Convert.ToInt32(cmd.Parameters["@IPID"].Value.ToString());
                    objOutput.HSPLocationId = Convert.ToInt32(cmd.Parameters["@HSPLocationId"].Value.ToString());
                    objOutput.MaxID = objOutput.IACode + "." + objOutput.RegistrationNo.ToString();
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public DefaultDiscount getDefaultDiscount(Int32 LocationID)
        {
            DefaultDiscount oDefaultDisc = new DefaultDiscount();
            oDefaultDisc.DiscName = "10% discount for Online payment";
            oDefaultDisc.DiscPer = 10;
            if (LocationID == 3)
                oDefaultDisc.DiscountID = 23259;
            else if (LocationID == 4)
                oDefaultDisc.DiscountID = 23260;
            else if (LocationID == 5)
                oDefaultDisc.DiscountID = 23261;
            else if (LocationID == 7)
                oDefaultDisc.DiscountID = 23262;
            else if (LocationID == 8)
                oDefaultDisc.DiscountID = 23263;
            else if (LocationID == 9)
                oDefaultDisc.DiscountID = 23264;
            else if (LocationID == 10)
                oDefaultDisc.DiscountID = 23265;
            else if (LocationID == 12)
                oDefaultDisc.DiscountID = 23266;
            else if (LocationID == 15)
                oDefaultDisc.DiscountID = 23267;
            else if (LocationID == 16)
                oDefaultDisc.DiscountID = 23268;
            else if (LocationID == 17)
                oDefaultDisc.DiscountID = 23269;
            else if (LocationID == 18)
                oDefaultDisc.DiscountID = 23270;
            else if (LocationID == 19)
                oDefaultDisc.DiscountID = 23271;
            else if (LocationID == 20)
                oDefaultDisc.DiscountID = 23272;
            else if (LocationID == 21)
                oDefaultDisc.DiscountID = 23273;
            else if (LocationID == 22)
                oDefaultDisc.DiscountID = 23274;
            else if (LocationID == 24)
                oDefaultDisc.DiscountID = 23275;
            else if (LocationID == 29)
                oDefaultDisc.DiscountID = 29780;
            else if (LocationID == 30)
                oDefaultDisc.DiscountID = 23276;
            else if (LocationID == 33)
                oDefaultDisc.DiscountID = 23277;
            else if (LocationID == 34)
                oDefaultDisc.DiscountID = 23278;
            else if (LocationID == 37)
                oDefaultDisc.DiscountID = 23279;
            else if (LocationID == 38)
                oDefaultDisc.DiscountID = 23280;
            else
            {
                oDefaultDisc.DiscountID = 0;
                oDefaultDisc.DiscName = "No Discount";
                oDefaultDisc.DiscPer = 10;
            }




            return oDefaultDisc;
        }

        public OutEPCountMPHRxRet EPCountMPHRx(InEPCountMPHRx objInput)
        {
            OutEPCountMPHRxRet objOutput = new OutEPCountMPHRxRet();

            DbCommand cmd = null;

            try
            {
                cmd = db.GetStoredProcCommand("Pr_OnWeb_EPrescription_Count_MPHRx"); //ajay
                db.AddInParameter(cmd, "@sDate", DbType.String, objInput.SDate);


                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    objOutput.RetValue = dr.AssembleBECollection<OutEPCountMPHRx>();
                    if (objOutput.RetValue.Count > 0)
                    {
                        Int32 i = 0;
                        for (i = 0; i < objOutput.RetValue.Count; i++)
                        {
                            if (objOutput.RetValue[i].Response == "SUCCESS")
                            {
                                objOutput.NCount = objOutput.RetValue[i].NCount;
                                break;
                            }
                        }
                        objOutput.Message = "EP-Count.";
                    }
                    else
                    {
                        objOutput.Message = "No count found.";
                    }
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutInterfaceData90Ret InterfaceData90OLD(InInterfaceData90 objInput) //not in used
        {
            OutInterfaceData90Ret objOutput = new OutInterfaceData90Ret();

            DbCommand cmd = null;
            SqlDatabase dbTMS = null;
            String strTMS = ConfigurationManager.ConnectionStrings["DBConnectionStringTMS"].ToString();
            strTMS = Decryptor.DecryptString(strTMS, "3w8motherw4fdcj7");
            dbTMS = new SqlDatabase(strTMS);

            try
            {
                cmd = dbTMS.GetStoredProcCommand("pr_l_InterfaceData"); //ajay
                dbTMS.AddInParameter(cmd, "@sTransDate", DbType.String, objInput.sTransDate);
                dbTMS.AddInParameter(cmd, "@NoOfOrders", SqlDbType.Int, objInput.NoOfOrders);
                dbTMS.AddInParameter(cmd, "@SampleCollected", SqlDbType.Int, objInput.SampleCollected);
                dbTMS.AddInParameter(cmd, "@LabReceived", SqlDbType.Int, objInput.LabReceived);
                dbTMS.AddInParameter(cmd, "@Result", SqlDbType.Int, objInput.Result);
                dbTMS.AddInParameter(cmd, "@Cancelled", SqlDbType.Int, objInput.Cancelled);
                dbTMS.AddInParameter(cmd, "@PDFAPICall", SqlDbType.Int, objInput.PDFAPICall);
                dbTMS.AddInParameter(cmd, "@AppName", DbType.String, objInput.AppName);

                ArrInterfaceData90 oArrInterfaceData90 = new ArrInterfaceData90();

                using (IDataReader dr = dbTMS.ExecuteReader(cmd))
                {
                    oArrInterfaceData90.RetValue = dr.AssembleBECollection<InterfaceData90>();
                    if (oArrInterfaceData90.RetValue.Count > 0)
                    {
                        objOutput.Message = oArrInterfaceData90.RetValue[0].msg;
                    }
                    else
                    {
                        objOutput.Message = "No count found.";
                    }
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutInterfaceData90Ret InterfaceData90(InInterfaceData90 objInput)
        {
            OutInterfaceData90Ret objOutput = new OutInterfaceData90Ret();

            String TransType = "";
            Int32 TransCount = 0;

            objOutput.Message = "";
            String retMessage = "";
            try
            {
                if (objInput.NoOfOrders > 0)
                {
                    TransType = "Orders";
                    TransCount = objInput.NoOfOrders;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.SampleCollected > 0)
                {
                    TransType = "SampleCollected";
                    TransCount = objInput.SampleCollected;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.LabReceived > 0)
                {
                    TransType = "LabReceived";
                    TransCount = objInput.LabReceived;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.Result > 0)
                {
                    TransType = "Result";
                    TransCount = objInput.Result;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.Cancelled > 0)
                {
                    TransType = "OrderCancelled";
                    TransCount = objInput.Cancelled;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.PDFAPICall > 0)
                {
                    TransType = "ReportPDFAPICall";
                    TransCount = objInput.PDFAPICall;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                if (objInput.EPCount > 0)
                {
                    TransType = "EPCount";
                    TransCount = objInput.EPCount;
                    retMessage = InterfaceData90Insert(TransType, TransCount, objInput.AppName, objInput.sTransDate);
                    objOutput.Message = objOutput.Message + TransType + ": " + retMessage + ", ";
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                return objOutput;
                throw ex;

            }
            finally
            {

            }
        }

        private string InterfaceData90Insert(String TransType, Int32 TransCount, String AppName, String sTransDate)
        {
            String RetMsg = "";
            DbCommand cmd = null;
            SqlDatabase dbTMS = null;
            String strTMS = ConfigurationManager.ConnectionStrings["DBConnectionStringTMS"].ToString();
            strTMS = Decryptor.DecryptString(strTMS, "3w8motherw4fdcj7");
            dbTMS = new SqlDatabase(strTMS);

            try
            {
                cmd = dbTMS.GetStoredProcCommand("pr_l_InterfaceDataRow"); //ajay
                dbTMS.AddInParameter(cmd, "@AppName", DbType.String, AppName);
                dbTMS.AddInParameter(cmd, "@sTransDate", DbType.String, sTransDate);
                dbTMS.AddInParameter(cmd, "@TransType", DbType.String, TransType);
                dbTMS.AddInParameter(cmd, "@TransCount", SqlDbType.Int, TransCount);
                ArrInterfaceData90 oArrInterfaceData90 = new ArrInterfaceData90();

                using (IDataReader dr = dbTMS.ExecuteReader(cmd))
                {
                    oArrInterfaceData90.RetValue = dr.AssembleBECollection<InterfaceData90>();
                    if (oArrInterfaceData90.RetValue.Count > 0)
                    {
                        RetMsg = oArrInterfaceData90.RetValue[0].msg;
                    }
                    else
                    {
                        RetMsg = "Failure";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    RetMsg = "Database server not found or not accessible.";
                else
                    RetMsg = ex.Message;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            return RetMsg;
        }

        public OutSendSMSFromTMSDB90Ret SendSMSFromTMSDB90(InSendSMSFromTMSDB90 objInput)
        {
            OutSendSMSFromTMSDB90Ret objOutput = new OutSendSMSFromTMSDB90Ret();
            OutSendSMSFromTMSDB90Ret oDBRet;
            DbCommand cmd = null;
            SqlDatabase dbTMSProd = null;
            String strTMSProd = ConfigurationManager.ConnectionStrings["DBConnectionStringTMSProd90"].ToString();
            strTMSProd = Decryptor.DecryptString(strTMSProd, "3w8motherw4fdcj7");
            dbTMSProd = new SqlDatabase(strTMSProd);


            //try
            //{

            //    OutSendSMSFromTMSDB90 oOutOne = new OutSendSMSFromTMSDB90();
            //    objOutput.RetVal = new List<OutSendSMSFromTMSDB90>();

            //    for (Int32 i = 0; i < objInput.Detail.Count; i++)
            //    {
            //        oDBRet = new OutSendSMSFromTMSDB90Ret();
            //        oDBRet.RetVal = new List<OutSendSMSFromTMSDB90>();

            //        cmd = dbTMSProd.GetStoredProcCommand("PR_OnWebSendSMSTemp"); //ajay
            //        dbTMSProd.AddInParameter(cmd, "@mobile", DbType.String, objInput.Detail[0].MobileNo);
            //        dbTMSProd.AddInParameter(cmd, "@msg", DbType.String, objInput.Detail[0].Message);
            //        dbTMSProd.AddInParameter(cmd, "@Async", DbType.String, "");
            //        dbTMSProd.AddInParameter(cmd, "@Feedid", DbType.String, objInput.FeedID);
            //        dbTMSProd.AddInParameter(cmd, "@UserName", DbType.String, objInput.UserName);
            //        dbTMSProd.AddInParameter(cmd, "@Password", DbType.String, objInput.Password);
            //        dbTMSProd.AddInParameter(cmd, "@JobName", DbType.String, objInput.JobName);
            //        dbTMSProd.AddInParameter(cmd, "@dttm", DbType.String, "");

            //        oOutOne = new OutSendSMSFromTMSDB90();

            //        using (IDataReader dr = dbTMSProd.ExecuteReader(cmd))
            //        {
            //            oDBRet.RetVal = dr.AssembleBECollection<OutSendSMSFromTMSDB90>();
            //            oOutOne.RefId = objInput.Detail[i].RefId;

            //            if (oDBRet.RetVal.Count > 0)
            //            {
            //                oOutOne.DBDetMessage = oDBRet.RetVal[0].DBDetMessage;                            
            //                oOutOne.iStatus = oDBRet.RetVal[0].iStatus;
            //                oOutOne.sStatusText = oDBRet.RetVal[0].sStatusText;                            
            //            }
            //            else
            //            {
            //                oOutOne.DBDetMessage = "Failure";
            //                oOutOne.iStatus = 0;
            //                oOutOne.sStatusText = "Failure";
            //            }

            //            objOutput.RetVal.Add(oOutOne);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message.Contains("The server was not found or was not accessible."))
            //        objOutput.Message = "Database server not found or not accessible.";
            //    else
            //        objOutput.Message = ex.ToString();

            //}
            //finally
            //{
            //    if (cmd != null)
            //        cmd.Dispose();
            //}
            return objOutput;
        }

        public OutQMSDisplayGetQueueRet QMSDisplayGetQueue(InQMSDisplayGetQueue objInput)
        {
            OutQMSDisplayGetQueueRet objOutput = new OutQMSDisplayGetQueueRet();

            DbCommand cmd = null;
            SqlDatabase dbTMS = null;
            String strTMSProd = ConfigurationManager.ConnectionStrings["DBConnectionStringTMS"].ToString();
            strTMSProd = Decryptor.DecryptString(strTMSProd, "3w8motherw4fdcj7");
            dbTMS = new SqlDatabase(strTMSProd);

            try
            {
                cmd = dbTMS.GetStoredProcCommand("PR_QMSDisplayGetQueue"); //ajay
                dbTMS.AddInParameter(cmd, "@DisplayID", SqlDbType.Int, objInput.DisplayID);

                using (IDataReader dr = dbTMS.ExecuteReader(cmd))
                {
                    objOutput.RetVal = dr.AssembleBECollection<OutQMSDisplayGetQueue>();
                    if (objOutput.RetVal.Count > 0)
                    {
                        objOutput.Message = "Token Detail";
                    }
                    else
                    {
                        objOutput.Message = "No Token Detail fount";
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.ToString();

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
            return objOutput;
        }

        public OutDietBarCodeKOTTAT DietBarCodeKOTTAT(InDietBarCodeKOTTAT objInput)
        {
            OutDietBarCodeKOTTAT objOutput = new OutDietBarCodeKOTTAT();
            DbCommand cmd = null;
            DietBarCodeKOTTAT oRes = new DietBarCodeKOTTAT();

            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_DietBarCodeKOTTATUpdate"); //ajay
                db.AddInParameter(cmd, "@BarCode", DbType.String, objInput.BarCode);
                db.AddInParameter(cmd, "@sScanDateTime", DbType.String, objInput.ScanDateTime);
                db.AddInParameter(cmd, "@TransType", DbType.String, objInput.TransType);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    oRes.RetVal = dr.AssembleBECollection<DietBarCodeKOTTATData>();
                    if (oRes.RetVal.Count > 0)
                    {

                        objOutput.RetMessage = oRes.RetVal[0].RetMas;
                        objOutput.retID = oRes.RetVal[0].RetID;
                        objOutput.Message = "Success";
                    }
                    else
                        objOutput.Message = "Failure";

                }
                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutLoginRet Login(InLogin objLoginInput)
        {
            OutLoginRet objLoginOutputAll = new OutLoginRet();
            LoginDataMain oLoginData = new LoginDataMain();

            DbCommand cmd = null;
            DbCommand cmd2 = null;
            String outMsg = "";
            try
            {
                string userName = objLoginInput.UserName;
                string Password = objLoginInput.OldPwd;
                string token = "";
                if (objLoginInput.MacAddress != null)
                    token = objLoginInput.MacAddress;

                int UserId = 0;
                cmd = db.GetStoredProcCommand("PR_DigiCare_CheckUserId_MAX"); //ajay
                db.AddInParameter(cmd, "@LoginID", DbType.String, userName);
                Int32 noOfDays = 0;
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    String dbPassword = "";
                    while (dr.Read())
                    {

                        dbPassword = (string)dr["User_Password"];
                        UserId = (int)dr["id"];
                        noOfDays = (int)dr["noOfDays"];
                    }

                    WebAPI.Controllers.LoginCheck ULogin = new WebAPI.Controllers.LoginCheck();
                    dbPassword = ULogin.DecryptPassword(dbPassword);

                    //var abc =abs.UserLoginAuthentication(objLoginInput);

                    //bool success = true;
                    // AD Login
                    Int64 intADDetail = 0;
                    if (dbPassword != Password)
                    {
                        CAPI abc = new CAPI();
                        intADDetail = abc.ADID(objLoginInput.UserName, objLoginInput.OldPwd);
                    }

                    if (((intADDetail == 1) || (dbPassword == Password)) && (UserId != 0))
                    {
                        cmd2 = db.GetStoredProcCommand("PR_OnWeb_LoginPharmacyOnly"); //ajay
                        db.AddInParameter(cmd2, "@UserId", SqlDbType.Int, UserId);
                        db.AddInParameter(cmd2, "@token", DbType.String, token);

                        using (IDataReader dr2 = db.ExecuteReader(cmd2))
                        {
                            objLoginOutputAll.DaysLeftForPwd = noOfDays;
                            oLoginData.RetValue = dr2.AssembleBECollection<LoginDataDetail>();
                            if (oLoginData.RetValue.Count > 0)
                            {
                                OutLogin oL;
                                objLoginOutputAll.UserId = oLoginData.RetValue[0].UserId;
                                objLoginOutputAll.SessionId = oLoginData.RetValue[0].SessionId;
                                objLoginOutputAll.MacAddress = oLoginData.RetValue[0].MacAddress;
                                objLoginOutputAll.UserFullName = oLoginData.RetValue[0].UserFullName;
                                objLoginOutputAll.Message = "Login Done Sucessfully";

                                objLoginOutputAll.RetValue = new List<OutLogin>();

                                for (int i = 0; i < oLoginData.RetValue.Count; i++)
                                {
                                    oL = new OutLogin();
                                    oL.HospitalId = oLoginData.RetValue[i].HospitalId;
                                    oL.HospitalName = oLoginData.RetValue[i].HospitalName;
                                    oL.StationId = oLoginData.RetValue[i].StationId;
                                    oL.StationName = oLoginData.RetValue[i].StationName;

                                    objLoginOutputAll.RetValue.Add(oL);

                                }
                            }

                        }
                    }
                    else
                    {
                        objLoginOutputAll.Code = 0;
                        objLoginOutputAll.Status = "Failure";
                        objLoginOutputAll.Message = "Invalid login";
                        if (UserId == 0)
                            objLoginOutputAll.Message = "Invalid User";
                        else if (dbPassword != Password)
                            objLoginOutputAll.Message = "Wrong Password. " + outMsg;
                    }
                    return objLoginOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objLoginOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objLoginOutputAll.Message = ex.Message;
                }

                objLoginOutputAll.Status = "Failure";
                objLoginOutputAll.Code = 0;
                return objLoginOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        public OutSearchMedicineRet SearchMedicine(InSearchMedicine objInput)
        {
            OutSearchMedicineRet objOutputAll = new OutSearchMedicineRet();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_SearchMedicine");
                dbReadOnly.AddInParameter(cmd, "@Stationid", SqlDbType.Int, objInput.StationId);
                dbReadOnly.AddInParameter(cmd, "@SearchText", DbType.String, objInput.SearchText);
                dbReadOnly.AddInParameter(cmd, "@NoOfRows", DbType.String, objInput.NoOfRows);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<OutSearchMedicine>();
                    if (objOutputAll.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "List of medicine";
                    }
                    else
                    {
                        objOutputAll.Message = "No medicine found";
                    }

                    return objOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutBatchWiseQuantityRet BatchWiseQuantity(InBatchWiseQuantity objInput)
        {
            OutBatchWiseQuantityRet objOutputAll = new OutBatchWiseQuantityRet();
            DbCommand cmd = null;
            try
            {


                cmd = db.GetStoredProcCommand("PR_OnWeb_BatchWiseQuantity"); //ajay
                db.AddInParameter(cmd, "@ItemID", SqlDbType.Int, objInput.ItemID);
                db.AddInParameter(cmd, "@StationId", SqlDbType.Int, objInput.StationId);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {

                    objOutputAll.RetValue = dr.AssembleBECollection<OutBatchWiseQuantity>();
                    if (objOutputAll.RetValue.Count > 0)
                        objOutputAll.Message = "List of Batch";
                    else
                        objOutputAll.Message = "No Batch found";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPharmacySaveCartOrderRet PharmacySaveCartOrder(InPharmacySaveCartOrder objInput)
        {

            OutPharmacySaveCartOrderRet objOutput = new OutPharmacySaveCartOrderRet();
            DbCommand cmd = null;
            try
            {

                DataTable dtItems = new DataTable();
                dtItems.Columns.Add("ItemID", typeof(Int32));
                dtItems.Columns.Add("BatchID", typeof(Int32));
                dtItems.Columns.Add("Quantity", typeof(Int32));
                dtItems.Columns.Add("MRP", typeof(Decimal));
                dtItems.Columns.Add("Remarks", typeof(String));
                if (objInput.Items != null)
                {
                    for (Int32 i = 0; i < objInput.Items.Count; i++)
                    {
                        dtItems.Rows.Add(objInput.Items[i].ItemID, objInput.Items[i].BatchID, objInput.Items[i].QuantityNew, objInput.Items[i].MRP, objInput.Items[i].RemarksItem);
                    }
                }

                DataTable dtUrl = new DataTable();
                dtUrl.Columns.Add("UrlLink", typeof(String));
                dtUrl.Columns.Add("UrlDesc", typeof(String));
                dtUrl.Columns.Add("SlNo", typeof(Int32));

                if (objInput.Urls != null)
                {
                    for (Int32 i = 0; i < objInput.Urls.Count; i++)
                    {
                        dtUrl.Rows.Add(objInput.Urls[i].Url, objInput.Urls[i].UrlDesc, objInput.Urls[i].SlNo);
                    }
                }


                cmd = db.GetStoredProcCommand("PR_OnWeb_PharmacyCartOrder"); //ajay
                db.AddInParameter(cmd, "@flag", DbType.String, objInput.Flag);
                db.AddInParameter(cmd, "@IACode", DbType.String, objInput.IACode);
                db.AddInParameter(cmd, "@PatFName", DbType.String, objInput.PatFName);
                db.AddInParameter(cmd, "@PatLName", DbType.String, objInput.PatLName);
                db.AddInParameter(cmd, "@PatMobNo", DbType.String, objInput.PatMobNo);
                db.AddInParameter(cmd, "@PatAddress", DbType.String, objInput.PatAddress);
                db.AddInParameter(cmd, "@sPatDOB", DbType.String, objInput.PatDOB);
                db.AddInParameter(cmd, "@PaymentMode", DbType.String, objInput.PaymentMode);
                db.AddInParameter(cmd, "@Remarks", DbType.String, objInput.RemarksMain);

                db.AddInParameter(cmd, "@BatchDetail", SqlDbType.Structured, dtItems);
                db.AddInParameter(cmd, "@EPUrl", SqlDbType.Structured, dtUrl);

                db.AddInParameter(cmd, "@Registrationno", SqlDbType.Int, objInput.Registrationno);
                db.AddInParameter(cmd, "@OrderByDocId", SqlDbType.Int, objInput.OrderByDocId);
                db.AddInParameter(cmd, "@UserID", SqlDbType.Int, objInput.UserID);
                db.AddInParameter(cmd, "@CartOrderID", SqlDbType.Int, objInput.CartOrderID);
                db.AddInParameter(cmd, "@StationID", SqlDbType.Int, objInput.StationID);
                db.AddInParameter(cmd, "@DiscountId", SqlDbType.Int, objInput.DiscountID);
                db.AddInParameter(cmd, "@Source", DbType.String, objInput.Source);
                db.AddInParameter(cmd, "@DeliveryType", DbType.String, objInput.DeliveryType);

                db.AddOutParameter(cmd, "@RetMsg", DbType.String, 200);
                db.AddOutParameter(cmd, "@RetCartOrderID", SqlDbType.Int, 8);
                db.AddOutParameter(cmd, "@NoOfItems", SqlDbType.Int, 8);
                db.AddOutParameter(cmd, "@TotalAmount", SqlDbType.Decimal, 12);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    objOutput.DBMessage = cmd.Parameters["@RetMsg"].Value.ToString();
                    objOutput.CartOrderID = Convert.ToInt32(cmd.Parameters["@RetCartOrderID"].Value);
                    objOutput.NoOfItems = Convert.ToInt32(cmd.Parameters["@NoOfItems"].Value);
                    objOutput.TotalAmount = Convert.ToDecimal(cmd.Parameters["@TotalAmount"].Value);
                    if (objOutput.DBMessage == "")
                        objOutput.Message = "Successfully";
                    else
                        objOutput.Message = objOutput.DBMessage;

                    if (objOutput.CartOrderID == 0)
                    {
                        objOutput.Code = WebAPI.Models.ProcessStatus.Fail;
                        objOutput.Status = "Fail";
                    }
                    else
                    {
                        objOutput.Code = WebAPI.Models.ProcessStatus.Success;
                        objOutput.Status = "Success";
                    }
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutput.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutput.Message = ex.Message;
                }

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutGetEPrescriptionListRet GetEPrescriptionList(InGetEPrescriptionList objInput)
        {
            OutGetEPrescriptionListRet objOutputAll = new OutGetEPrescriptionListRet();
            DbCommand cmd = null;
            try
            {


                cmd = db.GetStoredProcCommand("PR_OnWeb_GetEPrescriptionList"); //ajay
                db.AddInParameter(cmd, "@IACode", DbType.String, objInput.IACode);
                db.AddInParameter(cmd, "@RegistrationNo", SqlDbType.Int, objInput.RegistrationNo);
                db.AddInParameter(cmd, "@sVisitDateTimeFrom", DbType.String, objInput.VisitDateFrom);
                db.AddInParameter(cmd, "@sVisitDateTimeTo", DbType.String, objInput.VisitDateTo);
                db.AddInParameter(cmd, "@HospitalID", SqlDbType.Int, objInput.HospitalID);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {

                    objOutputAll.RetValue = dr.AssembleBECollection<OutGetEPrescriptionList>();
                    if (objOutputAll.RetValue.Count > 0)
                        objOutputAll.Message = "List of E-Prescription";
                    else
                        objOutputAll.Message = "No E-Prescription found";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPharmacyCartOrderSelectRet PharmacyCartOrderSelect(InPharmacyCartOrderSelect objInput)
        {
            OutPharmacyCartOrderSelectRet objOutputAll = new OutPharmacyCartOrderSelectRet();
            PharmacyCartOrderSelectIsDataFoundArr objDataFound = new PharmacyCartOrderSelectIsDataFoundArr();
            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_PharmacyCartOrderSelect"); //ajay
                db.AddInParameter(cmd, "@StationId", SqlDbType.Int, objInput.StationId);
                db.AddInParameter(cmd, "@CartOrderId", SqlDbType.Int, objInput.CartOrderId);
                db.AddInParameter(cmd, "@IACode", DbType.String, objInput.IACode);
                db.AddInParameter(cmd, "@Registrationno", SqlDbType.Int, objInput.Registrationno);
                db.AddInParameter(cmd, "@PatFName", DbType.String, objInput.PatFName);
                db.AddInParameter(cmd, "@PatMobNo", DbType.String, objInput.PatMobNo);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    objDataFound.RetValue = dr.AssembleBECollection<PharmacyCartOrderSelectIsDataFound>();
                    if (objDataFound.RetValue.Count > 0)
                    {
                        if (objDataFound.RetValue[0].isDataFound == 1)
                        {
                            objOutputAll.CartOrderID = objDataFound.RetValue[0].CartOrderID;
                            objOutputAll.IACode = objDataFound.RetValue[0].IACode;
                            objOutputAll.OrderByDocId = objDataFound.RetValue[0].OrderByDocId;
                            objOutputAll.PatAddress = objDataFound.RetValue[0].PatAddress;
                            objOutputAll.PatFName = objDataFound.RetValue[0].PatFName;
                            objOutputAll.PatLName = objDataFound.RetValue[0].PatLName;
                            objOutputAll.PatMobNo = objDataFound.RetValue[0].PatMobNo;
                            objOutputAll.Registrationno = objDataFound.RetValue[0].Registrationno;
                            objOutputAll.Message = "Cart detail found!";

                            dr.NextResult();
                            objOutputAll.RetValue = dr.AssembleBECollection<OutPharmacyCartOrderSelect>();
                        }
                        else
                        {
                            objOutputAll.Message = "Cart detail not found!";
                        }

                    }
                    else
                        objOutputAll.Message = "Cart detail not found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutInPatientDetailTPARet InPatientDetailTPA(InInPatientDetailTPA objInput)
        {
            OutInPatientDetailTPARet objOutput = new OutInPatientDetailTPARet();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_InPatientDetailTPA"); //ajay

                dbReadOnly.AddInParameter(cmd, "@IPID", SqlDbType.Int, objInput.IPID);
                dbReadOnly.AddInParameter(cmd, "@HSPLocationId", SqlDbType.Int, objInput.HospitalId);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    InPatientDetailTPADetailAll ad = new InPatientDetailTPADetailAll();
                    ad.RetVal = dr.AssembleBECollection<InPatientDetailTPADetail>();
                    if (ad.RetVal.Count == 1)
                    {
                        objOutput.Age = ad.RetVal[0].Age;
                        objOutput.Department = ad.RetVal[0].PrimDocDepartment;
                        objOutput.DOA = ad.RetVal[0].AdmitDateTime;
                        objOutput.DOD = ad.RetVal[0].EstdDchgDateTime;
                        objOutput.Gender = ad.RetVal[0].Gender;
                        objOutput.InvestigationDetails = ad.RetVal[0].Investigations;
                        objOutput.MedicalManagementDetails = ad.RetVal[0].MedicalManagement;
                        objOutput.PatientName = ad.RetVal[0].Firstname.Trim() + " " + ad.RetVal[0].LastName.Trim();
                        objOutput.Procedure = ad.RetVal[0].ProcedureName;
                        objOutput.ProfessionalFees = ad.RetVal[0].ProcedureFee;
                        objOutput.ReasonForHospitalization = ad.RetVal[0].PrimDocDepartment;
                        objOutput.RoomRent = ad.RetVal[0].FirstRoomRent;
                        objOutput.RoomType = ad.RetVal[0].BedTypeName + " - " + ad.RetVal[0].BedName;
                        objOutput.SurgicalManagement = ad.RetVal[0].ProcedureName;
                        objOutput.TreatingDoctorName = ad.RetVal[0].PrimDocName;
                        objOutput.TypeOfAdmission = ad.RetVal[0].AdmissionType;
                        objOutput.PrimDocMobNo = ad.RetVal[0].PrimDocMobNo;
                        objOutput.PatMobileNo = ad.RetVal[0].PatMobileNo;
                        objOutput.PatAddress = ad.RetVal[0].PatAddress;
                        objOutput.Message = "Successfully";
                    }
                    else if (ad.RetVal.Count > 1)
                        objOutput.Message = "More than one patient found";
                    else
                        objOutput.Message = "No patient found";
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutput.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutput.Message = ex.Message;
                }

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutInPatientClaimBookRet InPatientClaimBook(InInPatientClaimBook objInput)
        {
            OutInPatientClaimBookRet objOutput = new OutInPatientClaimBookRet();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_InPatientClaimBook"); //ajay

                dbReadOnly.AddInParameter(cmd, "@IPID", SqlDbType.Int, objInput.TreatmentId);
                dbReadOnly.AddInParameter(cmd, "@HSPLocationId", SqlDbType.Int, objInput.HospitalId);
                //dbReadOnly.AddInParameter(cmd, "@MaxID", SqlDbType.NVarChar, objInput.PatientId);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    InPatientClaimBookAll ad = new InPatientClaimBookAll();
                    ad.RetVal = dr.AssembleBECollection<InPatientClaimBookDetail>();
                    if (ad.RetVal.Count == 1)
                    {
                        objOutput.AdmissionDate = ad.RetVal[0].AdmissionDate;
                        objOutput.AdmissionTime = ad.RetVal[0].AdmissionTime;
                        objOutput.City = ad.RetVal[0].City;
                        objOutput.DischargeDate = ad.RetVal[0].DischargeDate;
                        objOutput.DischargeTime = ad.RetVal[0].DischargeTime;
                        objOutput.HospitalID = ad.RetVal[0].HospitalID;
                        objOutput.HospitalName = ad.RetVal[0].HospitalName;
                        objOutput.InsurerID = ad.RetVal[0].InsurerID;
                        objOutput.InsurerName = ad.RetVal[0].InsurerName;
                        objOutput.MRNNo = ad.RetVal[0].MRNNo;
                        objOutput.PatientID = ad.RetVal[0].PatientID;
                        objOutput.PatientName = ad.RetVal[0].PatientName;
                        objOutput.PIN = ad.RetVal[0].PIN;
                        objOutput.PolicyID = ad.RetVal[0].PolicyID;
                        objOutput.TPAID = ad.RetVal[0].TPAID;
                        objOutput.TreatmentID = ad.RetVal[0].TreatmentID;
                        objOutput.Message = "Success";
                    }
                    else if (ad.RetVal.Count > 1)
                        objOutput.Message = "More than one patient found";
                    else
                        objOutput.Message = "No patient found";
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutput.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutput.Message = ex.Message;
                }

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPharmacyCartOrderSearchRet PharmacyCartOrderSearch(InPharmacyCartOrderSearch objInput)
        {
            OutPharmacyCartOrderSearchRet objOutputAll = new OutPharmacyCartOrderSearchRet();

            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_PharmacyCartOrderSearch"); //ajay
                db.AddInParameter(cmd, "@IACode", DbType.String, objInput.IACode);
                db.AddInParameter(cmd, "@Registrationno", SqlDbType.Int, objInput.Registrationno);
                db.AddInParameter(cmd, "@PatMobNo", DbType.String, objInput.PatMobNo);
                db.AddInParameter(cmd, "@PatFName", DbType.String, objInput.PatFName);
                db.AddInParameter(cmd, "@SearchFlag", DbType.String, objInput.SearchFlag);
                db.AddInParameter(cmd, "@SDateFrom", DbType.String, objInput.DateFrom);
                db.AddInParameter(cmd, "@SDateTo", DbType.String, objInput.DateTo);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<OutPharmacyCartOrderSearch>();
                    if (objOutputAll.RetValue.Count > 0)
                        objOutputAll.Message = "List of Cart";
                    else
                        objOutputAll.Message = "No any Cart found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutAppointmentCancelRet AppointmentCancel(InAppointmentCancel objInput)
        {
            OutAppointmentCancelRet objOutputAll = new OutAppointmentCancelRet();
            DbCommand cmd = null;
            try
            {


                cmd = db.GetStoredProcCommand("PR_OnWeb_AppointmentCancel"); //ajay
                db.AddInParameter(cmd, "@paymentID", SqlDbType.Int, objInput.PaymentID);
                db.AddInParameter(cmd, "@SourceOfData", DbType.String, objInput.BookingSource);
                db.AddInParameter(cmd, "@sCancellationDate", DbType.String, objInput.CancellationDate);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    MAppointmentCancelArr oRV = new MAppointmentCancelArr();
                    oRV.RetValue = dr.AssembleBECollection<MAppointmentCancel>();
                    if (oRV.RetValue.Count > 0)
                    {
                        objOutputAll.Message = oRV.RetValue[0].retMsg;
                    }
                    else
                        objOutputAll.Message = "No data found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPharmacyCartOrderSearchDetailRet PharmacyCartOrderSearchDetail(InPharmacyCartOrderSearchDetail objInput)
        {
            OutPharmacyCartOrderSearchDetailRet objOutputAll = new OutPharmacyCartOrderSearchDetailRet();

            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_PharmacyCartOrderSearchByID"); //ajay
                db.AddInParameter(cmd, "@CartOrderID", SqlDbType.Int, objInput.CartID);

                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    PharmacyCartOrderSearchDetailArr cartOrder = new PharmacyCartOrderSearchDetailArr();
                    cartOrder.RetValue = dr.AssembleBECollection<OutPharmacyCartOrderSearchDetailRet>();
                    if (cartOrder.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "Cart detail.";
                        objOutputAll.CartOrderID = cartOrder.RetValue[0].CartOrderID;
                        objOutputAll.docName = cartOrder.RetValue[0].docName;
                        objOutputAll.IsActive = cartOrder.RetValue[0].IsActive;
                        objOutputAll.isBilled = cartOrder.RetValue[0].isBilled;
                        objOutputAll.MaxID = cartOrder.RetValue[0].MaxID;
                        objOutputAll.OrderByDocId = cartOrder.RetValue[0].OrderByDocId;
                        objOutputAll.PatAddress = cartOrder.RetValue[0].PatAddress;
                        objOutputAll.PatDOB = cartOrder.RetValue[0].PatDOB;
                        objOutputAll.PatFName = cartOrder.RetValue[0].PatFName;
                        objOutputAll.PatLName = cartOrder.RetValue[0].PatLName;
                        objOutputAll.PatMobNo = cartOrder.RetValue[0].PatMobNo;
                        objOutputAll.PaymentMode = cartOrder.RetValue[0].PaymentMode;
                        objOutputAll.Remarks = cartOrder.RetValue[0].Remarks;
                        objOutputAll.SaveUserID = cartOrder.RetValue[0].SaveUserID;
                        objOutputAll.StationID = cartOrder.RetValue[0].StationID;
                        objOutputAll.isCancelled = cartOrder.RetValue[0].isCancelled;
                        objOutputAll.SavedDateTime = cartOrder.RetValue[0].SavedDateTime;
                        objOutputAll.LastUpdateDateTime = cartOrder.RetValue[0].LastUpdateDateTime;
                        objOutputAll.DiscountId = cartOrder.RetValue[0].DiscountId;
                        objOutputAll.CancelledReasonId = cartOrder.RetValue[0].CancelledReasonId;
                        objOutputAll.CancelledRemarks = cartOrder.RetValue[0].CancelledRemarks;
                        objOutputAll.DeliveryType = cartOrder.RetValue[0].DeliveryType;

                        dr.NextResult();
                        objOutputAll.RetValue = dr.AssembleBECollection<OutPharmacyCartOrderSearchDetail>();

                        dr.NextResult();
                        objOutputAll.RetValueUrl = dr.AssembleBECollection<OutPharmacyCartOrderSearchDetailUrl>();
                    }
                }
                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutVNUAPIPatientRet VNUAPIPatient(InVNUAPIPatient objInput)
        {
            OutVNUAPIPatientRet objOutput = new OutVNUAPIPatientRet();
            DbCommand cmd = null;
            String[] arrReqInfo = objInput.ReqInfo.Split(',');

            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_APIPatient"); //ajay

                dbReadOnly.AddInParameter(cmd, "@IPID", SqlDbType.Int, objInput.TreatmentId);
                dbReadOnly.AddInParameter(cmd, "@HSPLocationId", SqlDbType.Int, objInput.HospitalId);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    VNUAPIPatientMainArr main = new VNUAPIPatientMainArr();
                    main.retVal = dr.AssembleBECollection<VNUAPIPatientMain>();
                    if (main.retVal.Count == 1)
                    {
                        objOutput.HospitalID = main.retVal[0].HospitalID;
                        objOutput.MRNNo = main.retVal[0].MRNNo;
                        objOutput.PatientID = main.retVal[0].PatientID;
                        objOutput.TreatmentID = main.retVal[0].TreatmentID;
                        objOutput.TreatingDoctor = main.retVal[0].TreatingDoctor;
                        objOutput.DoctorContactNumber = main.retVal[0].DoctorContactNumber;
                        objOutput.Qualification = main.retVal[0].Qualification;
                        objOutput.DocRegNoWithStateCode = main.retVal[0].DocRegNoWithStateCode;
                        objOutput.Department = main.retVal[0].Department;
                        objOutput.Speciality = main.retVal[0].Speciality;
                        objOutput.PatientLastName = main.retVal[0].PatientLastName;
                        objOutput.PatientFirstName = main.retVal[0].PatientFirstName;
                        objOutput.Gender = main.retVal[0].Gender;
                        objOutput.DOB = main.retVal[0].DOB;
                        objOutput.Weight = main.retVal[0].Weight;
                        objOutput.Height = main.retVal[0].Height;
                        objOutput.ContactNumber = main.retVal[0].ContactNumber;
                        objOutput.IDCardName = main.retVal[0].IDCardName;
                        objOutput.IDCard = main.retVal[0].IDCard;
                        objOutput.InsurerName = main.retVal[0].InsurerName;
                        objOutput.PolicyNumber = main.retVal[0].PolicyNumber;
                        objOutput.TPAID = main.retVal[0].TPAID;
                        objOutput.EMPID = main.retVal[0].EMPID;
                        objOutput.CorporateName = main.retVal[0].CorporateName;
                        objOutput.EmailID = main.retVal[0].EmailID;
                        objOutput.AdmissionDate = main.retVal[0].AdmissionDate;
                        objOutput.AdmissionTime = main.retVal[0].AdmissionTime;
                        objOutput.DischargeDate = main.retVal[0].DischargeDate;
                        objOutput.DischargeTime = main.retVal[0].DischargeTime;
                        objOutput.AdmissionNote = main.retVal[0].AdmissionNote;
                        objOutput.BedNo = main.retVal[0].BedNo;
                        objOutput.RoomType = main.retVal[0].RoomType;
                        objOutput.HistoryOfPresentIllness = main.retVal[0].HistoryOfPresentIllness;
                        objOutput.SurgicalDetail = main.retVal[0].SurgicalDetail;
                        objOutput.MedicalManagementDetail = main.retVal[0].MedicalManagementDetail;
                        objOutput.Investigations = main.retVal[0].Investigations;
                        objOutput.EmergencyOrPlanned = main.retVal[0].EmergencyOrPlanned;
                        objOutput.ExpectedStayinDays = main.retVal[0].ExpectedStayinDays;


                        //--IfAccident-- -
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("IfAccident")))
                        {
                            VNUAPIPatientIfAccidentArr acc = new VNUAPIPatientIfAccidentArr();
                            acc.retVal = dr.AssembleBECollection<VNUAPIPatientIfAccident>();
                            objOutput.IfAccident = new OutVNUAPIPatientIfAccident();
                            if (acc.retVal.Count == 1)
                            {
                                objOutput.IfAccident.InjuryDate = acc.retVal[0].InjuryDate;
                                objOutput.IfAccident.InjuryOrDiseasebySubstanceAbuse = acc.retVal[0].InjuryOrDiseasebySubstanceAbuse;
                                objOutput.IfAccident.MLCNo = acc.retVal[0].MLCNo;
                                objOutput.IfAccident.PoliceStation = acc.retVal[0].PoliceStation;
                                objOutput.IfAccident.ReportedToPolice = acc.retVal[0].ReportedToPolice;
                            }
                        }


                        //-- AdditionalDiagnosis--
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("AdditionalDiagnosis")))
                        {
                            objOutput.AdditionalDiagnosis = new List<string>();
                            while (dr.Read())
                            {
                                objOutput.AdditionalDiagnosis.Add(Convert.ToString(dr[0]));
                            }
                        }

                        //-- OtherInsurance --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("OtherInsurance")))
                        {
                            objOutput.OtherInsurance = new List<string>();
                            while (dr.Read())
                            {
                                objOutput.OtherInsurance.Add(Convert.ToString(dr[0]));
                            }
                        }

                        //-- DiagnosisInfo --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("DiagnosisInfo")))
                        {
                            objOutput.DiagnosisInfo = dr.AssembleBECollection<OutVNUAPIPatientDiagnosisInfo>();
                        }

                        //-- ComplaintsInfo --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("ComplaintsInfo")))
                        {
                            objOutput.ComplaintsInfo = dr.AssembleBECollection<OutVNUAPIPatientComplaintsInfo>();
                        }

                        //-- PastMedicalHistory --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("PastMedicalHistory")))
                        {
                            objOutput.PastMedicalHistory = dr.AssembleBECollection<OutVNUAPIPatientPastMedicalHistory>();
                        }

                        //-- RelevantClinicalfinding --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("RelevantClinicalFinding")))
                        {
                            objOutput.RelevantClinicalfinding = dr.AssembleBECollection<OutVNUAPIPatientRelevantclinicalfinding>();
                        }

                        //-- FamilyPhysician --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("FamilyPhysician")))
                        {
                            objOutput.FamilyPhysician = dr.AssembleBECollection<OutVNUAPIPatientFamilyPhysician>();
                        }

                        //-- ContactAttendingRelative --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("ContactAttendingRelative")))
                        {
                            objOutput.ContactAttendingrelative = dr.AssembleBECollection<OutVNUAPIPatientContactAttendingrelative>();
                        }

                        //-- EstimatInfo --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("EstimatInfo")))
                        {
                            objOutput.EstimatInfo = dr.AssembleBECollection<OutVNUAPIPatientEstimatInfo>();
                        }

                        //-- PaymentDetails --
                        dr.NextResult();
                        if ((objInput.ReqInfo == "") || (arrReqInfo.Contains("PaymentDetails")))
                        {
                            objOutput.PaymentDetails = dr.AssembleBECollection<OutVNUAPIPatientPaymentDetails>();
                        }
                        objOutput.Message = "Success";
                    }
                    else if (main.retVal.Count > 1)
                        objOutput.Message = "More than one patient found";
                    else
                        objOutput.Message = "No patient found";
                }

                return objOutput;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutput.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutput.Message = ex.Message;
                }

                objOutput.Status = "Failure";
                objOutput.Code = 0;
                return objOutput;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutAppointmentUpdateRefundRet AppointmentUpdateRefund(InAppointmentUpdateRefund objInput)
        {
            OutAppointmentUpdateRefundRet objOutputAll = new OutAppointmentUpdateRefundRet();
            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("Pr_OnWeb_UpdateRefundDetail"); //ajay
                db.AddInParameter(cmd, "@PaymentID", SqlDbType.Int, objInput.PaymentID);
                db.AddInParameter(cmd, "@PGRefundedAmount", SqlDbType.Float, objInput.RefundedAmount);
                db.AddInParameter(cmd, "@PGRefundID", DbType.String, objInput.PayURefundID);
                db.AddInParameter(cmd, "@sPGRefundDate", DbType.String, objInput.RefundDate);
                db.AddInParameter(cmd, "@SourceOfData", DbType.String, objInput.BookingSource);
                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    MAppointmentUpdateRefundArr oRV = new MAppointmentUpdateRefundArr();
                    oRV.RetValue = dr.AssembleBECollection<MAppointmentUpdateRefund>();
                    if (oRV.RetValue.Count > 0)
                        objOutputAll.Message = oRV.RetValue[0].retMsg;
                    else
                        objOutputAll.Message = "No data found!";
                }
                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPatientInformationRet PatientInformation(InPatientInformation objInput)
        {
            OutPatientInformationRet objOutputAll = new OutPatientInformationRet();
            DbCommand cmd = null;
            try
            {


                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_PatientInformation"); //ajay
                dbReadOnly.AddInParameter(cmd, "@MaxID", DbType.String, objInput.MaxID);
                dbReadOnly.AddInParameter(cmd, "@BillNo", DbType.String, objInput.BillNo);
                dbReadOnly.AddInParameter(cmd, "@IPID", SqlDbType.Int, objInput.IPID);
                dbReadOnly.AddInParameter(cmd, "@HospitalID", SqlDbType.Int, objInput.HospitalID);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<OutPatientInformation>();
                    if (objOutputAll.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "Patient Detail";
                    }
                    else
                        objOutputAll.Message = "No data found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPatientDemographyByIDArray PatientDemographyByID(InPatientDemographyByIDList objInput)
        {
            OutPatientDemographyByIDArray objOutputAll = new OutPatientDemographyByIDArray();
            DbCommand cmd = null;
            try
            {
                string patIds = objInput.PatID.ToString();
                Int32 i = 0;
                if (objInput.PatList != null)
                {
                    for (i = 0; i < objInput.PatList.Count; i++)
                    {
                        if (patIds != "")
                            patIds = patIds + ",";
                        patIds = patIds + objInput.PatList[i].PatID;
                    }
                }

                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_PatientDemographyByID"); //ajay                
                dbReadOnly.AddInParameter(cmd, "@PatID", DbType.String, patIds);
                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<OutPatientDemographyByID>();
                    if (objOutputAll.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "Patient Detail";
                    }
                    else
                        objOutputAll.Message = "No data found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutMyHealthEPrescriptionRet MyHealthEPrescription(InMyHealthEPrescription objInput)
        {
            OutMyHealthEPrescriptionRet objOutputAll = new OutMyHealthEPrescriptionRet();
            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("PR_TMS_MyHealthEpSave_Max"); //ajay
                db.AddInParameter(cmd, "@Bookingno", DbType.String, objInput.Bookingno);
                db.AddInParameter(cmd, "@Data1", DbType.String, objInput.Data1);
                db.AddInParameter(cmd, "@Data2", DbType.String, objInput.Data2);
                db.AddInParameter(cmd, "@Data3", DbType.String, objInput.Data3);
                db.AddInParameter(cmd, "@Data4", DbType.String, objInput.Data4);
                db.AddInParameter(cmd, "@Data5", DbType.String, objInput.Data5);
                db.AddInParameter(cmd, "@sUpdatedatetime", DbType.String, objInput.Updatedatetime);
                db.AddInParameter(cmd, "@sSavedatetime", DbType.String, objInput.Savedatetime);


                using (IDataReader dr = db.ExecuteReader(cmd))
                {
                    MyHealthEPrescriptionArray oEP = new MyHealthEPrescriptionArray();
                    oEP.RetValue = dr.AssembleBECollection<OutMyHealthEPrescription>();
                    if (oEP.RetValue.Count > 0)
                    {

                        objOutputAll.VisitID = oEP.RetValue[0].VisitID;
                        objOutputAll.Message = oEP.RetValue[0].RetMessage;

                    }
                    else
                        objOutputAll.Message = "Internal error. EPrescription not created!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutOPVisitCaseSummaryRet OPVisitCaseSummary(InOPVisitCaseSummary objInput)
        {
            OutOPVisitCaseSummaryRet objOutputAll = new OutOPVisitCaseSummaryRet();
            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("PR_OnWeb_OPVisit_CaseSummarySave");
                db.AddInParameter(cmd, "@id", SqlDbType.Int, objInput.CaseSummaryId);
                db.AddInParameter(cmd, "@MaxId", DbType.String, objInput.MaxId);
                db.AddInParameter(cmd, "@DoctorId", SqlDbType.Int, objInput.DoctorId);
                db.AddInParameter(cmd, "@OnlinePaymentId", SqlDbType.Int, objInput.OnlinePaymentId);
                db.AddInParameter(cmd, "@BookingNo", DbType.String, objInput.BookingNo);
                db.AddInParameter(cmd, "@BookingSource", DbType.String, objInput.BookingSource);
                db.AddInParameter(cmd, "@VisitId", DbType.Int32, objInput.VisitId);
                db.AddInParameter(cmd, "@CaseSummaryText", DbType.String, objInput.CaseSummaryText);
                db.AddInParameter(cmd, "@SourceOfData", DbType.String, objInput.Source);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    OutOPVisitCaseSummary retDB = new OutOPVisitCaseSummary();
                    retDB.RetValue = dr.AssembleBECollection<OutOPVisitCaseSummaryRet>();
                    if (retDB.RetValue.Count > 0)
                    {
                        objOutputAll.CaseSummaryId = retDB.RetValue[0].CaseSummaryId;
                        objOutputAll.DBMessage = retDB.RetValue[0].DBMessage;
                        objOutputAll.Message = retDB.RetValue[0].DBMessage;
                        objOutputAll.OnlinePaymentId = retDB.RetValue[0].OnlinePaymentId;
                        objOutputAll.VisitId = retDB.RetValue[0].VisitId;

                        objOutputAll.Code = WebAPI.Models.ProcessStatus.Success;
                        objOutputAll.Status = "Success";
                    }
                    else
                    {
                        objOutputAll.Message = "Error.";
                        objOutputAll.Code = WebAPI.Models.ProcessStatus.Fail;
                    }
                    return objOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = WebAPI.Models.ProcessStatus.Fail;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutPatientListDoctorwiseRet PatientListDoctorwise(InPatientListDoctorwise objInput)
        {
            OutPatientListDoctorwiseRet objOutputAll = new OutPatientListDoctorwiseRet();
            DbCommand cmd = null;
            try
            {
                cmd = db.GetStoredProcCommand("Pr_OnWeb_IP_PatientListDoctorwise");
                db.AddInParameter(cmd, "@DoctorId", SqlDbType.Int, objInput.DoctorID);
                db.AddInParameter(cmd, "@HSPLocationId", SqlDbType.Int, objInput.HospitalID);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.Patients = dr.AssembleBECollection<OutPatientListDoctorwise>();
                    if (objOutputAll.Patients.Count > 0)
                    {

                        objOutputAll.Code = WebAPI.Models.ProcessStatus.Success;
                        objOutputAll.Status = "Success";
                    }
                    else
                    {
                        objOutputAll.Message = "No any patient found.";
                        objOutputAll.Code = WebAPI.Models.ProcessStatus.Fail;
                    }
                    return objOutputAll;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = WebAPI.Models.ProcessStatus.Fail;
                return objOutputAll;
                throw ex;

            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        public OutLabOrdersCreateURLRet LabOrdersCreateURL(InLabOrdersCreateURLArr objInput)
        {
            OutLabOrdersCreateURLRet objOutput = new OutLabOrdersCreateURLRet();
            String LimsUrl = ConfigurationManager.AppSettings["LIMSUrl"].ToString();
            objOutput.urls = new List<OutLabOrdersCreateURL>();

            try
            {
                for (int i = 0; i < objInput.Orders.Count; i++)
                {
                    string testIds = "";
                    for (int j = 0; j < objInput.Orders[i].Items.Count; j++)
                    {
                        if (testIds != "")
                            testIds = testIds + ",";
                        testIds = testIds + objInput.Orders[i].Items[j].ItemId;
                    }
                    string strOrder = objInput.Orders[i].PatType + objInput.Orders[i].OrderId.ToString();
                    string encryporderid = Encrypt(strOrder);
                    string encryptestid = Encrypt(testIds);
                    objOutput.urls.Add(new OutLabOrdersCreateURL
                    {
                        urlPath = LimsUrl + "?LabNo=" + encryporderid + "&TestCode=" + encryptestid,
                        OrderId = objInput.Orders[i].OrderId,
                        Code = Models.ProcessStatus.Success,
                        PatType = objInput.Orders[i].PatType,
                        Status = "Success",
                        Message = "Generated URL"
                    });
                }
                objOutput.Code = Models.ProcessStatus.Success;
                objOutput.Status = "Success";
                objOutput.Message = "Success";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                    objOutput.Message = "Database server not found or not accessible.";
                else
                    objOutput.Message = ex.Message;

                objOutput.Status = "Failure";
                objOutput.Code = 0;

                throw ex;

            }
            finally
            {
            }
            return objOutput;
        }
        //Added By Dhirendra K Singh 11-April-2022 
        public OutOPBilltailsRet OPBillDetail(InOPBilltails objInput)
        {
            OutOPBilltailsRet objOutputAll = new OutOPBilltailsRet();
            DbCommand cmd = null;
            try
            {
                cmd = dbReadOnly.GetStoredProcCommand("PR_OnWeb_OPBillDetail");
                dbReadOnly.AddInParameter(cmd, "@BillId", DbType.Int32, objInput.BillId);
                dbReadOnly.AddInParameter(cmd, "@NoOfRecords", DbType.Int32, objInput.NoOfRecords);

                using (IDataReader dr = dbReadOnly.ExecuteReader(cmd))
                {
                    objOutputAll.RetValue = dr.AssembleBECollection<OutOPBilltails>();
                    if (objOutputAll.RetValue.Count > 0)
                    {
                        objOutputAll.Message = "OP Bill Detail";
                    }
                    else
                        objOutputAll.Message = "No data found!";
                }

                return objOutputAll;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server was not found or was not accessible."))
                {
                    objOutputAll.Message = "Database server not found or not accessible.";
                }
                else
                {
                    objOutputAll.Message = ex.Message;
                }

                objOutputAll.Status = "Failure";
                objOutputAll.Code = 0;
                return objOutputAll;
                throw ex;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }

        }

        private string Encrypt(string encryptText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptText.Replace("+", ""));
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptText;
        }

    }
}
