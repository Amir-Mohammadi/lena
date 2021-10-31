using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlTestUnit
{
  public class GetQualityControlTestUnitsInput : SearchInput<QualityControlTestUnitSortType>
  {
    public bool? IsActive { get; set; }
    public string Name { get; set; }
    public GetQualityControlTestUnitsInput(PagingInput pagingInput, QualityControlTestUnitSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
