using lena.Models.Common;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;

using lena.Domains.Enums;
namespace lena.Models.QualityControl.ConditionalQualityControl
{
  public class GetConditionalQualityControlsInput : SearchInput<ConditionalQualityControlSortType>
  {
    public GetConditionalQualityControlsInput(PagingInput pagingInput, ConditionalQualityControlSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }

    public int? QualityControlId { get; set; }
    public int? QualityControlAccepterId { get; set; }
    public int? StuffId { get; set; }
    public string Serial { get; set; }
    public QualityControlType? QualityControlType { get; set; }
    public int? WarehouseId { get; set; }
    public bool? ShowMyRelativeRequests { get; set; }
    public DateTime? ToDate { get; set; }
    public DateTime? FromDate { get; set; }

    public ConditionalQualityControlStatus? Status { get; set; }
    public ConditionalQualityControlStatus[] Statuses { get; set; }

    public int? QualityControlAccepterUserGroupId { get; set; }
    public int? ResponseConditionalConfirmationUserId { get; set; }
  }
}