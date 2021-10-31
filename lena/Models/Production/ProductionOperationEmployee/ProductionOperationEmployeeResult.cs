using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperationEmployee
{
  public class ProductionOperationEmployeeResult
  {
    public int Id { get; set; }
    public int OperationId { get; set; }
    public string OperationCode { get; set; }
    public string OperationName { get; set; }
    public int ProductionOperationId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public string EmployeeCode { get; set; }
  }
}
