using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetEPrescriptionListController : ApiController
    {
        [HttpPost]
        public OutGetEPrescriptionListRet PostMethod(InGetEPrescriptionList objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutGetEPrescriptionListRet objOutputAll = new OutGetEPrescriptionListRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.IACode + "^" + objInput.RegistrationNo + "^" + objInput.VisitDateFrom + "^" + objInput.VisitDateTo + "^" + objInput.HospitalID;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.IACode == null) || (objInput.IACode == ""))
                    strOutput = "IACode cannot be blank!";
                else if ((objInput.RegistrationNo == 0))
                    strOutput = "RegistrationNo cannot be blank!";
                else if ((objInput.VisitDateFrom == null) || (objInput.VisitDateFrom == ""))
                    strOutput = "Date From cannot be blank!";
                else if ((objInput.VisitDateTo == null) || (objInput.VisitDateTo == ""))
                    strOutput = "Date To cannot be blank!";
                
                             

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.GetEPrescriptionList(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.RetValue.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "GetEPrescriptionList", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "GetEPrescriptionList", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
