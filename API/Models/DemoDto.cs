using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class DemoDto : BaseDto
    {
        [Sortable]
        [Filterable]
        [Required(ErrorMessage ="You must provide Name")]
        public string Name { get; set; }

        public string Type { get; set; }

    }
}
