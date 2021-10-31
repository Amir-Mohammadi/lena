using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production.Exception
{
  public class ProductionOperatorMachineEmployeeNotSetTerminalIdException : InternalServiceException
  {
    public int Id { get; }

    public ProductionOperatorMachineEmployeeNotSetTerminalIdException(int id)
    {
      this.Id = id;
    }
  }
}
