using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeOperatorType
{
  public class GetMachineTypeOperatorTypesInput : SearchInput<MachineTypeOperatorTypeSortType>
  {
    public GetMachineTypeOperatorTypesInput(PagingInput pagingInput, MachineTypeOperatorTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? MachineTypeId { get; set; }
    public int? OperatorTypeId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsNecessary { get; set; }
  }
}
