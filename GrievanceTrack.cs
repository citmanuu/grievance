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
using System.Windows.Forms.DataVisualization.Charting;

namespace MANUUFinance
{
    public partial class GrievanceTrack : Form
    { 
        //Connection String
        string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

        public GrievanceTrack()
        {
            InitializeComponent();
        }

        private void GrievanceTrack_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'grievanceDataSet1.GrievanceTrack' table. You can move, or remove it, as needed.
            this.grievanceTrackTableAdapter.Fill(this.grievanceDataSet1.GrievanceTrack);
            combopreparationforaction();
            combopreparationforsectionID();
           
        }

        private void chardefault(int a, int b, int c, int d)
        {
                grievanceReportChart.Series.Clear();
            if(a>=0)
                grievanceReportChart.Series["Forward"].Points.AddXY("", a);
            if (b>=0)
                grievanceReportChart.Series["Pending"].Points.AddXY("", b);
            if(c>=0)
                grievanceReportChart.Series["Closed"].Points.AddXY("", c);
            if(d>=0)
                grievanceReportChart.Series["Open"].Points.AddXY("", d);
        }

        private void combopreparationforsectionID()
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

        private void combopreparationforaction()
        {
            var objLOVClass = new List<LOV>();
            objLOVClass.Add(new LOV(0, "-- Please Select --"));

            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string selectCommand = "SELECT GID, GENDESC FROM [Grievance].[dbo].[GMaster]";
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void btnfilter_Click(object sender, EventArgs e)
        {
            StringBuilder SearchStatement = new StringBuilder();
            try
            {
                if (dateTimePicker1.Value == dateTimePicker2.Value)
                {
                    grievanceTrackBindingSource.Filter = SearchStatement.ToString();
                }
                else
                {
                    grievanceTrackBindingSource.Filter = "forwardedDate >= '" + dateTimePicker1.Value + "' And " + "forwardedDate <= '" + dateTimePicker2.Value + "'";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {            
            StringBuilder SearchStatement = new StringBuilder();
            try
            {
                SearchStatement.Clear();
                if (comboAction.SelectedIndex != 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("action like '%" + comboAction.SelectedItem + "%'");
                }

                if (comboSection.SelectedIndex != 0)
                {
                    if (SearchStatement.Length > 0)
                    {
                        SearchStatement.Append(" and ");
                    }
                    SearchStatement.Append("fromUnit like '%" + comboSection.SelectedItem + "%'");
                }

                grievanceTrackBindingSource.Filter = SearchStatement.ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Exception");
            }
            buildchart();
        }

        private void buildchart()
        {
           // Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            // Prepare Update String
            string selectCommand1 = "SELECT count(*) from [Grievance].[dbo].[GrievanceTrack] where action = 201 AND fromUnit = '"+comboSection.SelectedValue + "'";
            SqlCommand objSelectCommand1 = new SqlCommand(selectCommand1, objSqlConnection);

            string selectCommand2 = "SELECT count(*) from [Grievance].[dbo].[GrievanceTrack] where action = 202 AND fromUnit = '" + comboSection.SelectedValue + "'";
            SqlCommand objSelectCommand2 = new SqlCommand(selectCommand2, objSqlConnection);

            string selectCommand3 = "SELECT count(*) from [Grievance].[dbo].[GrievanceTrack] where action = 203 AND fromUnit = '" + comboSection.SelectedValue + "'";
            SqlCommand objSelectCommand3 = new SqlCommand(selectCommand3, objSqlConnection);

            string selectCommand4 = "SELECT count(*) from [Grievance].[dbo].[GrievanceTrack] where action = 204 AND fromUnit = '" + comboSection.SelectedValue + "'";
            SqlCommand objSelectCommand4 = new SqlCommand(selectCommand4, objSqlConnection);

            objSqlConnection.Open();
            SqlDataReader dr1 = objSelectCommand1.ExecuteReader();
            while (dr1.Read())
            {
                label18.Text = dr1[0].ToString();
            }
            objSqlConnection.Close();
            objSqlConnection.Open();
            SqlDataReader dr2 = objSelectCommand2.ExecuteReader();
            while (dr2.Read())
            {
                label19.Text = dr2[0].ToString();
            }
            objSqlConnection.Close();
            objSqlConnection.Open();
            SqlDataReader dr3 = objSelectCommand3.ExecuteReader();
            while (dr3.Read())
            {
                label17.Text = dr3[0].ToString();
            }

            objSqlConnection.Close();
            objSqlConnection.Open();
            SqlDataReader dr4 = objSelectCommand4.ExecuteReader();
            while (dr4.Read())
            {
                label20.Text = dr4[0].ToString();
            }
            objSqlConnection.Close();
            chardefault(checked((int)(Convert.ToUInt32(label17.Text))), checked((int)(Convert.ToUInt32(label20.Text))), checked((int)(Convert.ToUInt32(label19.Text))), checked((int)(Convert.ToUInt32(label18.Text))));
        }

        private void btnSerachGID_Click(object sender, EventArgs e)
        {
            StringBuilder SearchStatement = new StringBuilder();

            SearchStatement.Clear();
            if (txtGrievnace.Text.Length > 0)
            {
                if (SearchStatement.Length > 0)
                {
                    SearchStatement.Append(" and ");
                }
                SearchStatement.Append("BillNumber like '%" + txtGrievnace.Text + "%'");
            }
            else
                MessageBox.Show("Pleas write the Grievance No.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            grievanceTrackBindingSource.Filter = SearchStatement.ToString();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            grievanceTrackBindingSource.Filter = "";
            comboAction.SelectedIndex = 0;
            comboSection.SelectedIndex = 0;
            chardefault(0,0,0,0);
            label17.Text = "00";
            label18.Text = "00";
            label19.Text = "00";
            label20.Text = "00";
        }
    }
}
