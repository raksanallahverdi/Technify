﻿namespace Technify.Areas.Admin.Models.User
{
    public class UserVM
    {
        public string Id { get; set; }  
        public string Email  { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Roles { get; set; }
    }
}
