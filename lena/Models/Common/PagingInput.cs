
using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class PagingInput
  {
    public int PageNumber { get; internal set; }
    public int PageSize { get; internal set; }
    public PagingInput(int pageNumber, int pageSize)
    {
      this.PageSize = pageSize;
      PageNumber = pageNumber;
    }
  }
}
