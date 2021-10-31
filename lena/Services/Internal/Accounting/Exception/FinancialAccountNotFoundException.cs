using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountNotFoundException : InternalServiceException
  {
    public int Id { get; }

    public FinancialAccountNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
