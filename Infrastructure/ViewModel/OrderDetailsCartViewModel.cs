using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
	public class OrderDetailsCartViewModel
	{
		public List<ShoppingCart>? ShoppingCarts { get; set; }
		public OrderHeader? OrderHeader { get; set; }
	}
}
