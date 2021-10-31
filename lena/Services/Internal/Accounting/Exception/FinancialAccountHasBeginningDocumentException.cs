using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountHasBeginningDocumentException : InternalServiceException
  {
    public string Code { get; }

    public FinancialAccountHasBeginningDocumentException(string code)
    {
      this.Code = code;
    }
  }
}
