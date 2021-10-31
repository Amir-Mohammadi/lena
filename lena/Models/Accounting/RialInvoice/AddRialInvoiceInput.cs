using lena.Domains.Enums;
namespace lena.Models.Accounting.RialInvoice
{
  public class AddRialInvoiceInput
  {
    public int ReceiptId { get; set; }
    public string Description { get; set; }
  }
}
