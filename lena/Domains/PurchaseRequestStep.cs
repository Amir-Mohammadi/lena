using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PurchaseRequestStep : IEntity
  {
    protected internal PurchaseRequestStep()
    {
      this.PurchaseRequestStepDetails = new HashSet<PurchaseRequestStepDetail>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public bool AllowUploadDocument { get; set; }
    public DateTime DateTime { get; set; }
    public Nullable<int> UserId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<PurchaseRequestStepDetail> PurchaseRequestStepDetails { get; set; }
    public virtual User User { get; set; }
  }
}