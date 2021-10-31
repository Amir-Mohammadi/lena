using lena.Domains.Enums;
namespace lena.Models.SaleManagement.ProductionRequest
{
  public class RemoveProductionRequestInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
  }
}
