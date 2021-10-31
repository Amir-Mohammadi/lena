using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddWarehouseFiscalPeriodInput
  {
    public string Name { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
  }
}
