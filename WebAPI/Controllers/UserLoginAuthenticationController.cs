using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class UserLoginAuthenticationPharmacyController : ApiController
    {
        [HttpPost]
        public OutLoginRet PostMethod(InLogin objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutLoginRet objOutputAll = new OutLoginRet();
            String strOutput = "";
            String strInput = "";
            strInput = objInput.UserName;
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.UserName == null) || (objInput.UserName == ""))
                    strOutput = "User Name cannot be blank!";
                else if ((objInput.OldPwd == null) || (objInput.OldPwd == ""))
                    strOutput = "Password cannot be blank!";
                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.Login(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    if (objOutputAll.RetValue != null)
                        strOutput = strOutput + "^Count=" + objOutputAll.RetValue.Count;
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "UserLoginAuthenticationPharmacy", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "UserLoginAuthenticationPharmacy", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
