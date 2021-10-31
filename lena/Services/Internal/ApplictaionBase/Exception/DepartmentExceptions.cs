using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class DepartmentNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public DepartmentNotFoundException(int departmentId)
    {
      Id = departmentId;
    }
  }
}
