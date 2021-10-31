using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class WorkPlanComboResult
  {
    public int Id { get; set; }
    public int Version { get; set; }
    public string Title { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
