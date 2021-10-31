using System;
using System.Collections.Generic;
using core.Models;
using lena.Domains.Enums;
namespace lena.Domains
{
  public partial class Message : IEntity
  {
    protected internal Message()
    {
      this.Attachments = new HashSet<Attachment>();
    }
    public int Id { get; set; }
    public string Number { get; set; }
    public string SendDate { get; set; }
    public int SenderUserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsSent { get; set; }
    public MessageAccessType MessageAccessType { get; set; }
    public bool IsArchive { get; set; }
    public bool IsDelete { get; set; }
    public DateTime? DeletedAt { get; set; }
    public byte[] RowVersion { get; set; }
    public virtual ICollection<Attachment> Attachments { get; set; }
    public virtual User SenderUser { get; set; }
  }
}
