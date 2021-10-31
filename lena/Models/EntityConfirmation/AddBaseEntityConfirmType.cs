using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class AddBaseEntityConfirmType
  {
    public int Id { get; set; }

    public EntityType ConfirmType { get; set; }
    public short DepartmentId { get; set; }
    public int? UserId { get; set; }
    public string ConfirmPageUrl { get; set; }
  }
}
