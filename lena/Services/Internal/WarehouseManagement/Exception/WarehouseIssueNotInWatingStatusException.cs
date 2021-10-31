using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class WarehouseIssueNotInWatingStatusException : InternalServiceException
  {
    public int Id { get; }

    public WarehouseIssueNotInWatingStatusException(int id)
    {
      this.Id = id;

    }
  }
}
