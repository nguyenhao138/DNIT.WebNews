using Microsoft.Extensions.Options;
using DNIT.WebAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;

namespace DNIT.WebAPI.Services
{
  public class AuthService
  {
    private readonly IMongoCollection<AccountModel> _accountModel;

    // Sử dụng IOptions<MongoDBSettings> để inject MongoDBSettings
    public AuthService(IMongoClient mongoClient, IOptions<MongoDBSettings> settings)
    {
      var database = mongoClient.GetDatabase(settings.Value.DatabaseName);
      _accountModel = database.GetCollection<AccountModel>("Account");
    }

    // Phương thức tìm người dùng theo tên người dùng (Username)
    public async Task<AccountModel> FindByUsernameAsync(string username)
    {
      var filter = Builders<AccountModel>.Filter.Eq(u => u.UserName, username);
      return await _accountModel.Find(filter).FirstOrDefaultAsync();
    }


    public List<AccountModel> Get() =>
        _accountModel.Find(account => true).ToList();

    public AccountModel Get(string id) =>
        _accountModel.Find(account => account.Id == id).FirstOrDefault();

    public AccountModel Create(AccountModel account)
    {
      _accountModel.InsertOne(account);
      return account;
    }

    public void Update(string id, AccountModel accountIn) =>
        _accountModel.ReplaceOne(account => account.Id == id, accountIn);

    public void Remove(string id) =>
        _accountModel.DeleteOne(account => account.Id == id);
  }
}
