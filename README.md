# VEGAS_Csharp

## **9.4：更新**

感悟：不要随意修改运行环境

调试出来以后，不要怀疑程序有大面积的问题，不要进行大面积更改。

## 总结：

### 1、选择文件**按钮事件**

布局好了按钮以后要实现按钮的触发事件需要

先设置按钮的属性，命名，以及触发方式：

```C#
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

```

### 2、提取文本中想要字符串信息事件

创建一个数组来存储想要的信息，我这边采用的是提取文件中的分辨率大小

```C#
private Tuple <int,int>Selectstr(string FilePath)
        {
           
              int nx;
              int ny;
            List<string> str = new List<string>();   //存取分辨率
            str = File.ReadAllLines(FilePath).Skip(0).Take(7).ToList(); //选择读取文件中的的指定哪一行到哪一行

            // string strLine = sr.ReadToEnd().Trim();
            
            string strLine = string.Join("  ",str.ToArray());  //将List<string>转化为string类型
            string[] sArry = strLine.Split(new string[] { "nx", "ny", "BegX" }, StringSplitOptions.RemoveEmptyEntries); //设定匹配字符串名称，读取到对应字符显示sArray[0]表示nx之前的字符，sArray[1]表示nx之后，ny之前的字符，以此类推。
            nx = Convert.ToInt32(sArry[1]);
              // textBox2.Text =Convert.ToString(nx);
            ny = Convert.ToInt32(sArry[2]);

            return new Tuple<int,int>(nx,ny);
        }
```

### 3、读取文件事件

**1、**文本读取的时候：可以采用**文件流**的方式**try--catch**的方式

```c#
 try
            {
                //读取文件
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    //生成字节
                    using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
                    {
                    //按行读取按列赋值,在进行字符分割，字符转换
                        for(int i=0;i<hight;i++)
                        {
                            string strLine = sr.ReadLine();
                            //string strLine = sr.ReadToEnd();
                            string data={"\\s+"}
                            string[] aryLine = strLine.Split(data, System.StringSplitOptions.RemoveEmptyEntries);
                            for(int j=0;j<width;j++)
                            {
                               res[j*width+i] = Convert.ToSingle(aryLine[i]);
                            }
                            
                        }
                    }
                 }
             }
catch(Exception ex)
{
    MessageBox.Show(ex.Message);
}
```

注意细节（split不可进行多个空格的字符分割，可以采用正则表达式进行分割字符串的替换，分割多个空格字符串的手法为"\\\s+",**不是"//s+"**）

```c#
Regex.split(strLine,"\\\s+",RegexOptions.IgnoreCase）
```

**2、**创建一**个List<string>数组用来存储读取到**的文件字符串

```C#
 public float[] ReadFile(int width, int height, string FilePath)
        {
            List<string> hang = new List<string>(); //定义一个集合用来存数据
            float[] res = new float[width*height];  //创建一个float[]数组变量，用来存储读取到的数据

            hang = File.ReadAllLines(FilePath).Skip(8).Take(height).ToList();
			//分割数据
            string strLine = String.Join(" ", hang);  //将List<string>转换为string格式
            //textBox2.Text = strLine;
            string[] mm = Regex.Split(strLine, "\\s+", RegexOptions.IgnoreCase); //对读取到的数据进行一个正则表达式筛选
                        
            //string[] data = { "\\s+" };
            // string[] aryLine = strLine.Split(data, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < mm.Length; i++)
               {
                res[i] = Convert.ToSingle(mm[i]);  //将字符串数组变量转换为
                }
            return res;
        }
```

### 4、开始转换按钮事件

采用OpenCVSharp来绘制图片。

```C#
private void button1_Click(object sender, EventArgs e)
  {
      //从Selectstr函数中读取到文本中的分辨率填到textBox控件中
      textBox3.Text= Selectstr(textBox1.Text.Trim()).Item1.ToString();
      textBox4.Text = Selectstr(textBox1.Text.Trim()).Item2.ToString();
      
      int width = Selectstr(textBox1.Text.Trim()).Item1;
      int height = Selectstr(textBox1.Text.Trim()).Item2;
 
       //加载数据，应用ReadFile函数
      float[] resData = ReadFile(width, height, textBox1.Text.Trim());
      Mat aa = new Mat(height, width, MatType.CV_32FC1, resData);   //创建一个Mat变量，用来将float变量转换为Mat类型矩阵
      if (aa.Empty())
      {
          MessageBox.Show("输入数组为空");
      }
      else
      {
          string strTime = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");  //创建文件名，其中采用System.DataTime.Now.Tostring来获取当前时间
          string name = strTime + ".tiff";  
          Cv2.ImWrite(name, aa);
          MessageBox.Show("图像保存于选择文件路径下");
          //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
          textBox2.Text = name;
      }
  }
```































