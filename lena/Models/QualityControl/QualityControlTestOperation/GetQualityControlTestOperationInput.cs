using lena.Models.Common;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl
{
  public class GetQualityControlTestOperationsInput : SearchInput<QualityControlTestOperationSortType>
  {
    public GetQualityControlTestOperationsInput(PagingInput pagingInput, QualityControlTestOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
