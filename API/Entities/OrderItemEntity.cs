using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class OrderItemEntity : BaseEntity
    {
        public Guid ProductId { get; set; }

        public int Amount { get; set; }
        
        public double SalePrice { get; set; }

        public double TotalMoney { get; set; }

        public Guid OrderId { get; set; }

        public virtual OrderEntity Order { get; set; }

        public virtual ProductEntity Product { get; set; }

    }
}
