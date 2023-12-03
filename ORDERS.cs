using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Models
{
    public class ORDERS
    {
		public int order_id { get; set; }
		public int post_id { get; set; }
		public int buyer_id { get; set; }
		public int seller_id { get; set; }
		public DateTime selling_date { get; set; }
		public int selling_price { get; set; }
	}
}
