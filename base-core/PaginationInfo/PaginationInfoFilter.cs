using core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
namespace core.PaginationInfo
{
  public class PaginationInfoFilter : IResultFilter
  {
    public void OnResultExecuted(ResultExecutedContext context)
    {
    }
    public void OnResultExecuting(ResultExecutingContext context)
    {
      var result = context.Result as ObjectResult;
      if (result?.Value is IPaginatedModel paginatedList)
      {
        var metadata = new
        {
          paginatedList.TotalCount,
          paginatedList.PageSize,
          paginatedList.PageIndex,
          paginatedList.TotalPages,
          paginatedList.HasNextPage,
          paginatedList.HasPreviousPage
        };
        context.HttpContext.Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
      }
    }
  }
}