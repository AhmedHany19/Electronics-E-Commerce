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
	public class Product : BaseId
	{
		[Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "Productname")]
		[MaxLength(500, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MaxLength")]
		[MinLength(3, ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "MinLength")]
		public string? Name { get; set; }

		[Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "PriceValue")]
		[Column(TypeName = "decimal(18,2)")]
		public double Price { get; set; }
		[Required(ErrorMessageResourceType = typeof(Resource.ResourceData), ErrorMessageResourceName = "DiscountValue")]
		[Column(TypeName = "decimal(18,2)")]
		public int? Discount { get; set; }
		public string? ImageUrl { get; set; }

		[Display(Name = "Product Color")]
		public string? ProductColor { get; set; }

		public string? Description { get; set; }

		[Required]
		[Display(Name = "Available")]
		public bool IsAvailable { get; set; }

		public DateTime UpdateDate { get; set; }
		public int CurrentState { get; set; }



		//Properties Navgition


		// ProductType
		[Display(Name = "Product Type")]
		public Guid CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public virtual Category? Category { get; set; }

		// Brand

		[Display(Name = "Brand")]
		public Guid BrandId { get; set; }
		[ForeignKey("BrandId")]
		public virtual Brand? Brand { get; set; }
	}
}
