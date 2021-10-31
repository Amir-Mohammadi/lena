using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionTemporaryStuff
{
  public class DeleteStuffFractionTemporaryStuffInput
  {
    public int[] Ids { get; set; }
    public int? UserId { get; set; }
    public bool DeleteCurrentUserStuffs { get; set; }

  }
}
