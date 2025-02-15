using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DNIT.Core.Models;
using MongoDB.Bson;

namespace DNIT.Core.Interface
{
  public interface IAuthService
  {
    Task<List<AccountModel>> ListAccount();

    Task<AccountModel?> GetAccount(string id);

    Task CreateAccount(AccountModel newBook);

    Task UpdateAccount(string id, AccountModel updatedBook);

    Task RemoveAccount(string id);
  }
}
