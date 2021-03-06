using lena.Services.Core.Foundation;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class ContactNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public ContactNotFoundException(int id)
    {
      Id = id;
    }
  }
}
