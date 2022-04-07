using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AppointmentUpdateRefundController : ApiController
    {
        [HttpPost]
        public OutAppointmentUpdateRefundRet PostMethod(InAppointmentUpdateRefund objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutAppointmentUpdateRefundRet objOutputAll = new OutAppointmentUpdateRefundRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.PaymentID + "^" + objInput.BookingSource;
            try
            {
                if (objInput.RefundDate == null)
                    objInput.RefundDate= "";
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.PaymentID == 0))
                    strOutput = "PaymentID cannot be blank!";
                else if ((objInput.BookingSource == null) || (objInput.BookingSource == ""))
                    strOutput = "BookingSource cannot be blank!";                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.AppointmentUpdateRefund(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "AppointmentUpdateRefund", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "AppointmentUpdateRefund", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
