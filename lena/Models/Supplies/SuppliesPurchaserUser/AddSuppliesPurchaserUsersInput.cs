using lena.Domains.Enums;
namespace lena.Models.Supplies.SuppliesPurchaserUser
{
  public class AddSuppliesPurchaserUsersInput
  {
    public int[] StuffIds { get; set; }
    public AddSuppliesPurchaserUserDetail[] Details { get; set; }
  }


  public class AddSuppliesPurchaserUserDetail
  {
    public int UserId { get; set; }
    public bool IsDefault { get; set; }
    public string Description { get; set; }
  }
}
