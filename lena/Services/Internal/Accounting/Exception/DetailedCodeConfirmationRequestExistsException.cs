using lena.Services.Core.Foundation;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class DetailedCodeConfirmationRequestExistsException : InternalServiceException
  {
    public int Id { get; set; }

    public DetailedCodeConfirmationRequestExistsException(int id)
    {
      this.Id = id;
    }
  }
}
