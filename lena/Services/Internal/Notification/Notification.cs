using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Core.Provider.Notification;
using lena.Services.Internals.Notification.Exception;
using lena.Models.Common;
using lena.Models.Notification;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using lena.Domains;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Notification
{
  public partial class Notification
  {
    #region Get
    public lena.Domains.Notification GetNotification(long id) => GetNotification(selector: e => e, id: id);
    public TResult GetNotification<TResult>(
        Expression<Func<lena.Domains.Notification, TResult>> selector,
        long id)
    {
      var notification = GetNotifications(selector: selector, id: id)


                .FirstOrDefault();
      if (notification == null)
        throw new NotificationNotFoundException(id);
      return notification;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetNotifications<TResult>(
        Expression<Func<lena.Domains.Notification, TResult>> selector,
        TValue<long> id = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isSeen = null,
        TValue<bool> isDelete = null,
        TValue<DateTime> requestDate = null,
        TValue<DateTime> seenDate = null,
        TValue<int> userId = null,
        TValue<int?> scrumEntityId = null,
        TValue<int?> groupedNotificationId = null
    )
    {
      var query = repository.GetQuery<lena.Domains.Notification>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (description != null)
        query = query.Where(x => x.Description == description);
      if (isDelete != null)
        query = query.Where(x => x.IsDelete == isDelete);
      if (isSeen != null)
        query = query.Where(x => x.IsSeen == isSeen);
      if (requestDate != null)
        query = query.Where(x => x.RequestDate == requestDate);
      if (seenDate != null)
        query = query.Where(x => x.SeenDate == seenDate);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (scrumEntityId != null)
        query = query.Where(x => x.ScrumEntityId == scrumEntityId);
      if (groupedNotificationId != null)
        query = query.Where(x => x.NotificationGroupId == groupedNotificationId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public lena.Domains.Notification AddNotification(
        string title,
        string description,
        int userId,
        int? scrumEntityId,
        int? notificationGroupId
    )
    {
      var entity = repository.Create<lena.Domains.Notification>();
      entity.Title = title ?? "";
      entity.Description = description ?? "";
      entity.IsSeen = false;
      entity.IsDelete = false;
      entity.RequestDate = DateTime.Now.ToUniversalTime();
      entity.SeenDate = null;
      entity.UserId = userId;
      entity.ScrumEntityId = scrumEntityId;
      entity.NotificationGroupId = notificationGroupId;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    private lena.Domains.Notification EditNotification(
        long id,
        byte[] rowVersion,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<bool> isSeen = null,
        TValue<bool> isDelete = null,
        TValue<DateTime> requestDate = null,
        TValue<DateTime> seenDate = null,
        TValue<int> userId = null,
        TValue<int?> scrumEntityId = null
    )
    {
      var notification = GetNotification(id: id);
      if (title != null) notification.Title = title;
      if (description != null) notification.Description = description;
      if (isSeen != null) notification.IsSeen = isSeen;
      if (isDelete != null) notification.IsDelete = isDelete;
      if (requestDate != null) notification.RequestDate = requestDate;
      if (seenDate != null) notification.SeenDate = seenDate;
      if (userId != null) notification.UserId = userId;
      if (scrumEntityId != null) notification.ScrumEntityId = scrumEntityId;
      repository.Update(rowVersion: rowVersion, entity: notification);
      return notification;
    }
    #endregion
    #region Delete
    private void DeleteNotification(long id)
    {
      var notification = GetNotification(id: id);
      repository.Delete(notification);
    }
    #endregion
    #region Search
    private IQueryable<NotificationResult> SearchNotificationResult(
        IQueryable<NotificationResult> query,
        string search)
    {
      if (!string.IsNullOrEmpty(search))
        query = from item in query
                where item.Title.Contains(search)
                select item;
      return query;
    }
    #endregion
    #region ToResult
    public readonly Expression<Func<lena.Domains.Notification, NotificationResult>> toNotificationResult =
        notification => new NotificationResult
        {
          Id = notification.Id,
          Title = notification.Title,
          Description = notification.Description,
          IsSeen = notification.IsSeen,
          RequestDate = notification.RequestDate,
          SeenDate = notification.RequestDate,
          ScrumEntityId = notification.ScrumEntityId,
          ScrumEntityRowVersion = notification.ScrumEntity.RowVersion,
          RowVersion = notification.RowVersion
        };
    #endregion
    public lena.Domains.Notification SeenNotificationProcess(long id, byte[] rowVersion)
    {
      var notifcatoin = SeenNotification(id: id,
                     rowVersion: rowVersion);
      if (notifcatoin.ScrumEntityId == null && notifcatoin.NotificationGroupId != null)
      {
        var groupedNotifications = GetNotifications(e => new { e.Id, e.RowVersion, e.IsSeen }, groupedNotificationId: notifcatoin.NotificationGroupId);
        groupedNotifications = groupedNotifications.Where(a => a.Id != notifcatoin.Id && a.IsSeen == false);
        foreach (var notification in groupedNotifications)
        {
          SeenNotification(id: notifcatoin.Id, rowVersion: notifcatoin.RowVersion);
        }
      }
      return notifcatoin;
    }
    public lena.Domains.Notification SeenNotification(long id, byte[] rowVersion)
    {
      var notifcatoin = EditNotification(id: id,
                     rowVersion: rowVersion,
                     isSeen: true,
                     seenDate: DateTime.Now.ToUniversalTime());
      Emit(
                userId: notifcatoin.UserId,
                eventKey: SystemEvents.OnNotificationSeen,
                payload: notifcatoin.Id
                );
      return notifcatoin;

    }
    public bool Emit(int userId, SystemEvents eventKey, object payload)
    {
      var result = false;
      App.Providers.Notification.Emit(userId.ToString(), eventKey.ToString(), payload);
      return result;
    }
    public bool Emit(SystemEvents eventKey, object payload)
    {
      var result = false;
      App.Providers.Notification.Emit(eventKey.ToString(), payload);
      return result;
    }
    public void Emit(List<int> users, SystemEvents eventKey, object payload)
    {
      foreach (var user in users)
      {
        Emit(userId: user,
                  eventKey: eventKey,
                  payload: payload
                  );
      }
    }
    public void EmitToDepartment(short departmentId, SystemEvents eventKey, object payload)
    {
      var userIds = App.Internals.UserManagement
                .GetEmployees(selector: e => (int?)e.User.Id, departmentId: departmentId)


                .Where(i => i != null)
                .Select(i => i.Value);
      Emit(
                    users: userIds.ToList(),
                    eventKey: eventKey,
                    payload: payload);
    }
    public bool NotifyToUser(int userId, string title, string description,
        int? scrumEntityId = null, int? notificationGroupId = null)
    {
      var addedNotification = AddNotification(
                       title: title,
                       description: description,
                       userId: userId,
                       scrumEntityId: scrumEntityId,
                       notificationGroupId: notificationGroupId
                       ); ; var notification = GetNotification(toNotificationResult, addedNotification.Id);
      return App.Providers.Notification.Push(userId.ToString(), notification);
    }
    public bool NotifyToSelf(string title, string description, int? scrumEntityId = null)
    {
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      return NotifyToUser(
                title: title,
                description: description,
                userId: userId,
                scrumEntityId: scrumEntityId);
    }
    public void NotifyToUsers(List<int> users, string title, string description, int? scrumEntityId = null)
    {
      var group = AddNotificationGroup();
      foreach (var user in users)
      {
        NotifyToUser(userId: user,
                  title: title,
                  description: description,
                  scrumEntityId: scrumEntityId,
                  notificationGroupId: group.Id
                  );
      }
    }
    public void NotifyUserGroup(int userGroupId, string title, string description, int? scrumEntityId = null)
    {
      var userIds = App.Internals.UserManagement
                .GetMemberships(selector: e => e.UserId, userGroupId: userGroupId)


                .ToList();
      NotifyToUsers(
                    users: userIds,
                    title: title,
                    description: description,
                    scrumEntityId: scrumEntityId);
    }
    public void NotifyToDepartment(short departmentId, string title, string description, int? scrumEntityId = null)
    {
      var userIds = App.Internals.UserManagement
                .GetEmployees(selector: e => (int?)e.User.Id, departmentId: departmentId)


                .Where(i => i != null)
                .Select(i => i.Value);
      NotifyToUsers(
                    users: userIds.ToList(),
                    title: title,
                    description: description,
                    scrumEntityId: scrumEntityId);
    }
    public IQueryable<NotificationResult> GetPendingNotifications()
    {
      var currentUser = App.Providers.Security.CurrentLoginData;
      var userNotifications = GetNotifications(
                                    selector: e => e,
                                    userId: currentUser.UserId,
                                        isDelete: false);
      var notificatios = userNotifications.OrderByDescending(a => new
      {
        IsSeen = a.IsSeen == false,
        a.Id
      });
      return notificatios.Select(toNotificationResult);
    }
    public IQueryable<NotificationResult> SearchNotification(IQueryable<NotificationResult> query, string searchText)
    {
      return from i in query
             where i.Title.Contains(searchText) ||
             i.Description.Contains(searchText)
             select i;
    }
    public IQueryable<NotificationResult> SortNotification(IQueryable<NotificationResult> query, NotificationSortType sortType, SortOrder sortOrder)
    {
      switch (sortType)
      {
        case NotificationSortType.Id:
          switch (sortOrder)
          {
            case SortOrder.Descending:
              return query.OrderByDescending(a => a.Id);
            case SortOrder.Unspecified:
            case SortOrder.Ascending:
            default:
              return query.OrderBy(a => a.Id);
          }
        default:
          return query.OrderBy(a => a.Id);
      }
    }
    public NotificationGroup AddNotificationGroup()
    {
      var group = repository.Create<NotificationGroup>();
      repository.Add(group);
      return group;
    }
    public IQueryable<NotificationGroup> GetNotificationGroups(TValue<int> id)
    {
      var groups = repository.GetQuery<NotificationGroup>();
      if (id != null)
        groups = groups.Where(a => a.Id == id);
      return groups;
    }
    public NotificationGroup GetNotificationGroup(int id)
    {
      var group = GetNotificationGroups(id: id).FirstOrDefault();
      if (group == null)
        throw new NotificationGroupNotFoundException(id);
      return group;
    }
    public void DeleteNotificationGroup(int id)
    {
      var group = GetNotificationGroup(id);
      repository.Delete(group);
    }
  }
}