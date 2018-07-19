using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class ProductEntity : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        public Guid ProductCategoryId { get; set; }

        [Required]
        public string ImageUrlList { get; set; }

        [Required]
        public double SalePrice { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public ProductCategoryEntity ProductCategory { get; set; }

    }
}