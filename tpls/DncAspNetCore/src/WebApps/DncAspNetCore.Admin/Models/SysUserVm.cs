﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DncAspNetCore.Admin.Models
{
    public class SysUserVm
    {
        public string UName { get; set; }
        public string Pwd { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Avatar { get; set; }
        public bool IsLocked { get; set; }
        public string Salt { get; set; }
        public DateTime? AllowLoginTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public int LoginErrNums { get; set; }
        public string LastLoginIP { get; set; }
        public DateTime? LastLogoutTime { get; set; }
    }
}
