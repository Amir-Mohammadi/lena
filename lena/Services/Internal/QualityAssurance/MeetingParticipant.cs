using System;
using System.Linq;
using lena.Domains;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.QualityAssurance.MeetingParticipant;
using lena.Services.Internals.Exceptions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public MeetingParticipant AddMeetingParticipant(
        int minutesMeetingId,
        int? participantEmployeeId,
        string guestParticipantName)
    {

      var meetingParticipant = repository.Create<MeetingParticipant>();
      meetingParticipant.MinutesMeetingId = minutesMeetingId;
      meetingParticipant.ParticipantEmployeeId = participantEmployeeId;
      meetingParticipant.GuestParticipantName = guestParticipantName;
      repository.Add(meetingParticipant);
      return meetingParticipant;
    }
    #endregion

    #region Get
    public MeetingParticipant GetMeetingParticipant(int id) => GetMeetingParticipant(selector: e => e, id: id);
    public TResult GetMeetingParticipant<TResult>(
        Expression<Func<MeetingParticipant, TResult>> selector,
        int id)
    {

      var meetingParticipant = GetMeetingParticipants(
                selector: selector,
                id: id).FirstOrDefault();
      if (meetingParticipant == null)
        throw new MeetingParticipantNotFoundException(id);
      return meetingParticipant;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetMeetingParticipants<TResult>(
        Expression<Func<MeetingParticipant, TResult>> selector,
        TValue<int> id = null,
        TValue<int> minutesMeetingId = null,
        TValue<int> participantEmployeeId = null,
        TValue<string> guestParticipantName = null
        )
    {

      var meetingParticipant = repository.GetQuery<MeetingParticipant>();
      if (id != null)
        meetingParticipant = meetingParticipant.Where(x => x.Id == id);
      if (minutesMeetingId != null)
        meetingParticipant = meetingParticipant.Where(i => i.MinutesMeetingId == minutesMeetingId);
      if (participantEmployeeId != null)
        meetingParticipant = meetingParticipant.Where(i => i.ParticipantEmployeeId == participantEmployeeId);
      if (guestParticipantName != null)
        meetingParticipant = meetingParticipant.Where(i => i.GuestParticipantName == guestParticipantName);
      return meetingParticipant.Select(selector);
    }
    #endregion

    #region Remove MeetingParticipant
    public void RemoveMeetingParticipant(int id, byte[] rowVersion)
    {

      var meetingParticipant = GetMeetingParticipant(id: id);

    }
    #endregion

    #region Delete MeetingParticipant
    public void DeleteMeetingParticipant(int id)
    {

      var meetingParticipant = GetMeetingParticipant(id: id);

      DeleteMeetingParticipant(meetingParticipant: meetingParticipant);
    }

    public void DeleteMeetingParticipant(MeetingParticipant meetingParticipant)
    {

      repository.Delete(meetingParticipant);
    }
    #endregion

    #region EditProcess
    public MeetingParticipant EditMeetingParticipantProcess(
        int id,
        byte[] rowVersion,
        TValue<int> participantEmployeeId = null,
        TValue<int> minutesMeetingId = null,
        TValue<string> guestParticipantName = null)
    {

      var meetingParticipant = GetMeetingParticipant(id: id);
      return EditMeetingParticipant(
                meetingParticipant: meetingParticipant,
                rowVersion: rowVersion,
                participantEmployeeId: participantEmployeeId,
                minutesMeetingId: minutesMeetingId,
                guestParticipantName: guestParticipantName);
    }
    public MeetingParticipant EditMeetingParticipant(
        MeetingParticipant meetingParticipant,
        byte[] rowVersion,
        TValue<int> participantEmployeeId = null,
        TValue<int> minutesMeetingId = null,
        TValue<string> guestParticipantName = null)
    {

      if (participantEmployeeId != null)
        meetingParticipant.ParticipantEmployeeId = participantEmployeeId;
      if (minutesMeetingId != null)
        meetingParticipant.MinutesMeetingId = minutesMeetingId;
      if (guestParticipantName != null)
        meetingParticipant.GuestParticipantName = guestParticipantName;
      repository.Update(meetingParticipant, rowVersion);
      return meetingParticipant;
    }
    #endregion

    #region Search
    public IQueryable<MeetingParticipantResult> SearchMeetingParticipant(IQueryable<MeetingParticipantResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.GuestParticipantName.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<MeetingParticipantResult> SortMeetingParticipantResult(IQueryable<MeetingParticipantResult> query,
        SortInput<MeetingParticipantSortType> sort)
    {
      switch (sort.SortType)
      {
        case MeetingParticipantSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case MeetingParticipantSortType.ParticipantEmployeeId:
          return query.OrderBy(a => a.ParticipantEmployeeId, sort.SortOrder);
        case MeetingParticipantSortType.MinutesMeetingId:
          return query.OrderBy(a => a.MinutesMeetingId);
        case MeetingParticipantSortType.GuestParticipantName:
          return query.OrderBy(a => a.GuestParticipantName);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToMeetingParticipantResult
    public Expression<Func<MeetingParticipant, MeetingParticipantResult>> ToMeetingParticipantResult =
        meetingParticipant => new MeetingParticipantResult
        {
          Id = meetingParticipant.Id,
          RowVersion = meetingParticipant.RowVersion,
          MinutesMeetingId = meetingParticipant.MinutesMeetingId,
          ParticipantEmployeeId = meetingParticipant.ParticipantEmployeeId,
          ParticipantEmployeeFullName = meetingParticipant.ParticipantEmployee.FirstName + " " + meetingParticipant.ParticipantEmployee.LastName,
          GuestParticipantName = meetingParticipant.GuestParticipantName,
        };
    #endregion
  }

}
