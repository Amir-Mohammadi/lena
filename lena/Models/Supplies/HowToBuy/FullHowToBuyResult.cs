using lena.Models.Supplies.HowToBuyDetail;
using System.Linq;

using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuy
{
  public class FullHowToBuyResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public IQueryable<HowToBuyDetailResult> HowToBuyDetails { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
