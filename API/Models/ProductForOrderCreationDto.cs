using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ProductForOrderCreationDto : BaseDto
    {
        public string Name { get; set; }

        public double SalePrice { get; set; }

        public int Amount { get; set; }

        public string Description { get; set; }

        public string ProductCategoryId { get; set; }
    }
}
