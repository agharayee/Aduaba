using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aduaba.Authorizations
{
    public class Authorization
    {
        public enum Roles
        {
            Administrator,
            User
        }


        public const string default_username = "admin";
        public const string default_email = "admin@secureapi.com";
        public const string default_password = "Password";
        public const Roles default_role = Roles.User;
    }
}
