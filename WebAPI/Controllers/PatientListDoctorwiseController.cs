using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PatientListDoctorwiseController : ApiController
    {
        [HttpPost]
        public OutPatientListDoctorwiseRet PostMethod(InPatientListDoctorwise objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutPatientListDoctorwiseRet objOutputAll = new OutPatientListDoctorwiseRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.DoctorID+"^"+objInput.HospitalID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot blank!";
                else if ((objInput.DoctorID == 0))
                    strOutput = "DoctorID cannot blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PatientListDoctorwise(objInput);
                    strOutput = objOutputAll.Message.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "DoctorDetailByBed", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "DoctorDetailByBed", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
