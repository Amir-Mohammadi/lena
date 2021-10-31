using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CanNotDeleteCostCenterException : InternalServiceException
  {
    public int Id { get; set; }

    public CanNotDeleteCostCenterException(int id)
    {
      Id = id;
    }
  }
}
