using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocumentType
{
  public class GetBillOfMaterialDocumentTypeInput : SearchInput<BillOfMaterialDocumentTypeSortType>
  {
    public GetBillOfMaterialDocumentTypeInput(PagingInput pagingInput, BillOfMaterialDocumentTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
