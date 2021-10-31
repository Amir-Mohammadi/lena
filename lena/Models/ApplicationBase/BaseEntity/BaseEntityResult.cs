using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntity
{
  public class BaseEntityResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
