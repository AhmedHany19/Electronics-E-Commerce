using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class LogProduct
	{
		public Guid Id { get; set; }
		public string? Action { get; set; }
		public DateTime Date { get; set; }
		public Guid UserId { get; set; }

		public Guid ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product? Product { get; set; }
	}
}
