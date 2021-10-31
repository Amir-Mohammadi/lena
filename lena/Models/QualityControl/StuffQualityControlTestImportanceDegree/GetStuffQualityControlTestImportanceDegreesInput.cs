using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetStuffQualityControlTestImportanceDegreesInput : SearchInput<StuffQualityControlTestImportanceDegreeSortType>
  {
    public int? StuffId { get; set; }
    public GetStuffQualityControlTestImportanceDegreesInput(PagingInput pagingInput, StuffQualityControlTestImportanceDegreeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
