using System;

using lena.Domains.Enums;
namespace lena.Models.WarehouseManagement.TagCounting
{
  public class TagCountingResult
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public string EmployeeFullName { get; set; }
    public double Amount { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public double UnitConversionRatio { get; set; }
    public DateTime DateTime { get; set; }
    public int StockCheckingTagId { get; set; }
    public int StockCheckingTagNumber { get; set; }
    public int StockCheckingId { get; set; }
    public short WarehouseId { get; set; }
    public string WarehouseName { get; set; }
    public int TagTypeId { get; set; }
    public string TagTypeName { get; set; }
    public int StuffId { get; set; }
    public string Serial { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public bool IsDelete { get; set; }
    public byte[] RowVersion { get; set; }
  }
}
