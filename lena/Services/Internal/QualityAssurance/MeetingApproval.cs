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
using lena.Models.QualityAssurance.MeetingApproval;
using lena.Services.Internals.Exceptions;
using lena.Domains.Enums;
using lena.Services.Core;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add Process
    public MeetingApproval AddMeetingApprovalProcess(
        string title,
        int? operatorUserId,
        string guestOperatorName,
        DateTime? actionDateTime,
        DateTime? actualDateTime,
        int minutesMeetingId,
        string description,
        short? operatorDepartmentId)
    {

      var meetingApproval = AddMeetingApproval(
                title: title,
                operatorUserId: operatorUserId,
                guestOperatorName: guestOperatorName,
                actionDateTime: actionDateTime,
                actualDateTime: actualDateTime,
                minutesMeetingId: minutesMeetingId,
                description: description,
                operatorDepartmentId: operatorDepartmentId);
      return meetingApproval;
    }
    #endregion
    #region Add
    public MeetingApproval AddMeetingApproval(
        string title,
        int? operatorUserId,
        string guestOperatorName,
        int minutesMeetingId,
        DateTime? actionDateTime,
        DateTime? actualDateTime,
        string description,
        short? operatorDepartmentId)
    {

      var meetingApproval = repository.Create<MeetingApproval>();
      meetingApproval.Title = title;
      meetingApproval.OperatorUserId = operatorUserId;
      meetingApproval.GuestOperatorName = guestOperatorName;
      meetingApproval.ActionDateTime = actionDateTime;
      meetingApproval.ActualDateTime = actualDateTime;
      meetingApproval.Description = description;
      meetingApproval.DocumentId = null;
      meetingApproval.Status = MeetingApprovalStatus.NotAction;
      meetingApproval.MinutesMeetingId = minutesMeetingId;
      meetingApproval.OperatorDepartmentId = operatorDepartmentId;
      repository.Add(meetingApproval);
      return meetingApproval;
    }
    #endregion
    #region Get
    public MeetingApproval GetMeetingApproval(int id) => GetMeetingApproval(selector: e => e, id: id);
    public TResult GetMeetingApproval<TResult>(
        Expression<Func<MeetingApproval, TResult>> selector,
        int id)
    {

      var meetingApproval = GetMeetingApprovals(
                selector: selector,
                id: id).FirstOrDefault();
      if (meetingApproval == null)
        throw new MeetingApprovalNotFoundException(id);
      return meetingApproval;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetMeetingApprovals<TResult>(
        Expression<Func<MeetingApproval, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<int> minutesMeetingId = null,
        TValue<int> operatorUserId = null,
        TValue<int> departmentId = null,
        TValue<string> guestOperatorName = null,
        TValue<DateTime> fromActionDateTime = null,
        TValue<DateTime> toActionDateTime = null,
        TValue<DateTime> fromActualDateTime = null,
        TValue<DateTime> toActualDateTime = null,
        TValue<MeetingApprovalStatus> status = null,
        TValue<string> description = null,
        TValue<short> operatorDepartmentId = null)
    {

      var meetingApprovals = repository.GetQuery<MeetingApproval>();
      if (id != null)
        meetingApprovals = meetingApprovals.Where(x => x.Id == id);
      if (title != null)
        meetingApprovals = meetingApprovals.Where(i => i.Title == title);
      if (minutesMeetingId != null)
        meetingApprovals = meetingApprovals.Where(i => i.MinutesMeetingId == minutesMeetingId);
      if (operatorUserId != null)
        meetingApprovals = meetingApprovals.Where(i => i.OperatorUserId == operatorUserId);
      if (guestOperatorName != null)
        meetingApprovals = meetingApprovals.Where(i => i.GuestOperatorName == guestOperatorName);
      if (departmentId != null)
        meetingApprovals = meetingApprovals.Where(i => i.OperatorUser.Employee.DepartmentId == departmentId);
      if (fromActionDateTime != null)
        meetingApprovals = meetingApprovals.Where(i => i.ActionDateTime >= fromActionDateTime);
      if (toActionDateTime != null)
        meetingApprovals = meetingApprovals.Where(i => i.ActionDateTime <= toActionDateTime);
      if (fromActualDateTime != null)
        meetingApprovals = meetingApprovals.Where(i => i.ActualDateTime >= fromActualDateTime);
      if (toActualDateTime != null)
        meetingApprovals = meetingApprovals.Where(i => i.ActualDateTime <= toActualDateTime);
      if (status != null)
        meetingApprovals = meetingApprovals.Where(i => i.Status == status);
      if (description != null)
        meetingApprovals = meetingApprovals.Where(i => i.Description == description);
      if (operatorDepartmentId != null)
        meetingApprovals = meetingApprovals.Where(i => i.OperatorDepartmentId == operatorDepartmentId);
      return meetingApprovals.Select(selector);
    }
    #endregion
    #region Remove MeetingApproval
    public void RemoveMeetingApproval(int id, byte[] rowVersion)
    {

      var meetingApproval = GetMeetingApproval(id: id);
    }
    #endregion
    #region Delete MeetingApproval
    public void DeleteMeetingApproval(int id)
    {

      var meetingApproval = GetMeetingApproval(id: id);
      DeleteMeetingApproval(meetingApproval: meetingApproval);
    }
    public void DeleteMeetingApproval(MeetingApproval meetingApproval)
    {

      repository.Delete(meetingApproval);
    }
    #endregion
    #region EditProcess
    public MeetingApproval EditMeetingApprovalProcess(
        int id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<int> operatorUserId = null,
        TValue<string> guestOperatorName = null,
        TValue<int> minutesMeetingId = null,
        TValue<DateTime> actualDateTime = null,
        TValue<DateTime> actionDateTime = null,
        TValue<string> description = null,
        TValue<UploadFileData> uploadFileData = null,
        TValue<MeetingApprovalStatus> status = null,
        TValue<short> operatorDepartmentId = null,
        TValue<DateTime> approvalDateTime = null)
    {

      var meetingApproval = GetMeetingApproval(id: id);
      return EditMeetingApproval(
                meetingApproval: meetingApproval,
                rowVersion: rowVersion,
                title: title,
                operatorUserId: operatorUserId,
                guestOperatorName: guestOperatorName,
                minutesMeetingId: minutesMeetingId,
                actionDateTime: actionDateTime,
                actualDateTime: actualDateTime,
                description: description,
                uploadFileData: uploadFileData,
                operatorDepartmentId: operatorDepartmentId);
    }
    public MeetingApproval EditMeetingApproval(
        MeetingApproval meetingApproval,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<int> operatorUserId = null,
        TValue<string> guestOperatorName = null,
        TValue<int> minutesMeetingId = null,
        TValue<DateTime> actualDateTime = null,
        TValue<DateTime> actionDateTime = null,
        TValue<string> description = null,
        TValue<UploadFileData> uploadFileData = null,
        TValue<MeetingApprovalStatus> status = null,
        TValue<short> operatorDepartmentId = null)
    {

      if (uploadFileData != null)
      {
        if (meetingApproval.DocumentId != null)
          App.Internals.ApplicationBase.DeleteDocument(meetingApproval.DocumentId.Value);
        var document = App.Internals.ApplicationBase.AddDocument(
                      name: uploadFileData.Value.FileName,
                      fileStream: uploadFileData.Value.FileData);
        meetingApproval.DocumentId = document.Id;
      }
      if (title != null)
        meetingApproval.Title = title;
      if (operatorUserId != null)
        meetingApproval.OperatorUserId = operatorUserId;
      if (guestOperatorName != null)
        meetingApproval.GuestOperatorName = guestOperatorName;
      if (minutesMeetingId != null)
        meetingApproval.MinutesMeetingId = minutesMeetingId;
      if (actualDateTime != null)
        meetingApproval.ActualDateTime = actualDateTime;
      if (actionDateTime != null)
        meetingApproval.ActionDateTime = actionDateTime;
      if (description != null)
        meetingApproval.Description = description;
      if (status != null)
        meetingApproval.Status = status;
      if (operatorDepartmentId != null)
        meetingApproval.OperatorDepartmentId = operatorDepartmentId;
      repository.Update(meetingApproval, rowVersion);
      return meetingApproval;
    }
    #endregion
    #region Search
    public IQueryable<MeetingApprovalResult> SearchMeetingApproval(IQueryable<MeetingApprovalResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems
        )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Title.Contains(searchText) ||
            item.OperatorUserFullName.Contains(searchText) ||
            item.GuestOperatorName.Contains(searchText) ||
            item.DepartmentName.Contains(searchText) ||
            item.Description.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<MeetingApprovalResult> SortMeetingApprovalResult(IQueryable<MeetingApprovalResult> query,
        SortInput<MeetingApprovalSortType> sort)
    {
      switch (sort.SortType)
      {
        case MeetingApprovalSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case MeetingApprovalSortType.Title:
          return query.OrderBy(a => a.Title, sort.SortOrder);
        case MeetingApprovalSortType.Status:
          return query.OrderBy(a => a.Status);
        case MeetingApprovalSortType.OperatorUserId:
          return query.OrderBy(a => a.OperatorUserId);
        case MeetingApprovalSortType.GuestOperatorName:
          return query.OrderBy(a => a.GuestOperatorName);
        case MeetingApprovalSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName);
        case MeetingApprovalSortType.MinutesMeetingId:
          return query.OrderBy(a => a.MinutesMeetingId);
        case MeetingApprovalSortType.Description:
          return query.OrderBy(a => a.Description);
        case MeetingApprovalSortType.ActualDateTime:
          return query.OrderBy(a => a.ActualDateTime, sort.SortOrder);
        case MeetingApprovalSortType.ActionDateTime:
          return query.OrderBy(a => a.ActionDateTime);
        case MeetingApprovalSortType.HasUploadedDocument:
          return query.OrderBy(a => a.HasUploadedDocument);
        case MeetingApprovalSortType.OperatorDepartmentId:
          return query.OrderBy(a => a.OperatorUserId);
        case MeetingApprovalSortType.operatorDepartmentFullName:
          return query.OrderBy(a => a.OperatorUserFullName);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToMeetingApprovalResult
    public Expression<Func<MeetingApproval, MeetingApprovalResult>> ToMeetingApprovalResult =
        meetingApproval => new MeetingApprovalResult
        {
          Id = meetingApproval.Id,
          Title = meetingApproval.Title,
          OperatorUserId = meetingApproval.OperatorUserId,
          OperatorUserFullName = meetingApproval.OperatorUser.Employee.FirstName + " " + meetingApproval.OperatorUser.Employee.LastName,
          DepartmentName = meetingApproval.OperatorUser.Employee.Department.Name,
          GuestOperatorName = meetingApproval.GuestOperatorName,
          MinutesMeetingId = meetingApproval.MinutesMeetingId,
          ActionDateTime = meetingApproval.ActionDateTime,
          ActualDateTime = meetingApproval.ActualDateTime,
          Status = meetingApproval.Status,
          Description = meetingApproval.Description,
          DocumentId = meetingApproval.DocumentId,
          HasUploadedDocument = meetingApproval.DocumentId != null,
          OperatorDepartmentId = meetingApproval.Department.Id,
          OperatorDepartmentFullName = meetingApproval.Department.Name,
          RowVersion = meetingApproval.RowVersion
        };
    #endregion
  }
}