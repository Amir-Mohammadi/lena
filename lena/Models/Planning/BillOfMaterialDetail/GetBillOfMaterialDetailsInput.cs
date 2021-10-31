using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialDetail
{
  public class GetBillOfMaterialDetailsInput : SearchInput<BillOfMaterialDetailSortType>
  {
    public int? StuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public int? BillOfMaterialStuffId { get; set; }

    public GetBillOfMaterialDetailsInput(PagingInput pagingInput, BillOfMaterialDetailSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
