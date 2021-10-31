using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class CustomerComplaintDepartment : IEntity
  {
    protected internal CustomerComplaintDepartment()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public int CustomerComplaintSummaryId { get; set; }
    public short DepartmentId { get; set; }
    public string InhibitionAction { get; set; }
    public Nullable<DateTime> DateOfInhibition { get; set; }
    public virtual CustomerComplaintSummary CustomerComplaintSummary { get; set; }
    public virtual Department Department { get; set; }
  }
}