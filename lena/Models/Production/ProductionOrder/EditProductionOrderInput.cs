using System;
using lena.Models.Production.ProductionOperator;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOrder
{
  public class EditProductionOrderInput
  {
    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public long Duration { get; set; }
    public int StuffId { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int WorkPlanId { get; set; }
    public int WorkPlanStepId { get; set; }
    public int Qty { get; set; }
    public byte UnitId { get; set; }
    public string Description { get; set; }
    public double ToleranceQty { get; set; }
    public int? ProductionScheduleId { get; set; }
    public int SupervisorEmployeeId { get; set; }
    public AddProductionOperatorInput[] AddProductionOperators { get; set; }
    public EditProductionOperatorInput[] EditProductionOperators { get; set; }
    public int[] DeleteProductionOperators { get; set; }
    public byte[] RowVersion;
  }
}
