using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetTestEquipmentsInput : SearchInput<TestEquipmentSortType>
  {
    public string Name { get; set; }
    public GetTestEquipmentsInput(PagingInput pagingInput, TestEquipmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
