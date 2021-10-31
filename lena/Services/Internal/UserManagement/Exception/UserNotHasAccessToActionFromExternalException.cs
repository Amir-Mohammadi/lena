using lena.Services.Core.Foundation;
using lena.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement.Exception
{
  public class UserNotHasAccessToActionFromExternalException : InternalServiceException
  {
    public int UserId { get; set; }
    public string ActionName { get; set; }
    public string ActionDisplayName { get; set; }

    public UserNotHasAccessToActionFromExternalException(int userId, string actionName, string actionDisplayName)
    {
      UserId = userId;
      ActionName = actionName;
      ActionDisplayName = actionDisplayName;
    }
  }
}
