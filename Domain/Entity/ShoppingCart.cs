using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ShoppingCart:BaseId
    {
        public ShoppingCart()
        {
            Count = 1;
        }

        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public int Count { get; set; }




		//Navigation Properties
		public string? ApplicationUserId { get; set; }
		[NotMapped]
		[ForeignKey("ApplicationUserId")]
		public virtual ApplicationUser? ApplicationUser { get; set; }

		public Guid ProductId { get; set; }
        [NotMapped]
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
