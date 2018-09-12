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
    public partial class MDIParent : Form
    {
        private int childFormNumber = 0;
        private int userId;
        public MDIParent(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }       
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MDIParent_Load(object sender, EventArgs e)
        {
            DisplayUserName();
        }

        private void DisplayUserName()
        {
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            objSqlConnection.Open();
            SqlCommand myCommand = new SqlCommand("SELECT Username FROM [Grievance].[dbo].[User] where UserID = '" + userId + "'", objSqlConnection);
            SqlDataReader objDataReader = myCommand.ExecuteReader();
            while (objDataReader.Read())
            {
                label1.Text = objDataReader["Username"].ToString();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void complainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGrievance objectgrievance = new frmGrievance(userId);
            objectgrievance.ShowDialog();
        }

        private void grievanceTrackReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGrievanceTrack objgrievanceTrack = new frmGrievanceTrack();
            objgrievanceTrack.ShowDialog();
        } 
    }
}
