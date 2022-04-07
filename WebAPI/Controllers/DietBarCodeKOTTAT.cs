using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DietBarCodeKOTTATController : ApiController
    {
        [HttpPost]
        public OutDietBarCodeKOTTAT PostMethod(InDietBarCodeKOTTAT objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutDietBarCodeKOTTAT objOutputAll = new OutDietBarCodeKOTTAT();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.BarCode+"^"+ objInput.ScanDateTime;
            try
            {
                //////////////////////////////////////
                ////// objInput.TransType :  ////////
                ////// D= DELIVERY TIME      ////////
                ////// O= Out Time           ////////
                /////////////////////////////////////
                if ((objInput.TransType == null) || (objInput.TransType != "O"))
                    objInput.TransType = "D";

                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.BarCode == null)|| (objInput.BarCode == ""))
                    strOutput = "BarCode cannot be blank!";
                else if ((objInput.ScanDateTime == null) || (objInput.ScanDateTime == ""))
                    strOutput = "ScanDateTime cannot be blank!";

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.DietBarCodeKOTTAT(objInput);
                    strOutput = objOutputAll.Message.ToString();                    
                }
                else
                    objOutputAll.RetMessage = strOutput;

                LogFile.log(dtStart, objInput.Source, "DietBarCodeKOTTAT", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "DietBarCodeKOTTAT", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
