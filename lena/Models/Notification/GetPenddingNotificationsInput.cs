using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Notification
{
  public class GetPenddingNotificationsInput : PagingInput
  {
    public string SearchText { get; set; }
    public GetPenddingNotificationsInput(string searchText, int pageNumber, int pageSize) : base(pageNumber, pageSize)
    {
      SearchText = searchText;
    }
  }
}
