using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class OPVisitCaseSummaryController : ApiController
    {
        [HttpPost]
        public OutOPVisitCaseSummaryRet PostMethod(InOPVisitCaseSummary objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutOPVisitCaseSummaryRet objOutputAll = new OutOPVisitCaseSummaryRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.MaxId+"^"+ objInput.DoctorId;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot blank!";
                else if ((objInput.MaxId == null) || (objInput.MaxId == ""))
                    strOutput = "MaxId cannot blank!";
                else if ((objInput.DoctorId == 0))
                    strOutput = "DoctorId cannot blank!";
                else if ((objInput.CaseSummaryText == null) || (objInput.CaseSummaryText == ""))
                    strOutput = "CaseSummaryText cannot blank!";

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.OPVisitCaseSummary(objInput);
                    strOutput = objOutputAll.Message.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "OPVisitCaseSummary", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "OPVisitCaseSummary", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
