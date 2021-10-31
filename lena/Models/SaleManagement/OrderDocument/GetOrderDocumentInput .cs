using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.SaleManagement.OrderDocument
{
  public class GetOrderDocumentInput : SearchInput<OrderDocumentSortType>
  {
    public GetOrderDocumentInput(PagingInput pagingInput, OrderDocumentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public DateTime? FromCreateDateTime { get; set; }
    public DateTime? ToCreateDateTime { get; set; }
  }
}
