using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using ApplicationCode;


namespace Staff
{
    public partial class StaffForm : System.Web.UI.Page
    {
        private User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = (User)Session["CurrentUser"];
            currentUserNameLbl.Text = String.Format(
                "{0} [{1}:{2}]",
                currentUser.fullName,
                currentUser.id,
                currentUser.networkLogin.ToUpper());

            if (Page.IsPostBack == false)
            {
                populateStaffClassesDdl();
                populateStudentsList();
            }

            if (Array.IndexOf(
                new string[] { "Head of School", "Year Level Manager", "Admin" },
                Session["UserStatus"].ToString()) >= 0)
            {
                ModifyActivitiesBtn.Visible = true;
            }
        }

        private void populateStaffClassesDdl()
        {
            try
            {
                using (SqlConnection synergyConn = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
                using (SqlCommand myCommand = new SqlCommand(
                        ConfigurationManager.AppSettings["staffClassesListProc"], synergyConn))
                {
                    myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("StaffId", currentUser.id);

                    synergyConn.Open();

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            ClassListDdl.DataSource = myReader;
                            ClassListDdl.DataTextField = "ClassCode";
                            ClassListDdl.DataValueField = "ClassCode";
                            ClassListDdl.DataBind();

                            if ((string)Session["ClassCode"] != null)
                            {
                                ClassListDdl.SelectedValue = (string)Session["ClassCode"];
                            }
                            else
                            {
                                ClassListDdl.SelectedIndex = 0;
                            }
                        }
                        else
                        {
                            throw new Exception(String.Format(
                                "Could not find classes for current user {0} [{1}] with network login {2}.",
                                currentUser.fullName, currentUser.id, currentUser.networkLogin.ToUpper()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("TutorGroupParticipation - Classes List Populator - " + ex.Message, true);
                throw (ex);
            }
        }

        protected void ClassListDdl_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["ClassCode"] = ClassListDdl.SelectedValue;
            populateStudentsList();
        }

        private void populateStudentsList()
        {
            // Clear existing values from the Student List. 
            StudentList.DataSource = null;
            StudentList.Items.Clear();
            StudentList.SelectedIndex = -1;

            // Populate with new student list from the database. 
            using (SqlConnection synergyConn = new SqlConnection())
            {
                try
                {
                    synergyConn.ConnectionString = ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString;

                    using (SqlCommand myCommand = new SqlCommand(ConfigurationManager.AppSettings["classStudentsListProc"], synergyConn))
                    {
                        myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("ClassCode", ClassListDdl.SelectedValue);

                        synergyConn.Open();

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            StudentList.DataSource = myReader.HasRows ? myReader : null;
                            StudentList.DataBind();
                            StudentList.SelectedIndex = -1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Students List Populator - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }
                     

        protected void StudentListEditBtn_Click(object sender, EventArgs e)
        {
            int studentId = int.Parse((sender as Button).CommandArgument);
            Session["StudentId"] = studentId.ToString();
            if (currentUser.CanEditAttributes(studentId))
            {
                Response.Redirect("./EditStudent.aspx");
            }
            else
            {
                Response.Redirect("./StudentSummary.aspx");
            }
        }


        protected void ModifyActivitiesBtn_Click(object sender, EventArgs e)
        {
            if (Array.IndexOf(
                new string[] { "Head of School", "Year Level Manager", "Admin" },
                Session["UserStatus"].ToString()) >= 0)
            {
                Response.Redirect("./ActivitiesEdit.aspx");
            }
            else
            {
                string errorMessage = "'Modify Activities' button is visible to user who is not authorised for this action.";
                errorMessage += "\nUSER_STATUS=" + Session["UserStatus"].ToString();

                ErrorHandler.HandleError(errorMessage, true);
                throw new Exception(errorMessage);
            }
        }
    }
}