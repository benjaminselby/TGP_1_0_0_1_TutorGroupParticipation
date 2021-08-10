using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ApplicationCode;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;


namespace TutorGroupParticipation
{
    public partial class RequestNewActivity : System.Web.UI.Page
    {
        private User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = (User)Session["CurrentUser"];

            if (IsPostBack == false)
            {
                getYearLevelManagerInfo();

                recipientNameLbl.InnerText = String.Format("{0} [{1}]", 
                    Session["yearLevelManagerName"].ToString(), 
                    Session["yearLevelManagerEmail"].ToString());
                senderNameLbl.InnerText = String.Format("{0} [{1}]", currentUser.fullName, currentUser.emailAddress);
            }
        }

        private void getYearLevelManagerInfo()
        {
            using (SqlConnection synergyOneConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            using (SqlCommand yearLevelManagerInfoCmd = new SqlCommand(
                ConfigurationManager.AppSettings["getYearLevelManagerProc"], synergyOneConnection))
            {
                try
                {
                    yearLevelManagerInfoCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    yearLevelManagerInfoCmd.Parameters.AddWithValue("StudentId", currentUser.id);

                    synergyOneConnection.Open();

                    SqlDataReader userInfoReader = yearLevelManagerInfoCmd.ExecuteReader();
                    if (userInfoReader.HasRows)
                    {
                        userInfoReader.Read();

                        Session["yearLevelManagerName"] = userInfoReader["FullName"].ToString();
                        Session["yearLevelManagerEmail"] = userInfoReader["EmailAddress"].ToString();
                    }
                    else
                    {
                        throw new Exception("Could not get Year Level Manager for current user " + currentUser.ToString() + ".");
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("RequestNewActivity - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyOneConnection.Close();
                }
            } 
        }

        protected void sendBtn_Click(object sender, EventArgs e)
        {
            // Swap out carriage return & newline characters in the student message for appropriate html tags. 

            Regex newlineRegex = new Regex("\r\n");
            string studentMessageClean = newlineRegex.Replace(MessageTbx.Text, "<br />");

            string messageText = String.Format(
                "<body>" +
                "<p>Dear staff member,</p>" +
                "<p>The following student has requested new activities to be added to the Tutor Group Participation list for Year {0}." +
                "If you approve of the request, please navigate to the " +
                "<a href='" + ConfigurationManager.AppSettings["systemRootUrl"] + "'>Tutor Group Participation System</a> " +
                "and add the new activities.</p>" +
                "<p>====================================================================================================</p>" +
                "<p><b>Message sender:</b> {1} [{2}]</p>" +
                "<p>====================================================================================================</p>" +
                "<p><b>Message text:</b></p>" +
                "<p>{3}</p>" +
                "<p>====================================================================================================</p>" +
                "</body>",
                Session["studentYearLevel"].ToString(),
                currentUser.fullName,
                currentUser.emailAddress,
                studentMessageClean);

            MailHandler.SendMail(
                currentUser.emailAddress,
                Session["yearLevelManagerEmail"].ToString(),
                "Tutor Group Participation System - New Activity Request from Student",
                messageText);

            Response.Redirect("./Main.aspx");
        }

        protected void cancelBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Main.aspx");
        }
    }
}