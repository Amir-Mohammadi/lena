using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeOperatorType
{
  public class GetEmployeeOperatorTypesInput : SearchInput<EmployeeOperatorTypeSortType>
  {
    public int? EmployeeId { get; set; }
    public short? OperatorTypeId { get; set; }
    public int[] EmployeeIds { get; set; }
    public short[] OperatorTypeIds { get; set; }

    public GetEmployeeOperatorTypesInput(PagingInput pagingInput, EmployeeOperatorTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
