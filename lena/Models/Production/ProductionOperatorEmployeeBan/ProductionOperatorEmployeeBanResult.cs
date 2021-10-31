using lena.Domains.Enums;
namespace lena.Models.Production.ProductionOperatorEmployeeBan
{
  public class ProductionOperatorEmployeeBanResult
  {
    public int Id { get; set; }
    public int ProductionOperatorId { get; set; }
    public int OperationId { get; set; }
    public int EmployeeId { get; set; }
    public bool IsBan { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
