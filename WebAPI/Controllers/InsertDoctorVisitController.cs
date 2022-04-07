using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class InsertDoctorVisitController : ApiController
    {
        [HttpPost]
        public OutInsertDoctorVisitRet PostMethod(InInsertDoctorVisit objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutInsertDoctorVisitRet objOutputAll = new OutInsertDoctorVisitRet();
            String strOutput = "";
            String strInput = "";
            strInput = "BedID=" + objInput.BedID+ "^DoctorID=" + objInput.DoctorID+ "^DateTime=" + objInput.DateTime;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.BedID == 0))
                    strOutput = "BedID cannot be blank!";
                else if ((objInput.DoctorID == 0))
                    strOutput = "DoctorID cannot be blank!";
                else if ((objInput.DateTime == null)|| (objInput.DateTime == ""))
                    strOutput = "DateTime cannot be blank!";
                

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.PatientByBedTimeStamp(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if(objOutputAll.MaxID !=null)
                        strOutput = strOutput + "^MaxID:" + objOutputAll.MaxID;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "InsertDoctorVisit", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "InsertDoctorVisit", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
