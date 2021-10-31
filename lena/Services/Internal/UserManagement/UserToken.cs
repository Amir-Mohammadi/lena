using lena.Services.Common;
using lena.Services.Core.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    public void AddUserToken(
        int userId,
        string token,
        string refreshToken,
        DateTime expiresIn)
    {



      var userToken = repository.Create<UserToken>();

      userToken.UserId = userId;
      userToken.Token = token.ComputeHashToken();
      userToken.RefreshToken = refreshToken;
      userToken.ExpiresIn = expiresIn;
      repository.Add(userToken);
    }

    public IQueryable<UserToken> GetUserTokens(
        TValue<int> id = null,
       TValue<int> userId = null,
       TValue<string> token = null)
    {

      var userTokens = repository.GetQuery<UserToken>();
      if (id != null)
        userTokens = userTokens.Where(i => i.Id == id);
      if (userId != null)
        userTokens = userTokens.Where(i => i.UserId == userId);
      if (token != null)
      {
        var t = token.Value.ComputeHashToken();
        userTokens = userTokens.Where(i => i.Token == t);
      }


      return userTokens;
    }
    public UserToken GetUserToken(int userId, string token)
    {

      var userToken = GetUserTokens(
                userId: userId,
                token: token)


            .SingleOrDefault();
      if (userToken == null)
        throw new UserTokenNotFoundException();
      return userToken;
    }


    public void RemoveUserToken(int userId, string token)
    {

      var userToken = GetUserToken(userId: userId, token: token);
      repository.Delete(userToken);
    }

    public void RemoveUserToken(UserToken userToken)
    {

      repository.Delete(userToken);
    }

  }
}
