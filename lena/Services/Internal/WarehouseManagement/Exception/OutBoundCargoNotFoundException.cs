using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement.Exception
{
  public class OutBoundCargoNotFoundException : InternalServiceException
  {
    public string Code { get; }
    public int Id { get; }

    public OutBoundCargoNotFoundException(string code)
    {
      this.Code = code;
    }

    public OutBoundCargoNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
