using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlSample
{
  public class GetQualityControlSampleInput : SearchInput<QaulityControlSampleSorttype>
  {
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public int? WarehouseId { get; set; }
    public int? QualityControlId { get; set; }
    public int? QualityControlItemId { get; set; }
    public QualityControlSampleStatus[] Statuses { get; set; }

    public GetQualityControlSampleInput(PagingInput pagingInput, QaulityControlSampleSorttype sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}

