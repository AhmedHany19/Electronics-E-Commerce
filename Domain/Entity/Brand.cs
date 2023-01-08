using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Brand : BaseId
	{
		[Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "BrandName")]
		[MaxLength(20, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
		[MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int CurrentState { get; set; }



		// Navigation Properties
		public Guid CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category? Category { get; set; }


		public ICollection<Product>? Products { get; set; }
		public ICollection<LogBrand>? LogBrands { get; set; }

	}
}
