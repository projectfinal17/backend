using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "You must provide Name")]
        public string Name { get; set; }
    }
}