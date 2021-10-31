using lena.Domains.Enums;
namespace lena.Models.Notification
{
  public class SendNotificationInput
  {
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int? ScrumEntityId { get; set; }
  }
}