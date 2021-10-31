using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class NotHavePermissionToConfirmOrRejectGeneralStuffRequestException : InternalServiceException
  {
    public int UserId { get; set; }
    public string StuffCode { get; set; }
    public string EmployeeFullName { get; set; }
    public int ValidUserGroupId { get; set; }
    public string ValidUserGroupName { get; set; }
    public NotHavePermissionToConfirmOrRejectGeneralStuffRequestException(string stuffCode, int validUserGroupId, string validUserGroupName)
    {
      StuffCode = stuffCode;
      ValidUserGroupId = validUserGroupId;
      ValidUserGroupName = validUserGroupName;
    }
  }
}
