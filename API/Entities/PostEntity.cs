using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class PostEntity : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string Tittle { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Image { get; set; }
    }
}