using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.SendProduct
{
  public class SendProductComboResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int CooperatorId { get; set; }
    public string CooperatorName { get; set; }
    public string CooperatorCode { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
