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
    public partial class Forward : Form
    {
        //Connection String
        string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
        int GID, fromUnit;
        string sectionNameID;
        Boolean retrivedata = false;
        public Forward(int GID, int fromUnit)
        {
            this.GID = GID;
            this.fromUnit = fromUnit;
            InitializeComponent();
        }

        private void Forward_Load(object sender, EventArgs e)
        {
            sectiontype();
            sectionName();

            richTextRemarks.Text = "Forward from the Section" + "    " + sectionNameID.ToUpper() + "" + "" ;
        }

        private void sectionName()
        {
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT  sectionName FROM [Grievance].[dbo].[SectionUnit] where sectionId = '"+ fromUnit + "'";
            SqlCommand objSelectCommand = new SqlCommand(selectCommand, objSqlConnection);
            try
            {
                objSqlConnection.Open();
                SqlDataReader objDataReader = objSelectCommand.ExecuteReader();
                while (objDataReader.Read())
                {
                    sectionNameID = Convert.ToString(objDataReader[0]);
                }
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

        private void sectiontype()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT sectionId, sectionName FROM [Grievance].[dbo].[SectionUnit]";
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
                comboSection.DisplayMember = "ListItemDesc"; // will display Name property
                comboSection.ValueMember = "ListItemID"; // will select Value property
                comboSection.DataSource = objLOVClass; // assign list (will populate comboBox1.Items)
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

        private void btnForward_Click(object sender, EventArgs e)
        {
            if (retrivedata)
            {
                //Connection String
                string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
                //Instantiate SQL Connection
                SqlConnection objSqlConnection = new SqlConnection(cs);
                //Prepare Update String
                string insertCommand = "Insert into GrievanceTrack (GID, fromUnit, ToUnit, remarks, action, forwardedDate) values " +
                                        "(@GID, @fromUnit, @ToUnit, @remarks, 203,@forwardedDate)";
                SqlCommand objInsertCommand = new SqlCommand(insertCommand, objSqlConnection);

                objInsertCommand.Parameters.AddWithValue("@GID", GID);
                objInsertCommand.Parameters.AddWithValue("@fromUnit", fromUnit);
                objInsertCommand.Parameters.AddWithValue("@ToUnit", comboSection.SelectedValue);
                objInsertCommand.Parameters.AddWithValue("@remarks", richTextRemarks.Text);
                objInsertCommand.Parameters.AddWithValue("@forwardedDate", DateTime.Now);
                try
                {
                    objSqlConnection.Open();
                    objInsertCommand.ExecuteNonQuery();
                   // MessageBox.Show("Record Added Successfully", "Record Addition Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("The following error occured : " + ex.Message, "Update Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    objSqlConnection.Close();
                    updateremarks();
                    this.Close();
                }
            }    
            else
                MessageBox.Show("Please select the Assigned To ", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void comboSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboSection.SelectedIndex !=0)
            {
                retrivedata = true;
            }            
        }

        private void updateremarks()
        {
            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string insertCommand = " update Grievances set forwardedRemarks = @remarks, Gstatus= 203 where GID = '"+ GID + "'";
            SqlCommand objInsertCommand = new SqlCommand(insertCommand, objSqlConnection);

            objInsertCommand.Parameters.AddWithValue("@remarks", richTextRemarks.Text);
            try
            {
                objSqlConnection.Open();
                objInsertCommand.ExecuteNonQuery();
                MessageBox.Show("Record Sent Successfully", "Record Addition Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}
