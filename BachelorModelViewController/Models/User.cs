﻿using BachelorModelViewController.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BachelorModelViewController.Models
{
    public class User
    {
        private Guid Token { get; set; }
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Dictionary<Guid,int> Organization_AuthenticationLevel { get; set; }
        private Organization OrganizationsWithAdminRights { get; set; }


    }
}