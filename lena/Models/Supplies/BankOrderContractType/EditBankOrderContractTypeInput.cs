using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderContractType
{
  public class EditBankOrderContractTypeInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }

  }
}
