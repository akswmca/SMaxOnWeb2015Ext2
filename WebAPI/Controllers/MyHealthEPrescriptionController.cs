using System;
using System.Web.Http;
using WebAPI.Models;
using System.Reflection;

namespace WebAPI.Controllers
{
    public class MyHealthEPrescriptionController : ApiController
    {
        [HttpPost]
        public OutMyHealthEPrescriptionRet PostMethod(InMyHealthEPrescription objInput)
        {
            MethodBase myM = MethodBase.GetCurrentMethod();
            DateTime dtStart = DateTime.Now;
            OutMyHealthEPrescriptionRet objOutputAll = new OutMyHealthEPrescriptionRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.Bookingno ;
            try
            {
                
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.Bookingno == null) || (objInput.Bookingno == ""))
                    strOutput = "Bookingno cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.MyHealthEPrescription(objInput);
                    strOutput = objOutputAll.Message.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, myM.ReflectedType.FullName, "Err", strInput, ex.Message);
            }
            return objOutputAll;
        }
    }
}
