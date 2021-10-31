using lena.Domains.Enums;
namespace lena.Models.QualityControl.QualityControlAccepter
{
  public class QualityControlAccepterResult
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserGroupId { get; set; }
    public string UserGroupName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
