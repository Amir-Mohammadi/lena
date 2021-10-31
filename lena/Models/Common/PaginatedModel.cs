using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace core.Models
{
  public class PaginatedModel<T> : List<T>, IPaginatedModel<T>
  {
    private readonly bool zeroBased;
    private readonly int firstPageIndex;
    public PaginatedModel(IEnumerable<T> data, int pageIndex, int pageSize, long totalCount, bool zeroBased = false)
    {
      this.zeroBased = zeroBased;
      firstPageIndex = zeroBased ? 0 : 1;
      CalculatePageInfo(pageIndex: pageIndex,
                        pageSize: pageSize,
                        totalCount: totalCount);
      TotalCount = totalCount;
      PageSize = pageSize;
      PageIndex = pageIndex;
      AddRange(data);
    }
    private void CalculatePageInfo(int pageIndex, int pageSize, long totalCount)
    {
      this.TotalCount = totalCount;
      this.PageSize = pageSize;
      this.PageIndex = pageIndex;
      if (pageSize > 0 && pageIndex > firstPageIndex)
      {
        this.TotalPages = Convert.ToInt32(totalCount / pageSize);
        if (totalCount % pageSize > 0)
          TotalPages++;
      }
    }
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public long TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => PageIndex > firstPageIndex;
    public bool HasNextPage => PageIndex + (zeroBased ? 1 : 0) < TotalPages;
  }
}