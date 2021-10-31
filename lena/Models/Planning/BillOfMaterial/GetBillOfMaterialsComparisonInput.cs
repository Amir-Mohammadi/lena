using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class GetBillOfMaterialsComparisonInput : SearchInput<BillOfMaterialComparisonSortType>
  {
    public bool BeRecursive { get; set; }
    public int StuffId1 { get; set; }
    public int Version1 { get; set; }
    public int StuffId2 { get; set; }
    public int Version2 { get; set; }
    public int EquivalentStuffDetailId { get; set; }

    public GetBillOfMaterialsComparisonInput(PagingInput pagingInput, BillOfMaterialComparisonSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
