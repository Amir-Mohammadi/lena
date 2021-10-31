using lena.Domains.Enums;
namespace lena.Models.Planning.WorkShift
{
  public class EditWorkShiftInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
