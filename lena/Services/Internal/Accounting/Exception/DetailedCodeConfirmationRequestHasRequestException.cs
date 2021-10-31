using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class DetailedCodeConfirmationRequestNotFoundException : InternalServiceException
  {
    public int Id { get; set; }

    public DetailedCodeConfirmationRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
