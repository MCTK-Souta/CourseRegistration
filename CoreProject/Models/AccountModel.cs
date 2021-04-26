using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreProject.Models
{
    public class AccountModel
    {
        public Guid Acc_sum_ID { get; set; }
        public string Account { get; set; }
        public string password { get; set; }
        public bool Type { get; set; }
    }
}
