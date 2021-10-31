using System;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models.Production.ProductionPerformanceInfo
{
  public class ProductionPerformanceInfoResult
  {
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<System.DateTime> DescriptionDateTime { get; set; }
    public string Description { get; set; }
    public string ResponsibleComment { get; set; }
    public string CorrectiveAction { get; set; }
    public Nullable<ProductionPerformanceInfoStatus> Status { get; set; }
    public int ProductionOrderId { get; set; }
    public Nullable<int> DepartmentId { get; set; }
    public string DepartmentFullName { get; set; }
    public string RegistratorUserName { get; set; }
    public string ConfirmatorUserName { get; set; }
    public string ProductionOrderLineName { get; set; }
    public string ProductionOrderStuffName { get; set; }
    public Nullable<double> ProductionOrderQty { get; set; }
    public Nullable<double> ProductionOrderProducedQty { get; set; }
  }
}