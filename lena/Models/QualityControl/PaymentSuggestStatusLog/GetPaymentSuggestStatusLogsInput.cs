using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.PaymentSuggestStatusLog
{
  public class GetPaymentSuggestStatusLogsInput : SearchInput<PaymentSuggestStatusLogsSortType>
  {
    public GetPaymentSuggestStatusLogsInput(PagingInput pagingInput, PaymentSuggestStatusLogsSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? QualityControlId { get; set; }
    public int? EmployeeId { get; set; }
  }
}