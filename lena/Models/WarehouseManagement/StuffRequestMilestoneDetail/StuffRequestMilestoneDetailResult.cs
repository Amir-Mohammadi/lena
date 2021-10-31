using lena.Domains.Enums;
using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.StuffRequestMilestoneDetail
{
  public class StuffRequestMilestoneDetailResult
  {
    public int Id { get; set; }
    public string Code { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public string Description { get; set; }
    public DateTime DateTime { get; set; }
    public DateTime DueDate { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int StuffId { get; set; }
    public string StuffName { get; set; }
    public string StuffCode { get; set; }
    public double Qty { get; set; }
    public double OrderedQty { get; set; }
    public double CargoedQty { get; set; }
    public double ReciptedQty { get; set; }
    public double QualityControlPassedQty { get; set; }
    public StuffRequestMilestoneDetailStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
