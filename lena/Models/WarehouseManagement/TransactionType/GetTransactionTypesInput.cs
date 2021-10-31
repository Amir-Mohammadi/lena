using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TransactionType
{
  public class GetTransactionTypesInput : SearchInput<TransactionTypeSortType>
  {
    public TValue<int> Id { get; set; }

    public GetTransactionTypesInput(PagingInput pagingInput, TransactionTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}