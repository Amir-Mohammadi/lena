using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class ExitReceiptDeleteRequestConfirmationLog : IEntity
  {
    public int Id { get; set; }
    public int ExitReceiptDeleteRequestId { get; set; }
    public int ConfirmerUserId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public ExitReceiptDeleteRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public User ConfirmerUser { get; set; }
    public virtual ExitReceiptDeleteRequest ExitReceiptDeleteRequest { get; set; }
  }
}
