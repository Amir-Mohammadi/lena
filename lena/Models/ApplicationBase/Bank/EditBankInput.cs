using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.Bank
{
  public class EditBankInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
