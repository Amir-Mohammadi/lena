using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.MeetingParticipant
{
  public class GetMeetingParticipantsInput : SearchInput<MeetingParticipantSortType>
  {
    public int? Id { get; set; }
    public int? MinutesMeetingId { get; set; }
    public int? ParticipantEmployeeId { get; set; }
    public string GuestParticipantName { get; set; }
    public GetMeetingParticipantsInput(PagingInput pagingInput, MeetingParticipantSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder)
    {
    }
  }
}