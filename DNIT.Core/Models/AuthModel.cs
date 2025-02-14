
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

namespace DNIT.Core.Models
{
  public class AuthModel : IdentityUser
  {
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public override string Id { get; set; }
    public string Username { get; set; }

    [BsonIgnore]
    public string Email { get; set; }

    public string Password { get; set; }

    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiryTime { get; set; }
  }
}
