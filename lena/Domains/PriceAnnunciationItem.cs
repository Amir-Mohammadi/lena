using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class PriceAnnunciationItem : IEntity
  {
    protected internal PriceAnnunciationItem()
    {
      this.ExitReceiptRequests = new HashSet<ExitReceiptRequest>();
    }
    public int Id { get; set; }
    public byte[] RowVersion { get; set; }
    public double Price { get; set; }
    public byte CurrencyId { get; set; }
    public int StuffId { get; set; }
    public double? Count { get; set; }
    public int PriceAnnunciationId { get; set; }
    public PriceAnnunciationItemStatus Status { get; set; }
    public string Description { get; set; }
    public int? ConfirmerUserId { get; set; }
    public Nullable<DateTime> ConfirmationDateTime { get; set; }
    public virtual Currency Currency { get; set; }
    public virtual Stuff Stuff { get; set; }
    public virtual PriceAnnunciation PriceAnnunciation { get; set; }
    public virtual User ConfirmerUser { get; set; }
    public virtual ICollection<ExitReceiptRequest> ExitReceiptRequests { get; set; }
  }
}
