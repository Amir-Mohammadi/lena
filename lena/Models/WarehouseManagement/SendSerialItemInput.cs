using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement
{
  public class SendSerialItemInput
  {
    public int Id { get; set; }
    public double Amount { get; set; }

    public string Description { get; set; }
  }
}
