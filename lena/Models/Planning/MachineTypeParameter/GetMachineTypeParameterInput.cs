using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.MachineTypeParameter
{
  public class GetMachineTypeParameterInput : SearchInput<MachineTypeParameterSortType>
  {
    public int? Id { get; set; }
    public int? MachineTypeId { get; set; }
    public string Name { get; set; }

    public GetMachineTypeParameterInput(PagingInput pagingInput, MachineTypeParameterSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
