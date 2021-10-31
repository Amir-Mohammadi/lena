using lena.Domains.Enums;
namespace lena.Models.Notification
{
  public class SeenNotificationInput
  {
    public long Id { get; set; }
    public byte[] RowVersion { get; set; }
  }
}