using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Layers.Admin.Model
{
    public class SystemUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime LastLogin { get; set; }
        public string GUID { get; set; }
        public string ForgotPassword { get; set; }
        public DateTime register { get; set; }
        public DateTime lastupdate { get; set; }
        public DateTime inactive { get; set; }
        public string deleted { get; set; }
    }
}