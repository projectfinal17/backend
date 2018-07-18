using API.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class PostForCreationDto : BaseDto
    {
        [Required(ErrorMessage = "You must provide Code")]
        public string Code { get; set; }

        [Required(ErrorMessage = "You must provide Tittle")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "You must provide CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "You must provide Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "You must provide Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "You must provide Image")]
        public string Image { get; set; }
    }
}
