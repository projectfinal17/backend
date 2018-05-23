﻿using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class UserForResetPasswordDto
    {

        [Required(ErrorMessage = "You should provide a Password value.")]
        public string Password { get; set; }
    }
}
