using lena.Domains.Enums;
namespace lena.Models
{
  public class AddIndicatorWeightInput
  {
    public string Name { get; set; }
    public string Code { get; set; }
    public short? DepartmentId { get; set; }
    public AddWeightDayInput[] AddWeightDayInputs { get; set; }
  }
}
