using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement.Exception
{
  public class ResponsibleDepartmentHasException : InternalServiceException
  {
    public int? Id { get; set; }
    public ResponsibleDepartmentHasException(int id)
    {
      this.Id = id;
    }
  }
}
