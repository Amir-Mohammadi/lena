using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.ProjectManagement.ProjectERPEventDocument
{
  public class GetProjectERPEventDocumentInput : SearchInput<ProjectERPEventDocumentSortType>
  {
    public int? Id { get; set; }
    public int? ProjectERPEventId { get; set; }
    public Guid? DocumentId { get; set; }

    public GetProjectERPEventDocumentInput(PagingInput pagingInput, ProjectERPEventDocumentSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
