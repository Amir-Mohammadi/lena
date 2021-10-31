using System.ComponentModel.DataAnnotations;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditProjectTemplateInput
  {
    public int Id { get; set; }
    [Required]
    [MaxLength(512)]
    public string Name { get; set; }
    [Required]
    public byte[] RowVersion { get; set; }
  }
}
