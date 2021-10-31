using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class DeleteProductionScheduleInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
