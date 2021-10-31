using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Domains.Enums;
using lena.Models.Common;
using System;

using lena.Domains.Enums;
namespace lena.Models.Supplies.ProvisionersCart
{
  public class GetProvisionersCartsInput : SearchInput<ProvisionersCartSortType>
  {
    public DateTime? DateTime { get; set; }
    public DateTime? FromRegisterDate { get; set; }
    public DateTime? ToRegisterDate { get; set; }
    public DateTime? ReportDate { get; set; }
    public int? EmployeeId { get; set; }
    public int? SupplierId { get; set; }
    public ProvisionersCartStatus? Status { get; set; }
    public int? RequestQty { get; set; }
    public int? SuppliedQty { get; set; }

    //public string Description { get; set; }
    public GetProvisionersCartsInput(PagingInput pagingInput, ProvisionersCartSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
