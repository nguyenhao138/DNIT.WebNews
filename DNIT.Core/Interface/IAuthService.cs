using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNIT.Core.Models;

namespace DNIT.Core.Interface
{
  public interface IAuthService
  {
    Task<AuthModel?> FindByUsernameAsync(string username);
    Task<string> GenerateAccessToken(string userId);
    Task<string> GenerateRefreshTokenAsync(string userId);
    Task<bool> UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime);
  }
}
