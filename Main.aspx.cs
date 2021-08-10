using System;
using System.Data.SqlClient;
using System.Configuration;
using ApplicationCode;
using System.Diagnostics;
using System.Linq;

namespace Main
{
    public partial class Main : System.Web.UI.Page
    {
        private string currentUserNetworkLogin;
        private User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            // If a debug user has been specified in the config file, we use that identity. 
            currentUserNetworkLogin = 
                (ConfigurationManager.AppSettings["debugUser"] is null 
                    || ConfigurationManager.AppSettings["debugUser"] == "")
                ? User.Identity.Name.Substring(ConfigurationManager.AppSettings["usernamePrefix"].Length)
                : currentUserNetworkLogin = ConfigurationManager.AppSettings["debugUser"];

            currentUser = new User(currentUserNetworkLogin);
            Session["CurrentUser"] = currentUser;

            // Redirect to appropriate page based on user status. 
            string userStatus = getUserStatus(currentUser.id);
            Session["UserStatus"] = userStatus;

            if (Array.IndexOf(
                new string[] { "Head of School", "Year Level Manager", "Tutor Group Teacher", "Admin" },
                userStatus) >= 0)
            {
                Response.Redirect("./Staff.aspx");
            }
            else if (userStatus == "Staff")
            {
                // ? Do what for staff who are not tutor group teachers or above?
            }
            else if (userStatus == "Student")
            {
                // Student page expects StudentId as a session variable, rather than using ID of the current user,
                // because staff need to be able to edit student pages as if they were students. 
                Session["StudentId"] = currentUser.id.ToString();

                if (currentUser.CanEditAttributes(currentUser.id))
                {
                    Response.Redirect("./EditStudent.aspx");
                }
                else
                {
                    Response.Redirect("./StudentSummary.aspx");
                }
            }
            else
            {
                string errorMessage = "Unrecognised status [" + userStatus + "] returned for user " + currentUserNetworkLogin.ToUpper();
                ErrorHandler.HandleError("TutorGroupParticipation - " + errorMessage, true);
                throw new Exception(errorMessage);
            }
        }


        private string getUserStatus(int userId)
        {
            using (SqlConnection synergyConn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            using (SqlCommand getUserStatusCmd = new SqlCommand(
                    ConfigurationManager.AppSettings["GetUserStatusProc"], synergyConn))
            {
                try
                {
                    getUserStatusCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    getUserStatusCmd.Parameters.AddWithValue("UserId", userId);

                    synergyConn.Open();

                    using (SqlDataReader userStatusReader = getUserStatusCmd.ExecuteReader())
                    {
                        if (userStatusReader.HasRows)
                        {
                            userStatusReader.Read();
                            return (string)userStatusReader["ReturnValue"];
                        }
                        else
                        {
                            string errorMessage = String.Format(
                                "No data returned from user status stored procedure for current user with ID {0}.",
                                User.ToString());
                            throw new Exception(errorMessage);
                        }
                    } 
                } 
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Classes List Populator - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyConn.Close();
                }
            } 
        }
    }
}