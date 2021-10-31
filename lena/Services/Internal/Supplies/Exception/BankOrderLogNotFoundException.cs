using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderLogNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public BankOrderLogNotFoundException(int id)
    {
      Id = id;
    }
  }
}
