using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Core.ViewModels.UserManagement;

namespace TaskManagement.Core.ViewModels.Login
{
        public class RefreshTokenModel
        {

            public Guid RefreshTokenId { get; set; }

            public string Token { get; set; }

            public DateTime Expires { get; set; }

            public bool IsRevoked { get; set; }

            public int UserId { get; set; }

            public string CreatedFromIp { get; set; }
            
            public UserModel User { get; set; }
        }
    
}
