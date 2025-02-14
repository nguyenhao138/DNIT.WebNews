using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using DNIT.Core.Models;

namespace DNIT.Core.Service
{
  public class AuthService
  {
    private readonly IMongoCollection<AuthModel> _authCollection;

    public AuthService(IMongoDatabase database)
    {
      _authCollection= database.GetCollection<AuthModel>("Account");
    }

    public List<AuthModel> Get() => _authCollection.Find(auth => true).ToList();

    public AuthModel Get(string id) => _authCollection.Find(auth => auth.Id == id).FirstOrDefault();

    public void Create(AuthModel authModel) => _authCollection.InsertOne(authModel);

    public void Update(string id, AuthModel authModelIn) =>
        _authCollection.ReplaceOne(auth => auth.Id == id, authModelIn);

    public void Remove(string id) =>
        _authCollection.DeleteOne(auth => auth.Id == id);
  }

}

