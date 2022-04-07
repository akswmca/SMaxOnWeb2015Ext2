using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InPatientClaimBookController : ApiController
    {
        [HttpPost]
        public OutInPatientClaimBookRet PostMethod(InInPatientClaimBook objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutInPatientClaimBookRet objOutputAll = new OutInPatientClaimBookRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.TreatmentId + "^" + objInput.HospitalId + "^" + objInput.PatientId;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.TreatmentId == 0))
                    strOutput = "TreatmentId cannot be blank!";                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.InPatientClaimBook(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "InPatientDetailTPA", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "InPatientDetailTPA", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
