using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CooperatorHasNoFinancialAccountException : InternalServiceException
  {
    public int CooperatorId { get; set; }

    public CooperatorHasNoFinancialAccountException(int cooperatorId)
    {
      CooperatorId = cooperatorId;
    }
  }
}
