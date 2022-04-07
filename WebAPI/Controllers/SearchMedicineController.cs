using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class SearchMedicineController : ApiController
    {
        [HttpPost]
        public OutSearchMedicineRet PostMethod(InSearchMedicine objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutSearchMedicineRet objOutputAll = new OutSearchMedicineRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.StationId + "^" + objInput.SearchText + "^" + objInput.NoOfRows;
            try
            {
                if (objInput.NoOfRows == 0)
                    objInput.NoOfRows = 30;

                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.SearchText == null) || (objInput.SearchText == ""))
                    strOutput = "Search text cannot be blank!";
                else if ((objInput.StationId == 0))
                    strOutput = "StationId cannot be blank or zero!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.SearchMedicine(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput + "^Count=" + objOutputAll.RetValue.Count;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "SearchMedicine", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "SearchMedicine", "Err", strInput, ex.Message);
            }
            return objOutputAll;
        }
    }
}
