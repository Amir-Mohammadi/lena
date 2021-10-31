using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class EditWarehouseFiscalPeriodInput
  {
    public short Id { get; set; }
    public string Name { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public byte[] RowVersion { get; set; }
  }
}