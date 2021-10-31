using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class ChangeArchiveStatusInput
  {
    public int Id { get; set; }
    public bool IsArchived { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
