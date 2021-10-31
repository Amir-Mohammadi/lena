using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionByProject
{
  public class GetStuffFractionsByProjectInput : SearchInput<StuffFractionByProjectSortType>
  {
    public GetStuffFractionsByProjectInput(PagingInput pagingInput, StuffFractionByProjectSortType sortType, SortOrder sortOrder)
        : base(pagingInput, sortType, sortOrder)
    {
    }

    public StuffFractionStuffInput[] Stuffs { get; set; }
    public int[] WarehouseIds { get; set; }


  }
}
