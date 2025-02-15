
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DNIT.Core.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace DNIT.Core.Service
{
  public class JwtService: IJwtService
  {
    private readonly IConfiguration _config;
    //private readonly IAuthService _authService;
    public JwtService(IConfiguration config, IAuthService authService)
    {
      _config = config;
      //_authService = authService;
    }

    public string GenerateAccessToken(string userId)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          issuer: _config["Jwt:Issuer"],
          audience: _config["Jwt:Audience"],
          claims: new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) },
          expires: DateTime.UtcNow.AddMinutes(30),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);

    }

    public string GenerateRefreshToken()
    {
      return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }


    //public async Task<IActionResult> RefreshToken([FromBody] AccountModel model)
    //{
    //  var user = await _authService.GetByUsername(model.Username);

    //  if (user == null || user.RefreshToken != model.RefreshToken)
    //  {
    //    return Unauthorized();
    //  }

    //  // Làm mới Access Token
    //  var newAccessToken = _jwtService.GenerateAccessToken(user.Id);
    //  var newRefreshToken = _jwtService.GenerateRefreshToken();

    //  await _userRepository.UpdateRefreshToken(user.Id, newRefreshToken, DateTime.UtcNow.AddDays(7));

    //  return Ok(new
    //  {
    //    AccessToken = newAccessToken,
    //    RefreshToken = newRefreshToken
    //  });
    //}
  }

}
