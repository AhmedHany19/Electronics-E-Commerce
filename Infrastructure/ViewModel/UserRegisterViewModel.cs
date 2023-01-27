using Domain.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
	public class UserRegisterViewModel
	{
			public string? Id { get; set; }

			[Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "RegisterName")]
			[MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MaxLength")]
			[MinLength(3, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLength")]
			public string? Name { get; set; }

			[Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "RegisterEmail")]
			[EmailAddress(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "RegisterEmailError")]
			public string? Email { get; set; }

			[Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "PhoneNumber")]
		    [Phone(ErrorMessageResourceType =typeof(ResourceData),ErrorMessageResourceName = "PhoneNumberError")]
			public int? PhoneNumber { get; set; }

			public bool ActiveUser { get; set; }
			[Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "Password")]
			[MaxLength(20, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MaxLength")]
			[MinLength(5, ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "MinLengthPassword")]
			public string? Password { get; set; }
			[Required(ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "ComparePassword")]
			[Compare("Password", ErrorMessageResourceType = typeof(ResourceData), ErrorMessageResourceName = "ComparePassword")]
			public string? ComfirmPassword { get; set; }


		}
	}

