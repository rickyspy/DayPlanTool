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

namespace Schedule
{
    public partial class TimingSettingForm : Form
    {
        public TimingSettingForm()
        {
            InitializeComponent();
        }

        Task TaskHere;

        private void TimingForm_Load(object sender, EventArgs e)
        {

                TaskHere = Form1.ObjTask2;
                label2.Text = TaskHere.Name;
                DateTime dt = System.DateTime.Now; //new DateTime();
                textBox1.Text = dt.Hour.ToString("00") + ":" + (dt.Minute).ToString("00") + ":" + dt.Second.ToString("00");

               // 初始化提醒音乐选项
                DirectoryInfo theFolder = new DirectoryInfo(@"Music\");
                FileInfo[] dirInfo = theFolder.GetFiles();
                //遍历文件夹
                foreach (FileInfo NextFile in dirInfo)
                {
                    this.comboBox1.Items.Add(NextFile.Name);
                }
                comboBox1.Text = comboBox1.Items[0].ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            TaskHere.ObjectTime = textBox1.Text;
            Form1.ObjTask2.ObjectTime = textBox1.Text;
            TaskHere.Music = comboBox1.Text;
            Form1.Tiform = new TimingForm(int.Parse(textBox4.Text), int.Parse(textBox5.Text), BackColor1, opacity);
            Form1.Tiform.Show();
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ColorDialog loColorForm = new ColorDialog();
            if (loColorForm.ShowDialog() == DialogResult.Yes)
            {
                Color loResultColor = loColorForm.Color;
            }
        }


        Color BackColor1 = Color.Black;

        private void button3_Click_1(object sender, EventArgs e)
        {
            ColorDialog loColorForm = new ColorDialog();
            if (loColorForm.ShowDialog() == DialogResult.OK)
            {
               BackColor1 = loColorForm.Color;
            }

            this.button3.BackColor = BackColor1;
        }

        double opacity = 0.7;
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            opacity = double.Parse(textBox2.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }


    }
}
