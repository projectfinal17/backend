using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ProductForCreationDto
    {
        [Required(ErrorMessage = "You must provide Code")]
        public string Code { get; set; }

        [Required (ErrorMessage ="You must provide Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide ImageUrlList")]
        public string ImageUrlList { get; set; }

        [Required(ErrorMessage = "You must provide SalePrice")]
        public double SalePrice { get; set; }

        [Required(ErrorMessage = "You must provide Description")]
        public string Description { get; set; }

        public Guid ProductCategoryId { get; set; }

    }
}
