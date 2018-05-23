using API.Entities;
using API.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class ProductCategoryDto : BaseDto
    {
        [Sortable]
        [Filterable]
        [Required(ErrorMessage = "You must provide Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must provide Code")]
        public string Code { get; set; }

        ICollection<ProductEntity> Products { get; set; }

    }
}
