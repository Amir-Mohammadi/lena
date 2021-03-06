using lena.Domains.Enums;
namespace lena.Models
{
  public class EditIndicatorInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public double Target { get; set; }
    public double Weight { get; set; }
    public int ApiInfoId { get; set; }
    public string Formula { get; set; }
    public short DepartmentId { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}