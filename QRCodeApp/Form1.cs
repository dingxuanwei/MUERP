using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QRCoder;

namespace QRCodeApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string code = textBox1.Text;
            Produce(code, pictureBox1, QRCodeGenerator.ECCLevel.L);
            Produce(code, pictureBox2, QRCodeGenerator.ECCLevel.M);
            Produce(code, pictureBox3, QRCodeGenerator.ECCLevel.Q);
            Produce(code, pictureBox4, QRCodeGenerator.ECCLevel.H);
        }

        private void Produce(string code, PictureBox pic, QRCodeGenerator.ECCLevel level)
        {
            QRCodeGenerator qr = new QRCodeGenerator();
            QRCodeData data = qr.CreateQrCode(code, level);
            QRCode qrcode = new QRCode(data);

            Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
            pic.Image = qrCodeImage;
        }
    }
}
