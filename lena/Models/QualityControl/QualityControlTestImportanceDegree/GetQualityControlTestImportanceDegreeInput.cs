using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetQualityControlTestImportanceDegreesInput : SearchInput<QualityControlTestImportanceDegreeSortType>
  {
    public GetQualityControlTestImportanceDegreesInput(PagingInput pagingInput, QualityControlTestImportanceDegreeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
