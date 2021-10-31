using lena.Services.Core.Foundation;
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
  class PermissionRequestActionIsConfirmedException : InternalServiceException
  {
    public int? Id { get; set; }
    public PermissionRequestActionIsConfirmedException(int id)
    {
      this.Id = id;
    }
  }
}