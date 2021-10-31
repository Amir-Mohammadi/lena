using lena.Domains.Enums;
namespace lena.Models.Production.ProductionLineEmployeeIntervalDetail
{
  public class ProductionLineEmployeeIntervalDetailResult
  {
    public int Id { get; set; }
    public int OperationId { get; set; }
    public string OperationTitle { get; set; }
    public int ProductionLineEmployeeIntervalId { get; set; }
    public long Time { get; set; }
    public byte[] RowVersion { get; set; }

  }
}
