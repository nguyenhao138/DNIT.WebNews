using MongoDB.Driver;
using DNIT.Core.Models;
using DNIT.Core.Interface;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DNIT.Core.Service
{
  public class AuthService: IAuthService
  {
    private readonly IMongoCollection<AccountModel> _authCollection;

    public AuthService(IMongoDatabase database)
    { 
      _authCollection= database.GetCollection<AccountModel>("Account");
    }

    public async Task<List<AccountModel>> ListAccount() =>
        await _authCollection.Find(_ => true).ToListAsync();

    public async Task<AccountModel?> GetAccount(string id) =>
        await _authCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAccount(AccountModel newBook) =>
        await _authCollection.InsertOneAsync(newBook);

    public async Task UpdateAccount(string id, AccountModel updatedBook) =>
        await _authCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

    public async Task RemoveAccount(string id) =>
        await _authCollection.DeleteOneAsync(x => x.Id == id);
  }
}

