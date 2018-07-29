using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class OrderEntity : CodeBaseEntity
    {

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Address { get; set; }

        public virtual List<OrderItemEntity> OrderItems { get; set; }

        [Required]
        public double TotalMoney { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public virtual UserEntity User { get; set; }

        [Required]
        public bool isDeleted { get; set; }


    }
}