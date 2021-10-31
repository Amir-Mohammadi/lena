using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPEvent;
using lena.Models.ProjectManagement.ProjectERPEventDocument;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ProjectManagement
{
  public partial class ProjectManagement
  {

    #region Get
    public ProjectERPEvent GetProjectERPEvent(int id) => GetProjectERPEvent(selector: e => e, id: id);
    public TResult GetProjectERPEvent<TResult>(
        Expression<Func<ProjectERPEvent, TResult>> selector,
        int id)
    {

      var projectERPEvent = GetProjectERPEvents(selector: selector,
                id: id).FirstOrDefault();
      //if (projectERPEvent == null)
      //    throw new ProjectERPEventNotFoundException(id: id);
      return projectERPEvent;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPEvents<TResult>(
        Expression<Func<ProjectERPEvent, TResult>> selector,
        TValue<int> id = null,
        TValue<int> projectERPId = null,
        TValue<short> projectERPEventCategoryId = null,
        TValue<int> audienceEmployeeId = null,
        TValue<int> projectERPEventDocumentId = null,
        TValue<string> audience = null,
        TValue<bool> confidential = null,
        TValue<string> nextAction = null,
        TValue<DateTime> nextActionDateTime = null,
        TValue<DateTime> CRMDateTime = null,
        TValue<ProjectERPEventType> type = null,
        TValue<ProjectERPEventAnnouncementType> announcementType = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<ProjectERPEvent>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (projectERPEventCategoryId != null)
        query = query.Where(m => m.ProjectERPEventCategoryId == projectERPEventCategoryId);
      if (projectERPId != null)
        query = query.Where(m => m.ProjectERPId == projectERPId);
      if (audienceEmployeeId != null)
        query = query.Where(m => m.AudienceEmployeeId == audienceEmployeeId);
      if (audience != null)
        query = query.Where(m => m.Audience == audience);
      if (confidential != null)
        query = query.Where(m => m.Confidential == confidential);
      if (nextAction != null)
        query = query.Where(m => m.NextAction == nextAction);
      if (nextActionDateTime != null)
        query = query.Where(m => m.NextActionDateTime == nextActionDateTime);
      if (CRMDateTime != null)
        query = query.Where(m => m.CRMDateTime == CRMDateTime);
      if (type != null)
        query = query.Where(m => m.Type == type);
      if (announcementType != null)
        query = query.Where(m => m.AnnouncementType == announcementType);
      if (description != null)
        query = query.Where(m => m.Description == description);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPEvent
    public ProjectERPEvent AddProjectERPEventProcess(
        int projectERPId,
        short projectERPEventCategoryId,
        int? audienceEmployeeId,
        string audience,
        bool confidential,
        string nextAction,
        DateTime? nextActionDateTime,
        DateTime? CRMDateTime,
        ProjectERPEventType type,
        string description,
        string announcementDescription,
        AddProjectERPEventDocumentInput[] addProjectERPEventDocumentInputs,
        TValue<ProjectERPEventAnnouncementType[]> announcementTypes = null
        )
    {


      var projectERPEventAnnouncementTypeEnum = ProjectERPEventAnnouncementType.None;
      var saleManagements = App.Internals.SaleManagement;
      if (announcementTypes != null)
      {
        foreach (var item in announcementTypes.Value)
          projectERPEventAnnouncementTypeEnum = projectERPEventAnnouncementTypeEnum | item;
      }

      var projectERPEvent = AddProjectERPEvent(
                 projectERPId: projectERPId,
                 projectERPEventCategoryId: projectERPEventCategoryId,
                 audienceEmployeeId: audienceEmployeeId,
                 audience: audience,
                 confidential: confidential,
                 nextAction: nextAction,
                 nextActionDateTime: nextActionDateTime,
                 CRMDateTime: CRMDateTime,
                 type: type,
                 announcementType: projectERPEventAnnouncementTypeEnum,
                 announcementDescription: announcementDescription,
                 description: description);
      foreach (var addProjectERPEventDocumentInput in addProjectERPEventDocumentInputs)
      {
        UploadFileData uploadFileData = null;
        if (addProjectERPEventDocumentInput.FileKey != null)
          uploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPEventDocumentInput.FileKey);
        Document document = null;
        if (uploadFileData != null)
        {
          var accounting = App.Internals.Accounting;
          document = App.Internals.ApplicationBase.AddDocument(
                        name: uploadFileData.FileName,
                        fileStream: uploadFileData.FileData);

          AddProjectERPEventDocument(
                    projectERPEventId: projectERPEvent.Id,
                    description: addProjectERPEventDocumentInput.Description,
                    documentId: document.Id);
        }
      }
      return projectERPEvent;

    }

    public ProjectERPEvent AddProjectERPEvent(
     int projectERPId,
     short projectERPEventCategoryId,
     int? audienceEmployeeId,
     string audience,
     bool confidential,
     string nextAction,
     DateTime? nextActionDateTime,
     DateTime? CRMDateTime,
     ProjectERPEventType type,
     ProjectERPEventAnnouncementType announcementType,
     string announcementDescription,
     string description)
    {

      var projectERPEvent = repository.Create<ProjectERPEvent>();

      projectERPEvent.ProjectERPId = projectERPId;
      projectERPEvent.RegisterUserId = App.Providers.Security.CurrentLoginData.UserId;
      projectERPEvent.RegisterDateTime = DateTime.UtcNow;
      projectERPEvent.ProjectERPEventCategoryId = projectERPEventCategoryId;
      projectERPEvent.AudienceEmployeeId = audienceEmployeeId;
      projectERPEvent.Audience = audience;
      projectERPEvent.Confidential = confidential;
      projectERPEvent.NextAction = nextAction;
      projectERPEvent.NextActionDateTime = nextActionDateTime;
      projectERPEvent.CRMDateTime = CRMDateTime;
      projectERPEvent.Type = type;
      projectERPEvent.AnnouncementType = announcementType;
      projectERPEvent.AnnouncementDescription = announcementDescription;
      projectERPEvent.Description = description;
      repository.Add(projectERPEvent);
      return projectERPEvent;
    }

    #endregion

    #region EditProjectERPEvent
    public ProjectERPEvent EditProjectERPEventProcess(
          int id,
          byte[] rowVersion,
          AddProjectERPEventDocumentInput[] addProjectERPEventDocumentInputs,
          DeleteProjectERPEventDocumentInput[] deleteProjectERPEventDocumentInputs,
          TValue<int> projectERPId = null,
          TValue<short> projectERPEventCategoryId = null,
          TValue<int> audienceEmployeeId = null,
          TValue<string> audience = null,
          TValue<bool> confidential = null,
          TValue<string> nextAction = null,
          TValue<DateTime> nextActionDateTime = null,
          TValue<DateTime> CRMDateTime = null,
          TValue<ProjectERPEventType> type = null,
          TValue<ProjectERPEventAnnouncementType[]> announcementTypes = null,
          TValue<string> announcementDescription = null,
          TValue<string> description = null)
    {

      var projectERPEvent = GetProjectERPEvent(id: id);

      #region ProjectERPEventDocument
      #region Add
      foreach (var addProjectERPEventDocumentInput in addProjectERPEventDocumentInputs)
      {
        UploadFileData uploadFileData = null;
        if (addProjectERPEventDocumentInput.FileKey != null)
          uploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPEventDocumentInput.FileKey);
        Document document = null;
        if (uploadFileData != null)
        {
          var accounting = App.Internals.Accounting;
          document = App.Internals.ApplicationBase.AddDocument(
                        name: uploadFileData.FileName,
                        fileStream: uploadFileData.FileData);

          AddProjectERPEventDocument(
                    projectERPEventId: id,
                    description: addProjectERPEventDocumentInput.Description,
                    documentId: document.Id);
        }
      }

      #endregion

      #region Delete
      foreach (var deleteProjectERPEventDocumentInput in deleteProjectERPEventDocumentInputs)
      {
        DeleteProjectERPEventDocument(
                  id: deleteProjectERPEventDocumentInput.Id);
      }
      #endregion

      #endregion


      #region  Set ProjectERPEventAnnouncementType
      var projectERPEventAnnouncementTypeEnum = ProjectERPEventAnnouncementType.None;
      var saleManagements = App.Internals.SaleManagement;
      if (announcementTypes != null)
      {
        foreach (var item in announcementTypes.Value)
          projectERPEventAnnouncementTypeEnum = projectERPEventAnnouncementTypeEnum | item;
      }
      #endregion

      return EditProjectERPEvent(
          id: id,
              projectERPId: projectERPId,
              projectERPEventCategoryId: projectERPEventCategoryId,
              audienceEmployeeId: audienceEmployeeId,
              audience: audience,
              confidential: confidential,
              nextAction: nextAction,
              nextActionDateTime: nextActionDateTime,
              CRMDateTime: CRMDateTime,
              type: type,
              announcementType: projectERPEventAnnouncementTypeEnum,
              announcementDescription: announcementDescription,
              description: description,
              rowVersion: rowVersion);

    }

    public ProjectERPEvent EditProjectERPEvent(
         int id,
         byte[] rowVersion,
         TValue<int> projectERPId = null,
         TValue<short> projectERPEventCategoryId = null,
         TValue<int> audienceEmployeeId = null,
         TValue<UploadFileData> uploadFileData = null,
         TValue<string> audience = null,
         TValue<bool> confidential = null,
         TValue<string> nextAction = null,
         TValue<DateTime> nextActionDateTime = null,
         TValue<DateTime> CRMDateTime = null,
         TValue<ProjectERPEventType> type = null,
         TValue<ProjectERPEventAnnouncementType> announcementType = null,
         TValue<string> announcementDescription = null,
         TValue<string> description = null)
    {

      var projectERPEvent = GetProjectERPEvent(id: id);



      if (projectERPId != null)
        projectERPEvent.ProjectERPId = projectERPId;
      if (projectERPEventCategoryId != null)
        projectERPEvent.ProjectERPEventCategoryId = projectERPEventCategoryId;
      if (audienceEmployeeId != null)
        projectERPEvent.AudienceEmployeeId = audienceEmployeeId;
      if (audience != null)
        projectERPEvent.Audience = audience;
      if (confidential != null)
        projectERPEvent.Confidential = confidential;
      if (nextAction != null)
        projectERPEvent.NextAction = nextAction;
      if (nextActionDateTime != null)
        projectERPEvent.NextActionDateTime = nextActionDateTime;
      if (CRMDateTime != null)
        projectERPEvent.CRMDateTime = CRMDateTime;
      if (type != null)
        projectERPEvent.Type = type;
      if (announcementType != null)
        projectERPEvent.AnnouncementType = announcementType;
      if (announcementDescription != null)
        projectERPEvent.AnnouncementDescription = announcementDescription;
      if (description != null)
        projectERPEvent.Description = description;

      repository.Update(entity: projectERPEvent, rowVersion: projectERPEvent.RowVersion);
      return projectERPEvent;
    }
    #endregion

    #region DeleteProjectERPEvent
    public void DeleteProjectERPEvent(int id)
    {

      var projectERPEvent = GetProjectERPEvent(id: id);
      repository.Delete(projectERPEvent);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPEventResult> SortProjectERPEventResult(
        IQueryable<ProjectERPEventResult> query, SortInput<ProjectERPEventSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPEventSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ProjectERPEventSortType.ProjectERPTitle:
          return query.OrderBy(a => a.ProjectERPTitle, type.SortOrder);
        case ProjectERPEventSortType.RegisterEmployeeFullName:
          return query.OrderBy(a => a.RegisterEmployeeFullName, type.SortOrder);
        case ProjectERPEventSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, type.SortOrder);
        case ProjectERPEventSortType.AudienceEmployeeFullName:
          return query.OrderBy(a => a.AudienceEmployeeFullName, type.SortOrder);
        case ProjectERPEventSortType.Audience:
          return query.OrderBy(a => a.Audience, type.SortOrder);
        case ProjectERPEventSortType.Confidential:
          return query.OrderBy(a => a.Confidential, type.SortOrder);
        case ProjectERPEventSortType.NextAction:
          return query.OrderBy(a => a.NextAction, type.SortOrder);
        case ProjectERPEventSortType.NextActionDateTime:
          return query.OrderBy(a => a.NextActionDateTime, type.SortOrder);
        case ProjectERPEventSortType.Description:
          return query.OrderBy(a => a.Description, type.SortOrder);
        case ProjectERPEventSortType.CRMDateTime:
          return query.OrderBy(a => a.CRMDateTime, type.SortOrder);
        case ProjectERPEventSortType.Type:
          return query.OrderBy(a => a.Type, type.SortOrder);
        case ProjectERPEventSortType.AnnouncementType:
          return query.OrderBy(a => a.AnnouncementType, type.SortOrder);
        case ProjectERPEventSortType.ProjectERPEventCategoryName:
          return query.OrderBy(a => a.ProjectERPEventCategoryName, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPEventResult> SearchProjectERPEventResult(
        IQueryable<ProjectERPEventResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.Description.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPEvent, ProjectERPEventResult>> ToProjectERPEventResult =
         projectERPEvent => new ProjectERPEventResult()
         {
           Id = projectERPEvent.Id,
           ProjectERPId = projectERPEvent.ProjectERPId,
           ProjectERPEventCategoryId = projectERPEvent.ProjectERPEventCategoryId,
           ProjectERPEventCategoryName = projectERPEvent.ProjectERPEventCategory.Name,
           RegisterUserId = projectERPEvent.RegisterUserId,
           RegisterEmployeeFullName = projectERPEvent.RegisterUser.Employee.FirstName + " " + projectERPEvent.RegisterUser.Employee.LastName,
           RegisterDateTime = projectERPEvent.RegisterDateTime,
           AudienceEmployeeId = projectERPEvent.AudienceEmployeeId,
           AudienceEmployeeFullName = projectERPEvent.AudienceEmployee.FirstName + " " + projectERPEvent.AudienceEmployee.LastName,
           Audience = projectERPEvent.Audience,
           NextAction = projectERPEvent.NextAction,
           NextActionDateTime = projectERPEvent.NextActionDateTime,
           CRMDateTime = projectERPEvent.CRMDateTime,
           Type = projectERPEvent.Type,
           AnnouncementType = projectERPEvent.AnnouncementType,
           AnnouncementDescription = projectERPEvent.AnnouncementDescription,
           Description = projectERPEvent.Description,
           RowVersion = projectERPEvent.RowVersion,

           ProjectERPEventDocumentQty = projectERPEvent.ProjectERPEventDocuments.Count(),
           ProjectERPEventDocuments = projectERPEvent.ProjectERPEventDocuments.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPEventDocumentResult)
         };
    #endregion
  }
}
