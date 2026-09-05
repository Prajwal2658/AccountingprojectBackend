using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Application.Authentication
{
    public interface IJwtTokenService
    {
        string GenerateToken(
            int userId,
            string username,
            IEnumerable<string> roles);
    }
}
