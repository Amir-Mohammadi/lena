using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Domains.Enums;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ScrumManagement.Exception;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ScrumManagement.ScrumTask;
using lena.Models.UserManagement.User;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ScrumManagement
{
  public partial class ScrumManagement
  {
    public ScrumTask AddScrumTask(
        ScrumTask scrumTask,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumTaskTypeId,
        int? userId,
        long spentTime,
        long remainedTime,
        ScrumTaskStep scrumTaskStep,
        int scrumBackLogId,
        int? baseEntityId)
    {
      scrumTask = scrumTask ?? repository.Create<ScrumTask>();
      scrumTask.ScrumTaskTypeId = scrumTaskTypeId;
      scrumTask.UserId = userId;
      scrumTask.SpentTime = spentTime;
      scrumTask.RemainedTime = remainedTime;
      scrumTask.ScrumBackLogId = scrumBackLogId;
      scrumTask.ScrumTaskStep = scrumTaskStep;
      AddScrumEntity(scrumEntity: scrumTask,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    baseEntityId: baseEntityId);
      return scrumTask;
    }
    #region AddScrumTaskProcess
    public ScrumTask AddScrumTaskProcess(
        ScrumTask scrumTask,
        string name,
        string description,
        string color,
        short departmentId,
        long estimatedTime,
        bool isCommit,
        int scrumTaskTypeId,
        int? userId,
        long spentTime,
        long remainedTime,
        ScrumTaskStep scrumTaskStep,
        int scrumBackLogId,
        int? baseEntityId)
    {
      #region AddScrumTask
      AddScrumTask(
              scrumTask: scrumTask,
              name: name,
              description: description,
              color: color,
              departmentId: departmentId,
              estimatedTime: estimatedTime,
              isCommit: isCommit,
              scrumTaskTypeId: scrumTaskTypeId,
              userId: userId,
              spentTime: spentTime,
              remainedTime: remainedTime,
              scrumTaskStep: scrumTaskStep,
              scrumBackLogId: scrumBackLogId,
              baseEntityId: baseEntityId);
      #endregion
      #region Notify
      if (userId == null)
      {
        #region Notify to department
        //App.Internals.Notification.NotifyToDepartment(
        //    departmentId: departmentId,
        //    title: name,
        //    description: description,
        //    scrumEntityId: scrumTask.Id)
        //    
        //;
        //App.Internals.Notification.EmitToDepartment(
        //    departmentId: departmentId,
        //    eventKey: SystemEvents.OnScrumTasksChange,
        //    payload: null
        //    )
        //;
        #endregion
      }
      else
      {
        #region Notify to user
        App.Internals.Notification.NotifyToUser(
            userId: (int)userId,
            title: name,
            description: description,
            scrumEntityId: scrumTask.Id);
        App.Internals.Notification.Emit(
                  userId: (int)userId,
                         eventKey: SystemEvents.OnScrumTasksChange,
                  payload: null
                  );
        #endregion
      }
      #endregion
      return scrumTask;
    }
    #endregion
    public ScrumTask EditScrumTask(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<int> scrumBackLogId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {
      var scrumTask = GetScrumTask(id: id);
      return
                EditScrumTask(
                        scrumTask: scrumTask,
                        rowVersion: rowVersion,
                        code: code,
                        name: name,
                        description: description,
                        color: color,
                        departmentId: departmentId,
                        estimatedTime: estimatedTime,
                        isCommit: isCommit,
                        scrumTaskTypeId: scrumTaskTypeId,
                        userId: userId,
                        spentTime: spentTime,
                        remainedTime: remainedTime,
                        scrumTaskStep: scrumTaskStep,
                        scrumBackLogId: scrumBackLogId,
                        isDelete: isDelete,
                        isArchive: isArchive,
                        baseEntityId: baseEntityId);
    }
    public ScrumTask EditScrumTask(
        ScrumTask scrumTask,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<short> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<int> scrumBackLogId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null)
    {
      if (scrumTaskTypeId != null)
        scrumTask.ScrumTaskTypeId = scrumTaskTypeId;
      if (userId != null)
        scrumTask.UserId = userId;
      if (spentTime != null)
        scrumTask.SpentTime = spentTime;
      if (remainedTime != null)
        scrumTask.RemainedTime = remainedTime;
      if (scrumBackLogId != null)
        scrumTask.ScrumBackLogId = scrumBackLogId;
      if (scrumTaskStep != null)
        scrumTask.ScrumTaskStep = scrumTaskStep;
      var retValue = EditScrumEntity(
                    scrumEntity: scrumTask,
                    rowVersion: rowVersion,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    departmentId: departmentId,
                    estimatedTime: estimatedTime,
                    isCommit: isCommit,
                    isDelete: isDelete,
                    isArchive: isArchive,
                    baseEntityId: baseEntityId);
      return retValue as ScrumTask;
    }
    public IQueryable<TResult> GetScrumTasks<TResult>(
        Expression<Func<ScrumTask, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<string> color = null,
        TValue<int> departmentId = null,
        TValue<long> estimatedTime = null,
        TValue<bool> isCommit = null,
        TValue<int> scrumTaskTypeId = null,
        TValue<int?> userId = null,
        TValue<long> spentTime = null,
        TValue<long> remainedTime = null,
        TValue<ScrumTaskStep> scrumTaskStep = null,
        TValue<ScrumTaskStep[]> scrumTaskSteps = null,
        TValue<int> scrumBackLogId = null,
        TValue<bool> isDelete = null,
        TValue<bool> isArchive = null,
        TValue<int?> baseEntityId = null,
        TValue<short?> recursiveParentDepartmentId = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null)
    {
      var baseQuery = GetScrumEntities(
                    id: id,
                    code: code,
                    name: name,
                    description: description,
                    color: color,
                    isCommit: isCommit,
                    estimatedTime: estimatedTime,
                    departmentId: departmentId,
                    isDelete: isDelete,
                    isArchive: isArchive,
                    baseEntityId: baseEntityId);
      var query = baseQuery.OfType<ScrumTask>();
      if (scrumTaskTypeId != null)
        query = query.Where(i => i.ScrumTaskTypeId == scrumTaskTypeId);
      if (userId != null)
        query = query.Where(i => i.UserId == userId);
      if (spentTime != null)
        query = query.Where(i => i.SpentTime == spentTime);
      if (remainedTime != null)
        query = query.Where(i => i.RemainedTime == remainedTime);
      if (scrumBackLogId != null)
        query = query.Where(i => i.ScrumBackLogId == scrumBackLogId);
      if (scrumTaskStep != null)
        query = query.Where(i => i.ScrumTaskStep == scrumTaskStep);
      if (scrumTaskSteps != null)
        query = query.Where(i => scrumTaskSteps.Value.Contains(i.ScrumTaskStep));
      if (recursiveParentDepartmentId != null)
      {
        var departmentIds = App.Internals.ApplicationBase.GetDepartments(
                  recursiveParentDepartmentId: recursiveParentDepartmentId)
              .Select(i => i.Id).ToList();
        query = query.Where(i => departmentIds.Contains(i.DepartmentId));
      }
      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);
      return query.Select(selector);
    }
    public ScrumTask GetScrumTask(int id) => GetScrumTask(selector: e => e, id: id);
    public TResult GetScrumTask<TResult>(
        Expression<Func<ScrumTask, TResult>> selector,
        int id)
    {
      var scrumTask = GetScrumTasks(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (scrumTask == null)
        throw new ScrumTaskNotFoundException(id);
      return scrumTask;
    }
    public void DeleteScrumTask(int id)
    {
      var scrumTask = GetScrumTask(id);
      repository.Delete(scrumTask);
    }
    public IQueryable<ScrumTaskResult> SearchScrumTaskResult(IQueryable<ScrumTaskResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.Code.Contains(searchText) ||
                item.Name.Contains(searchText) ||
                item.Description.Contains(searchText) ||
                item.DepartmentName.Contains(searchText) ||
                item.UserName.Contains(searchText) ||
                item.EmployeeName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public IOrderedQueryable<ScrumTaskResult> SortScrumTaskResult(IQueryable<ScrumTaskResult> query, SortInput<ScrumTaskSortType> sort)
    {
      switch (sort.SortType)
      {
        case ScrumTaskSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ScrumTaskSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case ScrumTaskSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case ScrumTaskSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case ScrumTaskSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, sort.SortOrder);
        case ScrumTaskSortType.EstimatedTime:
          return query.OrderBy(a => a.EstimatedTime, sort.SortOrder);
        case ScrumTaskSortType.SpentTime:
          return query.OrderBy(a => a.SpentTime, sort.SortOrder);
        case ScrumTaskSortType.UserName:
          return query.OrderBy(a => a.UserName, sort.SortOrder);
        case ScrumTaskSortType.EmployeeName:
          return query.OrderBy(a => a.EmployeeName, sort.SortOrder);
        case ScrumTaskSortType.RemainingTime:
          return query.OrderBy(a => a.RemainedTime, sort.SortOrder);
        case ScrumTaskSortType.IsArchive:
          return query.OrderBy(a => a.IsArchive, sort.SortOrder);
        case ScrumTaskSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case ScrumTaskSortType.ScrumTaskTypeName:
          return query.OrderBy(a => a.ScrumTaskTypeName, sort.SortOrder);
        case ScrumTaskSortType.CreationDateTime:
          return query.OrderBy(a => a.CreationDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public Expression<Func<ScrumTask, ScrumTaskResult>> ToScrumTaskResult =
                            scrumTask => new ScrumTaskResult
                            {
                              Id = scrumTask.Id,
                              Code = scrumTask.Code,
                              Name = scrumTask.Name,
                              Description = scrumTask.Description,
                              Color = scrumTask.Color,
                              DepartmentId = scrumTask.DepartmentId,
                              DepartmentName = scrumTask.Department.Name,
                              UserId = scrumTask.UserId,
                              UserName = scrumTask.User.UserName,
                              EmployeeCode = scrumTask.User.Employee.Code,
                              EmployeeName = scrumTask.User.Employee.FirstName + " " + scrumTask.User.Employee.LastName,
                              EmployeePicture = scrumTask.User.Employee.Image,
                              EstimatedTime = scrumTask.EstimatedTime,
                              RemainedTime = scrumTask.RemainedTime,
                              SpentTime = scrumTask.SpentTime,
                              ScrumBackLogId = scrumTask.ScrumBackLogId,
                              ScrumTaskTypeId = scrumTask.ScrumTaskTypeId,
                              ScrumTaskTypeName = scrumTask.ScrumTaskType.Name,
                              Status = scrumTask.ScrumTaskStep,
                              IsCommit = scrumTask.IsCommit,
                              IsDelete = scrumTask.IsDelete,
                              IsArchive = scrumTask.IsArchive,
                              IsMyTask = scrumTask.UserId == App.Providers.Security.CurrentLoginData.UserId,
                              BaseEntityId = scrumTask.BaseEntityId,
                              CreationDateTime = scrumTask.DateTime,
                              RowVersion = scrumTask.RowVersion
                            };
    /// <summary>
    /// To assign task to logined user set userId Null
    /// </summary>
    /// <param name="rowVersion"></param>
    /// <param name="id"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    public ScrumTask AssignScrumTask(byte[] rowVersion, int id, int? userId)
    {
      var _userId = userId != null ? userId : App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString()).UserId;
      return EditScrumTask(
                    id: id,
                    rowVersion: rowVersion,
                    userId: _userId);
    }
    public void AssignScrumTasks(AssignScrumTasksInput assignScrumTasksInput)
    {
      foreach (var scrumTasksInput in assignScrumTasksInput.AssignScrumTaskInput)
      {
        var _userId = scrumTasksInput.UserId != null ? scrumTasksInput.UserId : App.Providers.Session.GetAs<LoginResult>(SessionKey.UserCredentials.ToString()).UserId;
        EditScrumTask(
                     id: scrumTasksInput.Id,
                     rowVersion: scrumTasksInput.RowVersion,
                     userId: _userId);
      }
    }
    public ScrumTask UnassignScrumTask(byte[] rowVersion, int id)
    {
      return EditScrumTask(rowVersion: rowVersion, id: id, userId: new TValue<int?>(null));
    }
    public ScrumTask BeginScrumTask(
        int id,
        byte[] rowVersion)
    {
      return EditScrumTask(
                    id: id,
                    rowVersion: rowVersion,
                    scrumTaskStep: ScrumTaskStep.InProcess);
    }
    public TResult BeginScrumTaskProcess<TResult>(
        Expression<Func<ScrumTask, TResult>> selector,
        int id,
        byte[] rowVersion)
    {
      var scrumTask = AssignScrumTask(
                    rowVersion: rowVersion,
                    id: id,
                    userId: null);
      BeginScrumTask(
                id: id,
                    rowVersion: scrumTask.RowVersion);
      return GetScrumTask(
                    selector: selector,
                    id: id);
    }
    public ScrumTask DoneScrumTask(
        int id,
        byte[] rowVersion)
    {
      var scrumTask = EditScrumTask(
                id: id,
                rowVersion: rowVersion,
                scrumTaskStep: ScrumTaskStep.Done);
      #region Notify
      #region Notify to department
      App.Internals.Notification.EmitToDepartment(
          departmentId: scrumTask.DepartmentId,
          eventKey: SystemEvents.OnScrumTasksChange,
          payload: null
      );
      #endregion
      if (scrumTask.UserId != null)
      {
        #region Notify to user
        App.Internals.Notification.Emit(
            userId: (int)scrumTask.UserId,
            eventKey: SystemEvents.OnScrumTasksChange,
            payload: null
        );
        #endregion
      }
      #endregion
      return scrumTask;

    }
    public ScrumTask DoneScrumTask(
        ScrumTask scrumTask,
        byte[] rowVersion)
    {
      #region set `IsSeen = true` for notifications that are related to this scrum task
      var notifications = App.Internals.
                         Notification.GetNotifications(e => new { e.Id, e.RowVersion }, scrumEntityId: scrumTask.Id);
      foreach (var notification in notifications)
      {
        App.Internals.Notification
              .SeenNotificationProcess(id: notification.Id, rowVersion: notification.RowVersion);
      }
      #endregion
      scrumTask = EditScrumTask(
              scrumTask: scrumTask,
              rowVersion: rowVersion,
              scrumTaskStep: ScrumTaskStep.Done);
      #region Notify
      #region Notify to department
      App.Internals.Notification.EmitToDepartment(
              departmentId: scrumTask.DepartmentId,
              eventKey: SystemEvents.OnScrumTasksChange,
              payload: null
          );
      #endregion
      if (scrumTask.UserId != null)
      {
        #region Notify to user
        App.Internals.Notification.Emit(
                userId: (int)scrumTask.UserId,
                eventKey: SystemEvents.OnScrumTasksChange,
                payload: null
            );
        #endregion
      }
      #endregion
      return scrumTask;
    }
    public ScrumTask GetBaseEntityScrumTask(
        int baseEntityId,
        ScrumTaskTypes scrumTaskType
        )
    {
      var scrumTasks = GetScrumTasks(
                    selector: e => e,
                    baseEntityId: baseEntityId,
                    scrumTaskTypeId: (int)scrumTaskType,
                    isDelete: false);
      var scrumTask = scrumTasks.FirstOrDefault(i => i.ScrumTaskStep == ScrumTaskStep.ToDo || i.ScrumTaskStep == ScrumTaskStep.InProcess);
      return scrumTask;
    }
    public ScrumTask GetBaseEntityDoneScrumTask(
        int baseEntityId,
        ScrumTaskTypes scrumTaskType)
    {
      var scrumTasks = GetScrumTasks(
                    selector: e => e,
                    baseEntityId: baseEntityId,
                    scrumTaskTypeId: (int)scrumTaskType,
                    isDelete: false);
      var scrumTask = scrumTasks.FirstOrDefault(i => i.ScrumTaskStep == ScrumTaskStep.Done);
      return scrumTask;
    }
    public ScrumTask ToDoScrumTask(
        int id,
        byte[] rowVersion)
    {
      return EditScrumTask(
                id: id,
                rowVersion: rowVersion,
                scrumTaskStep: ScrumTaskStep.ToDo);
    }
  }
}