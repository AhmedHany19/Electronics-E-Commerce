using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Category : BaseId
	{
		[Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "CtaegoryName")]
		[MaxLength(20, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
		[MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
		public string? Name { get; set; }
		public string? Description { get; set; }
		public int CurrentState { get; set; }
		public string? ImageUrl { get; set; }



		// Navigation Properties

		public ICollection<Product>? Products { get; set; }
		public ICollection<Brand>? Brands { get; set; }






	}
}
