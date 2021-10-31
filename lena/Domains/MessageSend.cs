using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class MessageSend : IEntity
  {
    protected internal MessageSend()
    {
    }
    public int Id { get; set; }
    public int MessageId { get; set; }
    public int ReciverUserId { get; set; }
    public MessageSendType MessageSendType { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadDate { get; set; }
    public bool IsArchive { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual User ReciverUser { get; set; }
  }
}