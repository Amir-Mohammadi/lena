using lena.Domains.Enums;
namespace lena.Models
{
  public class AddProjectTemplateStepInput
  {
    public int ProjectTemplateId { get; set; }
    public int ProjectStepTypeId { get; set; }
    public long? Duration { get; set; }
  }
}
