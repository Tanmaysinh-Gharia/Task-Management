using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.ViewModels.Login
{
    public class AuthenticationResponse
    {
        
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

