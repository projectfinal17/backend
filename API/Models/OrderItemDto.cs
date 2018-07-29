using API.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class OrderItemDto : BaseDto
    {
        public Guid ProductId { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public double SalePrice { get; set; }

        public int Amount { get; set; }

        public double TotalMoney { get; set; }

        public Guid OrderId { get; set; }

    }
}
