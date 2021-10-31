using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using System;
using lena.Domains.Enums;

using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.MeetingApproval
{
  public class GetMeetingApprovalsInput : SearchInput<MeetingApprovalSortType>
  {
    public GetMeetingApprovalsInput(PagingInput pagingInput, MeetingApprovalSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
    public int? Id { get; set; }
    public int? MinutesMeetingId { get; set; }
    public string Title { get; set; }
    public int? OperatorUserId { get; set; }
    public int? DepartmentId { get; set; }
    public DateTime? FromActionDateTime { get; set; }
    public DateTime? ToActionDateTime { get; set; }
    public DateTime? FromActualDateTime { get; set; }
    public DateTime? ToActualDateTime { get; set; }
    public MeetingApprovalStatus? Status { get; set; }
    public string Description { get; set; }

  }
}