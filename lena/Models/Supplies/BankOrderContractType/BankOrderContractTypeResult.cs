using lena.Domains.Enums;
namespace lena.Models.Supplies.BankOrderContractType
{
  public class BankOrderContractTypeResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
