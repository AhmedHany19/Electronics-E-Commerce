using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
	public class SliderViewModel
	{
		public List<Slider>? Sliders { get; set; }
		public Slider? NewSlider { get; set; }
	}
}
