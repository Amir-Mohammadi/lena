using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.EquivalentStuff
{
  public class GetEquivalentStuffsInput : SearchInput<EquivalentStuffSortType>
  {
    public int? Id { get; set; }
    public int? BillOfMaterialDetailId { get; set; }
    public bool? IsActive { get; set; }
    public EquivalentStuffType? EquivalentStuffType { get; set; }

    public GetEquivalentStuffsInput(PagingInput pagingInput, EquivalentStuffSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
