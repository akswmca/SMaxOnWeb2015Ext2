using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class QMSDisplayGetQueueController : ApiController
    {
        [HttpPost]
        public OutQMSDisplayGetQueueRet PostMethod(InQMSDisplayGetQueue objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutQMSDisplayGetQueueRet objOutputAll = new OutQMSDisplayGetQueueRet();
            String strOutput = "";
            String strInput = "";
            strInput = "DisplayID=" + objInput.DisplayID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.DisplayID == 0))
                    strOutput = "DisplayID can not be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.QMSDisplayGetQueue(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetVal != null)
                        strOutput = strOutput + "^Count=" + objOutputAll.RetVal.Count;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "QMSDisplayGetQueue", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "QMSDisplayGetQueue", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
