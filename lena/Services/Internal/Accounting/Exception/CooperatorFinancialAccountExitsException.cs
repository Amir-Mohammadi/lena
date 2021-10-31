using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class CooperatorFinancialAccountExitsException : InternalServiceException
  {
    public string Code { get; }

    public CooperatorFinancialAccountExitsException(string code)
    {
      this.Code = code;
    }
  }
}
