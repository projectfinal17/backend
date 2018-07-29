﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class CodeBaseEntity : BaseEntity
    {
        [Required]
        public string Code { get; set; }

    }
}
