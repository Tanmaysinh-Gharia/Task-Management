﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.ViewModels.UserManagement
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
