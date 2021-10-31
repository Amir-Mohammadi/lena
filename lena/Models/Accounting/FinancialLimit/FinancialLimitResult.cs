using lena.Domains.Enums;
namespace lena.Models.Accounting.FinancialLimit
{
  public class FinancialLimitResult
  {
    public int Id { get; set; }
    public int Allowance { get; set; }
    public bool IsArchive { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyName { get; set; }
    public string EmployeeCode { get; set; }
    public string EmployeeFullName { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
