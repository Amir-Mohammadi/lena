using System.Linq;
using lena.Models.Production.ProductionOperatorMachineEmployee;

using lena.Domains.Enums;
namespace lena.Models.Production.FaildProductionOperation
{
  public class FaildProductionOperationResult
  {
    public int Id { get; set; }
    public int? OperationSequenceId { get; set; }
    public string OperationTitle { get; set; }
    public int OperationId { get; set; }
    public int ProductionOrderId { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? MachineTypeOperatorTypeId { get; set; }
    public string MachineTypeOperatorTypeTitle { get; set; }
    public int? OperatorTypeId { get; set; }
    public string OperatorTypeName { get; set; }
    public long DefaultTime { get; set; }
    public IQueryable<ProductionOperatorMachineEmployeeResult> ProductionOperatorMachineEmployees { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
