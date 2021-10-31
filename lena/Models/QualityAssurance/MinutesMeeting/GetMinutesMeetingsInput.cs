using lena.Models.Common;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Models.Common;
using System;
using lena.Domains.Enums;
namespace lena.Models.QualityAssurance.MinutesMeeting
{
  public class GetMinutesMeetingsInput : SearchInput<MinutesMeetingSortType>
  {
    public GetMinutesMeetingsInput(
        PagingInput pagingInput, MinutesMeetingSortType sortType, SortOrder sortOrder) : base(pagingInput, sortType, sortOrder
            )
    {
    }

    public int? Id { get; set; }
    public int? RegistrantUserId { get; set; }
    public string Agenda { get; set; }
    public string Place { get; set; }
    public int? SecretaryUserId { get; set; }
    public int? BossUserId { get; set; }
    public bool? IsConfidential { get; set; }
    public DateTime? FromRegistrationDateTime { get; set; }
    public DateTime? ToRegistrationDateTime { get; set; }
    public DateTime? FromMeetingDateTime { get; set; }
    public DateTime? ToMeetingDateTime { get; set; }

  }
}