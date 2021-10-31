using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.ReturnSerialToPreviousStateRequest
{
  public class GetReturnSerialToPreviousStateRequestsInput : SearchInput<ReturnSerialToPreviousStateRequestSortType>
  {
    public int Id { get; set; }
    public int Serial { get; set; }
    public int StuffId { get; set; }
    public int StuffCode { get; set; }
    public int UserId { get; set; }
    public int confirmerUserId { get; set; }
    public int WrongDoerUserId { get; set; }
    public string Description { get; set; }
    public GetReturnSerialToPreviousStateRequestsInput(PagingInput pagingInput, ReturnSerialToPreviousStateRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}