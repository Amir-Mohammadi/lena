using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOperatorMachineEmployeeNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public ProductionOperatorMachineEmployeeNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
