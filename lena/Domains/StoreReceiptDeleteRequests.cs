using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceiptDeleteRequest : IEntity
  {
    protected internal StoreReceiptDeleteRequest()
    {
      this.StoreReceiptDeleteRequestStuffSerials = new HashSet<StoreReceiptDeleteRequestStuffSerial>();
      this.StoreReceiptDeleteRequestConfirmationLogs = new HashSet<StoreReceiptDeleteRequestConfirmationLog>();
    }
    public int Id { get; set; }
    public int StoreReceiptId { get; set; }
    public DateTime DateTime { get; set; }
    public int UserId { get; set; }
    public StoreReceiptDeleteRequestStatus Status { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string Description { get; set; }
    public byte[] RowVersion { get; set; }
    public User User { get; set; }
    public virtual StoreReceipt StoreReceipt { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequestStuffSerial> StoreReceiptDeleteRequestStuffSerials { get; set; }
    public virtual ICollection<StoreReceiptDeleteRequestConfirmationLog> StoreReceiptDeleteRequestConfirmationLogs { get; set; }
  }
}