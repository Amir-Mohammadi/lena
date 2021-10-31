using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class EditBaseEntityConfirmType : AddBaseEntityConfirmType
  {

    public byte[] RowVersion { get; set; }
  }
}
