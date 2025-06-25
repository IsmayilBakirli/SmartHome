using SmartHome.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Application.Common
{
    public class AuthResult
    {
        public string? Token { get; set; }
        public Object? User { get; set; }
        public AuthResult(string token, Object user)
        {
            Token = token;
            User = user;
        }
    }
}
