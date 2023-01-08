using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Domain.Entity
{
    public class OrderHeader
    {

        public int Id { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }

        
        [NotMapped]
        public DateTime PickUpDate { get; set; }
        [Required]
		public string? City { get; set; }
        [Required]
		public string? Address { get; set; }
        [Required]
		public string? Country { get; set; }
		public string? Email { get; set; }
        [Required]
		public string? PostalCode { get; set; }

		public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Comments { get; set; }

        [Display(Name = "Pickup Name")]
        public string? PickUpName { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? TransactionId { get; set; }


		[Required]
		public string? UserId { get; set; }
		[ForeignKey("UserId")]
		public virtual ApplicationUser? ApplicationUser { get; set; }
	}
}
