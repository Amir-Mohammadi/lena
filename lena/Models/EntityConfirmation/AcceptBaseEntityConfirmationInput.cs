using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class AcceptBaseEntityConfirmationInput
  {
    public int Id { get; set; }
    public int BaseEntityConfirmTypeId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
