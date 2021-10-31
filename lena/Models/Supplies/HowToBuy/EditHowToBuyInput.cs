using lena.Models.Supplies.HowToBuyDetail;

using lena.Domains.Enums;
namespace lena.Models.Supplies.HowToBuy
{
  public class EditHowToBuyInput
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public AddHowToBuyDetailInput[] AddHowToBuyDetails { get; set; }
    public EditHowToBuyDetailInput[] EditHowToBuyDetails { get; set; }
    public int[] DeleteHowToBuyDetails { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
