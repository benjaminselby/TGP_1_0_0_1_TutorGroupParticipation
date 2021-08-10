using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using ApplicationCode;
using System.Data;

namespace TutorGroupParticipation
{
    public partial class StudentSummary : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack == false)
            {
                populateStudentInformationForm();
            }
        }


        private void populateStudentInformationForm()
        {
            using (SqlConnection synergyConn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            using (SqlCommand myCommand = new SqlCommand(ConfigurationManager.AppSettings["StudentInfoProc"], synergyConn))
            {
                try
                {
                    int studentId = int.Parse(Session["StudentId"].ToString());

                    myCommand.CommandType = CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("StudentId", studentId);

                    synergyConn.Open();

                    using (SqlDataReader studentInfoReader = myCommand.ExecuteReader())
                    {
                        if (studentInfoReader.HasRows)
                        {
                            StudentInfoFrm.DataSource = studentInfoReader;
                            StudentInfoFrm.DataBind();
                        }
                        else
                        {
                            throw new Exception("Could not get information for student with id=" + studentId.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - StudentSummary.aspx - " + ex.Message, true);
                    throw ex;
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }
    }
}