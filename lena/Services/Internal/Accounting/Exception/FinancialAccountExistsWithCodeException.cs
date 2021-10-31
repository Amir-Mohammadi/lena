using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountExistsWithCodeException : InternalServiceException
  {
    public string Code { get; }

    public FinancialAccountExistsWithCodeException(string code)
    {
      this.Code = code;
    }
  }
}
