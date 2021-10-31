using System;

using lena.Domains.Enums;
namespace lena.Models
{
  public class AddProductionYearInput
  {
    public string Code { get; set; }
    public DateTime Year { get; set; }
  }
}