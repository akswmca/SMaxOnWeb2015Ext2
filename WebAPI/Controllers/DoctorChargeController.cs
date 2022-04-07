using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DoctorChargeController : ApiController
    {
        [HttpPost]
        public DoctorChargeRet PostMethod(DoctorChargeIn objInput)
        {
            DateTime dtStart = DateTime.Now;
            DoctorChargeRet objOutputAll = new DoctorChargeRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.HospitalID + "^" + objInput.DoctorID + "^" + objInput.IAcode + "^" + objInput.RegistrationNo + "^" + objInput.SourceAmount;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.HospitalID == 0))
                    strOutput = "HospitalID cannot be blank!";
                else if ((objInput.DoctorID == 0))
                    strOutput = "DoctorID cannot be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.DoctorCharge(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if ((objOutputAll.RetValue != null)&& (objOutputAll.RetValue.Count>0))
                        strOutput = strOutput + "^" + objOutputAll.RetValue[0].CompanyID+ "^" + objOutputAll.RetValue[0].DiscountID 
                            + "^" + objOutputAll.RetValue[0].DiscPer + "^" + objOutputAll.RetValue[0].DoctorCharge 
                            + "^" + objOutputAll.RetValue[0].BasePrice + "^" + objOutputAll.RetValue[0].followup;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "DoctorCharge", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "DoctorCharge", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
