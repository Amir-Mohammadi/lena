using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class EditBaseEntityConfirmation : AddBaseEntityConfirmation
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
