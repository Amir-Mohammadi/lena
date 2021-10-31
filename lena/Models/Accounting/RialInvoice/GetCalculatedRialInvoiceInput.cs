using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class GetCalculatedRialInvoiceInput : SearchInput<CalculatedRialInvoiceSortType>
  {
    public int ReceiptId { get; set; }

    public GetCalculatedRialInvoiceInput(PagingInput pagingInput, CalculatedRialInvoiceSortType sortType, SortOrder sortOrder) :
        base(pagingInput: pagingInput, sortType: sortType, sortOrder: sortOrder)
    {
    }
  }
}
