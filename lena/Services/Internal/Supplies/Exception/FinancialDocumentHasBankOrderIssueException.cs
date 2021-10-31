using lena.Services.Core.Foundation;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies.Exception
{
  public class FinancialDocumentHasBankOrderIssueException : InternalServiceException
  {
    public int Id { get; set; }

    public FinancialDocumentHasBankOrderIssueException(int id)
    {
      Id = id;
    }
  }
}
