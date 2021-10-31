using lena.Domains.Enums;
namespace lena.Models.Planning.PlanCode
{
  public class EditPlanCodeInput
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public bool? IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}