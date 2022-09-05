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
using OpenCvSharp.Extensions;
using System.Timers;
using System.Text.RegularExpressions;

namespace EMS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       
        
        
       
        /// <summary>
        /// 开始转换按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //从Selectstr函数中读取到文本中的分辨率填到textBox控件中
            textBox3.Text= Selectstr(textBox1.Text.Trim()).Item1.ToString();
            textBox4.Text = Selectstr(textBox1.Text.Trim()).Item2.ToString();
            //int width = Convert.ToInt32(textBox3.Text.Trim());
            //int height= Convert.ToInt32(textBox4.Text.Trim());

            int width = Selectstr(textBox1.Text.Trim()).Item1;
            int height = Selectstr(textBox1.Text.Trim()).Item2;

             //加载数据，应用ReadFile函数
            float[] resData = ReadFile(width, height, textBox1.Text.Trim());
            Mat aa = new Mat(height, width, MatType.CV_32FC1, resData);

            
            //Cv2.ImShow("Result_image", aa);
            //Cv2.NamedWindow("Result_image", WindowMode.AutoSize);

            if (aa.Empty())
            {
                MessageBox.Show("输入数组为空");
            }
            else
            {
                string strTime = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
                string name = strTime + ".tiff";
                Cv2.ImWrite(name, aa);
                MessageBox.Show("图像保存于选择文件路径下");
                //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                textBox2.Text = name;
            }
        }
    
        /// <summary>
        /// 读取亮度数据
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public float[] ReadFile(int width, int height, string FilePath)
        {
            List<string> hang = new List<string>();//定义一个集合用来存数据
            float[] res = new float[width*height];
            try
            {
                //读取文件
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    //生成字节
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                        //读取数据
                         //string strLine = sr.ReadToEnd().Trim();
                        hang = File.ReadAllLines(FilePath).Skip(8).Take(height).ToList();

                        //分割数据
                        string strLine = String.Join(" ", hang);
                        //textBox2.Text = strLine;
                        string[] mm = Regex.Split(strLine, "\\s+", RegexOptions.IgnoreCase);
                        

                        //string[] data = { "\\s+" };
                        // string[] aryLine = strLine.Split(data, System.StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < mm.Length; i++)
                        {
                            res[i] = Convert.ToSingle(mm[i]);
                        }
                    }
                }
            }
            catch (Exception ex )
            {
                MessageBox.Show(ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 读取文本中分辨率的数据
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        private Tuple <int,int>Selectstr(string FilePath)
        {
           
              int nx;
              int ny;
            List<string> str = new List<string>();   //存取分辨率
            str = File.ReadAllLines(FilePath).Skip(0).Take(7).ToList();

            // string strLine = sr.ReadToEnd().Trim();
            
            string strLine = string.Join("  ",str.ToArray());
            string[] sArry = strLine.Split(new string[] { "nx", "ny", "BegX" }, StringSplitOptions.RemoveEmptyEntries);
            nx = Convert.ToInt32(sArry[1]);
              // textBox2.Text =Convert.ToString(nx);
            ny = Convert.ToInt32(sArry[2]);

            return new Tuple<int,int>(nx,ny);
        }

        /// <summary>
        /// 选取文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        
        /// <summary>
        /// 保存文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
