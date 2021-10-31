using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class AddBaseEntityConfirmation
  {
    public int BaseEntityConfirmTypeId { get; set; }
    public int ConfirmerId { get; set; }
    public int ConfirmingEntityId { get; set; }
    public string ConfirmDescription { get; set; }
    public ConfirmationStatus Status { get; set; }
  }
}
