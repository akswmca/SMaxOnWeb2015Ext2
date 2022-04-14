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
    public class GetOnWebEPDataSamparkController : ApiController
    {
        [HttpPost]
        public OutEPDataSamparkRet PostMethod(InEPDataSampark objInput)
        {
            DateTime dtStart = DateTime.Now;
            MethodBase myM = MethodBase.GetCurrentMethod();

            OutEPDataSamparkRet objOutputAll = new OutEPDataSamparkRet();

            String strOutput = "";
            String strInput = "";
            try
            {
                if (objInput.MaxId == "")
                    strOutput = "Source cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.EPDataSampark(objInput);
                    strOutput = objOutputAll.Message.ToString();

                }
                else
                    objOutputAll.Message = strOutput;
                LogFile.log(dtStart, "", myM.ReflectedType.FullName, "Log", strInput, strOutput);

            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, "", myM.ReflectedType.FullName, "Err", strInput, ex.Message);
            }
            return objOutputAll;
        }
    }
}
