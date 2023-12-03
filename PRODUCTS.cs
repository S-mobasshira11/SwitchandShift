using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Switch_and_Shift.Models
{
    public class PRODUCTS
    {
		[Key]
		public int Product_ID { get; set; }


		[Required]
	
		public string product_category { get; set; }

		[Required]

		public int product_price { get; set; }

		[Required]
	
		public string product_brand { get; set; }

		[Required]

		public string product_model { get; set; }


		
		public string product_details { get; set; }
		
		public string product_warranty { get; set; }
		
		public string product_usage { get; set; }
	
		
		public string product_condition { get; set; }


		
		public int UserId { get; set; }

		
		[DisplayName("Image Name")]
		public string image_name { get; set; }

		[NotMapped]
		[Required]
		[DisplayName("Upload File")]
		public IFormFile ImageFile { get; set; }


	}

}
