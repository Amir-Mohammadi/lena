using System.Collections.Generic;
namespace core.Models
{
  public interface IPaginatedModel
  {
    int PageIndex { get; }
    int PageSize { get; }
    long TotalCount { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
  }
  public interface IPaginatedModel<T> : IPaginatedModel, IList<T>
  { }
}