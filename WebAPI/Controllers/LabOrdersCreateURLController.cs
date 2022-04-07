using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class LabOrdersCreateURLController : ApiController
    {
        [HttpPost]
        public OutLabOrdersCreateURLRet PostMethod(InLabOrdersCreateURLArr objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutLabOrdersCreateURLRet objOutputAll = new OutLabOrdersCreateURLRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.UserId;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot blank!";
                else if ((objInput.Orders == null)|| (objInput.Orders.Count== 0))
                    strOutput = "Orders cannot blank!";
                else if ((objInput.UserId == null) || (objInput.UserId == "0"))
                    strOutput = "UserId cannot blank!";

                if (strOutput == "")
                {
                    strInput = strInput + "^";
                    for (int i = 0; i < objInput.Orders.Count; i++)
                    {
                        strInput = strInput + objInput.Orders[i].OrderId + ":";
                        if ((objInput.Orders[i].Items == null) || (objInput.Orders[i].Items.Count == 0))
                        {
                            strOutput = "Item in order "+ objInput.Orders[i].OrderId + " is blank!";
                            continue;
                        }
                        for (int j=0;j<objInput.Orders[i].Items.Count;j++)
                            strInput = strInput + objInput.Orders[i].Items[j].ItemId + ",";
                    }
                }

                if (strOutput == "")
                {   
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.LabOrdersCreateURL(objInput);
                    for (int i = 0; i < objOutputAll.urls.Count; i++)
                    {
                        strOutput = objOutputAll.urls[i].urlPath;
                    }
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "OutLabOrdersCreateURL", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "OutLabOrdersCreateURL", "Err", strInput, ex.ToString());
                objOutputAll.Message = ex.Message;
            }


            return objOutputAll;
        }
    }
}
