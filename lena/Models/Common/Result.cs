using System.Collections.Generic;
using System.Linq;
using lena.Domains.Enums;
namespace lena.Models.Common
{
  public class Result
  {
    public Result()
    {
      Success = true;
    }
    public bool Success { get; set; }
    public string Message { get; set; }
  }
  public class Result<T> : Result
  {
    public T Data { get; set; }
    public dynamic DataInfo { get; set; }
    public Result(T data)
    {
      Data = data;
    }
    public Result()
    {
    }
    public Result(T data, dynamic dataInfo)
    {
      Data = data;
      DataInfo = dataInfo;
    }
  }
  public class ResultList<T> : Result<List<T>>
  {
    public PagingInfo PagingInfo { get; set; }
    public ResultList()
    {
    }
    public ResultList(IEnumerable<T> data, object dataInfo = null)
        : base(data.ToList(), dataInfo)
    {
    }
    public ResultList(IQueryable<T> data, object dataInfo = null) :
        base(data.ToList(), dataInfo)
    {
    }
    public ResultList(PagingInput pagingInput, IQueryable<T> pagingQuery, object dataInfo = null)
    {
      var totalCount = 0;
      //var sql = ( pagingQuery as ObjectQuery<T>).ToTraceString();
      if (pagingInput == null)
      {
        Data = pagingQuery.ToList();
        totalCount = Data.Count;
        return;
      }
      totalCount = pagingQuery.Count();
      if (totalCount < (pagingInput.PageNumber - 1) * pagingInput.PageSize)
        pagingInput.PageNumber = 1;
      var result = pagingQuery.Paging(pagingInput).AsQueryable();
      PagingInfo = new PagingInfo(
          totalCount: totalCount,
          pageSize: pagingInput.PageSize,
          pageNumber: pagingInput.PageNumber);
      Data = result.ToList();
      DataInfo = dataInfo;
    }
    public ResultList<object> ToObject()
    {
      var r = new ResultList<object>
      {
        DataInfo = DataInfo,
        Message = Message,
        PagingInfo = PagingInfo,
        Success = Success,
        Data = Data.Select(x => (object)x).ToList()
      };
      return r;
    }
  }
}