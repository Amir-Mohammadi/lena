using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CooperatorFinancialAccountNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public CooperatorFinancialAccountNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
