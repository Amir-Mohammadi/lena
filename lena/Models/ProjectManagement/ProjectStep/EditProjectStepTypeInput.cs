using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectStep
{
  public class EditProjectStepTypeInput
  {
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public int ScrumTaskTypeId { get; set; }
    [Required]
    [MaxLength(512)]
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
