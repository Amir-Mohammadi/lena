using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetQualityControlTestEquipmentsInput : SearchInput<QualityControlTestEquipmentSortType>
  {
    public GetQualityControlTestEquipmentsInput(PagingInput pagingInput, QualityControlTestEquipmentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
