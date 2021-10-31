using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeOperatorType
{
  public class AddEmployeeOperatorTypesInput
  {
    public int[] EmployeeIds { get; set; }
    public short[] OperatorTypeIds { get; set; }
  }
}
