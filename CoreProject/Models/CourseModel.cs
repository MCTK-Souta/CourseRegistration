using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Models
{
    public class CourseModel
    {
        public string Course_ID { get; set; }
        public int Teacher_ID { get; set; }
        public string C_Name { get; set; }
        public int MaxNumEnrolled { get; set; }
        public int MinNumEnrolled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public int Place_ID { get; set; }
        public int Price { get; set; }
        public string CourseIntroduction { get; set; }
        public Guid b_empno { get; set; }
        public DateTime b_date { get; set; }
        public Guid? e_empno { get; set; }
        public DateTime? e_date { get; set; }
        public Guid? d_empno { get; set; }
        public DateTime? d_date { get; set; }

    }
}
