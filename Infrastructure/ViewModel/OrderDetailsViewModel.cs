using Domain.Entity;

namespace Infrastructure.ViewModel
{
    public class OrderDetailsViewModel
    {
        public OrderHeader? OrderHeader { get; set; }
        public List<OrderDetail>? OrderDetails { get; set; }

    }
}
