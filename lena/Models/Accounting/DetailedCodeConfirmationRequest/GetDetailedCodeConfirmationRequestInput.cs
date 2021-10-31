using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Accounting.DetailedCodeConfirmationRequest
{
  public class GetDetailedCodeConfirmationRequestInput : SearchInput<DetailedCodeConfirmationRequestSortType>
  {
    public GetDetailedCodeConfirmationRequestInput(PagingInput pagingInput, DetailedCodeConfirmationRequestSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? BillOfMaterialStuffId { get; set; }
    public int? BillOfMaterialVersion { get; set; }
    public string Code { get; set; }
    public DetailCodeConfirmationStatus? Status { get; set; }



  }
}
