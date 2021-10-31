using lena.Domains.Enums;
namespace lena.Models.Supplies.CargoItem
{
  public class CargoItemComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int CargoId { get; set; }
    public string CargoCode { get; set; }
    public int? ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string ProviderCode { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte[] RowVersion { get; set; }


  }
}
