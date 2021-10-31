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
using lena.Models.QualityAssurance.MinutesMeeting;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.QualityAssurance.Exception;
using lena.Services.Core;
using lena.Domains.Enums;
using lena.Models.QualityAssurance.MeetingApproval;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public MinutesMeeting AddMinutesMeeting(
        string place,
        string agenda,
        int secretaryUserId,
        int? bossUserId,
        bool isConfidential,
        DateTime? meetingDateTime
        )
    {

      var minutesMeeting = repository.Create<MinutesMeeting>();
      minutesMeeting.Agenda = agenda;
      minutesMeeting.BossUserId = bossUserId;
      minutesMeeting.IsConfidential = isConfidential;
      minutesMeeting.MeetingDateTime = meetingDateTime;
      minutesMeeting.Place = place;
      minutesMeeting.SecretaryUserId = secretaryUserId;
      minutesMeeting.RegistrationDateTime = DateTime.UtcNow;
      minutesMeeting.RegistrantUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(minutesMeeting);
      return minutesMeeting;
    }
    #endregion
    #region Add Process
    public void AddMinutesMeetingProcess(
        string place,
        string agenda,
        int secretaryUserId,
        int? bossUserId,
        bool isConfidential,
        DateTime? meetingDateTime,
        AddMeetingApprovalInput[] meetingApprovalInput,
        AddMeetingParticipantInput[] meetingParticipantInput)
    {

      #region Add MinutesMeeting
      var minutesMeeting = AddMinutesMeeting(
          place: place,
          agenda: agenda,
          secretaryUserId: secretaryUserId,
          bossUserId: bossUserId,
          isConfidential: isConfidential,
          meetingDateTime: meetingDateTime);
      #endregion
      #region Add MeetingApproval
      foreach (var item in meetingApprovalInput)
      {
        AddMeetingApproval(
                  title: item.Title,
                  operatorUserId: item.OperatorUserId,
                  guestOperatorName: item.GuestOperatorName,
                  actionDateTime: item.ActionDateTime,
                  actualDateTime: item.ActualDateTime,
                  description: item.Description,
                  minutesMeetingId: minutesMeeting.Id,
                  operatorDepartmentId: item.OperatorDepartmentId);
      }
      #endregion
      #region Add MeetingParticipant
      foreach (var item in meetingParticipantInput)
      {
        AddMeetingParticipant(
                  minutesMeetingId: minutesMeeting.Id,
                  participantEmployeeId: item.ParticipantEmployeeId,
                  guestParticipantName: item.GuestParticipantName);
      }
      #endregion
    }
    #endregion

    #region Get
    public MinutesMeeting GetMinutesMeeting(int id) => GetMinutesMeeting(selector: e => e, id: id);
    public TResult GetMinutesMeeting<TResult>(
        Expression<Func<MinutesMeeting, TResult>> selector,
        int id)
    {

      var minutesMeeting = GetMinutesMeetings(
                selector: selector,
                id: id).FirstOrDefault();
      if (minutesMeeting == null)
        throw new MinutesMeetingNotFoundException(id);
      return minutesMeeting;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetMinutesMeetings<TResult>(
        Expression<Func<MinutesMeeting, TResult>> selector,
        TValue<int> id = null,
        TValue<int> registrantUserId = null,
        TValue<DateTime> fromRegistrationDateTime = null,
        TValue<DateTime> toRegistrationDateTime = null,
        TValue<DateTime> fromMeetingDateTime = null,
        TValue<DateTime> toMeetingDateTime = null,
        TValue<string> agenda = null,
        TValue<string> place = null,
        TValue<int> secretaryUserId = null,
        TValue<int> bossUserId = null,
        TValue<bool> isConfidential = null
        )
    {

      var minutesMeetings = repository.GetQuery<MinutesMeeting>();
      if (id != null)
        minutesMeetings = minutesMeetings.Where(x => x.Id == id);
      if (registrantUserId != null)
        minutesMeetings = minutesMeetings.Where(i => i.RegistrantUserId == registrantUserId);
      if (fromRegistrationDateTime != null)
        minutesMeetings = minutesMeetings.Where(i => i.RegistrationDateTime >= fromRegistrationDateTime);
      if (toRegistrationDateTime != null)
        minutesMeetings = minutesMeetings.Where(i => i.RegistrationDateTime <= toRegistrationDateTime);
      if (fromMeetingDateTime != null)
        minutesMeetings = minutesMeetings.Where(i => i.MeetingDateTime >= fromMeetingDateTime);
      if (toMeetingDateTime != null)
        minutesMeetings = minutesMeetings.Where(i => i.MeetingDateTime <= toMeetingDateTime);
      if (place != null)
        minutesMeetings = minutesMeetings.Where(i => i.Place.Contains(place));
      if (agenda != null)
        minutesMeetings = minutesMeetings.Where(i => i.Agenda.Contains(agenda));
      if (secretaryUserId != null)
        minutesMeetings = minutesMeetings.Where(i => i.SecretaryUserId == secretaryUserId);
      if (bossUserId != null)
        minutesMeetings = minutesMeetings.Where(i => i.BossUserId == bossUserId);
      if (isConfidential != null)
        minutesMeetings = minutesMeetings.Where(i => i.IsConfidential == isConfidential);
      return minutesMeetings.Select(selector);
    }
    #endregion

    #region EditProcess
    public void EditMinutesMeetingProcess(
        int id,
        byte[] rowVersion,
        DateTime? meetingDateTime,
        string place,
        string agenda,
        int? secretaryUserId,
        int? bossUserId,
        bool? isConfidential,
        AddMeetingApprovalInput[] addMeetingApprovalInputs,
        EditMeetingApprovalInput[] editMeetingApprovalInputs,
        int[] meetingApprovalDeleteIds,
        AddMeetingParticipantInput[] addMeetingParticipantInputs,
        EditMeetingParticipantInput[] editMeetingParticipantInputs,
        int[] meetingParticipantDeleteIds)
    {

      #region GetMinutesMeeting
      var minutesMeeting = GetMinutesMeeting(id: id);
      #endregion

      #region EditMinutesMeeting
      EditMinutesMeeting(
          minutesMeeting: minutesMeeting,
          rowVersion: rowVersion,
          meetingDateTime: meetingDateTime,
          place: place,
          agenda: agenda,
          secretaryUserId: secretaryUserId,
          bossUserId: bossUserId,
          isConfidential: isConfidential
          );
      #endregion

      var meetingApprovalItems = minutesMeeting.MeetingAprovals.ToList();
      var meetingParticipantItems = minutesMeeting.MeetingParticipants.ToList();
      #region AddMeetingApproval

      foreach (var item in addMeetingApprovalInputs)
      {

        AddMeetingApproval(
                  title: item.Title,
                  operatorUserId: item.OperatorUserId,
                  guestOperatorName: item.GuestOperatorName,
                  actionDateTime: item.ActionDateTime,
                  actualDateTime: item.ActualDateTime,
                  description: item.Description,
                  minutesMeetingId: minutesMeeting.Id,
                  operatorDepartmentId: item.OperatorDepartmentId);
      }

      foreach (var item in editMeetingApprovalInputs)
      {
        var meetingApproval = meetingApprovalItems.FirstOrDefault(i => i.Id == item.Id);

        if (meetingApproval != null)
        {
          if (item.Title != meetingApproval.Title
                || item.OperatorUserId != meetingApproval.OperatorUserId
                || item.GuestOperatorName != meetingApproval.GuestOperatorName
                || item.ActionDateTime != meetingApproval.ActionDateTime
                || item.ActualDateTime != meetingApproval.ActualDateTime
                || item.Status != meetingApproval.Status
                || item.Description != meetingApproval.Description
                )
            EditMeetingApproval(meetingApproval: meetingApproval,
                      rowVersion: item.RowVersion,
                      title: item.Title,
                      operatorUserId: item.OperatorUserId,
                      guestOperatorName: item.GuestOperatorName,
                      actionDateTime: item.ActionDateTime,
                      actualDateTime: item.ActualDateTime,
                      status: item.Status,
                      description: item.Description,
                      uploadFileData: string.IsNullOrWhiteSpace(item.FileKey) ? null : Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey)
                      );
        }

      }

      foreach (var item in meetingApprovalDeleteIds)
      {
        var meetingApproval = meetingApprovalItems.FirstOrDefault(i => i.Id == item);
        if (meetingApproval != null)
        {
          DeleteMeetingApproval(meetingApproval: meetingApproval);
        }

      }
      foreach (var item in addMeetingParticipantInputs)
      {

        AddMeetingParticipant(
                  participantEmployeeId: item.ParticipantEmployeeId,
                  guestParticipantName: item.GuestParticipantName,
                  minutesMeetingId: minutesMeeting.Id);
      }

      foreach (var item in editMeetingParticipantInputs)
      {
        var meetingParticipant = meetingParticipantItems.FirstOrDefault(i => i.Id == item.Id);

        if (meetingParticipant != null)
        {
          if (item.ParticipantEmployeeId != meetingParticipant.ParticipantEmployeeId
                || item.GuestParticipantName != meetingParticipant.GuestParticipantName
                )
            EditMeetingParticipant(meetingParticipant: meetingParticipant,
                      rowVersion: item.RowVersion,
                      participantEmployeeId: item.ParticipantEmployeeId,
                      guestParticipantName: item.GuestParticipantName
                      );
        }

      }

      foreach (var item in meetingParticipantDeleteIds)
      {
        var meetingParticipant = meetingParticipantItems.FirstOrDefault(i => i.Id == item);
        if (meetingParticipant != null)
        {
          DeleteMeetingParticipant(meetingParticipant: meetingParticipant);
        }

      }


      #endregion

    }
    #endregion
    #region Edit
    public MinutesMeeting EditMinutesMeeting(
      int id,
      byte[] rowVersion,
        TValue<DateTime> meetingDateTime = null,
        TValue<string> place = null,
        TValue<string> agenda = null,
        TValue<int> secretaryUserId = null,
        TValue<int> bossUserId = null,
        TValue<bool> isConfidential = null)
    {

      var minutesMeeting = GetMinutesMeeting(id: id);
      return EditMinutesMeeting(
                minutesMeeting: minutesMeeting,
                rowVersion: rowVersion,
                meetingDateTime: meetingDateTime,
                place: place,
                agenda: agenda,
                secretaryUserId: secretaryUserId,
                bossUserId: bossUserId,
                isConfidential: isConfidential);
    }
    public MinutesMeeting EditMinutesMeeting(
        MinutesMeeting minutesMeeting,
        byte[] rowVersion,
        TValue<DateTime> meetingDateTime = null,
        TValue<string> place = null,
        TValue<string> agenda = null,
        TValue<int> secretaryUserId = null,
        TValue<int> bossUserId = null,
        TValue<bool> isConfidential = null)
    {

      if (meetingDateTime != null)
        minutesMeeting.MeetingDateTime = meetingDateTime;
      if (place != null)
        minutesMeeting.Place = place;
      if (agenda != null)
        minutesMeeting.Agenda = agenda;
      if (secretaryUserId != null)
        minutesMeeting.SecretaryUserId = secretaryUserId;
      if (bossUserId != null)
        minutesMeeting.BossUserId = bossUserId;
      if (isConfidential != null)
        minutesMeeting.IsConfidential = isConfidential;




      repository.Update(minutesMeeting, minutesMeeting.RowVersion);
      return minutesMeeting;
    }
    #endregion

    #region Delete
    public void DeleteMinutesMeeting(int id)
    {

      var minutesMeeting = GetMinutesMeeting(id: id);

      var meetingParticipants = GetMeetingParticipants(selector: e => e, minutesMeetingId: id);

      foreach (var participant in meetingParticipants)
      {
        if (participant != null)
        {
          repository.Delete(participant);
        }

      }

      var meetingApprovals = GetMeetingApprovals(selector: e => e, minutesMeetingId: id);

      foreach (var approval in meetingApprovals)
      {
        if (approval != null)
        {
          repository.Delete(approval);
        }
      }

      repository.Delete(minutesMeeting);
    }
    #endregion

    #region Search
    public IQueryable<MinutesMeetingResult> SearchMinutesMeeting(IQueryable<MinutesMeetingResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Agenda.Contains(searchText) ||
            item.Place.Contains(searchText) ||
            item.RegistrantUserFullName.Contains(searchText) ||
            item.SecretaryUserFullName.Contains(searchText) ||
            item.BossUserFullName.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<MinutesMeetingResult> SortMinutesMeetingResult(IQueryable<MinutesMeetingResult> query,
        SortInput<MinutesMeetingSortType> sort)
    {
      switch (sort.SortType)
      {
        case MinutesMeetingSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case MinutesMeetingSortType.Agenda:
          return query.OrderBy(a => a.Agenda, sort.SortOrder);
        case MinutesMeetingSortType.MeetingDateTime:
          return query.OrderBy(a => a.MeetingDateTime, sort.SortOrder);
        case MinutesMeetingSortType.BossUserId:
          return query.OrderBy(a => a.BossUserId, sort.SortOrder);
        case MinutesMeetingSortType.BossUserFullName:
          return query.OrderBy(a => a.BossUserFullName, sort.SortOrder);
        case MinutesMeetingSortType.IsConfidential:
          return query.OrderBy(a => a.IsConfidential, sort.SortOrder);
        case MinutesMeetingSortType.RegistrationDateTime:
          return query.OrderBy(a => a.RegistrationDateTime, sort.SortOrder);
        case MinutesMeetingSortType.Place:
          return query.OrderBy(a => a.Place, sort.SortOrder);
        case MinutesMeetingSortType.SecretaryUserId:
          return query.OrderBy(a => a.SecretaryUserId, sort.SortOrder);
        case MinutesMeetingSortType.SecretaryUserFullName:
          return query.OrderBy(a => a.SecretaryUserFullName, sort.SortOrder);
        case MinutesMeetingSortType.RegistrantUserId:
          return query.OrderBy(a => a.RegistrantUserId, sort.SortOrder);
        case MinutesMeetingSortType.RegistrantUserFullName:
          return query.OrderBy(a => a.RegistrantUserFullName, sort.SortOrder);
        case MinutesMeetingSortType.DepartmentId:
          return query.OrderBy(a => a.DepartmentId, sort.SortOrder);
        case MinutesMeetingSortType.DepartmentFullName:
          return query.OrderBy(a => a.DepartmentFullName);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToMinutesMeetingResult
    public Expression<Func<MinutesMeeting, MinutesMeetingResult>> ToMinutesMeetingResult =
        minutesMeeting => new MinutesMeetingResult
        {
          Id = minutesMeeting.Id,
          RegistrantUserId = minutesMeeting.RegistrantUserId,
          RegistrantUserFullName = minutesMeeting.RegistrantUser.Employee.FirstName + " " + minutesMeeting.RegistrantUser.Employee.LastName,
          RegistrationDateTime = minutesMeeting.RegistrationDateTime,
          MeetingDateTime = minutesMeeting.MeetingDateTime,
          Place = minutesMeeting.Place,
          Agenda = minutesMeeting.Agenda,
          SecretaryUserId = minutesMeeting.SecretaryUserId,
          SecretaryUserFullName = minutesMeeting.SecretaryUser.Employee.FirstName + " " + minutesMeeting.SecretaryUser.Employee.LastName,
          BossUserId = minutesMeeting.BossUserId,
          BossUserFullName = minutesMeeting.BossUser.Employee.FirstName + " " + minutesMeeting.BossUser.Employee.LastName,
          IsConfidential = minutesMeeting.IsConfidential,
          RowVersion = minutesMeeting.RowVersion
        };


    public IQueryable<MinutesMeetingResult> ToFullMinutesMeetingResult(
        IQueryable<MinutesMeeting> minutesMeetings,
        IQueryable<MeetingApproval> meetingApprovals)
    {
      var result = from minutesMeeting in minutesMeetings
                   join meetingApproval in meetingApprovals on
                   minutesMeeting.Id equals meetingApproval.MinutesMeetingId
                   select new MinutesMeetingResult
                   {
                     Id = minutesMeeting.Id,
                     RegistrantUserId = minutesMeeting.RegistrantUserId,
                     RegistrantUserFullName = minutesMeeting.RegistrantUser.Employee.FirstName + " " + minutesMeeting.RegistrantUser.Employee.LastName,
                     RegistrationDateTime = minutesMeeting.RegistrationDateTime,
                     MeetingDateTime = minutesMeeting.MeetingDateTime,
                     Place = minutesMeeting.Place,
                     Agenda = minutesMeeting.Agenda,
                     SecretaryUserId = minutesMeeting.SecretaryUserId,
                     SecretaryUserFullName = minutesMeeting.SecretaryUser.Employee.FirstName + " " + minutesMeeting.SecretaryUser.Employee.LastName,
                     BossUserId = minutesMeeting.BossUserId,
                     BossUserFullName = minutesMeeting.BossUser.Employee.FirstName + " " + minutesMeeting.BossUser.Employee.LastName,
                     IsConfidential = minutesMeeting.IsConfidential,
                     DepartmentId = meetingApproval.OperatorUser.Employee.Department.Id,
                     DepartmentFullName = meetingApproval.OperatorUser.Employee.Department.Name,
                     operatorDepartmentId = meetingApproval.Department.Id,
                     operatorDepartmentFullName = meetingApproval.Department.Name,
                     RowVersion = minutesMeeting.RowVersion
                   };

      return result;


    }
    #endregion

    #region toMinutesMeetingResult
    public Expression<Func<MinutesMeeting, MinutesMeetingResult>> ToMinuteMeetingsResult =
        minutesMeetingResult => new MinutesMeetingResult
        {
          Id = minutesMeetingResult.Id,
          RegistrantUserId = minutesMeetingResult.RegistrantUserId,
          RegistrantUserFullName = minutesMeetingResult.RegistrantUser.Employee.FirstName + " " + minutesMeetingResult.RegistrantUser.Employee.LastName,
          RegistrationDateTime = minutesMeetingResult.RegistrationDateTime,
          MeetingDateTime = minutesMeetingResult.MeetingDateTime,
          Place = minutesMeetingResult.Place,
          Agenda = minutesMeetingResult.Agenda,
          SecretaryUserId = minutesMeetingResult.SecretaryUserId,
          SecretaryUserFullName = minutesMeetingResult.SecretaryUser.Employee.FirstName + " " + minutesMeetingResult.SecretaryUser.Employee.LastName,
          BossUserId = minutesMeetingResult.BossUserId,
          BossUserFullName = minutesMeetingResult.BossUser.Employee.FirstName + " " + minutesMeetingResult.BossUser.Employee.LastName,
          IsConfidential = minutesMeetingResult.IsConfidential,
          RowVersion = minutesMeetingResult.RowVersion
        };

    #endregion
  }

}
