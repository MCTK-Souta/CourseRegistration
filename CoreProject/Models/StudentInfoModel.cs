using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Models
{
    public class StudentInfoModel
    {
        public Guid Student_ID { get; set; }
        public string S_FirstName { get; set; }
        public string S_LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string Idn { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string Education { get; set; }
        public string School_ID { get; set; }
        public string Experience { get; set; }
        public string ExYear { get; set; }
        public string gender { get; set; }
        public string b_empno { get; set; }
        public DateTime b_date { get; set; }
        public string e_empno { get; set; }
        public DateTime e_date { get; set; }
        public string d_empno { get; set; }
        public DateTime d_date { get; set; }

    }
}
