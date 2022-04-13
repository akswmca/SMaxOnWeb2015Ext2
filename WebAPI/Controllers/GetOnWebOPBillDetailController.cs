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
    public class GetOnWebOPBillDetailController : ApiController
    {
        [HttpPost]
        public OutOPBilltailsRet PostMethod(InOPBilltails objInput)
        {
            DateTime dtStart = DateTime.Now;
            MethodBase myM = MethodBase.GetCurrentMethod();

            OutOPBilltailsRet objOutputAll = new OutOPBilltailsRet();

            String strOutput = "";
            String strInput = "";
            try
            {
                if (objInput.BillId == 0)
                    strOutput = "Source cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.OPBillDetail(objInput);
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
