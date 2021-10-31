using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;


using lena.Domains.Enums;
namespace lena.Models.SaleManagement.CustomerComplaint
{
  public class GetCustomerComplaintsInput : SearchInput<CustomerComplaintSortType>
  {
    public int Id { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int? CustomerId { get; set; }
    public GetCustomerComplaintsInput(PagingInput pagingInput, CustomerComplaintSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
