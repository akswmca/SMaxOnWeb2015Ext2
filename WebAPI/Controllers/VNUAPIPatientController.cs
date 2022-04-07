using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class VNUAPIPatientController : ApiController
    {
        [HttpPost]
        public OutVNUAPIPatientRet PostMethod(InVNUAPIPatient objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutVNUAPIPatientRet objOutputAll = new OutVNUAPIPatientRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.TreatmentId + "^" + objInput.HospitalId ;
            try
            {
                if (objInput.ReqInfo == null)
                    objInput.ReqInfo = "";
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.TreatmentId == 0))
                    strOutput = "TreatmentId cannot be blank!";
                else if ((objInput.HospitalId == 0))
                    strOutput = "HospitalId cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.VNUAPIPatient(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "VNUAPIPatient", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "VNUAPIPatient", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }

        [HttpGet]
        public string GetMethod()
        {
            return "Working fine";
        }
    }
}
