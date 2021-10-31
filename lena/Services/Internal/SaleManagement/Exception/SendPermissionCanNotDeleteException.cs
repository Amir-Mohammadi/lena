using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class SendPermissionCanNotDeleteException : InternalServiceException
  {
    public int Id { get; }

    public SendPermissionCanNotDeleteException(int id)
    {
      this.Id = id;
    }
  }
}
