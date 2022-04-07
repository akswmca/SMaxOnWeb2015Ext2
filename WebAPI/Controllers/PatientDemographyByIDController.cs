using System;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PatientDemographyByIDController : ApiController
    {
        [HttpPost]
        public OutPatientDemographyByIDArray PostMethod(InPatientDemographyByIDList objInput)
        {
            MethodBase myM = MethodBase.GetCurrentMethod();
            DateTime dtStart = DateTime.Now;
            OutPatientDemographyByIDArray objOutputAll = new OutPatientDemographyByIDArray();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.PatID.ToString() ;
            try
            {                
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.PatID== 0)&&((objInput.PatList==null)&& (objInput.PatList.Count == 0)))
                    strOutput = "PatID cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PatientDemographyByID(objInput);
                    strOutput = objOutputAll.Message.ToString();                    
                }
                else
                    objOutputAll.Message = strOutput;
                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Err", strInput, ex.Message);
                objOutputAll.Message = ex.Message;
            }
            return objOutputAll;
        }
    }
}
