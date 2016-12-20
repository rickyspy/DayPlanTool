using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Schedule
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        FScheduleForm FSForm; //任务窗体
        TScheduleForm TSForm;//计划窗体
        TimingSettingForm TiSettingForm;// 计时设置窗体
        public static TimingForm Tiform; //倒计时窗体
        public static Task ObjTask2;
        public static Task ObjTask1;
        public static List<Task> FScheduleList = new List<Task>();//将来任务list 
        public static List<Task> TScheduleList = new List<Task>(); //几日安排list


        #region MyRegion

        public void AddFutureSchedule()
        {
            FSForm = new FScheduleForm();
            FSForm.Show();
        }
        public void AddTodaySchedule(int t)
        {
            TSForm = new TScheduleForm();
            TSForm.TaskType = t;
            TSForm.Show();
        }
        public string cutString(string strInput, int intLen) // 字节数切割
        {
            strInput = strInput.Trim();
            byte[] myByte = System.Text.Encoding.Default.GetBytes(strInput);
            if (myByte.Length > intLen)
            {
                string resultStr = "";
                for (int i = 0; i < strInput.Length; i++)
                {
                    byte[] tempByte = System.Text.Encoding.Default.GetBytes(resultStr);
                    if (tempByte.Length < intLen)
                    {

                        resultStr += strInput.Substring(i, 1);
                    }
                    else
                    {
                        break;
                    }
                }
                return resultStr + "...";
            }
            else
            {
                return strInput;
            }
        }
        public void ShowFutureSchedule()//DatagridView中显示tasks
        {
            // 设置表头
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Emergency Level", typeof(string));
            dt.Columns.Add("Importance", typeof(string));

            // 数据显示  

            for (int i = 0; i < FScheduleList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = FScheduleList[i].Name;
                dr[1] = ShowLevel(FScheduleList[i].EmergencyLevel);
                dr[2] = ShowLevel(FScheduleList[i].Importance);
                dt.Rows.Add(dr);
            }

            //将datatable绑定到datagridview上显示结果  
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
            dataGridView1.Columns[1].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
            dataGridView1.Columns[2].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;

            // textbox 中 show task
            if (FScheduleList.Count != 0)
                ObjTask1 = FScheduleList[0];

            if (ObjTask1 != null)
                textBox1.Text = ObjTask1.Note;


            // showtask period in PIC 


        }

        public string ShowLevel(int level)//显示级别
        {
            string stars = "";
            if (level == 1)
                stars = "★☆☆☆☆";
            else if (level == 2)
                stars = "★★☆☆☆";
            else if (level == 3)
                stars = "★★★☆☆";
            else if (level == 4)
                stars = "★★★★☆";
            else if (level == 5)
                stars = "★★★★★";
            return stars;
        }
        public void ShowTodaySchedule()// listbox2 中显示 tasks
        {
            // 设置表头
            DataTable dt = new DataTable();
           // dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Expected Time", typeof(string));
            dt.Columns.Add("Alarm Time", typeof(string));

            // 数据显示
            for (int i = 0; i < TScheduleList.Count; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = TScheduleList[i].Name;
                dr[1] = TScheduleList[i].ExpectedTime.ToString();
                dr[2] = TScheduleList[i].AlarmTime.ToShortTimeString();
                if (dr[1].ToString() == "0")
                    dr[1] = "-";
                if (dr[2].ToString()=="23:59")
                    dr[2] = "-";
                dt.Rows.Add(dr);
            }

            // 将datatable 绑定到 datagridview 上
            dataGridView2.DataSource = dt;
            dataGridView2.Columns[0].Width = (dataGridView2.Width - dataGridView2.RowHeadersWidth) / 3;
            dataGridView2.Columns[1].Width = (dataGridView2.Width - dataGridView2.RowHeadersWidth) / 3;
            dataGridView2.Columns[2].Width = (dataGridView2.Width - dataGridView2.RowHeadersWidth) / 3;

            if (TScheduleList.Count != 0)
                ObjTask2 = TScheduleList[0];
            if (ObjTask2 != null)
                textBox2.Text = ObjTask2.Note;

        }

        public void TaskShow()
        {
            // show
            ShowFutureSchedule();
            ShowTodaySchedule();
        }
        public void Save()
        {
            // Objtask 的 note更新
            if (ObjTask1 != null)
                ObjTask1.Note = textBox1.Text;
            if (ObjTask2 != null)
                ObjTask2.Note = textBox2.Text;

            //输出task
            OutputFutureSchedule();
            OutputTodaySchdule();
        }
        public void Refresh1()
        {
            // Objtask 的 note更新
            if (ObjTask1 != null)
                ObjTask1.Note = textBox1.Text;
            if (ObjTask2 != null)
                ObjTask2.Note = textBox2.Text;

             //show
            ShowFutureSchedule();
            ShowTodaySchedule();

            //输出task
            OutputFutureSchedule();
            OutputTodaySchdule();


        }

        public void OutputFutureSchedule()
        {
            using (StreamWriter sw = new StreamWriter(@"FutureSchedule\Tasklist.txt", false))
            {
                foreach (var task in FScheduleList)
                {
                    sw.WriteLine(task.Name + "\t" + task.EmergencyLevel + "\t" + task.Importance + "\t" + task.ExpectedTime +
                        "\t" + task.StartDate + "\t" + task.ObjectTime);
                    // task period
                    using (StreamWriter swperiod = new StreamWriter(@"FutureSchedule\" + task.Name + "_period.txt", false))
                    {
                        if (task.TaskPeriod != null)
                        {
                            foreach (var item in task.TaskPeriod)
                            {
                                swperiod.WriteLine(item.Name + "\t" + item.EStartTime + "\t" + item.EEndTime + "\t" + item.AStartTime + "\t" + item.AEndTime);
                            }
                        }
                    }
                    using (StreamWriter swnote = new StreamWriter(@"FutureSchedule\" + task.Name + "_task.txt", false))
                        swnote.Write(task.Note);
                }
            }
        }
        public void OutputTodaySchdule()
        {
            using (StreamWriter sw = new StreamWriter(@"TodaySchedule\Schedulelist.txt", false))
            {
                foreach (var task in TScheduleList)
                {
                    sw.WriteLine(task.Name  +"\t"+ task.ExpectedTime +"\t" + task.AlarmTime);
                    using (StreamWriter swnote = new StreamWriter(@"TodaySchedule\" + task.Name + "_schedule.txt", false))
                        swnote.Write(task.Note);
                }
            }
        }
        public void InputFutureSchedule()
        {
            using (StreamReader sr = new StreamReader(@"FutureSchedule\Tasklist.txt", false))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();

                    if (s != "")
                    {
                        string[] ss = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                        string snote;
                        using (StreamReader srnote = new StreamReader(@"FutureSchedule\" + ss[0] + "_task.txt"))
                            snote = srnote.ReadToEnd();
                        Task tk = new Task(ss[0], int.Parse(ss[1]), int.Parse(ss[2]),snote);
                        // input period 

                        using (StreamReader srperiod = new StreamReader(@"FutureSchedule\" + ss[0] + "_period.txt", false))
                        {
                            while (!srperiod.EndOfStream)
                            {
                                string speriod = srperiod.ReadLine();
                                if (speriod != "")
                                {
                                    string[] ssperiod = speriod.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
                                    Period p = new Period(ssperiod[0], DateTime.Parse(ssperiod[1]), DateTime.Parse(ssperiod[2]), 
                                        DateTime.Parse(ssperiod[3]), DateTime.Parse(ssperiod[4]));
                                    tk.TaskPeriod.Add(p);
                                }
                            }
                        }
                        FScheduleList.Add(tk);
                    }
                }
            }

        }
        public void InputTodaySchdule()
        {
            using (StreamReader sr = new StreamReader(@"TodaySchedule\Schedulelist.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string s = sr.ReadLine();
                    string[] ss = s.Split(new string[] { "\t" }, StringSplitOptions.None);
                    if (ss[0] != "")
                    {
                        string snote;
                        using (StreamReader srnote = new StreamReader(@"TodaySchedule\" + ss[0] + "_schedule.txt"))
                            snote = srnote.ReadToEnd();
                        Task tk = new Task(ss[0],  double.Parse(ss[1]), DateTime.Parse(ss[2]), snote);
                        TScheduleList.Add(tk);
                    }

                }
            }
        }


        #endregion


        #region Future Schedule Zone

        // TaskList_ADD 
        private void button1_Click(object sender, EventArgs e)
        {
            AddFutureSchedule();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddFutureSchedule();
        }

        // TaskList_Edit
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count)
            {
                FSForm = new FScheduleForm(ObjTask1);
                FSForm.Show();
            }
        }
        private void eDITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count)
            {
                FSForm = new FScheduleForm(ObjTask1);
                FSForm.Show();
            }
        }

        // TaskList_Delete
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count)
            {
                FScheduleList.Remove(ObjTask1);
            }
            Refresh1();

        }
        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count)
            {
                FScheduleList.Remove(ObjTask1);
            }
            Refresh1();
        }

        // Save
        private void button7_Click(object sender, EventArgs e)
        {
            Save();
        }


        // datagridview click
        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count)
            {
                FSForm = new FScheduleForm(ObjTask1);
                FSForm.Show();
            }
            else
                AddFutureSchedule();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {
            //   string ss =  dataGridView1.SelectedCells[0].RowIndex;
            string s = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            foreach (var item in FScheduleList)
            {
                if (item.Name.ToString() == s)
                {
                    ObjTask1 = item;
                    break;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                dataGridView1.ContextMenuStrip = null;
                contextMenuStrip1.Show(dataGridView1, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (dataGridView1.SelectedCells[0].RowIndex < dataGridView1.Rows.Count - 1)
                    this.textBox1.Text = ObjTask1.Note;
                else
                    AddFutureSchedule();

            }
            pictureBox1.Refresh();

        }

        #endregion

        #region Today Schedule Zone

        // Schedule_Timing
        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjTask2 != null)
                {
                    TiSettingForm = new TimingSettingForm();
                    TiSettingForm.Show();
                }
            }
            catch
            {

            }
        }
        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            TiSettingForm = new TimingSettingForm();
            TiSettingForm.Show();
        }


        // Schedule_Add
        private void button3_Click_1(object sender, EventArgs e)
        {
            AddTodaySchedule(0);
            Refresh1();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AddTodaySchedule(0);
            Refresh1();
        }


        // ScheduleList_Edit
        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count)
            {
                TSForm = new TScheduleForm(ObjTask2);
                TSForm.Show();
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count)
            {
                TSForm = new TScheduleForm(ObjTask2);
                TSForm.Show();
            }


        }


        // ScheduleList_Delete
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count)
            {
                TScheduleList.Remove(ObjTask2);
            }
            Refresh1();

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

            if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count)
            {
                TScheduleList.Remove(ObjTask2);
            }
            Refresh1();
        }


        // datagridview2 click
        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            //   string ss =  dataGridView1.SelectedCells[0].RowIndex;
            string s = dataGridView2.Rows[dataGridView2.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            foreach (var item in TScheduleList)
            {
                if (item.Name.ToString() == s)
                {
                    ObjTask2 = item;
                    break;
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                dataGridView2.ContextMenuStrip = null;
                contextMenuStrip2.Show(dataGridView2, new Point(e.X, e.Y));
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count - 1)
                    this.textBox2.Text = ObjTask2.Note;
                else
                    AddTodaySchedule(0);
            }


        }
        #endregion


        #region Others
        private void Form1_Load(object sender, EventArgs e)
        {
            InputFutureSchedule();
            InputTodaySchdule();
            ShowFutureSchedule();
            ShowTodaySchedule();
            timer1.Start();
         
        }
        //双击托盘方法
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.Show();
            WindowState = FormWindowState.Normal;
            this.Focus();
        }

        //窗体变化方法
        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)    //最小化到系统托盘
            {
                this.Hide();    //隐藏窗口
                notifyIcon1.Visible = true;    //显示托盘图标

            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            TaskShow();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult ans = MessageBox.Show("Would you like to save before exit?", "Wanna to leave?", MessageBoxButtons.YesNoCancel);
            if ( ans == DialogResult.Yes)
            {
                Save();
                //Application.Exit();
            }
            else if (ans == DialogResult.No)
            {

            }
            else
            {
                e.Cancel = true;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            Save();
        }
        #endregion


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (ObjTask1 == null || ObjTask1.TaskPeriod == null)
                return;


            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;



            int xAxisHeight = 30, yAxisWidth = 100, dayWidth = 40, periodHeight = 20;



             // 确定size
            DateTime dt = ObjTask1.TaskPeriod[0].EStartTime;
            DateTime enddt =  DateTime.Now;
            if (enddt < ObjTask1.TaskPeriod[ObjTask1.TaskPeriod.Count - 1].AEndTime)
                enddt = ObjTask1.TaskPeriod[ObjTask1.TaskPeriod.Count - 1].AEndTime;
            if (enddt < (ObjTask1.TaskPeriod[ObjTask1.TaskPeriod.Count - 1].EEndTime))
                enddt = ObjTask1.TaskPeriod[ObjTask1.TaskPeriod.Count - 1].EEndTime;

            //picture size 
            TimeSpan ts0 = enddt - ObjTask1.TaskPeriod[0].EStartTime;
            pictureBox1.Size = new Size(ts0.Days * dayWidth + yAxisWidth + 100, ObjTask1.TaskPeriod.Count * (periodHeight * 2 + 5) + xAxisHeight);

            // draw days
            while (dt <=enddt)
            {
                TimeSpan ts = dt - ObjTask1.TaskPeriod[0].EStartTime;
                g.DrawString((dt.Month + "-" + dt.Day + " | ").PadRight(20), new Font("微软雅黑", 10, FontStyle.Regular),
                  Brushes.Black, new Point(ts.Days * dayWidth + yAxisWidth+3, 0));
                dt = dt.AddDays(1);
            }



            for (int i = 0; i < ObjTask1.TaskPeriod.Count; i++)
            {
                Period p = ObjTask1.TaskPeriod[i];

                // draw period name 
                g.DrawString(p.Name.PadRight(10), new Font("微软雅黑", 10, FontStyle.Regular),
                Brushes.Black, new Point(0, i * periodHeight*2 + xAxisHeight));


                // draw 预期任务 主体
                TimeSpan ts1, ts2;
                ts1 = p.EStartTime - ObjTask1.TaskPeriod[0].EStartTime;
                ts2 = p.EEndTime - p.EStartTime;
                g.FillRectangle(Brushes.DeepSkyBlue, yAxisWidth + ts1.Days * dayWidth, xAxisHeight + periodHeight * i * 2, (ts2.Days + 1) * dayWidth, periodHeight);

                // draw 完成情况 主体 
                // 我们只绘制实际完成时间和实际开始时间之间的矩形
                TimeSpan ts3 = TimeSpan.Zero, ts4 = TimeSpan.Zero;
                if (p.AStartTime.ToShortDateString() == "0001/1/1")
                {

                }
                else if (p.AEndTime.ToShortDateString() == "0001/1/1")
                {
                    ts3 = p.AStartTime - ObjTask1.TaskPeriod[0].EStartTime;
                    ts4 = DateTime.Now - p.AStartTime;
                    g.FillRectangle(Brushes.Orange, yAxisWidth + ts3.Days * dayWidth, xAxisHeight + periodHeight * (i * 2 + 1), (ts4.Days + 1) * dayWidth, periodHeight);

                }
                else
                {
                    ts3 = p.AStartTime - ObjTask1.TaskPeriod[0].EStartTime;
                    ts4 = p.AEndTime - p.AStartTime;
                    g.FillRectangle(Brushes.Orange, yAxisWidth + ts3.Days * dayWidth, xAxisHeight + periodHeight * (i * 2 + 1), (ts4.Days + 1) * dayWidth, periodHeight);

                }
             
            }
        }


        private void textBox1_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            Save();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var item in TScheduleList)
            {
                if (DateTime.Now.Hour == item.AlarmTime.Hour && DateTime.Now.Minute == item.AlarmTime.Minute && DateTime.Now.Second==0)
                {
                    MessageBox.Show(item.Name+"\t"+item.AlarmTime);
   
                }
                
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AddTodaySchedule(0);
            Refresh1();
        }



        private void cLOSEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }





        private void Form1_Deactivate(object sender, EventArgs e)
        {
            Save();
        }

        private void dataGridView2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (dataGridView2.SelectedCells[0].RowIndex < dataGridView2.Rows.Count)
            {
                TSForm = new TScheduleForm(ObjTask2);
                TSForm.Show();
            }
            else
                AddTodaySchedule(0);
        }






        #region closing
        //private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        //{
        //    //注意判断关闭事件Reason来源于窗体按钮，否则用菜单退出时无法退出!
        //    if (e.CloseReason == CloseReason.UserClosing)
        //    {
        //        e.Cancel = true;    //取消"关闭窗口"事件
        //        this.WindowState = FormWindowState.Minimized;    //使关闭时窗口向右下角缩小的效果
        //        notifyIcon1.Visible = true;
        //        this.Hide();
        //        return;
        //    }
        //}
        #endregion

    }
}
