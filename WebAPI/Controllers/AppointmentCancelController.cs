using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AppointmentCancelController : ApiController
    {
        [HttpPost]
        public OutAppointmentCancelRet PostMethod(InAppointmentCancel objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutAppointmentCancelRet objOutputAll = new OutAppointmentCancelRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.PaymentID + "^" + objInput.BookingSource;
            try
            {
                if (objInput.CancellationDate == null)
                    objInput.CancellationDate = "";
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.PaymentID == 0))
                    strOutput = "PaymentID cannot be blank!";
                else if ((objInput.BookingSource == null) || (objInput.BookingSource == ""))
                    strOutput = "BookingSource cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.AppointmentCancel(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "AppointmentCancel", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "AppointmentCancel", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
