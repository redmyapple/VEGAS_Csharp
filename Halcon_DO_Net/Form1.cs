using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;



namespace Halcon_DO_Net
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       

  
        private void hSmartWindowControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "JPEG文件|*.jpg|BMP文件|*.bmp";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK && (openFileDialog.FileName != "")) 
            {
                
            }

        }


            private void button2_Click(object sender, EventArgs e)
        {
            // Local iconic variables 

            HObject ho_Image, ho_ImageGray;

            HOperatorSet.GenEmptyObj(out ho_Image);
            HOperatorSet.GenEmptyObj(out ho_ImageGray);
          
            //读取图片-pic.jpg
            HOperatorSet.ReadImage(out ho_Image, "C:\\Users\\Jarvis\\Desktop\\pic\\12.jpg");
            //将打开的图片灰度化处理
            HOperatorSet.Rgb3ToGray(ho_Image, ho_Image, ho_Image, out ho_ImageGray);


            //显示图片
            hSmartWindowControl1.HalconWindow.DispObj(ho_Image);
            hWindowControl2.HalconWindow.DispObj(ho_ImageGray);
            ho_Image.Dispose();
            ho_ImageGray.Dispose();

        }

        private void hWindowControl2_HMouseMove(object sender, HMouseEventArgs e)
        {

        }
    }
}
