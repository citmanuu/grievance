using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class Emailsend : Form
    {
        OpenFileDialog ofdAttachment;
        String fileName = "",UserEmail;
        int enroll, gid, sectionID;
        Boolean retrivedata = false;

        public Emailsend(string UserEmail, int enroll, int gid, int sectionID)
        {
            this.UserEmail = UserEmail;
            this.enroll = enroll;
            this.gid = gid;
            this.sectionID = sectionID;
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                //Smpt Client Details
                //gmail >> smtp server : smtp.gmail.com, port : 587 , ssl required
                //yahoo >> smtp server : smtp.mail.yahoo.com, port : 587 , ssl required
                SmtpClient clientDetails = new SmtpClient();
                clientDetails.Port = 587;
                //Convert.ToInt32(txtPortNumber.Text.Trim());
                clientDetails.Host = "Smtp.gmail.com";
                    //txtSmtpServer.Text.Trim();
                clientDetails.EnableSsl = true;
                clientDetails.DeliveryMethod = SmtpDeliveryMethod.Network;
                clientDetails.UseDefaultCredentials = false;
                clientDetails.Credentials = new NetworkCredential(txtSenderEmail.Text.Trim(), txtSenderPassword.Text.Trim());

                //Message Details
                MailMessage mailDetails = new MailMessage();
                mailDetails.From = new MailAddress(txtSenderEmail.Text.Trim());
                mailDetails.To.Add(UserEmail);              
                    //txtRecipientEmail.Text.Trim());
                //for multiple recipients
                //mailDetails.To.Add("another recipient email address");
                //for bcc
                //mailDetails.Bcc.Add("bcc email address")
                mailDetails.Subject = txtSubject.Text.Trim();
               // mailDetails.IsBodyHtml = cbxHtmlBody.Checked;
                mailDetails.Body = rtbBody.Text.Trim();


                //file attachment
                if (fileName.Length > 0)
                {
                    Attachment attachment = new Attachment(fileName);
                    mailDetails.Attachments.Add(attachment);
                }

                clientDetails.Send(mailDetails);               
                MessageBox.Show("Your mail has been sent.", "Sent Email", MessageBoxButtons.OK, MessageBoxIcon.Information);
                fileName = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                retrivedata = true;
                updatethedetail();
                updategrievancetrack();
                this.Close();
            }
        }

        private void updategrievancetrack()
        {
            if (retrivedata)
            {

                //Connection String
                string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
                //Instantiate SQL Connection
                SqlConnection objSqlConnection = new SqlConnection(cs);
                //Prepare Update String
                string insertCommand = "Insert into GrievanceTrack (GID, fromUnit, ToUnit, remarks, action, forwardedDate) values " +
                                        "(@GID, @fromUnit, @ToUnit, @remarks, 202,@forwardedDate)";
                SqlCommand objInsertCommand = new SqlCommand(insertCommand, objSqlConnection);

                objInsertCommand.Parameters.AddWithValue("@GID", gid);
                objInsertCommand.Parameters.AddWithValue("@fromUnit", sectionID);
                objInsertCommand.Parameters.AddWithValue("@ToUnit", "");
                objInsertCommand.Parameters.AddWithValue("@remarks", rtbBody.Text);
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
                    this.Close();
                }
            }           
        }

        private void updatethedetail()
        {

            //Connection String
            string cs = ConfigurationManager.ConnectionStrings["FinanceConnectionString"].ConnectionString;
            //Instantiate SQL Connection
            SqlConnection objSqlConnection = new SqlConnection(cs);
            //Prepare Update String
            string insertCommand = " update Grievances set forwardedRemarks = @remarks, Gstatus= 202 where GID = '" + gid + "'";
            SqlCommand objInsertCommand = new SqlCommand(insertCommand, objSqlConnection);

            objInsertCommand.Parameters.AddWithValue("@remarks", rtbBody.Text);
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

        private void Emailsend_Load(object sender, EventArgs e)
        {
            txtRecipientEmail.Enabled = false;
            txtRecipientEmail.Text = UserEmail;
            txtSubject.Text = "Responds of the Ticket # " + gid ;
        }

        private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            try
            {
                ofdAttachment = new OpenFileDialog();
                ofdAttachment.Filter = "Images(.jpg,.png)|*.png;*.jpg;|Pdf Files|*.pdf";
                if (ofdAttachment.ShowDialog() == DialogResult.OK)
                {
                    fileName = ofdAttachment.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
