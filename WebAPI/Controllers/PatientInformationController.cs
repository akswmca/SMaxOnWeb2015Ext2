using System;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PatientInformationController : ApiController
    {
        [HttpPost]
        public OutPatientInformationRet PostMethod(InPatientInformation objInput)
        {
            DateTime dtStart = DateTime.Now;
            MethodBase myM = MethodBase.GetCurrentMethod();
            
            OutPatientInformationRet objOutputAll = new OutPatientInformationRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.MaxID + "^" + objInput.BillNo + "^" + objInput.IPID + "^" + objInput.HospitalID;
            try
            {
                if (objInput.BillNo == null)
                    objInput.BillNo = "";

                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.MaxID== null) || (objInput.MaxID== ""))
                    strOutput = "MaxID cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PatientInformation(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
