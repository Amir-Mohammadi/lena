using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class GetBillOfMaterialEquivalentStuffInput : SearchInput<BillOfMaterialEquivalentStuffSortType>
  {
    public int? StuffId { get; set; }
    public int? DetailStuffId { get; set; }
    public int? Version { get; set; }

    public GetBillOfMaterialEquivalentStuffInput(PagingInput pagingInput, BillOfMaterialEquivalentStuffSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}