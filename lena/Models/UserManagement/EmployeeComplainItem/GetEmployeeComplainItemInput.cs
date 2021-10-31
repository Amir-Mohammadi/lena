using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models
{
  public class GetEmployeeComplainItemInput : SearchInput<EmployeeComplainItemSortType>
  {
    public int Id { get; set; }
    public int EmployeeComplainId { get; set; }
    public EmployeeComplainType Type { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public GetEmployeeComplainItemInput(PagingInput pagingInput, EmployeeComplainItemSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
