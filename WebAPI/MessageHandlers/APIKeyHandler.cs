using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using WebAPI.Controllers;
using WebAPI.Models;

namespace WebAPI.MessageHandlers
{
    public class APIKeyHandler : DelegatingHandler
    {
      
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            String EnvironmentName = "MaxOnWeb";
            String key = "";
            String method = "";
            try
            {
                String invalidMsg = "";
                String invalidMsgForLog = "";
                IEnumerable<string> lsHeaders;
                DAL dal = new DAL();

                JObject account = JObject.Parse(request.Content.ReadAsStringAsync().Result);

                String SignatureInAPI = "";
                var checkApiSignatureExists = request.Headers.TryGetValues("Signature", out lsHeaders);
                if (checkApiSignatureExists)
                    SignatureInAPI = lsHeaders.FirstOrDefault().ToString();
                if (SignatureInAPI == "")
                {
                    invalidMsg = "Unauthorized Access--100"; // Signature not exist in API header
                    invalidMsgForLog = "Signature not found in header";
                }

                if (invalidMsg == "")
                {
                    var checkApiKeyExists = request.Headers.TryGetValues("API_KEY", out lsHeaders);
                    if (checkApiKeyExists)
                        key = lsHeaders.FirstOrDefault().ToString();
                    if (key == "")
                    {
                        invalidMsg = "Unauthorized Access--110"; // key not exist in API header
                        invalidMsgForLog = "Key not found in header";
                    }
                }
                

                String strDateTimeinAPI = "";
                DateTime DateTimeinAPI = DateTime.Now.Date.AddDays(-1);
                if (invalidMsg == "")
                {
                    var checkApiDateTimeExists = request.Headers.TryGetValues("timestamp", out lsHeaders);
                    if (checkApiDateTimeExists)
                        strDateTimeinAPI = lsHeaders.FirstOrDefault().ToString();
                    if (strDateTimeinAPI == "")
                    {
                        invalidMsg = "Unauthorized Access--120"; // date time not exist in API header
                        invalidMsgForLog = "Date/Time not found in header";
                    }
                }
                if (invalidMsg == "")
                {
                    DateTimeinAPI = GetDateTime(strDateTimeinAPI);
                    if (DateTimeinAPI.ToString() == "01/01/0001 00:00:00")
                    {
                        invalidMsg = "Unauthorized Access--121"; // date time is wrong in API header
                        invalidMsgForLog = "Wrong Date/Time: " + DateTimeinAPI;
                    }
                }
                if (invalidMsg == "")
                {
                    Double timeTaken = (DateTime.Now - DateTimeinAPI).TotalMinutes;
                    if ((timeTaken > 10) || (timeTaken < -10))
                    {
                        invalidMsg = "Unauthorized Access--122 "; // More than 30 minutes
                        invalidMsgForLog = "More than 30 minutes. DateTimeinAPI:" + strDateTimeinAPI + " timeTaken: " + timeTaken.ToString();
                    }
                }

                String localPath = request.RequestUri.LocalPath; // : "/API/PatSearch"
                String[] arrUri = localPath.Split('/');
                
                if (invalidMsg == "")
                {
                    if (arrUri.Length > 0)
                        method = arrUri[arrUri.Length-1];
                    if (method == "")
                    {
                        invalidMsg = "Unauthorized Access--130"; // Method not found in API header
                        invalidMsgForLog = "Method not found in API header";
                    }
                }
                String salt = "";
                if (invalidMsg == "")
                {
                    Int32 retVal=0;
                    salt = dal.GetAPISalt(EnvironmentName, key,method, ref retVal);
                    if (retVal == 1)
                    {
                        invalidMsg = "Unauthorized Access--131 " + EnvironmentName; // Environment not found in Data base. Table: M_APIEnvironment
                        invalidMsgForLog = "Environment not found in Data base. Table: M_APIEnvironment. EnvironmentName: " + EnvironmentName;
                    }
                    else if (retVal == 2)
                    {
                        invalidMsg = "Unauthorized Access--132 " + method; // Method not found in Data base. Table: M_APIMethod
                        invalidMsgForLog = "Method not found in Data base. Table: M_APIMethod. Method: " + method;
                    }
                    else if (retVal == 3)
                    {
                        invalidMsg = "Unauthorized Access--133 " + key; // Key not found in Data base. Table: M_APIClientKeySalt
                        invalidMsgForLog = "Key not found in Data base. Table: M_APIClientKeySalt. key: " + key + " DETAIL: "+account;
                    }
                    else if (retVal == 4)
                    {
                        invalidMsg = "Unauthorized Access--134"; // Method not accessable. Table: l_APIClientKeyMethod
                        invalidMsgForLog = "Method not accessable for this user/key. Table: l_APIClientKeyMethod. key: " + key + ". Method: " + method;
                    }
                    else if (salt == "")
                    {
                        invalidMsg = "Unauthorized Access--135 " + key; // salt not exists in Data base
                        invalidMsgForLog = "salt not exists in Data base. key: " + key;
                    }
                }

                String var1 = "";
                String var2 = "";
                String var3 = "";
                

                if (invalidMsg == "")                
                    getVarValue(account, method, ref var1, ref var2, ref var3, ref invalidMsg, key, ref invalidMsgForLog);

                if (invalidMsg == "")
                {
                    //String strDateTimeForSignature =  strDateTimeinAPI
                    String toHash = key + "|" + method + "|" + var1;
                    if (var2 != "")
                        toHash = toHash + "|" + var2;
                    if (var3 != "")
                        toHash = toHash + "|" + var3;
                    toHash = toHash + "|" + strDateTimeinAPI + "|" + salt;

                    String Signature = Generatehash512(toHash);
                    if (SignatureInAPI == Signature)
                        invalidMsg = "";
                    else
                    {
                        invalidMsg = "Unauthorized Access--999"; // Wrong Signature
                        invalidMsgForLog = "Wrong Signature. key:" + key + ". var1:" + var1 + ". var2:" + var2 + ". var3:" + var3 +". Time: "+ strDateTimeinAPI + ". API Sign:" + SignatureInAPI+". Actual: "+ Signature;
                    }
                }

                Base ba = new Base();
                if (invalidMsg != "")
                {
                    ba.Code = 0;
                    ba.Message = invalidMsg;
                    ba.Status = "Failure";
                    LogFile.log(DateTime.Now, key, method, "Sec", "Security: " + invalidMsgForLog, invalidMsg);
                    return request.CreateResponse(ba);
                }
                
            }
            catch (Exception Ex)
            {
                LogFile.log(DateTime.Now, key, method, "Sec", "End-Error", Ex.Message);
            }
            
