using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.Cuda;
using System.Timers;

namespace EMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

 
        private void button1_Click(object sender, EventArgs e)
        {

            int width = Convert.ToInt32(textBox3.Text.Trim());
            int height= Convert.ToInt32(textBox4.Text.Trim());
           
            //加载数据
            float[] resData = ReadFile(width, height, textBox1.Text.Trim());

            Mat aa = new Mat(height, width, MatType.CV_32FC1, resData);

            if (!aa.Empty())
            {
                MessageBox.Show("输入数组为空");
            }

           // Cv2.ImShow("Result_image", aa);
            //Cv2.NamedWindow("Result_image", WindowMode.AutoSize);

            Cv2.ImWrite("Result_image.tiff", aa);
            Cv2.WaitKey(0);
         
     }
    
  
        private float[] ReadFile(int width, int height, string FilePath)
        {
            
           float[] res = new float[width * height];
            try
            {
                //创建一个负责读取的文件流
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    //生成字节流
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        //按行开始循环
                        for (int j = 0; j < height; j++)
                        {
                            //读取数据
                             string strLine = sr.ReadLine().Trim();
                            string[] separators = { " \\s+" };
                            string[] aryLine = strLine.Split(separators, System.StringSplitOptions.RemoveEmptyEntries);


                            //按列赋值
                            for (int i = 0; i < width; i++)
                            {
                               res[width * j + i] = Convert.ToSingle(aryLine[i]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); ;
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
          
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;  //该值确定是否可以选择多个文件
            dialog.Title = "请选择文件"; //弹窗的标题
            dialog.InitialDirectory = "C: \\Users";  //默认打开的文件夹的位置
            dialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";  //筛选文件
            dialog.ShowHelp = true;  //是否显示帮助按钮
            dialog.FilterIndex = 1;

            if (dialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = dialog.FileName; 
            }
        }

        

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    SaveFileDialog savedialog = new SaveFileDialog();
        //    savedialog.Filter = "Tiff图片|*.tiff";
        //    savedialog.FilterIndex = 0;
        //    savedialog.RestoreDirectory = true;   //保存对话框是否记忆上一次打开的位置
        //    savedialog.CheckPathExists = true;  //检查目录
        //    savedialog.FileName = System.DateTime.Now.ToString("yyyyMMddHHmmss");  //设置默认文件名
        //    if (savedialog.ShowDialog() == DialogResult.OK)
        //    { 


        //    }
        //}
    }
}
