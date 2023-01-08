using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
	public class Slider:BaseId
	{
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
