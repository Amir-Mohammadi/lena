using lena.Domains.Enums;
namespace lena.Models.Planning.EmployeeOperatorType
{
  public class EmployeeOperatorTypeResult
  {
    public int OperatorTypeId { get; set; }
    public string OperatorTypeName { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
