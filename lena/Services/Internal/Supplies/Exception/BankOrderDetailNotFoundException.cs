using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderDetailNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public BankOrderDetailNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
