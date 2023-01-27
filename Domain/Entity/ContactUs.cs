using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class ContactUs
	{
		public int Id { get; set; }
		[Required]
		public string? Name { get; set; }
		[Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }
		[Required]
		[MinLength(6),MaxLength(500)]
		public string? Message { get; set; }
	}
}
