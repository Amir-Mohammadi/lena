using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class WarehouseFiscalPeriodResult
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public bool IsClosed { get; set; }
    public bool IsCurrent { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
