using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuyDetail
{
  public class EditHowToBuyDetailInput
  {
    public int Id { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
