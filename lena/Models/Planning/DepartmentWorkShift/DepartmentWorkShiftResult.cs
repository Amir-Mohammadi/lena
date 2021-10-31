using lena.Domains.Enums;
namespace lena.Models.Planning.DepartmentWorkShift
{
  public class DepartmentWorkShiftResult
  {
    public int DepartmentId { get; set; }
    public int WorkShiftId { get; set; }
    public string DepartmentName { get; set; }
    public string WorkShiftName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
