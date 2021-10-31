using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class ProductionOrderLastEmployeeResult
  {
    public int Id { get; set; }
    public string EmployeeFullName { get; set; }
    public int? OperatorTypeId { get; set; }
    public int? OperationId { get; set; }

    public int? EmployeeId { get; set; }

    public int? ProductionTerminalId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}