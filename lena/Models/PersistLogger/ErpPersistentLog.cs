using lena.Domains.Enums;
namespace lena.Models.PersistLogger
{
  public class ErpPersistentLog
  {
    public string Url { get; set; }
    public string Method { get; set; }
    public object Params { get; set; }
  }
}
