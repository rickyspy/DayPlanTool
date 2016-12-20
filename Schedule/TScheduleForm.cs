using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Schedule
{
    public partial class TScheduleForm : Form
    {
        public TScheduleForm()
        {
            InitializeComponent();
        }

        public TScheduleForm(Task tk)
            : this()
        {
            TaskHere = tk;
            textBox1.Text = TaskHere.Name;
            textBox5.Text = TaskHere.ExpectedTime.ToString();
            textBox2.Text = TaskHere.AlarmTime.ToShortTimeString();
            TaskType = 1;
        }


       Task TaskHere ;
       public int TaskType;

       private void button1_Click(object sender, EventArgs e)//添加schedule
       {
           if (textBox1.Text == "")
           {
               MessageBox.Show("No name Task！Please re-enter!");
           }
           else
           {
               if (TaskType == 0||TaskType==2)
               {
                   TaskHere = new Task();
                   TaskHere.Name = textBox1.Text;
                   TaskHere.ExpectedTime = double.Parse(textBox5.Text);
                   if (DateTime.TryParse(textBox2.Text, out TaskHere.AlarmTime))
                       TaskHere.AlarmTime = DateTime.Parse(textBox2.Text);
                   else
                       TaskHere.AlarmTime = DateTime.MaxValue;

                   Form1.TScheduleList.Add(TaskHere);
                   this.Dispose();
               }
               else if (TaskType == 1)
               {
                   TaskHere.Name = textBox1.Text;
                   TaskHere.ExpectedTime = double.Parse(textBox5.Text);

                   if (DateTime.TryParse(textBox2.Text, out TaskHere.AlarmTime))
                       TaskHere.AlarmTime = DateTime.Parse(textBox2.Text);
                   else
                       TaskHere.AlarmTime = DateTime.MinValue;
                   this.Dispose();
               }

              
           }
       }

       private void textBox2_MouseDoubleClick(object sender, MouseEventArgs e)
       {
           textBox2.Text = DateTime.Now.ToShortTimeString();
       }




    }
}
