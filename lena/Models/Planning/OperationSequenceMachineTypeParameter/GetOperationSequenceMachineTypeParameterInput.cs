using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.OperationSequenceMachineTypeParameter
{
  public class GetOperationSequenceMachineTypeParameterInput : SearchInput<OperationSequenceMachineTypeParameterSortType>
  {
    public int? Id { get; set; }
    public int? OperationSquenceId { get; set; }
    public int? MachineTypeParameterId { get; set; }

    public GetOperationSequenceMachineTypeParameterInput(PagingInput pagingInput, OperationSequenceMachineTypeParameterSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
