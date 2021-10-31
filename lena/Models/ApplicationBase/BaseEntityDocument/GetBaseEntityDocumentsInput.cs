using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.ApplicationBase.BaseEntityDocument
{
  public class GetBaseEntityDocumentsInput : SearchInput<BaseEntityDocumentSortType>
  {
    public int? UserId { get; set; }
    public int? BaseEntityDocumentTypeId { get; set; }
    public int? CooperatorId { get; set; }
    public bool? IsDelete { get; set; }
    public int? EmployeeId { get; set; }
    public int? BaseEntityId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public EntityType? EntityType { get; set; }

    public GetBaseEntityDocumentsInput(PagingInput pagingInput, BaseEntityDocumentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
