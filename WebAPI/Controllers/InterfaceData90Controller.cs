using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InterfaceData90Controller : ApiController
    {
        [HttpPost]
        public OutInterfaceData90Ret PostMethod(InInterfaceData90 objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutInterfaceData90Ret objOutputAll = new OutInterfaceData90Ret();
            String strOutput = "";
            String strInput = "";
            if ((objInput.AppName == null) || (objInput.AppName == ""))
                objInput.AppName = objInput.Source;
            strInput = "sTransDate=" + objInput.sTransDate+ "^AppName=" + objInput.AppName;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.sTransDate == null) || (objInput.sTransDate == ""))
                    strOutput = "TransDate cannot be blank!";

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.InterfaceData90(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "InterfaceData90", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "InterfaceData90", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
