using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialDocument
{
  public class GetProviderFinancialDocumentsInput : SearchInput<ProviderFinancialDocumentSortType>
  {
    public int? ProviderId { get; set; }
    public string PlanCode { get; set; }

    public GetProviderFinancialDocumentsInput(PagingInput pagingInput, ProviderFinancialDocumentSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
