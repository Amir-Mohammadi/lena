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
  public class DepartmentManagerNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public DepartmentManagerNotFoundException(int departmentId)
    {
      Id = departmentId;
    }
  }
}
