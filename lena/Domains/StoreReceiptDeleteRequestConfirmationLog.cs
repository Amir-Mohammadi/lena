using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class StoreReceiptDeleteRequestConfirmationLog : IEntity
  {
    public int Id { get; set; }
    public int StoreReceiptDeleteRequestId { get; set; }
    public int ConfirmerUserId { get; set; }
    public DateTime DateTime { get; set; }
    public string Description { get; set; }
    public StoreReceiptDeleteRequestStatus Status { get; set; }
    public byte[] RowVersion { get; set; }
    public User ConfirmerUser { get; set; }
    public virtual StoreReceiptDeleteRequest StoreReceiptDeleteRequest { get; set; }
  }
}