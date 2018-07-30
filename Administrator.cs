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
    public partial class Administrator : Form
    {
        public int userId { get; set; }
        public Administrator()
        {
            InitializeComponent();
        }

        private void submit_Click(object sender, EventArgs e)
        {
            //if (validateRecord())
            //{
            //    //Connection String
            //    string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;

            //    //Instantiate SQL Connection
            //    SqlConnection con = new SqlConnection(cs);

            //    // Open the connection
            //    con.Open();
            //    // Get the number of the row in database
            //    SqlCommand cmd = new SqlCommand("User_Validation", con);

            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.AddWithValue("@Name", txtUsername.Text);
            //    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
            //    userId = Convert.ToInt32(cmd.ExecuteScalar());
            //    con.Close();

            //    switch (userId)
            //    {
            //        case -1:
            //            MessageBox.Show("password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            cleartextbox();
            //            break;
            //        default:
            //            this.Close();
            //            MDIParent objectmdiparent = new MDIParent(userId);
            //            objectmdiparent.ShowDialog();
            //            break;
            //    }
            //}

            MessageBox.Show("Account is Not activated","Warning" , MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void cleartextbox()
        {
            txtUsername.Text = String.Empty;
            txtPassword.Text = String.Empty;
        }

        private bool validateRecord()
        {
            bool validationResult = true;
            string validationMessage = "";
            if (txtUsername.Text.Length == 0)
            {
                validationMessage += "Please provide User Name\n";
                validationResult = false;
            }
            if (txtPassword.Text.Length == 0)
            {
                validationMessage += "Please provide Password\n";
                validationResult = false;
            }
            if (validationResult == false)
            {
                MessageBox.Show(validationMessage, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
                return true;
        }
    }
}
