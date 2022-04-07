using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class DoctorDetailByBedController : ApiController
    {
        [HttpPost]
        public OutDoctorDetailByBedRet PostMethod(InDoctorDetailByBed objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutDoctorDetailByBedRet objOutputAll = new OutDoctorDetailByBedRet();
            String strOutput = "";
            String strInput = "";
            strInput = "BedID=" + objInput.BedID ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.BedID == 0))
                    strOutput = "BedID cannot be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.DoctorDetailByBed(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.MaxID != null)
                        strOutput = strOutput + "^MaxID=" + objOutputAll.MaxID;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "DoctorDetailByBed", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "DoctorDetailByBed", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
