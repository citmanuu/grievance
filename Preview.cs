using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class Preview : Form
    {
        int GID;
        public Preview(int GID)
        {
            this.GID = GID;
            InitializeComponent();
        }

        private void Preview_Load(object sender, EventArgs e)
        {
            txtDescription.ReadOnly = true;
            richTextBox1.ReadOnly = true;
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            string selectCommand = "Select GID, StudentId, sectionID, GType, GUsername, GDescription, [Gregistrationdate&time]," +
                "Gstatus, forwardedRemarks from Grievances where GID = '" + GID + "' ";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                if (objDataReader.Read())
                {
                    label17.Text = objDataReader["GID"].ToString();
                    label19.Text = objDataReader["StudentId"].ToString();
                    label5.Text = objDataReader["sectionID"].ToString();
                    getsectionID();
                    studentdetail();
                    label19.Text = objDataReader["Gstatus"].ToString();
                    getstatus();
                    txtDescription.Text = objDataReader["GDescription"].ToString();
                    richTextBox1.Text = objDataReader["forwardedRemarks"].ToString();
                    label4.Text = objDataReader["Gregistrationdate&time"].ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void getsectionID()
        {
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT sectionName, forwardtoEmail FROM [Grievance].[dbo].[SectionUnit] WHERE sectionId = '" + Convert.ToInt32(label5.Text) + "'";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                if (objDataReader.Read())
                {
                    label6.Text = objDataReader["forwardtoEmail"].ToString();
                    //label6.Text = objDataReader["sectionName"].ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void getstatus()
        {
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT GENDESC FROM [Grievance].[dbo].[GMaster] WHERE GID = '" + Convert.ToInt32(label19.Text) + "'";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                if (objDataReader.Read())
                {
                    label19.Text = objDataReader["GENDESC"].ToString();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void studentdetail()
        {
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            string selectCommand = "Select UserName, EnrollmentID, YearOfAdmission, mobileNumber, courseName, emailID, DOB,center from RegistrationUser where StudentId  = '" + Convert.ToInt32(label19.Text) + "' ";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                if (objDataReader.Read())
                {
                    textBox7.Text = objDataReader["UserName"].ToString();
                    textBox4.Text = objDataReader["EnrollmentID"].ToString();
                    textBox2.Text = objDataReader["YearOfAdmission"].ToString();
                    textBox6.Text = objDataReader["emailID"].ToString();
                    textBox3.Text = objDataReader["DOB"].ToString();
                    textBox1.Text = objDataReader["courseName"].ToString();
                    textBox5.Text = objDataReader["mobileNumber"].ToString();
                    label9.Text = objDataReader["center"].ToString();                   
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error");
            }
            finally
            {
                objSqlConnection.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Emailsend objectemail = new Emailsend((label6.Text).ToString(),0, GID, Convert.ToInt32(label5.Text));
            objectemail.ShowDialog();
        }
    }
}
