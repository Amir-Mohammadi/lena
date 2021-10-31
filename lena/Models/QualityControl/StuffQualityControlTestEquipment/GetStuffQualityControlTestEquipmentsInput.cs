using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;


using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetStuffQualityControlTestEquipmentsInput : SearchInput<StuffQualityControlTestEquipmentSortType>
  {
    public int? StuffId { get; set; }
    public GetStuffQualityControlTestEquipmentsInput(PagingInput pagingInput, StuffQualityControlTestEquipmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
