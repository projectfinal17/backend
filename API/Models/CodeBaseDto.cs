using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class CodeBaseDto : BaseDto
    {
        [Required(ErrorMessage = "You must provide Code")]
        [Sortable]
        [Filterable]
        public string Code { get; set; }
    }
}
