using lena.Domains.Enums;
namespace lena.Models.Planning.ProductionSchedule
{
  public class ChangeSchedulePublishStateInput
  {
    public int Id { get; set; }
    public bool IsPublish { get; set; }
    public byte[] RowVersion { get; set; }
  }
}