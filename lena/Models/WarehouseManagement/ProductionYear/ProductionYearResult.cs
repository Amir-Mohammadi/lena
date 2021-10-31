using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class ProductionYearResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public DateTime Year { get; set; }
    public byte[] RowVersion { get; set; }

  }
}