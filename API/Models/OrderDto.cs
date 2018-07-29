using API.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class OrderDto : CodeBaseDto
    {

        [Required(ErrorMessage = "You must provide TotalMoney")]
        public double TotalMoney { get; set; }

        [Required(ErrorMessage = "You must provide CreatedDate")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "You must provide Address")]
        public string Address { get; set; }
        
        public List<OrderItemDto> OrderItems { get; set; }

        public Guid UserId { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public bool isDeleted { get; set; }

    }
}
