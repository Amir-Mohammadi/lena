using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class HowToBuyDetail : IEntity
  {
    protected internal HowToBuyDetail()
    {
      this.PurchaseSteps = new HashSet<PurchaseStep>();
    }
    public int Id { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public short HowToBuyId { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual HowToBuy HowToBuy { get; set; }
    public virtual ICollection<PurchaseStep> PurchaseSteps { get; set; }
  }
}
