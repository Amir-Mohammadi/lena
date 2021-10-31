using lena.Domains.Enums;
namespace lena.Models.Supplies.Ladings
{
  public class LadingComboResult
  {
    public int Id { get; set; }

    public string LadingCode { get; set; }
    public int? BankOrderId { get; set; }

  }
}
