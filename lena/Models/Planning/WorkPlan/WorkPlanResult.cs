using System;

using lena.Domains.Enums;
namespace lena.Models.Planning.WorkPlan
{
  public class WorkPlanResult
  {
    public string Title { get; set; }
    public int Version { get; set; }
    public string BillOfMaterialTitle { get; set; }
    public int BillOfMaterialVersion { get; set; }
    public int QtyPerBox { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public int BillOfMaterialStuffId { get; set; }
    public string StuffCode { get; set; }
    public string StuffName { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public int Id { get; set; }

    public bool IsPublished { get; set; }
  }
}
