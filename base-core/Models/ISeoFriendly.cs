namespace core.Models
{
  public interface ISeoFriendly
  {
    string UrlTitle { get; set; }
    string BrowserTitle { get; set; }
    string MetaDescription { get; set; }
  }
}