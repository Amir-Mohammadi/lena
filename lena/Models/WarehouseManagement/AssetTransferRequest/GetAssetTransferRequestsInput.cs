using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums;
namespace lena.Models
{
  public class GetAssetTransferRequestsInput : SearchInput<AssetTransferRequestSortType>
  {
    public int? Id { get; set; }
    public int? AssetId { get; set; }
    public string AssetCode { get; set; }
    public int? StuffId { get; set; }
    public int? NewEmployeeId { get; set; }
    public short? NewDepartmentId { get; set; }
    public GetAssetTransferRequestsInput(PagingInput pagingInput, AssetTransferRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

  }
}
