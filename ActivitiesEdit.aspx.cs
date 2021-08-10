using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ApplicationCode;
using System.Diagnostics;


namespace TutorGroupParticipation
{
    public partial class ActivitiesEditForm : System.Web.UI.Page
    {
        private User currentUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentUser"] == null)
            {
                string errorMessage = "Missing Current User in Session variables.";
                ErrorHandler.HandleError(errorMessage, true);
                throw new Exception(errorMessage);
            }

            currentUser = (User)Session["CurrentUser"];

            if (Page.IsPostBack == false)
            {
                populateYearLevelDdl();
            }
        }

        private void populateYearLevelDdl()
        {
            try
            {
                using (SqlConnection synergyConn = new SqlConnection(
                        ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
                using (SqlCommand myCommand = new SqlCommand(
                        ConfigurationManager.AppSettings["yearLevelsForStaffProc"], synergyConn))
                {
                    myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    myCommand.Parameters.AddWithValue("StaffId", currentUser.id);

                    synergyConn.Open();

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        if (myReader.HasRows)
                        {
                            YearLevelDdl.DataSource = myReader;
                            YearLevelDdl.DataTextField = "YearLevel";
                            YearLevelDdl.DataValueField = "YearLevel";
                            YearLevelDdl.SelectedIndex = 0;
                            YearLevelDdl.DataBind();

                            populateActivitiesGrd();
                        }
                        else
                        {
                            throw new Exception(String.Format(
                                "Could not find year levels for activities edit by current user {0} [{1}].",
                                currentUser.fullName, currentUser.id, currentUser.networkLogin.ToUpper()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("TutorGroupParticipation - Activities Edit YearLevelsDdl Populator - " 
                        + ex.Message, true);
                throw (ex);
            }
        }

        private void populateActivitiesGrd()
        {
            using (SqlConnection synergyConn = new SqlConnection())
            {
                try
                {
                    synergyConn.ConnectionString = ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString;

                    using (SqlCommand myCommand = new SqlCommand(ConfigurationManager.AppSettings["activitiesForYearLevelProc"], synergyConn))
                    {
                        myCommand.CommandType = System.Data.CommandType.StoredProcedure;
                        myCommand.Parameters.AddWithValue("YearLevel", YearLevelDdl.SelectedValue.ToString());

                        synergyConn.Open();

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter sda = new SqlDataAdapter(myCommand))
                        {
                            sda.Fill(dt);
                            ActivitiesGrd.DataSource = dt;
                            ActivitiesGrd.DataBind();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Activities Edit YearLevelsDdl Populator - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyConn.Close();
                }
            }
        }

        protected void YearLevelDdl_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateActivitiesGrd();
        }

        protected void ActivitiesGrd_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ActivitiesGrd.EditIndex = -1;
            populateActivitiesGrd();
        }

        protected void ActivitiesGrd_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ActivitiesGrd.EditIndex = e.NewEditIndex;
            populateActivitiesGrd();
        }

        protected void ActivitiesGrd_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            string activityId = ActivitiesGrd.DataKeys[e.RowIndex].Value.ToString();
            string newActivityText = (ActivitiesGrd.Rows[e.RowIndex].FindControl("activityTbx") as TextBox).Text;

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                try
                {
                    sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString;

                    using (SqlCommand updateCommand = new SqlCommand(ConfigurationManager.AppSettings["updateActivityTextProc"], sqlConnection))
                    {
                        updateCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        updateCommand.Parameters.AddWithValue("ActivitySeq", activityId);
                        updateCommand.Parameters.AddWithValue("NewActivityText", newActivityText);

                        sqlConnection.Open();

                        updateCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Activities Edit - Update Activity Text - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    sqlConnection.Close();
                    ActivitiesGrd.EditIndex = -1;
                    populateActivitiesGrd();
                }
            }
        }

        protected void ActivitiesGrd_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string activityId = ActivitiesGrd.DataKeys[e.RowIndex].Value.ToString();

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                try
                {
                    sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString;

                    using (SqlCommand updateCommand = new SqlCommand(ConfigurationManager.AppSettings["deleteActivityProc"], sqlConnection))
                    {
                        updateCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        updateCommand.Parameters.AddWithValue("ActivitySeq", activityId);

                        sqlConnection.Open();

                        updateCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Activities Edit - Delete Activity - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    sqlConnection.Close();
                    populateActivitiesGrd();
                }
            }
        }

        protected void AddNewActivityBtn_Click(object sender, EventArgs e)
        {
            if (NewActivityName.Text.Trim() == "")
            {
                return;
            }

            string newActivity = NewActivityName.Text;
            int yearLevel;

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                try
                {
                    if (int.TryParse(YearLevelDdl.SelectedValue, out yearLevel) == false)
                    {
                        throw new Exception(String.Format(
                            "Could not convert year-level drop-down list selected value [{0}] to integer.",
                            YearLevelDdl.SelectedValue));
                    }

                    sqlConnection.ConnectionString = ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString;

                    using (SqlCommand insertCommand = new SqlCommand(ConfigurationManager.AppSettings["insertActivityProc"], sqlConnection))
                    {
                        insertCommand.CommandType = System.Data.CommandType.StoredProcedure;

                        insertCommand.Parameters.AddWithValue("YearLevel", yearLevel);
                        insertCommand.Parameters.AddWithValue("NewActivity", newActivity);

                        sqlConnection.Open();

                        insertCommand.ExecuteNonQuery();

                        sqlConnection.Close();
                    }
                }
                catch (System.Data.SqlClient.SqlException sqlEx)
                {
                    switch (sqlEx.Number)
                    {
                        case 2627:  // Unique constraint error
                            Response.Write("<script>alert('Could not add the activity [" + newActivity + "] as it already exists in the table.');</script>");
                            break;
                        case 547:   // Constraint check violation
                        case 2601:  // Duplicated key row error
                                    // Constraint violation exception
                        default:
                            // Unexpected.
                            throw new Exception(sqlEx.Message,sqlEx.InnerException);
                    }
                }            
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - Activities Edit - Insert Activity - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    sqlConnection.Close();
                    populateActivitiesGrd();
                }
            }

        }

        protected void backToMainPageBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("./Main.aspx");
        }
    }
}