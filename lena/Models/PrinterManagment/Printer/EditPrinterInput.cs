using lena.Domains.Enums;
namespace lena.Models.Printers
{
  public class EditPrinterInput : AddPrinterInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
