﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dnc.API.Models
{
    public class UserInputVm
    {
        public string UName { get; set; }
        public string Pwd { get; set; }
    }
}