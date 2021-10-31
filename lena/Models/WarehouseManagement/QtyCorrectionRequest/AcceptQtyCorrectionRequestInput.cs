using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.QtyCorrectionRequest
{
  public class AcceptQtyCorrectionRequestInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public string Description { get; set; }
  }
}
