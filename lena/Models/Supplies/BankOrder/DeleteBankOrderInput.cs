using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrder
{
  public class DeleteBankOrderInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
