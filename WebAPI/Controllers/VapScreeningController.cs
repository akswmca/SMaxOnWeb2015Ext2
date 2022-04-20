using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using WebAPI.Models;


namespace WebAPI.Controllers
{
    public class VapScreeningController : ApiController
    {
        [HttpPost]
        public OutVapScreeningRet PostMethod(InVapScreening objInput)
        {
            DateTime dtStart = DateTime.Now;

            OutVapScreeningRet objOutputAll = new OutVapScreeningRet();

            String strOutput = "";
            String strInput = "";
            try
            {
                if (objInput.FromDate == "")
                    strOutput = "From date cannot be blank!";
                else if (objInput.ToDate == "")
                    strOutput = "To date cannot be blank!";
                else if (objInput.Source == "")
                    strOutput = "Source cannot be blank!";

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.VapScreening(objInput);
                    strOutput = objOutputAll.Message.ToString();

                }
                else
                    objOutputAll.Message = strOutput;
                LogFile.log(dtStart, "", this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Log", strInput, strOutput);

            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, "", this.GetType().FullName + "." + MethodBase.GetCurrentMethod().Name, "Err", strInput, ex.Message);
            }
            return objOutputAll;
        }
    }
}
