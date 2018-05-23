﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class DemoEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        public string Type { get; set; }

    }
}