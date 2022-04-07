using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Configuration;

namespace WebAPI.Controllers
{
    public class LogFile
    {
        public static void log(DateTime dtStartTime, String strSource, String strFunction, String LogType, String InputParam, String OutputParam)
        {

            String fpath = ConfigurationManager.AppSettings["FolderOfLogFile"].ToString() + "Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            try
            {
                if (!File.Exists(fpath))
                {
                    FileStream fs = new FileStream(fpath, FileMode.OpenOrCreate);
                    string text = "Source\tDate/Time\tStart Time\tTime Taken\tType\tFunction\tInput\tOutput";
                    byte[] info = new UTF8Encoding(true).GetBytes(text);
                    fs.Write(info, 0, info.Length);

                    byte[] newline = Encoding.ASCII.GetBytes(Environment.NewLine);
                    fs.Write(newline, 0, newline.Length);

                    fs.Close();

                    //delete old file 
                    Int32 DaysToDeleteFile = Convert.ToInt32(ConfigurationManager.AppSettings["DaysToDeleteFile"].ToString());
                    String pathOld = ConfigurationManager.AppSettings["FolderOfLogFile"].ToString() + "Log" + DateTime.Now.AddDays(0 - DaysToDeleteFile).ToString("yyyy-MM-dd") + ".log";
                    if (File.Exists(pathOld))
                        File.Delete(pathOld);
                }
                using (StreamWriter w = File.AppendText(fpath))
                {
                    String strStartDateTime = dtStartTime.ToLongDateString() + " " + dtStartTime.ToLongTimeString();
                    String timeTaken = (DateTime.Now - dtStartTime).TotalSeconds.ToString();

                    w.WriteLine("{0}\t{1} {2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}", strSource,DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), strStartDateTime, timeTaken, LogType, strFunction, InputParam, OutputParam);
                    w.Flush();
                    w.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }


    }

}