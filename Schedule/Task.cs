using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public  class Task
    {

        public Task()
        {

        }
                      
        public Task(string name, int emergencylevel, int importance, double expectedtime, DateTime alarmtime, string enddate, string note)
        {
            this.Name = name;
            this.EmergencyLevel = emergencylevel;
            this.Importance = importance;
            this.ExpectedTime = expectedtime;
            //this.StartDate = startdate;
            this.AlarmTime = alarmtime;
            this.ObjectTime = enddate;
            this.Note = note;
        }

        public Task(string name,  double expectedtime,DateTime alarmtime, string note) // ScheduleList 导入用
        {
            this.Name = name;
            //this.State = state;
            this.ExpectedTime = expectedtime;
            this.AlarmTime = alarmtime;
            this.Note = note;
        }

        public Task(string name, int emergencylevel, int importance, string note) // TaskList 导入用
        {
            this.Name = name;
            this.EmergencyLevel = emergencylevel;
            this.Importance = importance;
            this.Note = note;
        }

        // public bool State;
        public string Name=" ";
        public string Note=" ";
        public DateTime AlarmTime;
        public string StartDate=" ";
        public string ObjectTime=" ";
        public int Importance;
        public int EmergencyLevel;
        public double ExpectedTime;
        public string Music;
        public List<Period> TaskPeriod = new List<Period>();
    }
}
