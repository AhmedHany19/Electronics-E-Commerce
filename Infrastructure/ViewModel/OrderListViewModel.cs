using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ViewModel
{
    public class OrderListViewModel
    {
        public IList<OrderDetailsViewModel>? Orders { get; set; }

    }
}
