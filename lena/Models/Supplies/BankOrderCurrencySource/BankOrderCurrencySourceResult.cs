using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class BankOrderCurrencySourceResult
  {
    public int Id { get; set; }
    public int BankOrderId { get; set; }
    public int LadingId { get; set; }
    public string LadingCode { get; set; }
    public string BankOrderNumber { get; set; }
    public double FOB { get; set; }
    public string SataCode { get; set; }
    public double ActualWeight { get; set; }
    public double TransferCost { get; set; }
    public int BoxCount { get; set; }
    public int CurrencyId { get; set; }
    public string CurrencyTitle { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public DateTime DateTime { get; set; }
    public byte[] RowVersion { get; set; }

  }
}