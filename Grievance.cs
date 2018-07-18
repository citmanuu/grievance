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
    public partial class Grievance : Form
    {
        //Connection String
        string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

        public int GlobalId =0;
        public Grievance()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {          
            Forward objforward = new Forward();
            objforward.ShowDialog();
        }

        private void Grievance_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'grievanceDataSet.grievanceView' table. You can move, or remove it, as needed.
            this.grievanceViewTableAdapter.Fill(this.grievanceDataSet.grievanceView);
            actionnature();

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
                statusdetail();
                lockbutton();
            }
        }

        private void lockbutton()
        {
            btnForward.Enabled = false;
            btnresponse.Enabled = false;
            btnPending.Enabled = false;

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
    }
}
