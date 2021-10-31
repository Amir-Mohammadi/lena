using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class GetSendProductInput : SearchInput<SendProductSortType>
  {
    public int? ExitReceiptId { get; set; }
    public int? StuffId { get; set; }
    public int? CooperatorId { get; set; }
    public bool? ExitReceiptconfirmed { get; set; }
    public GetSendProductInput(PagingInput pagingInput, SendProductSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
