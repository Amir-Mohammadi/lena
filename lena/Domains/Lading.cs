using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Lading : BaseEntity, IEntity
  {
    protected internal Lading()
    {
      this.LadingCustomhouseLogs = new HashSet<LadingCustomhouseLog>();
      this.LadingItems = new HashSet<LadingItem>();
      this.LadingCosts = new HashSet<LadingCost>();
      this.LadingBankOrderLogs = new HashSet<LadingBankOrderLog>();
      this.LadingChangeRequests = new HashSet<LadingChangeRequest>();
    }
    public bool HasReceiptLicence { get; set; }
    public LadingType Type { get; set; }
    public bool HasLadingChangeRequest { get; set; }
    public DateTime ReceiptLicenceDateTime { get; set; }
    public Nullable<int> BankOrderId { get; set; }
    public Nullable<double> CustomsValue { get; set; }
    public Nullable<double> ActualWeight { get; set; }
    public Nullable<long> BoxCount { get; set; }
    public string KotazhCode { get; set; }
    public string SataCode { get; set; }
    public short CityId { get; set; }
    public Nullable<DateTime> DeliveryDateTime { get; set; }
    public Nullable<DateTime> TransportDateTime { get; set; }
    public Nullable<short> CustomhouseId { get; set; }
    public bool IsLocked { get; set; }
    public Nullable<int> LadingBlockerId { get; set; }
    public bool NeedToCost { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual City City { get; set; }
    public virtual BankOrder BankOrder { get; set; }
    public virtual Customhouse Customhouse { get; set; }
    public virtual ICollection<LadingCustomhouseLog> LadingCustomhouseLogs { get; set; }
    public virtual LadingCustomhouseLog CurrentLadingCustomhouseLog { get; set; }
    public virtual ICollection<LadingItem> LadingItems { get; set; }
    public virtual ICollection<LadingCost> LadingCosts { get; set; }
    public virtual ICollection<LadingBankOrderLog> LadingBankOrderLogs { get; set; }
    public virtual LadingBankOrderLog CurrentLadingBankOrderLog { get; set; }
    public virtual LadingBlocker LadingBlocker { get; set; }
    public virtual ICollection<LadingChangeRequest> LadingChangeRequests { get; set; }
    public virtual BankOrderCurrencySource BankOrderCurrencySource { get; set; }
  }
}
