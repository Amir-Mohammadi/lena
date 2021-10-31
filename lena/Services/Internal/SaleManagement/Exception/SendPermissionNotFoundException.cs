using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class SendPermissionNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public SendPermissionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
