using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetStuffQualityControlTestOperationsInput : SearchInput<StuffQualityControlTestOperationSortType>
  {
    public int? StuffId { get; set; }
    public GetStuffQualityControlTestOperationsInput(PagingInput pagingInput, StuffQualityControlTestOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
