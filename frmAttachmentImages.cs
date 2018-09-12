using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MANUUFinance
{
    public partial class frmAttachmentImages : Form
    {
        public int imageId { get; set; }
        public frmAttachmentImages(int imageId)
        {
            InitializeComponent();
            this.imageId = imageId;
        }

        private void frmAttachmentImages_Load(object sender, EventArgs e)
        {
            Image img;
            img = Image.FromFile(@"C:/Users/Md Shoaib Alam/Desktop/attahment/" + imageId + ".jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = img;
        }
    }
}
