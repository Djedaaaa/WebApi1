using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Contracts.v1.Responses
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }

        public string ExpiresIn { get; set; }

        public string RefreshToken { get; set; }
    }
}
