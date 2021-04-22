using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Models
{
    public class Account_summaryModel
    {
        public Guid Acc_sum_ID { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string department { get; set; }
        public string Account { get; set; }
        public string Pwdcheck { get; set; }
        public DateTime datetime { get; set; }
    }
}
