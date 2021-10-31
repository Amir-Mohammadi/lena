using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class WarehouseIssueItemNotFoundException : InternalServiceException

  {
    public int Id { get; }
    public string Code { get; }

    public WarehouseIssueItemNotFoundException(int id)
    {
      this.Id = id;
    }

    public WarehouseIssueItemNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
