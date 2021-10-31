using System;

using lena.Domains.Enums;
namespace lena.Models.Notification
{
  public class NotificationResult
  {
    public long Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public bool IsSeen { get; set; }

    public DateTime RequestDate { get; set; }

    public DateTime SeenDate { get; set; }
    public byte[] RowVersion { get; set; }
    public int? ScrumEntityId { get; set; }
    public byte[] ScrumEntityRowVersion { get; set; }
  }
}
