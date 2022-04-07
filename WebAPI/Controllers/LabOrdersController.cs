using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LabOrdersController : ApiController
    {
        [HttpPost]
        public OutLabOrderRet PostMethod(InLabOrder objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutLabOrderRet objOutputAll = new OutLabOrderRet();
            String strOutput = "";
            String strInput = "";
            strInput = "MaxID=" + objInput.MaxID + "^FromDate=" + objInput.FromDate + "^ToDate=" + objInput.ToDate + "^PatientType=" + objInput.PatientType;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.MaxID == null) || (objInput.MaxID == ""))
                    strOutput = "MaxID can not be blank!";
                else if ((objInput.FromDate == null) || (objInput.FromDate == ""))
                    strOutput = "FromDate can not be blank!";
                else if ((objInput.ToDate == null) || (objInput.ToDate == ""))
                    strOutput = "ToDate can not be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.GetLabOrders(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.TestIDs != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.TestIDs.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "LabOrders", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "LabOrders", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
