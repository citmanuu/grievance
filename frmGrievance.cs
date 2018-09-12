using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Author     : Md Shaoib Alam
// Department : Center for Information Technology 

namespace MANUUFinance
{
    public partial class frmGrievance : Form
    {
        //Connection String
        string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

        public int GlobalId =0, UserId;
        Boolean retrivedata = false;
        public frmGrievance(int UserId)
        {
            InitializeComponent();
            this.UserId = UserId;
            btnAttachment.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (retrivedata)
            {
                Forward objforward = new Forward(Convert.ToInt32(label17.Text), findSection());
                objforward.ShowDialog();
                clearthebox();
            }
            else
                MessageBox.Show("Please Click the DataGridView ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void Grievance_Load(object sender, EventArgs e)
        {
            Load_datagridview();
            actionnature();
            btnAttachment.Enabled = false;
        }

        private void Load_datagridview()
        {

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            if (new AdministratorLogin().administratorLogin(UserId))
            {
                SqlDataAdapter sqldb = new SqlDataAdapter("Select A.GID, B.UserName, B.EnrollmentID, B.YearOfAdmission, B.mobileNumber, B.courseName, B.emailID, B.DOB, B.center, A.GDescription, A.Gstatus, A.forwardedRemarks from Grievances A, RegistrationUser B where B.StudentId = A.StudentId ", objSqlConnection);
                DataTable dtb1 = new DataTable();
                sqldb.Fill(dtb1);
                DGVGrievance.DataSource = dtb1;
            }
            else
            {
                SqlDataAdapter sqldb = new SqlDataAdapter("Select A.GID, B.UserName, B.EnrollmentID, B.YearOfAdmission, B.mobileNumber, B.courseName, B.emailID, B.DOB, B.center, A.GDescription, A.Gstatus, A.forwardedRemarks " +
                    "from Grievances A, RegistrationUser B " +
                    "where A.StudentId = B.StudentId and A.Gstatus != 202 and A.sectionID = '" + findSection() + "' ", objSqlConnection);
                DataTable dtb1 = new DataTable();
                sqldb.Fill(dtb1);
                DGVGrievance.DataSource = dtb1;

            }
        }

        private int findSection()
        {
            int SID = 0;
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            string selectCommand = "SELECT sectionId FROM [Grievance].[dbo].[User] WHERE UserID = '" + UserId + "'";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                if (objDataReader.Read())
                {
                    SID = Convert.ToInt32(objDataReader["sectionId"].ToString());
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
            return SID; 
        }

        private void actionnature()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));
            
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT GID, GENDESC FROM [Grievance].[dbo].[GMaster] WHERE GID !=201 ";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    objLOVClass.Add(new LOV(Convert.ToInt32(objDataReader[0]), Convert.ToString(objDataReader[1])));
                }
                // Bind combobox list to the items
                comboAction.DisplayMember = "ListItemDesc"; // will display Name property
                comboAction.ValueMember = "ListItemID"; // will select Value property
                comboAction.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
            }
            catch (SqlException ex)
            {
                MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                objSqlConnection.Close();
            }           
        }

        private void DGVGrievance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                label17.Text = DGVGrievance.Rows[e.RowIndex].Cells[0].FormattedValue.ToString();
                label18.Text = DGVGrievance.Rows[e.RowIndex].Cells[8].FormattedValue.ToString();
                txtName.Text = DGVGrievance.Rows[e.RowIndex].Cells[1].FormattedValue.ToString();
                txtEnrollment.Text = DGVGrievance.Rows[e.RowIndex].Cells[2].FormattedValue.ToString();
                txtYOA.Text = DGVGrievance.Rows[e.RowIndex].Cells[3].FormattedValue.ToString();
                txtEmail.Text = DGVGrievance.Rows[e.RowIndex].Cells[6].FormattedValue.ToString();
                txtDOB.Text = DGVGrievance.Rows[e.RowIndex].Cells[7].FormattedValue.ToString();
                txtCourseName.Text = DGVGrievance.Rows[e.RowIndex].Cells[5].FormattedValue.ToString();
                txtMobileNumber.Text = DGVGrievance.Rows[e.RowIndex].Cells[4].FormattedValue.ToString();
                txtDescription.Text = DGVGrievance.Rows[e.RowIndex].Cells[9].FormattedValue.ToString();
                GlobalId = Convert.ToInt32(DGVGrievance.Rows[e.RowIndex].Cells[10].FormattedValue.ToString());
                richTextRemarks.Text = DGVGrievance.Rows[e.RowIndex].Cells[11].FormattedValue.ToString();
                retrivedata = true;
                statusdetail();
                richTextRemarks.ReadOnly = true;
                txtDescription.ReadOnly = true;
                lockbutton();
                findattachment(Convert.ToInt32(label17.Text));
            }
        }

        private void findattachment(int imageId)
        {
            if (new imagesAvailable().imagesavailable(imageId))
            {
                btnAttachment.Enabled = true;
            }
            else
            {
                btnAttachment.Enabled = false;
            }
        }

        private void lockbutton()
        {
            btnForward.Enabled = false;
            btnresponse.Enabled = false;
            btnPending.Enabled = false;
            btnresponse.Enabled = false;

        }

        private void statusdetail()
        {            
             //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT GENDESC FROM [Grievance].[dbo].[GMaster] WHERE GID = '" + GlobalId + "'";
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

        private void comboAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(comboAction.SelectedIndex == 2)
            {
                btnForward.Enabled = true;
            }     
            if (comboAction.SelectedIndex == 1)
            {
                btnresponse.Enabled = true;
            }
        }

        private void clearthebox()
        {
            txtName.Text = "";
            txtCourseName.Text = "";
            txtDescription.Text = "";
            txtDOB.Text = "";
            txtEmail.Text = "";
            txtEnrollment.Text = "";
            txtMobileNumber.Text = "";
            txtYOA.Text = "";
            label17.Text = "0000";
            label19.Text = "None";
            label18.Text = "None";
            richTextRemarks.Text = "";
            richTextRemarks.Enabled = true;
            txtDescription.Enabled = true;
            btnresponse.Enabled = true;
            btnPending.Enabled = true;
            btnForward.Enabled = true;
            btnAttachment.Enabled = false;
            retrivedata = false;
            comboAction.SelectedIndex = 0;
            Load_datagridview();
        }

        private void btnresponse_Click(object sender, EventArgs e)
        {
            if (retrivedata)
            {
                Emailsend objectemailsend = new Emailsend(Convert.ToString(txtEmail.Text), Convert.ToInt32(txtEnrollment.Text), Convert.ToInt32(label17.Text), findSection());
                objectemailsend.ShowDialog();
                clearthebox();
            }
            else
                MessageBox.Show("Please Click the DataGridView ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnPending_Click(object sender, EventArgs e)
        {
            if (retrivedata)
            {

            }
            else
                MessageBox.Show("Please Click the DataGridView ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (retrivedata)
            {
                clearthebox();
                btnClearSearch_Click(null, null);
            }
            else
                MessageBox.Show("Please Click the DataGridView ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder SearchStatement = new StringBuilder();
            try
            {
                SearchStatement.Clear();
                if (txtGrievanceNumber.Text.Length > 0)
                {
                    SearchStatement.Append("Convert([GID],'System.String') like '%" + txtGrievanceNumber.Text + "%'");
                }
                if (txtCenterSearch.Text.Length > 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("center like '%" + txtCenterSearch.Text + "%'");
                }
                if (textEmail.Text.Length > 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("emailID like '%" + textEmail.Text + "%'");

                }
                if (SearchStatement.ToString().Length > 0)
                {
                    grievanceViewBindingSource.Filter = SearchStatement.ToString();
                }
                else
                    MessageBox.Show("Nothing to Quyery. Please select/set values for query in the form", "Query paramters not set", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttachment_Click(object sender, EventArgs e)
        {
            frmAttachmentImages objectimage = new frmAttachmentImages(Convert.ToInt32(label17.Text));
            objectimage.ShowDialog();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            grievanceViewBindingSource.Filter = "";
            txtGrievanceNumber.Text = "";
            txtCenterSearch.Text = "";
            textEmail.Text = "";
        }
    }
}
