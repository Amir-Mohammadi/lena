using lena.Domains.Enums;
namespace lena.Models
{
  public class ProjectTemplateStepResult
  {
    public int Id { get; set; }
    public int ProjectTemplateId { get; set; }
    public string ProjectTemplateName { get; set; }
    public int ProjectStepTypeId { get; set; }
    public string ProjectStepTypeName { get; set; }
    public long? Duration { get; set; }
  }
}
