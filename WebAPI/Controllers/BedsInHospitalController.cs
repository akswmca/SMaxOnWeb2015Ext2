using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class BedsInHospitalController : ApiController
    {
        [HttpPost]
        public BedsInHospitalRet PostMethod(BedsInHospitalIn objInput)
        {
            DateTime dtStart = DateTime.Now;
            BedsInHospitalRet objOutputAll = new BedsInHospitalRet();
            String strOutput = "";
            String strInput = "";
            strInput = "HospitalID=" + objInput.HospitalID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.HospitalID == 0))
                    strOutput = "HospitalID can not be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.BedOfUnit(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput + "^Count=" + objOutputAll.RetValue.Count;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "BedsInHospital", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "BedsInHospital", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
