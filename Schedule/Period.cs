using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedule
{
    public class Period
    {
        public Period()
        {
 
        }

        public Period(string name, DateTime estarttime,DateTime eendtime)
        {
            this.Name = name;
            this.EStartTime = estarttime;
            this.EEndTime = eendtime;
        }

        public Period(string name, DateTime estarttime, DateTime eendtime, DateTime astarttime, DateTime aendtime)
            :this(name,estarttime,eendtime)
        {
            this.AStartTime = astarttime;
            this.AEndTime = aendtime;
        }

        // 属性 
        public string Name;
        public DateTime EStartTime;
        public DateTime EEndTime;
        public DateTime AStartTime;
        public DateTime AEndTime;
    }
}
