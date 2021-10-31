using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectStep
{
  public class AddProjectStepTypeInput
  {
    public int DepartmentId { get; set; }
    public int ScrumTaskTypeId { get; set; }
    [Required]
    [MaxLength(512)]
    public string Name { get; set; }

  }
}
