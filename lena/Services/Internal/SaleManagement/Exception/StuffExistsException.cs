using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class StuffExistsException : InternalServiceException
  {
    public string Code { get; }

    public StuffExistsException(string code)
    {
      this.Code = code;
    }
  }
}
