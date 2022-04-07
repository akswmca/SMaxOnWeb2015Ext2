using System;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class SendSMSFromTMSDB90Controller : ApiController
    {
        [HttpPost]
        public OutSendSMSFromTMSDB90Ret PostMethod(InSendSMSFromTMSDB90 objInput)
        {
            DateTime dtStart = DateTime.Now;
            OutSendSMSFromTMSDB90Ret objOutputAll = new OutSendSMSFromTMSDB90Ret();
            String strOutput = "";
            String strInput = "";
            
            if ((objInput.JobName == null) || (objInput.JobName == ""))
                objInput.JobName = "HIS";
            if ((objInput.Password == null) )
                objInput.Password = "";
            if ((objInput.UserName == null) )
                objInput.UserName = "";


            strInput = "SMS from API";
            
            try
            {
                if ((objInput.Source == null) || (objInput.Source == ""))
                    strOutput = "Source cannot be blank!";
                else if ((objInput.FeedID == null) || (objInput.FeedID == ""))
                    strOutput = "FeedID cannot be blank!";
                else
                {
                    for (Int32 i = 0; i < objInput.Detail.Count; i++)
                    {
                        if ((objInput.Detail[i].RefId == null) || (objInput.Detail[i].RefId == ""))
                            strOutput = "RefId is blank for the sequence:" + i;
                        else if ((objInput.Detail[i].MobileNo == null) || (objInput.Detail[i].MobileNo == ""))
                            strOutput = "Mobile no. is blank for RefID=" + objInput.Detail[i].RefId;
                        else if ((objInput.Detail[i].Message == null) || (objInput.Detail[i].Message == ""))
                            strOutput = "Message is blank for RefID=" + objInput.Detail[i].RefId;
                        if (strOutput != "")
                            break;
                    }
                }

                if (strOutput == "")
                {
                    DAL objDAL = new DAL();
                    objOutputAll = objDAL.SendSMSFromTMSDB90(objInput);
                    strOutput = objOutputAll.Message.ToString();
                    
                }
                else
                    objOutputAll.Message = strOutput;

                LogFile.log(dtStart, objInput.Source, "SendSMSFromTMSDB90", "Log", strInput, strOutput);
            }
            catch (Exception ex)
            {
                LogFile.log(dtStart, objInput.Source, "SendSMSFromTMSDB90", "Err", strInput, ex.Message);
            }


            return objOutputAll;
        }
    }
}
