using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuy
{
  public class HowToBuyResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}