using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DNIT.Core.Interface
{
  public interface IJwtService
    {
    string GenerateAccessToken(string userId);

    string GenerateRefreshToken();
  }
}