            var response = await base.SendAsync(request, cancellationToken);
            LogFile.log(DateTime.Now, key, method, "Sec", "End", "Done");
            return response;
        }

        private string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private DateTime GetDateTime(string text)
        {
           
            DateTime dt = new DateTime(1, 1, 1, 0, 0, 0);
            try
            {
                String[] aSDT = text.Split('/');
                if (aSDT.Length > 4)
                {
                    int[] aDT = new int[aSDT.Length];
                    int i;
                    for (i = 0; i < aSDT.Length; i++)
                        aDT[i] = Convert.ToInt32(aSDT[i]);

                    dt = new DateTime(aDT[2], aDT[1], aDT[0], aDT[3], aDT[4], 0);
                }
            }
            catch
            {

            }
                
            return dt;
        }

        private void getVarValue(JObject account,String method,ref String var1, ref String var2, ref String var3, ref String invalidMsg, String key, ref String invalidMsgForLog)
        {
            var1 = "";
            var2 = "";
            var3 = "";
            String Source = "";
            Source= (String)account["Source"];
            if (Source != key)
            {
                invalidMsg = "Unauthorized Access--192 " + method; // Source != Key
                invalidMsgForLog = "Source != Key";
            }
            else if (method == "GetItemList")
            {
                var1 = (String)account["HospitalId"] + "";
                var2 = (String)account["ServiceId"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "GetDoctorList")
            {
                var1 = (String)account["HospitalID"] + "";
                var2 = (String)account["DocType"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "LabOrders")
            {
                var1 = (String)account["MaxID"] + "";
                var2 = (String)account["FromDate"] + "";
                var3 = (String)account["ToDate"] + "";
                if ((var1 == "") || (var2 == "") || (var3 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "PatSearchByBed")
            {
                var1 = (String)account["BedID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "DoctorCharge")
            {
                var1 = (String)account["HospitalID"] + "";
                var2 = (String)account["DoctorID"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "BedsInHospital")
            {
                var1 = (String)account["HospitalID"] + "";

                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "InsertDoctorVisit")
            {
                var1 = (String)account["BedID"] + "";
                var2 = (String)account["DoctorID"] + "";

                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "EPCountMPHRx")
            {
                var1 = (String)account["SDate"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "InterfaceData90")
            {
                var1 = (String)account["sTransDate"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "SendSMSFromTMSDB90")
            {
                var1 = (String)account["FeedID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "DoctorDetailByBed")
            {
                var1 = (String)account["BedID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "QMSDisplayGetQueue")
            {
                var1 = (String)account["DisplayID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "DietBarCodeKOTTAT")
            {
                var1 = (String)account["BarCode"] + "";
                var2 = (String)account["ScanDateTime"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "UserLoginAuthenticationPharmacy")
            {
                var1 = (String)account["UserName"] + "";

                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "SearchMedicine")
            {
                var1 = (String)account["StationId"] + "";
                var2 = (String)account["SearchText"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "BatchWiseQuantity")
            {
                var1 = (String)account["ItemID"] + "";
                var2 = (String)account["StationId"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "PharmacySaveCartOrder")
            {
                var1 = (String)account["UserID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "GetEPrescriptionList")
            {
                var1 = (String)account["IACode"] + "";
                var2 = (String)account["RegistrationNo"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "InPatientDetailTPA")
            {
                var1 = (String)account["IPID"] + "";
                var2 = (String)account["HospitalId"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "PharmacyCartOrderSearch")
            {
                var1 = (String)account["Source"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "AppointmentCancel")
            {
                var1 = (String)account["PaymentID"] + "";
                var2 = (String)account["BookingSource"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "PharmacyCartOrderSearchDetail")
            {
                var1 = (String)account["CartID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "InPatientClaimBook")
            {
                var1 = (String)account["TreatmentId"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "VNUAPIMaster")
            {
                var1 = (String)account["TreatmentId"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "VNUAPIPatient")
            {
                var1 = (String)account["TreatmentId"] + "";
                var2 = (String)account["HospitalId"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "AppointmentUpdateRefund")
            {
                var1 = (String)account["PaymentID"] + "";
                var2 = (String)account["BookingSource"] + "";
                if ((var1 == "") || (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }

            }
            else if (method == "PatientInformation")
            {
                var1 = (String)account["MaxID"] + "";                
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "PatientDemographyByID")
            {
                var1 = (String)account["PatID"] + "";

                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "MyHealthEPrescription")
            {
                var1 = (String)account["Bookingno"] + "";

                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "OPVisitCaseSummary")
            {
                var1 = (String)account["MaxId"] + "";
                var2 = (String)account["DoctorId"] + "";
                if ((var1 == "")|| (var2 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "PatientListDoctorwise")
            {
                var1 = (String)account["DoctorID"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else if (method == "LabOrdersCreateURL")
            {
                var1 = (String)account["UserId"] + "";
                if ((var1 == ""))
                {
                    invalidMsg = "Unauthorized Access--191 " + method;
                    invalidMsgForLog = "Variable(s) used in signature not exist in JSon/body.";
                }
            }
            else
            {
                invalidMsg = "Unauthorized Access--190 " + method;
                invalidMsgForLog = "Method not configured in APIKeyHandler.cs";
            }

        }
    }
}