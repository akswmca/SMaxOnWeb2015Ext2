using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PharmacyCartOrderSearchController : ApiController
    {
        [HttpPost]
        public OutPharmacyCartOrderSearchRet PostMethod(InPharmacyCartOrderSearch objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutPharmacyCartOrderSearchRet objOutputAll = new OutPharmacyCartOrderSearchRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.IACode + "^" + objInput.Registrationno + "^" + objInput.PatMobNo + "^" + objInput.PatFName;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if (((objInput.IACode == null) || (objInput.IACode == "")) && ((objInput.Registrationno== 0))
                    && ((objInput.PatMobNo == null) || (objInput.PatMobNo == "")) && ((objInput.PatFName == null) || (objInput.PatFName == "")))
                    strOutput = "All search fields are blank!";                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PharmacyCartOrderSearch(objInput);
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
