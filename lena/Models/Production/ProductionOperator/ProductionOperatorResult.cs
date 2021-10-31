using System.Linq;
using lena.Models.Planning.OperationSequenceMachineTypeParameter;
using lena.Models.Production.ProductionOperatorEmployeeBan;
using lena.Models.Production.ProductionOperatorMachineEmployee;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperator
{
  public class ProductionOperatorResult
  {
    public int Id { get; set; }
    public int? OperationSequenceId { get; set; }
    //public int? ProductionTerminalId { get; set; }
    public string OperationTitle { get; set; }
    //public string ProductionTerminalDescription { get; set; }
    public int OperationId { get; set; }
    public int ProductionOrderId { get; set; }
    public string ProductionOrderCode { get; set; }
    public int? MachineTypeOperatorTypeId { get; set; }
    public string MachineTypeOperatorTypeTitle { get; set; }
    public int? OperatorTypeId { get; set; }
    public string OperatorTypeName { get; set; }
    public long DefaultTime { get; set; }
    public int? WrongLimitCount { get; set; }
    public IQueryable<ProductionOperatorEmployeeBanResult> ProductionOperatorEmployeeBans { get; set; }
    public IQueryable<OperationSequenceMachineTypeParameterResult> OperationSequenceMachineTypeParameters { get; set; }
    public IQueryable<ProductionOperatorMachineEmployeeResult> ProductionOperatorMachineEmployees { get; set; }
    public byte[] RowVersion { get; set; }
    public int Index { get; set; }
  }
}
