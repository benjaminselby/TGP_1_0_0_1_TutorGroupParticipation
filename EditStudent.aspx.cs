using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using ApplicationCode;
using System.Data;

namespace TutorGroupParticipation
{
    public partial class EditStudent: System.Web.UI.Page
    {
        private int studentId;

        protected void Page_Load(object sender, EventArgs e)
        {
            studentId = int.Parse(Session["StudentId"].ToString());

            if (Page.IsPostBack == false)
            {
                populateStudentInformationForm();
                populateParticipationCheckboxList();

                if ((string)Session["UserStatus"] == "Student")
                {
                    RequestNewActivityDiv.Visible = true;
                    RequestNewActivityBtn.Visible = true;
                }
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
                    myCommand.CommandType = System.Data.CommandType.StoredProcedure;
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
                            throw new Exception("Could not get information for user with id=" + studentId.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - EditStudent.aspx - " + ex.Message, true);
                    throw ex;
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }


        private void populateParticipationCheckboxList()
        {
            using (SqlConnection synergyConn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            { 
                try
                {
                    // 1. Add checkboxes corresponding to all possible student activites. 

                    using (SqlCommand myCommand = new SqlCommand(
                            ConfigurationManager.AppSettings["activitiesForYearLevelProc"], synergyConn))
                    {
                        myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("YearLevel", ((Label)StudentInfoFrm.FindControl("YearLevelLbl")).Text);

                        synergyConn.Open();

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            if (myReader.HasRows)
                            {
                                ParticipationCbxLst.DataSource = myReader;
                                ParticipationCbxLst.DataTextField = "Activity";
                                ParticipationCbxLst.DataValueField = "Seq";
                                ParticipationCbxLst.DataBind();
                            }
                            else
                            {
                                String errorMessage = String.Format("Could not find Tutor Group Activities for YearLevel={0}"
                                    + " [current student: {1} (#{2})] ",
                                    ((Label)StudentInfoFrm.FindControl("YearLevelLbl")).Text,
                                    ((Label)StudentInfoFrm.FindControl("NameLbl")).Text,
                                    ((Label)StudentInfoFrm.FindControl("IdLbl")).Text);

                                ErrorHandler.HandleError(errorMessage, true);
                                throw new Exception(errorMessage);
                            }
                            myReader.Close();
                        }
                    }

                    // 2. Check all of the checkboxes where the current student already has an activity recorded. 

                    using (SqlCommand myCommand = new SqlCommand(
                            ConfigurationManager.AppSettings["participationForStudentProc"], synergyConn))
                    {
                        myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("StudentId", studentId);

                        using (SqlDataReader myReader = myCommand.ExecuteReader())
                        {
                            while (myReader.Read())
                            {
                                ParticipationCbxLst.Items.FindByValue(myReader["ActivitySeq"].ToString()).Selected = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - EditStudent.aspx - Student Information Populator - " + ex.Message, true);
                    throw ex;
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }


        protected void SaveCurrentStudentParticipation()
        {
            // The stored procedure in Synergy which saves student participation
            // accepts a comma-separated list of Activity sequence numbers as a parameter. 

            // Get a comma-separated string list of Activity IDs for all the currently selected checkboxes. 
            string activitySequenceList = "";
            foreach (ListItem checkBox in ParticipationCbxLst.Items)
            {
                if (checkBox.Selected)
                {
                    activitySequenceList += checkBox.Value + ",";
                }
            }

            if (activitySequenceList.Length > 0)
            {
                // Remove trailing comma. 
                activitySequenceList = activitySequenceList.Substring(0, activitySequenceList.Length - 1);
            }

            // Write the new list of activities to the database.
            using (SqlConnection synergyConn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            {
                try
                {
                    using (SqlCommand myCommand = new SqlCommand(
                            ConfigurationManager.AppSettings["saveStudentActivitiesProc"], synergyConn))
                    {
                        myCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter StudentIdParam = new SqlParameter();
                        StudentIdParam.ParameterName = "StudentId";
                        StudentIdParam.SqlDbType = SqlDbType.Int;
                        StudentIdParam.Value = studentId;
                        myCommand.Parameters.Add(StudentIdParam);

                        SqlParameter ModifiedByIdParam = new SqlParameter();
                        ModifiedByIdParam.ParameterName = "ModifiedById";
                        ModifiedByIdParam.SqlDbType = SqlDbType.Int;
                        ModifiedByIdParam.Value = ((User)Session["CurrentUser"]).id;
                        myCommand.Parameters.Add(ModifiedByIdParam);

                        myCommand.Parameters.AddWithValue("ActivitySeqList", activitySequenceList);

                        synergyConn.Open();

                        int returnValue = (int)myCommand.ExecuteScalar();

                        if (returnValue != 1)
                        {
                            throw new Exception(String.Format(
                                "Error calling stored procedure to update Tutor Group activities for student {0} - Return value = {1}",
                                studentId,
                                returnValue));
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - EditStudent.aspx - Activities Checkboxlist - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }

        protected void SaveBtn_Click(object sender, EventArgs e)
        {
            SaveCurrentStudentParticipation();
            if ((string)Session["UserStatus"] == "Student")
            {
                Response.Redirect("./Saved.aspx");
            }
            else
            {
                Response.Redirect("./Staff.aspx");
            }
        }

        protected void RequestNewActivityBtn_Click(object sender, EventArgs e)
        {
            // We are navigating away from the page, so we assume the student wants to keep
            // the current set of selected activities. 
            SaveCurrentStudentParticipation();

            if ((string)Session["UserStatus"] == "Student")
            {
                Session["studentYearLevel"] = ((Label)StudentInfoFrm.FindControl("YearLevelLbl")).Text;
                Response.Redirect("./RequestNewActivity.aspx");
            }
            else
            {
                string errorMessage = String.Format(
                    "RequestNewActivity button should not be available for current user [{0}] with STATUS={1}",
                    Session["CurrentUser"].ToString(),
                    Session["UserStatus"].ToString());
                ErrorHandler.HandleError(errorMessage, true);
                throw new Exception(errorMessage);
            }
        }

        protected void CancelBtn_Click(object sender, EventArgs e)
        {
            // If the user is a student, clear and reset the checkboxes to match what has been saved previously. 
            if ((string)Session["UserStatus"] == "Student")
            {
                populateParticipationCheckboxList();
            }
            else
            {
                Response.Redirect("./Staff.aspx");
            }
        }
    }
}