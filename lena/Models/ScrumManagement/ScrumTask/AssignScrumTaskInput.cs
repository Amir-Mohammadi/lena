using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumTask
{
  public class AssignScrumTaskInput
  {
    public int Id { get; set; }
    public int? UserId { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
