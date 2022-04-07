using System;
using System.Web.Http;
using WebAPI.Models;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    public class PharmacySaveCartOrderController : ApiController
    {
        [HttpPost]
        public OutPharmacySaveCartOrderRet PostMethod(InPharmacySaveCartOrder objInput)
        {
            String[] PaymentMode = { "CS","CC","MO","OP", "CH" };

            DateTime dtStart = DateTime.Now;
            OutPharmacySaveCartOrderRet objOutputAll = new OutPharmacySaveCartOrderRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.Flag + "^"+objInput.IACode + "^" + objInput.Registrationno + "^" + objInput.PatFName + "^" + objInput.PatLName + "^" + objInput.UserID;
            try
            {
                if ((objInput.PaymentMode == null)||(objInput.PaymentMode == ""))
                    objInput.PaymentMode = "CS";
                if (Array.IndexOf(PaymentMode, objInput.PaymentMode) < 0)
                    objInput.PaymentMode = "CS";

                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot blank!";
                else if ((objInput.UserID == 0))
                    strOutput = "UserID cannot blank!";
                else if ((objInput.StationID == 0))
                    strOutput = "StationID cannot blank!";                
                else if ((objInput.Flag == null) || ((objInput.Flag != "I") && (objInput.Flag != "U") && (objInput.Flag != "D")))
                    strOutput = "Flag should be either I or U or D!";
                else if ((objInput.Flag == "U") || (objInput.Flag == "I") )                    
                {
                    if ((objInput.OrderByDocId == 0))
                        strOutput = "Order By Doctor Id cannot blank!";
                    else if ((objInput.Items == null) || (objInput.Items.Count == 0))
                        strOutput = "Item detail cannot blank!";
                    else if ((objInput.IACode == null) || (objInput.IACode == "") || (objInput.Registrationno == 0))
                    {
                        if ((objInput.PatFName == null) || (objInput.PatFName == ""))
                            strOutput = "Both MaxID and Patient's first name cannot blank!";
                        //else if ((objInput.PatLName == null) || (objInput.PatLName == ""))
                        //    strOutput = "Both MaxID and Patient's last name cannot be blank!";
                        else if ((objInput.PatMobNo == null) || (objInput.PatMobNo == ""))
                            strOutput = "Both MaxID and Patient's mobile no. cannot blank!";
                        //else if ((objInput.PatDOB == null) || (objInput.PatDOB == ""))
                        //    strOutput = "Both MaxID and Patient's date of birth cannot be blank!";
                        else if ((objInput.PatAddress == null) || (objInput.PatAddress == ""))
                            strOutput = "Both MaxID and Patient's address cannot blank!";
                    }
                }

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();

                    //InPharmacySaveCartOrderItems Items = JsonConvert.DeserializeObject<InPharmacySaveCartOrderItems>(objInput.Items);


                    objOutputAll = objDAL.PharmacySaveCartOrder(objInput);
                    strOutput = objOutputAll.Message + "^" + objOutputAll.DBMessage + "^" + objOutputAll.CartOrderID;
                }
                else
                {
                    objOutputAll.Message = strOutput;
                    objOutputAll.Code = ProcessStatus.Fail;
                    objOutputAll.Status = "Fail";
                }
                LogFile.log(dtStart, objInput.Source, "PharmacySaveCartOrder", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "PharmacySaveCartOrder", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
