using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class BaseEntityConfirmTypeResult
  {
    public int Id { get; set; }

    public string ConfirmPageUrl { get; set; }

    public EntityType ConfirmType { get; set; }

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public int? UserId { get; set; }
    public string UserFullName { get; set; }
  }
}
