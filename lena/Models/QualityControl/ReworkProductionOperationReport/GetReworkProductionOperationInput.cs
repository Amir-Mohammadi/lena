using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ReworkProductionOperationReport
{
  public class GetReworkProductionOperationInput : SearchInput<ReworkProductionOperationSortType>
  {
    public GetReworkProductionOperationInput(PagingInput pagingInput, ReworkProductionOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? EmployeeId { get; set; }
    public int? OperationId { get; set; }
    public string OperationCode { get; set; }
    public bool GroupByEmployee { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }


  }
}
