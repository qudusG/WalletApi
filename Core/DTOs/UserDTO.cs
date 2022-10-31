﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UserDTO
    {
        public string FirstName { get;set;}
        public string LastName { get;set;}
        public string Email { get;set;}
        public string Password { get;set;}  
        public WalletType WalletType { get; set; }
    }
}
