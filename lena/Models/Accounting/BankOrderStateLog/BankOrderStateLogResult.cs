
using lena.Domains.Enums;
namespace lena.Models.Accounting.BankOrderStateLog
{
  public class BankOrderStateLogResult
  {
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public int BankOrderStateId { get; set; }
    public string OrderNumber { get; set; }
    public string BankOrderStateTitle { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public System.DateTime DateTime { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
