using lena.Domains.Enums;
namespace lena.Models.Production.RepairUnit
{
  public class EditProductionLineRepairUnitInput : AddProductionLineRepairUnitInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
