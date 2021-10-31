using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Planning.BillOfMaterialPublishRequest
{
  public class GetBillOfMaterialPublishRequestInput : SearchInput<BillOfMaterialPublishRequestSortType>
  {
    public GetBillOfMaterialPublishRequestInput(PagingInput pagingInput, BillOfMaterialPublishRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? BillOfMaterialStuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string Code { get; set; }
    public BillOfMaterialPublishRequestStatus? Status { get; set; }



  }
}
