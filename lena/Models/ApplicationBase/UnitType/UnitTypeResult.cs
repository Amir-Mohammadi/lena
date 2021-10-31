using lena.Domains.Enums;
namespace lena.Models.UnitType
{
  public class UnitTypeResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public byte[] RowVersion { get; set; }
  }
}