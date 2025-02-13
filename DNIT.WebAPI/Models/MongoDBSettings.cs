using MongoDB.Driver;

namespace DNIT.WebAPI.Models
{
  public class MongoDBSettings
  {
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
  }
}
