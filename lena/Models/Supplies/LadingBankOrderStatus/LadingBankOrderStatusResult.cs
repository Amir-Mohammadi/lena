using lena.Domains.Enums;
namespace lena.Models.Supplies.LadingBankOrderStatus
{
  public class LadingBankOrderStatusResult
  {
    public int Id { get; set; }
    public string Code { get; set; }

    public string Name { get; set; }

    public byte[] RowVersion { get; set; }
  }
}
