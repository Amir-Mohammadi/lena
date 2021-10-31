using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.SerialFailedOperation
{
  public class GetSerialFailedOperationInput : SearchInput<SerialFailedOperationSortType>
  {
    public string ProductionOrderCode { get; set; }
    public string Serial { get; set; }
    public int? FailedInOperationId { get; set; }
    public DateTime? FromDateTime { get; set; }
    public DateTime? ToDateTime { get; set; }
    public int? stuffId { get; set; }
    public bool? IsRepaired { get; set; }
    public SerialFailedOperationStatus? Status { get; set; }
    public GetSerialFailedOperationInput(PagingInput pagingInput, SerialFailedOperationSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}
