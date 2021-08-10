using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using static ApplicationCode.ErrorHandler;

namespace ApplicationCode
{
    public class User
    {
        public int id;
        public string networkLogin;
        public string fullName;
        public string emailAddress;

        public User(string networkLogin)
        {
            /* Extracts information about the current user from Synergy. Uses this information 
            * to create a new User object. If no user information can be obtained from Synergy, 
            * an exception is thrown. */

            using (SqlConnection synergyOneConnection = new SqlConnection(
                ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            using (SqlCommand userInfoCommand = new SqlCommand(
                ConfigurationManager.AppSettings["GetUserDetails"], synergyOneConnection))
            {
                userInfoCommand.CommandType = System.Data.CommandType.StoredProcedure;
                userInfoCommand.Parameters.AddWithValue("NetworkLogin", networkLogin);

                try
                {
                    synergyOneConnection.Open();

                    SqlDataReader userInfoReader = userInfoCommand.ExecuteReader();
                    if (userInfoReader.HasRows)
                    {
                        userInfoReader.Read();

                        this.id = int.Parse(userInfoReader["ID"].ToString());
                        this.networkLogin = networkLogin;
                        this.fullName = userInfoReader["Preferred"].ToString() + " " + userInfoReader["Surname"].ToString();
                        this.emailAddress = userInfoReader["OccupEmail"].ToString();
                    }
                    else
                    {
                        throw new Exception(
                            "Could not find information for current user with network login "
                            + networkLogin.ToUpper() + " in Synergy database.");
                    }
                }
                catch (Exception ex)
                {
                    HandleError("TutorGroupParticipation - User Object Constructor - " + ex.Message, true);
                    throw (ex);
                }
                finally
                {
                    synergyOneConnection.Close();
                }
            }
        }

        public override string ToString()
        {
            return String.Format(
                "{0} [{1}]",
                this.fullName,
                this.id);
        }

        internal bool CanEditAttributes(int studentId)
        {
            using (SqlConnection synergyConn = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["SynergyOne"].ConnectionString))
            using (SqlCommand canEditAttributesCmd = new SqlCommand(
                    ConfigurationManager.AppSettings["UserCanEditAttributesProc"], synergyConn))
            {
                try
                {
                    canEditAttributesCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    canEditAttributesCmd.Parameters.AddWithValue("UserId", this.id);
                    canEditAttributesCmd.Parameters.AddWithValue("StudentId", studentId);

                    synergyConn.Open();

                    using (SqlDataReader canEditAttributesReader = canEditAttributesCmd.ExecuteReader())
                    {
                        if (canEditAttributesReader.HasRows)
                        {
                            canEditAttributesReader.Read();
                            return (int.Parse(canEditAttributesReader["ReturnValue"].ToString()) == 1);                            
                        }
                        else
                        {
                            throw new Exception("Could not get information for user with id=" + studentId.ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorHandler.HandleError("TutorGroupParticipation - User Object - " + ex.Message, true);
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