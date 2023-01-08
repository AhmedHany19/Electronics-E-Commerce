using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class LogBrand
	{
		public Guid Id { get; set; }
		public string? Action { get; set; }
		public DateTime Date { get; set; }
		public Guid UserId { get; set; }

		public Guid BrandId { get; set; }
		[ForeignKey("BrandId")]
		public Brand? Brand { get; set; }
	}
}
