using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting.Exception
{
  public class AddFinancialDocumentArgumentException : InternalServiceException
  {
    public string ParameterName { get; }

    public AddFinancialDocumentArgumentException(string parameterName)
    {
      ParameterName = parameterName;
    }
  }
}
