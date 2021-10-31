using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlAccepter
{
  public class GetQualityControlAcceptersInput : SearchInput<QualityControlAccepterSortType>
  {
    public GetQualityControlAcceptersInput(PagingInput pagingInput, QualityControlAccepterSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
