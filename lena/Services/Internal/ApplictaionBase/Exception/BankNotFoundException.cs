using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class BankNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public BankNotFoundException(int id)
    {
      Id = id;
    }
  }
}
