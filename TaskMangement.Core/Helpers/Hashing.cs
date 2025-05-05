using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Core.Helpers
{
    public class Hashing
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyHashedPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
        public bool VerifyPassword(string password, string hashedPassword)
        {
            string hased = HashPassword(password);
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
