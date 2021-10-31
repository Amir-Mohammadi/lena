using System;

using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class PagingInfo
  {
    public PagingInfo(int totalCount, int pageSize, int pageNumber)
    {
      TotalCount = totalCount;
      PageSize = pageSize;
      PageNumber = pageNumber;
    }
    public int TotalCount { get; }
    public int PageSize { get; }
    public int PageNumber { get; }
    public int PageCount => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (decimal)PageSize) : 0;
  }
}
