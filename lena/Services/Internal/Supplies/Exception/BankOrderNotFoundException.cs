
using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class BankOrderNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public BankOrderNotFoundException(string code)
    {
      this.Code = code;
    }

    public BankOrderNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
