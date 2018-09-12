using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class frmAttachment : Form
    {
        public int imageId { get; set; }
        public frmAttachment(int imageId)
        {
            InitializeComponent();
            this.imageId = imageId;
        }

        private void Attachment_Load(object sender, EventArgs e)
        {
            Image img;
            img = Image.FromFile(@"C:/Users/Md Shoaib Alam/Desktop/attahment/" + imageId +".jpg");
            pictureBox1.Image = img;
        }
    }
}
