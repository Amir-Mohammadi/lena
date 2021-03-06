using System;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Production.ProductionPerformanceInfo
{
  public class EditProductionPerformanceInfoInput
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<System.DateTime> DescriptionDateTime { get; set; }
    public string Description { get; set; }
    public bool DescriptionMode { get; set; }
    public string ResponsibleComment { get; set; }
    public string CorrectiveAction { get; set; }
    public Nullable<ProductionPerformanceInfoStatus> Status { get; set; }
    public int ProductionOrderId { get; set; }
    public Nullable<short> DepartmentId { get; set; }
  }
}