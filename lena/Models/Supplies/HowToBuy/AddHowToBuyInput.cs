using lena.Models.Supplies.HowToBuyDetail;

using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuy
{
  public class AddHowToBuyInput
  {
    public string Title { get; set; }
    public AddHowToBuyDetailInput[] AddHowToBuyDetails { get; set; }
  }
}
