using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement.Exception
{
  public class WarehouseIssueNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Code { get; }

    public WarehouseIssueNotFoundException(int id)
    {
      this.Id = id;
    }

    public WarehouseIssueNotFoundException(string code)
    {
      this.Code = code;
    }
  }
}
