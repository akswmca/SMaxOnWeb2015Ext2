using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class GetDoctorListController : ApiController
    {
        [HttpPost]
        public OutDoctorsListRet PostMethod(InDoctorsList objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutDoctorsListRet objOutputAll = new OutDoctorsListRet();
            String strOutput = "";
            String strInput = "";
            strInput = "HospitalID=" + objInput.HospitalID + "^DocType=" + objInput.DocType ;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source can not be blank!";
                else if ((objInput.HospitalID == 0))
                    strOutput = "HospitalID can not be blank!";
                else if ((objInput.DocType == null)|| (objInput.DocType == ""))
                    strOutput = "DocType can not be blank!";
                
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.GetDoctorList(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.DoctorList != null)
                        strOutput = strOutput +"^NoOfRecord="+ objOutputAll.DoctorList.Count.ToString();
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "GetDoctorList", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "GetDoctorList", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
