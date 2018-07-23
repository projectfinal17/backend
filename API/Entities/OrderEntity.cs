using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class OrderEntity : BaseEntity
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string ProductList { get; set; }

        [Required]
        public double TotalMoney { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public UserEntity User { get; set; }

        public Guid ProductId { get; set; }

        ICollection<ProductEntity> Products { get; set; }


    }
}