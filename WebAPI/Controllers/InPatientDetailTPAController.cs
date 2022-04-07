using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InPatientDetailTPAController : ApiController
    {
        [HttpPost]
        public OutInPatientDetailTPARet PostMethod(InInPatientDetailTPA objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutInPatientDetailTPARet objOutputAll = new OutInPatientDetailTPARet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.IPID + "^" + objInput.HospitalId;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.IPID == 0))
                    strOutput = "IPID cannot be blank!";
                else if ((objInput.HospitalId == 0))
                    strOutput = "HospitalId cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.InPatientDetailTPA(objInput);
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
