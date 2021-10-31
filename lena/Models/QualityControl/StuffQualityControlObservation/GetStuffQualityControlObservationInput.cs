using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.StuffQualityControlObservation
{
  public class GetStuffQualityControlObservationInput : SearchInput<StuffQualityControlObservationSortType>
  {

    public GetStuffQualityControlObservationInput(PagingInput pagingInput, StuffQualityControlObservationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? StuffId { get; set; }
  }
}
