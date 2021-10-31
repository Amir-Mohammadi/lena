using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Models.Common;
using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentDueDate
{
  public class GetPaymentDueDateInput : SearchInput<PaymentDueDateSortType>
  {
    public GetPaymentDueDateInput(PagingInput pagingInput, PaymentDueDateSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? OrderId { get; set; }
    public int? CustomerId { get; set; }
    public int? PaymentTypeId { get; set; }
    public DateTime? PaymentDate { get; set; }
    public DateTime? FromRequestDate { get; set; }
    public DateTime? ToRequestDate { get; set; }
    public bool GroupByCustomer { get; set; }
    public bool GroupByPaymentType { get; set; }
    public bool GroupByPaymentDate { get; set; }
  }
}

