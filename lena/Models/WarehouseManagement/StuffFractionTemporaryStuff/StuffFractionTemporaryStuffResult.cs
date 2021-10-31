using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffFractionByProject
{
  public class StuffFractionTemporaryStuffResult
  {
    public int Id { get; set; }
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string ProjectCode { get; set; }
    public double Qty { get; set; }
    public int UserId { get; set; }
    public string UserFullName { get; set; }
    public DateTime DateTime { get; set; }
  }
}
