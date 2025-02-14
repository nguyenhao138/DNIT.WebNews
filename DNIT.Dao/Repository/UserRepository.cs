using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNIT.Core.Models;
using MongoDB.Driver;

namespace DNIT.Dao.Repository
{
  public class UserRepository
  {
    private readonly IMongoCollection<AuthModel> _users;

    public UserRepository(IMongoDatabase database)
    {
      _users = database.GetCollection<AuthModel>("Users");
    }

    public async Task<AuthModel?> FindByUsernameAsync(string username)
    {
      return await _users.Find(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiryTime)
    {
      var update = Builders<AuthModel>.Update
          .Set(u => u.RefreshToken, refreshToken)
          .Set(u => u.RefreshTokenExpiryTime, expiryTime);
      var result = await _users.UpdateOneAsync(u => u.Id == userId, update);
      return result.ModifiedCount > 0;
    }
  }

}
