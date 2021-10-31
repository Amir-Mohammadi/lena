using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class ApiErrorContainer
  {
    public ApiErrorContainer(string errorNo)
    {
      errorno = errorNo;
      data = null;
    }

    public string errorno { get; set; }

    public object data { get; set; }
  }
}
