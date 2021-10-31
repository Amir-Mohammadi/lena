using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class UnPublishedWorkPlanInput
  {
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }
  }
}
