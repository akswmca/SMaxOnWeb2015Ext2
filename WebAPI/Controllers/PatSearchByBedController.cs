using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PatSearchByBedController : ApiController
    {
        [HttpPost]
        public OutPatSearchByBedRet PostMethod(InPatSearchByBed objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutPatSearchByBedRet objOutputAll = new OutPatSearchByBedRet();
            String strOutput = "";
            String strInput = "";
            strInput = "BedID=" + objInput.BedID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.BedID == 0))
                    strOutput = "BedID can not be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PatSearchByBed(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.MaxID != null)
                        strOutput = strOutput + "^MaxID=" + objOutputAll.MaxID;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "PatSearchByBed", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "PatSearchByBed", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
