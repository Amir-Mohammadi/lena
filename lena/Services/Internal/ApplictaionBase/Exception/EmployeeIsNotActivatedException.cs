using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class EmployeeIsNotActivatedException : InternalServiceException
  {
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int EmployeeId { get; set; }
    public EmployeeIsNotActivatedException(int userId, string userName, int employeeId)
    {
      UserId = userId;
      UserName = userName;
      EmployeeId = employeeId;
    }
  }
}
