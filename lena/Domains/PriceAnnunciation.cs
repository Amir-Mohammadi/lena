using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PriceAnnunciation : IEntity
  {
    protected internal PriceAnnunciation()
    {
      this.PriceAnnunciationItems = new HashSet<PriceAnnunciationItem>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public DateTime FromDate { get; set; }
    public Nullable<DateTime> ToDate { get; set; }
    public Nullable<Guid> DocumentId { get; set; }
    public PriceAnnunciationStatus Status { get; set; }
    public int RegisterarUserId { get; set; }
    public int CooperatorId { get; set; }
    public string Description { get; set; }
    public DateTime RegisterDateTime { get; set; }
    public virtual User RegisterarUser { get; set; }
    public virtual Cooperator Cooperator { get; set; }
    public virtual ICollection<PriceAnnunciationItem> PriceAnnunciationItems { get; set; }
  }
}