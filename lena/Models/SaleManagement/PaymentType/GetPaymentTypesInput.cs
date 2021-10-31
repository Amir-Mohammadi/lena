using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.SaleManagement.PaymentType
{
  public class GetPaymentTypesInput : SearchInput<PaymentTypeSortType>
  {
    public GetPaymentTypesInput(PagingInput pagingInput, PaymentTypeSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
