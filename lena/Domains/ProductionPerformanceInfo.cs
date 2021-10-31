using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ProductionPerformanceInfo : IEntity
  {
    protected internal ProductionPerformanceInfo()
    {
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public Nullable<DateTime> DescriptionDateTime { get; set; }
    public string Description { get; set; }
    public string ResponsibleComment { get; set; }
    public string CorrectiveAction { get; set; }
    public Nullable<ProductionPerformanceInfoStatus> Status { get; set; }
    public Nullable<DateTime> RegistrationDate { get; set; }
    public Nullable<DateTime> ConfirmationDate { get; set; }
    public int ProductionOrderId { get; set; }
    public Nullable<short> DepartmentId { get; set; }
    public Nullable<int> RegistratorUserId { get; set; }
    public Nullable<int> ConfirmatorUserId { get; set; }
    public virtual User ConfirmatorUser { get; set; }
    public virtual User RegistratorUser { get; set; }
    public virtual Department Department { get; set; }
    public virtual ProductionOrder ProductionOrder { get; set; }
  }
}