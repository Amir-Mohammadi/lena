using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.Accounting.CostCenter
{
  public class GetCostCenterInput : SearchInput<CostCenterSortType>
  {

    public string Name { get; set; }
    public CostCenterStatus? Status { get; set; }
    public GetCostCenterInput(PagingInput pagingInput, CostCenterSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
