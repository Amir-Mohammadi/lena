using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class DepartmentWorkShiftNotFoundException : InternalServiceException
  {
    public int DepartmentId { get; }
    public int WorkShiftId { get; }

    public DepartmentWorkShiftNotFoundException(int departmentId, int workShiftId)
    {
      this.DepartmentId = departmentId;
      this.WorkShiftId = workShiftId;
    }
  }
}
