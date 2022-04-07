using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetItemListController : ApiController
    {
        [HttpPost]
        public OutItemListRet PostMethod(InItemList objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutItemListRet objOutputAll = new OutItemListRet();
            String strOutput = "";
            String strInput = "";
            strInput = "ServiceId=" + objInput.ServiceId + "^HospitalId=" + objInput.HospitalId + "^SpokeId=" + objInput.SpokeId ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.ServiceId == 0))
                    strOutput = "ServiceId can not be blank!";
                else if ((objInput.HospitalId == 0))
                    strOutput = "HospitalId can not be blank!";
                else if ((objInput.SpokeId == 0))
                    strOutput = "SpokeId can not be blank!";

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.GetItemList(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.ItemList != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.ItemList.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "GetItemList", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "GetItemList", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
