using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.WeightDay
{
  public class GetWeightDaysInput : SearchInput<WeightDaySortType>
  {
    public int? DepartmentId { get; set; }
    public GetWeightDaysInput(PagingInput pagingInput, WeightDaySortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}