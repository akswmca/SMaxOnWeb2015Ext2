using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class BatchWiseQuantityController : ApiController
    {
        [HttpPost]
        public OutBatchWiseQuantityRet PostMethod(InBatchWiseQuantity objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutBatchWiseQuantityRet objOutputAll = new OutBatchWiseQuantityRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.StationId + "^" + objInput.ItemID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.StationId == 0))
                    strOutput = "StationId cannot be blank!";
                else if ((objInput.ItemID == 0))
                    strOutput = "ItemID cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.BatchWiseQuantity(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.RetValue.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "BatchWiseQuantity", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "BatchWiseQuantity", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
