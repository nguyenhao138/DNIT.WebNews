﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;

namespace DNIT.WebAPI.Models
{
  public class AccountModel : IdentityUser
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } 
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}
