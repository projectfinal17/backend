using API.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class OrderForCreationDto : BaseDto
    {
        [Required(ErrorMessage = "You must provide Address")]
        public string Address { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
