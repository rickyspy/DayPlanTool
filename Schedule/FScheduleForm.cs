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
    public partial class FScheduleForm : Form
    {
        public FScheduleForm()
        {
            InitializeComponent();
            Tasktype = 0;
        }
        public FScheduleForm(Task tk)
            : this()
        {
            TaskHere = tk;
            Tasktype = 1;
        }

        int Tasktype; // 0, ADD TASK; 1, EDIT TASK 
        Task TaskHere; // 表示本类中的目标task
        bool TaskOK; // 确认task设置完备的bool变量

        private void TaskForm_Load(object sender, EventArgs e)// 导入taskform 时 需要设置的东西
        {
            InitialDataGridView();
            if (Tasktype == 1)
                LoadParametersToControls();
        }

        private void button1_Click(object sender, EventArgs e) // 确认 TASK
        {
            TaskOK = true;
            if (textBox1.Text == "")
            {
                MessageBox.Show("No name Task！Please re-enter!");
            }
            else
            {
                if (Tasktype == 0)
                {
                    TaskHere = new Task();
                    SaveToTaskList();
                    Form1.FScheduleList.Add(TaskHere);
                }
                else if (Tasktype == 1)
                {
                    SaveToTaskList();
                }

                if (TaskOK)
                    this.Dispose();
            }


        }

        #region 具体方法
        DataTable dt; // 用于在datagridview中显示
        private void InitialDataGridView() // 初始化datagridview 
        {
            // datatable 
            dt = new DataTable();
            dt.Columns.Add("Period", typeof(string));
            dt.Columns.Add("EStartDate", typeof(DateTime));
            dt.Columns.Add("EEndDate", typeof(DateTime));
            dt.Columns.Add("AStartDate", typeof(DateTime));
            dt.Columns.Add("AEndDate", typeof(DateTime));

            // 加第一组默认值
            DataRow dr = dt.NewRow();
            dr[1] = DateTime.Now.ToShortDateString();
            dr[2] = DateTime.Now.ToShortDateString();
            //  dr[3] = DateTime.Now.ToShortDateString();
            //dr[4] = 
            dt.Rows.Add(dr);

            // 用datagridview 来表示这组dataset 
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 4;
            dataGridView1.Columns[1].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
            dataGridView1.Columns[2].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
            dataGridView1.Columns[3].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
            dataGridView1.Columns[4].Width = (dataGridView1.Width - dataGridView1.RowHeadersWidth) / 3;
        }
        private void LoadParametersToControls() // 若为 Task Edit 模式，则将原task中的参数设置到form中的各个控件上
        {
            textBox1.Text = TaskHere.Name;
            NoteBox.Text = TaskHere.Note;

            if (TaskHere.EmergencyLevel == 1)
                radioButton1.Checked = true;
            else if (TaskHere.EmergencyLevel == 2)
                radioButton2.Checked = true;
            else if (TaskHere.EmergencyLevel == 3)
                radioButton3.Checked = true;
            else if (TaskHere.EmergencyLevel == 4)
                radioButton4.Checked = true;
            else if (TaskHere.EmergencyLevel == 5)
                radioButton5.Checked = true;

            if (TaskHere.Importance == 1)
                radioButton6.Checked = true;
            else if (TaskHere.Importance == 2)
                radioButton7.Checked = true;
            else if (TaskHere.Importance == 3)
                radioButton8.Checked = true;
            else if (TaskHere.Importance == 4)
                radioButton9.Checked = true;
            else if (TaskHere.Importance == 5)
                radioButton10.Checked = true;


            if (Tasktype == 1)
            {
                if (dt != null)
                    dt.Rows[0].Delete();
                for (int i = 0; i < TaskHere.TaskPeriod.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = TaskHere.TaskPeriod[i].Name;
                    dr[1] = TaskHere.TaskPeriod[i].EStartTime;
                    dr[2] = TaskHere.TaskPeriod[i].EEndTime;
                    dr[3] = TaskHere.TaskPeriod[i].AStartTime;
                    dr[4] = TaskHere.TaskPeriod[i].AEndTime;

                    dt.Rows.Add(dr);
                }
            }


        }
        private void SaveToTaskList() // 保存List至 ----- TaskList
        {
            // 
            TaskHere.Name = textBox1.Text;
            TaskHere.Note = NoteBox.Text;

            // emergency level 
            if (radioButton1.Checked == true)
                TaskHere.EmergencyLevel = 1;
            else if (radioButton2.Checked == true)
                TaskHere.EmergencyLevel = 2;
            else if (radioButton3.Checked == true)
                TaskHere.EmergencyLevel = 3;
            else if (radioButton4.Checked == true)
                TaskHere.EmergencyLevel = 4;
            else if (radioButton5.Checked == true)
                TaskHere.EmergencyLevel = 5;

            //importance
            if (radioButton6.Checked == true)
                TaskHere.Importance = 1;
            else if (radioButton7.Checked == true)
                TaskHere.Importance = 2;
            else if (radioButton8.Checked == true)
                TaskHere.Importance = 3;
            else if (radioButton9.Checked == true)
                TaskHere.Importance = 4;
            else if (radioButton10.Checked == true)
                TaskHere.Importance = 5;

            // task period 
            TaskHere.TaskPeriod = new List<Period>();
            for (int i = 0; i < dataGridView1.RowCount - 1; i++)
            {
                //try
                //{
                    Period p = new Period();
                    p.Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    if (p.Name == "")
                        p.Name = TaskHere.Name;
                    p.EStartTime = DateTime.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    p.EEndTime = DateTime.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[3].Value.ToString() != "")
                        p.AStartTime = DateTime.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    if (dataGridView1.Rows[i].Cells[4].Value.ToString() != "")
                        p.AEndTime = DateTime.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString());
                    TaskHere.TaskPeriod.Add(p);
                //}
                //catch
                //{
                //    MessageBox.Show("Period is wrong!");
                //    TaskOK = false;
                //    break;
                //}

                //p.Name = PeriodBox1.Text.ToString();
                //System.IFormatProvider format = new System.Globalization.CultureInfo("zh-cn", true); // 声明datetime的标准 画蛇一波
                //p.StartTime = DateTime.Parse(StartPeriod1.Text + " 0:00:00", format);
                //p.EndTime = DateTime.Parse(EndPeriod1.Text );
            }


        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)// datagridview 自动设置为当前日
        {
            //   string ss = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (e.ColumnIndex > 0 && (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "" ||
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "0001/1/1 0:00:00"))
            {
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = DateTime.Now.ToShortDateString();
            }
        }

        #endregion




    }
}
