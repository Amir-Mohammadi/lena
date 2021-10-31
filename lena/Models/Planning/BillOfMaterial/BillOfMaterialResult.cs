using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterial
{
  public class BillOfMaterialResult
  {
    public int StuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public int Version { get; set; }
    public int QtyPerBox { get; set; }
    public string Title { get; set; }
    public BillOfMaterialVersionType BillOfMaterialVersionType { get; set; }
    public bool IsActive { get; set; }
    public bool IsPublished { get; set; }
    public System.DateTime CreateDate { get; set; }
    public DateTime UtcCreateDate { get { return this.CreateDate.ToUniversalTime(); } }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public double Value { get; set; }
    public byte UnitId { get; set; }
    public string UnitName { get; set; }
    public int ProductionStepId { get; set; }
    public string ProductionStepName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
  }
}

