using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNIT.Core.Models;
using DNIT.Core.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DNIT.Dao.Repository
{
  public class UserRepository
  {
    private readonly IMongoCollection<AccountModel> _account;

    private readonly JwtService _jwtService;
    private readonly IPasswordHasher<AccountModel> _passwordHasher;
    public UserRepository(IMongoDatabase database, JwtService jwtService, IPasswordHasher<AccountModel> passwordHasher)
    {
      _account = database.GetCollection<AccountModel>("Account");
      _jwtService = jwtService;
      _passwordHasher = passwordHasher;
    }

    public async Task<AccountModel?> GetByUsername(string username)
    {
      return await _account.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateRefreshToken(string userId, string refreshToken, DateTime expiryTime)
    {
      var update = Builders<AccountModel>.Update
          .Set(u => u.RefreshToken, refreshToken)
          .Set(u => u.RefreshTokenExpiryTime, expiryTime);
      var result = await _account.UpdateOneAsync(u => u.Id == userId, update);
      return result.ModifiedCount > 0;
    }
    public async Task<AccountModel> Login([FromBody] AccountModel model)
    {
      var user = await GetByUsername(model.Username);
      if (user == null)
        return null;

      var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.PasswordHash);
      if (passwordVerificationResult == PasswordVerificationResult.Failed)
        return null;

      var accessToken = _jwtService.GenerateAccessToken(user.Id);
      var refreshToken = _jwtService.GenerateRefreshToken();
      var RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15);
      await UpdateRefreshToken(user.Id, refreshToken, RefreshTokenExpiryTime);
      return user;
    }


    //[HttpPost("refresh-token")]
    //public async Task<IActionResult> RefreshToken([FromBody] AccountModel model)
    //{
    //  var user = await _userRepository.GetByUsername(model.Username);

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
