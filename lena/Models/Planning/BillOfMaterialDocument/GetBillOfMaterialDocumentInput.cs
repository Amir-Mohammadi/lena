using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDocument
{
  public class GetBillOfMaterialDocumentInput : SearchInput<BillOfMaterialDocumentSortType>
  {
    public GetBillOfMaterialDocumentInput(PagingInput pagingInput, BillOfMaterialDocumentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int BillOfMaterialVersion { get; set; }
    public int BillOfMaterialStuffId { get; set; }
  }
}
