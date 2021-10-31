using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class FinancialAccountHasFinancialDocumentException : InternalServiceException
  {
    public string Code { get; }

    public FinancialAccountHasFinancialDocumentException(string code)
    {
      this.Code = code;
    }
  }
}
