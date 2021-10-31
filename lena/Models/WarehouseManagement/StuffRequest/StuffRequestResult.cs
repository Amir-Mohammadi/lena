using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequest
{
  public class StuffRequestResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
