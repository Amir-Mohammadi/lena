using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceiptDeleteRequest : IEntity, IHasDescription, IRemovable
  {
    protected internal ExitReceiptDeleteRequest()
    {
      this.ExitReceiptDeleteRequestStuffSerials = new HashSet<ExitReceiptDeleteRequestStuffSerial>();
      this.ExitReceiptDeleteRequestConfirmationLogs = new HashSet<ExitReceiptDeleteRequestConfirmationLog>();
    }
    public int Id { get; set; }
    public string Description { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? ChangeStatusDateTime { get; set; }
    public int CreatorUserId { get; set; }
    public int? ChangeStatusUserId { get; set; }
    public User CreatorUser { get; set; }
    public User ChangeStatusUser { get; set; }
    public ExitReceiptDeleteRequestStatus Status { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int ExitReceiptId { get; set; }
    public ExitReceipt ExitReceipt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequestStuffSerial> ExitReceiptDeleteRequestStuffSerials { get; set; }
    public virtual ICollection<ExitReceiptDeleteRequestConfirmationLog> ExitReceiptDeleteRequestConfirmationLogs { get; set; }
  }
}
