using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuyDetail
{
  public class HowToBuyDetailResult
  {
    public int Id { get; set; }
    public int Order { get; set; }
    public string Title { get; set; }
    public int HowToBuyId { get; set; }
    public string HowToBuyTitle { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
