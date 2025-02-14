using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using DNIT.Core.Models;

namespace DNIT.WebAPI.Data
{
  public class MongoUserManager
  {
    private readonly IMongoCollection<AuthModel> _accountsCollection;
    private readonly IPasswordHasher<AuthModel> _passwordHasher;

    // Constructor với IMongoCollection và IPasswordHasher
    public MongoUserManager(IMongoDatabase database, IPasswordHasher<AuthModel> passwordHasher)
    {
      _accountsCollection = database.GetCollection<AuthModel>("Accounts");
      _passwordHasher = passwordHasher;
    }

    // Tạo tài khoản mới
    public async Task<IdentityResult> CreateAsync(AuthModel user, string password)
    {
      var existingUser = await _accountsCollection
          .Find(x => x.Username == user.Username)
          .FirstOrDefaultAsync();

      if (existingUser != null)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Description = "Tài khoản đã tồn tại"
        });
      }

      // Mã hóa mật khẩu và lưu tài khoản vào MongoDB
      user.Password = _passwordHasher.HashPassword(user, password);
      await _accountsCollection.InsertOneAsync(user);
      return IdentityResult.Success;
    }

    // Tìm tài khoản theo tên người dùng
    public async Task<AuthModel> FindByNameAsync(string username)
    {
      var user = await _accountsCollection
          .Find(x => x.Username == username)
          .FirstOrDefaultAsync();
      return user;
    }

    // Kiểm tra mật khẩu
    public async Task<bool> CheckPasswordAsync(AuthModel user, string password)
    {
      return _passwordHasher.VerifyHashedPassword(user, user.Password, password) == PasswordVerificationResult.Success;
    }

    // Cập nhật thông tin tài khoản
    public async Task<IdentityResult> UpdateAsync(AuthModel user)
    {
      var result = await _accountsCollection.ReplaceOneAsync(u => u.Id == user.Id, user);
      if (result.ModifiedCount == 0)
      {
        return IdentityResult.Failed(new IdentityError
        {
          Description = "Cập nhật tài khoản thất bại"
        });
      }
      return IdentityResult.Success;
    }
  }
}
