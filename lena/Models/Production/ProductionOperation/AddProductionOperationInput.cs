using lena.Models.Production.ProductionStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperation
{
  public class AddProductionOperationInput
  {
    public int Time { get; set; }
    public int[] EmployeeIds { get; set; }
    public int? ProductionOperatorId { get; set; }
    public short OperationId { get; set; }
    public AddRepairProductionStuffDetailInput[] AddProductionStuffDetails { get; set; }
  }
}
