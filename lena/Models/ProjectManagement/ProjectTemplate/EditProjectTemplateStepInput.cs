using lena.Domains.Enums;
namespace lena.Models
{
  public class EditProjectTemplateStepInput
  {
    public int Id { get; set; }
    public int ProjectTemplateId { get; set; }
    public int ProjectStepTypeId { get; set; }
    public long? Duration { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
