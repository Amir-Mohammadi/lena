using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.UserManagement.User;

namespace lena.Services.Core.Provider.Security
{
  public class SecurityProvider : Provider
  {

    public virtual LoginResult CurrentLoginData => App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString());
  }
}
