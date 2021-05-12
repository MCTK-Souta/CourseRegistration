using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Models
{
    public class StudentCourseTimeModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public string ClassName { get; set; }

        public StudentCourseTimeModel(DateTime startdate, DateTime enddate, string classname)
        {
            StartDate = startdate;
            EndDate = enddate;
            ClassName = classname;
            DayOfWeek = startdate.DayOfWeek;
        }

        public bool Check(DateTime date)
        {
            if (date.DayOfWeek != DayOfWeek)
                return false;
            if (date >= StartDate && date <= EndDate)
                return true;
            return false;
        }
    }
}
