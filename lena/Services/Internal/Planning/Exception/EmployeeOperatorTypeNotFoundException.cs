using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class EmployeeOperatorTypeNotFoundException : InternalServiceException
  {
    public int EmployeeId { get; }
    public int OperatorTypeId { get; }

    public EmployeeOperatorTypeNotFoundException(int employeeId, int operatorTypeId)
    {
      this.EmployeeId = employeeId;
      this.OperatorTypeId = operatorTypeId;
    }
  }
}
