using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Models
{
    public class COMMENTS
    {
        public int Comment_ID { get; set; }
        public int UserID { get; set; }
        public int Post_ID { get; set; }
        public string Comment { get; set; }
        public DateTime Comment_date_time { get; set; }

    }
}
