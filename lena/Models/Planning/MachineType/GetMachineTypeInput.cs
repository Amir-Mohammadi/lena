using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.MachineType
{
  public class GetMachineTypeInput : SearchInput<MachineTypeSortType>
  {
    public GetMachineTypeInput(PagingInput pagingInput, MachineTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
