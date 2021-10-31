using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.EntityConfirmation
{
  public class GetBaseEntityConfirmationInput : SearchInput<GetBaseEntityConfirmationSortType>
  {
    public GetBaseEntityConfirmationInput(PagingInput pagingInput, GetBaseEntityConfirmationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public ConfirmationStatus? Status { get; set; }
    public string ConfirmDescription { get; set; }
    public int? BaseEntityConfirmTypeId { get; set; }
    public int? UserId { get; set; }

  }
}
