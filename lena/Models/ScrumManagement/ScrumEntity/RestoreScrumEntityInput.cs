using lena.Domains.Enums;
namespace lena.Models.ScrumManagement.ScrumEntity
{
  public class RestoreScrumEntityInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
