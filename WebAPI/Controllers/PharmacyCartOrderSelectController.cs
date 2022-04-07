using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PharmacyCartOrderSelectController : ApiController
    {
        [HttpPost]
        public OutPharmacyCartOrderSelectRet PostMethod(InPharmacyCartOrderSelect objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutPharmacyCartOrderSelectRet objOutputAll = new OutPharmacyCartOrderSelectRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.StationId + "^" + objInput.CartOrderId + "^" + objInput.IACode + "^" + objInput.Registrationno + "^" + objInput.PatFName + "^" + objInput.PatMobNo;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.StationId == 0))
                    strOutput = "StationId cannot be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PharmacyCartOrderSelect(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.RetValue.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "PharmacyCartOrderSelect", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "PharmacyCartOrderSelect", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
