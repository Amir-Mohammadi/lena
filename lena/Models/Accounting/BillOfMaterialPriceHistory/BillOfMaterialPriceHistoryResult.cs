using System;

using lena.Domains.Enums;
namespace lena.Models.Accounting.BillOfMaterialPriceHistory
{
  public class BillOfMaterialPriceHistoryResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int? Version { get; set; }
    public double TotalPrice { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int UserId { get; set; }
    public int EmployeeId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
  }
}
