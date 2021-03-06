﻿using Dnc.Seedwork;
using System;

namespace Dnc.Biz.Admin
{
    public class SysUser
        : Entity, IDataStatusBehavior
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
        public DataStatus DataStatus { get; set; }
    }
}
