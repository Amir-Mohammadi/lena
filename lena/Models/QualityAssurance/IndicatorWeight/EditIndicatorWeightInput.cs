using lena.Domains.Enums;
namespace lena.Models
{
  public class EditIndicatorWeightInput
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public short? DepartmentId { get; set; }
    public AddWeightDayInput[] AddWeightDayInputs { get; set; }
    public EditWeightDayInput[] EditWeightDayInputs { get; set; }
    public int[] DeleteWeightDayInputs { get; set; }
    public byte[] RowVersion { get; set; }
  }
}