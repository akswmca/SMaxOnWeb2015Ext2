using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class EPCountMPHRxController : ApiController
    {
        [HttpPost]
        public OutEPCountMPHRxRet PostMethod(InEPCountMPHRx objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutEPCountMPHRxRet objOutputAll = new OutEPCountMPHRxRet();
            String strOutput = "";
            String strInput = "";
            strInput = "SDate=" + objInput.SDate ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.SDate == null) || (objInput.SDate == ""))
                    strOutput = "Date cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.EPCountMPHRx(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput + "^Count=" + objOutputAll.RetValue.Count;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "EPCountMPHRx", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "EPCountMPHRx", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
