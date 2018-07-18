using API.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class PostDto : BaseDto
    {
        [Sortable]
        [Filterable]
        [Required(ErrorMessage ="You must provide Code")]
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
