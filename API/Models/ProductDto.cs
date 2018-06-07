using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ProductDto : BaseDto
    {
        [Required(ErrorMessage = "You must provide Code")]
        public string Code { get; set; }

        [Required (ErrorMessage ="You must provide Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide ImageUrlList")]
        public string ImageUrlList { get; set; }

        [Required(ErrorMessage = "You must provide SalePrice")]
        public double SalePrice { get; set; }

        [Required(ErrorMessage = "You must provide WholeSalePrice")]
        public double WholeSalePrice { get; set; }

        [Required(ErrorMessage = "You must provide Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must provide CreatedDate")]
        public DateTime CreatedDate { get; set; }

        public bool IsActive { get; set; }

        public Guid ProductCategoryId { get; set; }

        public ProductCategoryDto ProductCategory { get; set; }

    }
}
