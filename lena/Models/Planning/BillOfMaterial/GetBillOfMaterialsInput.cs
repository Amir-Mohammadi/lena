using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class GetBillOfMaterialsInput : SearchInput<BillOfMaterialSortType>
  {
    public int? StuffId { get; set; }
    public int? DetailStuffId { get; set; }
    public int? Version { get; set; }
    public BillOfMaterialVersionType? BillOfMaterialVersionType { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsPublished { get; set; }
    public int? EquivalentStuffId { get; set; }
    public int[] DetailStuffIds { get; set; }





    public GetBillOfMaterialsInput(PagingInput pagingInput, BillOfMaterialSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}