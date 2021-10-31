using lena.Models.Production.ProductionStuffDetail;

using lena.Domains.Enums;
namespace lena.Models.Production.RepairProductionFault
{
  public class AddRepairProductionFaultInput
  {
    public short CorrectiveOperationId { get; set; }

    public int? ProductionFaultTypeId { get; set; }

    public int Time { get; set; }

    public AddRepairProductionStuffDetailInput[] RepairProductionStuffDetails { get; set; }

  }
}
