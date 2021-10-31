using System;
using lena.Domains.Enums;
using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;

using lena.Domains.Enums;
namespace lena.Models.Production.ProductionEmployeeCostReport
{
  public class GetProductionEmployeeCostReportInput : SearchInput<ProductionEmployeeCostReportSortType>
  {
    public DateTime FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public double EmployeeCostPerHour { get; set; } // مبلغ نفر ساعت
    public int? StuffId { get; set; }
    public GetProductionEmployeeCostReportInput(PagingInput pagingInput, ProductionEmployeeCostReportSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
