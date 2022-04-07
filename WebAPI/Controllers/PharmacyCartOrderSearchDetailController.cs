using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PharmacyCartOrderSearchDetailController : ApiController
    {
        [HttpPost]
        public OutPharmacyCartOrderSearchDetailRet PostMethod(InPharmacyCartOrderSearchDetail objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutPharmacyCartOrderSearchDetailRet objOutputAll = new OutPharmacyCartOrderSearchDetailRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.CartID.ToString() ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot blank!";
                else if (objInput.CartID == 0) 
                    strOutput = "CartID cannot blank!";                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PharmacyCartOrderSearchDetail(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.RetValue.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "PharmacyCartOrderDetailSearch", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "PharmacyCartOrderDetailSearch", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
