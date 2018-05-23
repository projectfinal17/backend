using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class ProductForCreationDto 
    {
   
        [Required (ErrorMessage ="You must provide Name")]
        public string Name { get; set; }

        public Guid ProductCategoryId { get; set; }
    }
}
