using System;
using System.Configuration;
using System.Linq;
using System.Web;
using static ApplicationCode.MailHandler;
using System.IO;

namespace ApplicationCode
{
    public static class ErrorHandler
    {
        public static void HandleError(string message, Boolean sendEmail)
        {
            string errorMessage = String.Format("{0} - ERROR: {1}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                message);

            StreamWriter sw = File.AppendText(HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["errorLogPath"]));
            sw.WriteLine(errorMessage);
            sw.Close();

            if (sendEmail)
            {
                SendMail(
                    ConfigurationManager.AppSettings["dataManagementEmail"],
                    ConfigurationManager.AppSettings["dataManagementEmail"],
                    "ERROR - Student Tutor Group Participation system",
                    errorMessage);
            }
        }
    }
}