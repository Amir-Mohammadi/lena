using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERP;
using lena.Models.ProjectManagement.ProjectERPDocument;
using lena.Models.ProjectManagement.ProjectERPLabelLog;
using lena.Models.ProjectManagement.ProjectERPResponsibleEmployee;
using lena.Models.ProjectManagement.ProjectERPTask;
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
    public ProjectERP GetProjectERP(int id) => GetProjectERP(selector: e => e, id: id);
    public TResult GetProjectERP<TResult>(
        Expression<Func<ProjectERP, TResult>> selector,
        int id)
    {
      var projectERP = GetProjectERPs(selector: selector,
                id: id).FirstOrDefault();
      if (projectERP == null)
        throw new ProjectERPNotFoundException(id: id);
      return projectERP;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProjectERPs<TResult>(
        Expression<Func<ProjectERP, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> stuffId = null,
        TValue<string> title = null,
        TValue<bool> isActive = null,
        TValue<short> progress = null,
        TValue<int> customerId = null,
        TValue<ProjectERPPriority> priority = null)
    {
      var query = repository.GetQuery<ProjectERP>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (title != null)
        query = query.Where(m => m.Title == title);
      if (code != null)
        query = query.Where(m => m.Code == code);
      if (priority != null)
        query = query.Where(m => m.Priority == priority);
      if (isActive != null)
        query = query.Where(m => m.IsActive == isActive);
      if (progress != null)
        query = query.Where(m => m.Progress == progress);
      if (stuffId != null)
        query = query.Where(m => m.StuffId == stuffId);
      if (customerId != null)
        query = query.Where(m => m.CustomerId == customerId);
      return query.Select(selector);
    }
    #endregion
    #region AddProjectERP
    public ProjectERP AddProjectERPProcess(
        string title,
        string code,
        int version,
        ProjectERPPriority priority,
        bool isActive,
        string description,
        AddProjectERPResponsibleEmployeeInput[] addProjectERPResponsibleEmployeeInputs,
        AddProjectERPLabelLogInput[] addProjectERPLabelLogInputs,
        //AddProjectERPEventInput[] addProjectERPEventInputs,
        AddProjectERPDocumentInput[] addProjectERPDocumentInputs,
        TValue<short> progress = null,
        TValue<short> projectERPCategoryId = null,
        TValue<short?> projectERPTypeId = null,
        TValue<short> projectERPPhaseId = null,
        TValue<DateTime> realStartDateTime = null,
        TValue<DateTime> estimateStartDateTime = null,
        TValue<int?> stuffId = null,
        TValue<int?> customerId = null)
    {
      #region Check Exist ProjectERPCode
      var projectERPResult = App.Internals.ProjectManagement.GetProjectERPs(e => e, code: code);
      if (projectERPResult.Any())
        throw new ProjectERPCodeHasExistException(code: code);
      #endregion
      #region AddProjectERP
      var projectERP = AddProjectERP(
                          title: title,
                          code: code,
                          version: version,
                          priority: priority,
                          isActive: isActive,
                          stuffId: stuffId,
                          customerId: customerId,
                          progress: progress,
                          projectERPPhaseId: projectERPPhaseId,
                          projectERPTypeId: projectERPTypeId,
                          projectERPCategoryId: projectERPCategoryId,
                          estimateStartDateTime: estimateStartDateTime,
                          realStartDateTime: realStartDateTime,
                          description: description);
      #endregion
      #region AddProjectERPResponsibleEmployees
      foreach (var addProjectERPResponsibleEmployeeInput in addProjectERPResponsibleEmployeeInputs)
      {
        AddProjectERPResponsibleEmployee(
                       projectERPId: projectERP.Id,
                       responsibleEmployeeId: addProjectERPResponsibleEmployeeInput.ResponsibleEmployeeId,
                       isActive: addProjectERPResponsibleEmployeeInput.IsActive,
                       description: addProjectERPResponsibleEmployeeInput.Description
                  );
      }
      #endregion
      #region AddProjectERPLabelLogInput
      foreach (var addProjectERPLabelLogInput in addProjectERPLabelLogInputs)
      {
        AddProjectERPLabelLog(
                     projectERPId: projectERP.Id,
                     projectERPLabelId: addProjectERPLabelLogInput.ProjectERPLabelId
                  );
      }
      #endregion
      #region AddProjectERPEventInputs
      //foreach (var addProjectERPEventInput in addProjectERPEventInputs)
      //{
      //    var projectERPEventAnnouncementTypeEnum = ProjectERPEventAnnouncementType.None;
      //    var saleManagements = App.Internals.SaleManagement;
      //    if (addProjectERPEventInput.AnnouncementTypes != null)
      //    {
      //        foreach (var item in addProjectERPEventInput.AnnouncementTypes)
      //            projectERPEventAnnouncementTypeEnum = projectERPEventAnnouncementTypeEnum | item;
      //    }
      //    AddProjectERPEvent(
      //        projectERPId: projectERP.Id,
      //        projectERPEventCategoryId: addProjectERPEventInput.ProjectERPEventCategoryId,
      //        audienceEmployeeId: addProjectERPEventInput.AudienceEmployeeId,
      //        //projectERPEventDocumentId: addProjectERPEventInput.ProjectERPEventDocumentId,
      //        audience: addProjectERPEventInput.Audience,
      //        confidential: addProjectERPEventInput.Confidential,
      //        nextAction: addProjectERPEventInput.NextAction,
      //        nextActionDateTime: addProjectERPEventInput.NextActionDateTime,
      //        CRMDateTime: addProjectERPEventInput.CRMDateTime,
      //        type: addProjectERPEventInput.Type,
      //        announcementType: projectERPEventAnnouncementTypeEnum,
      //        description: addProjectERPEventInput.Description)
      //    
      //;
      //}
      #endregion
      #region Add Project ERP Documnets
      foreach (var addProjectERPDocumentInput in addProjectERPDocumentInputs)
      {
        if (!string.IsNullOrWhiteSpace(addProjectERPDocumentInput.FileKey))
          addProjectERPDocumentInput.UploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPDocumentInput.FileKey);
        Document document = null;
        document = App.Internals.ApplicationBase.AddDocument(
                name: addProjectERPDocumentInput.UploadFileData.FileName,
                fileStream: addProjectERPDocumentInput.UploadFileData.FileData);
        AddProjectERPDocument(
                  projectERPId: projectERP.Id,
                  documentId: document.Id,
                  projectERPDocumentTypeId: addProjectERPDocumentInput.ProjectERPDocumentTypeId,
                  description: addProjectERPDocumentInput.Description);
      }
      #endregion
      return projectERP;
    }
    public ProjectERP AddProjectERP(
        string title,
        string code,
        int version,
        ProjectERPPriority priority,
        bool isActive,
        short projectERPCategoryId,
        string description,
        short progress,
        TValue<short?> projectERPTypeId = null,
        TValue<short> projectERPPhaseId = null,
        TValue<DateTime> realStartDateTime = null,
        TValue<DateTime> estimateStartDateTime = null,
        TValue<int?> stuffId = null,
        TValue<int?> customerId = null)
    {
      var projectERP = repository.Create<ProjectERP>();
      projectERP.Code = code;
      projectERP.Title = title;
      projectERP.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      projectERP.CreateDateTime = DateTime.Now.ToUniversalTime();
      projectERP.Version = version;
      projectERP.Priority = priority;
      projectERP.IsActive = isActive;
      projectERP.ProjectERPPhaseId = projectERPPhaseId;
      projectERP.ProjectERPTypeId = projectERPTypeId;
      projectERP.ProjectERPCategoryId = projectERPCategoryId;
      projectERP.EstimateStartDateTime = estimateStartDateTime;
      projectERP.RealStartDateTime = realStartDateTime;
      projectERP.Description = description;
      projectERP.StuffId = stuffId;
      projectERP.CustomerId = customerId;
      projectERP.Progress = progress;
      repository.Add(projectERP);
      return projectERP;
    }
    #endregion
    #region EditProjectERP
    public ProjectERP EditProjectERPProcess(
        int id,
        byte[] rowVersion,
        AddProjectERPResponsibleEmployeeInput[] addProjectERPResponsibleEmployeeInputs,
        DeleteProjectERPResponsibleEmployeeInput[] deleteProjectERPResponsibleEmployeeInputs,
        AddProjectERPDocumentInput[] addProjectERPDocumentInputs,
        DeleteProjectERPDocumentInput[] deleteProjectERPDocumentInputs,
        AddProjectERPTaskInput[] addProjectERPTaskInputs,
        DeleteProjectERPTaskInput[] deleteProjectERPTaskInputs,
        TValue<string> title = null,
        TValue<string> code = null,
        TValue<int> version = null,
        TValue<ProjectERPPriority> priority = null,
        TValue<bool> isActive = null,
        TValue<int> stuffId = null,
        TValue<int> customerId = null,
        TValue<short> progress = null,
        TValue<short> projectERPPhaseId = null,
        TValue<short> projectERPTypeId = null,
        TValue<short> projectERPCategoryId = null,
        TValue<DateTime> estimateStartDateTime = null,
        TValue<DateTime> realStartDateTime = null,
        TValue<string> description = null)
    {
      #region EditProjectERP
      var projectERP = EditProjectERP(
          id: id,
          rowVersion: rowVersion,
          title: title,
          code: code,
          version: version,
          priority: priority,
          isActive: isActive,
          stuffId: stuffId,
          customerId: customerId,
          progress: progress,
          projectERPPhaseId: projectERPPhaseId,
          projectERPTypeId: projectERPTypeId,
          projectERPCategoryId: projectERPCategoryId,
          estimateStartDateTime: estimateStartDateTime,
          realStartDateTime: realStartDateTime,
          description: description);
      #endregion
      #region ProjectERPTask
      #region Add ProjectERPTask
      foreach (var addProjectERPTaskInput in addProjectERPTaskInputs)
      {
        AddProjectERPTask(
                      title: addProjectERPTaskInput.Title,
                      description: addProjectERPTaskInput.Description,
                      output: addProjectERPTaskInput.Output,
                      status: addProjectERPTaskInput.Status,
                      startDateTime: addProjectERPTaskInput.StartDateTime,
                      dueDateTime: addProjectERPTaskInput.DueDateTime,
                      estimateTime: addProjectERPTaskInput.EstimateTime,
                      durationMinute: addProjectERPTaskInput.DurationMinute,
                      progressPercentage: addProjectERPTaskInput.ProgressPercentage,
                      priority: addProjectERPTaskInput.Priority,
                      projectERPId: projectERP.Id,
                      assigneeEmployeeId: addProjectERPTaskInput.AssigneeEmployeeId,
                      projectERPTaskCategoryId: addProjectERPTaskInput.ProjectERPTaskCategoryId,
                      parentTaskId: addProjectERPTaskInput.ParentTaskId);
      }
      #endregion
      #region Delete ProjectERPTask
      foreach (var deleteProjectERPTaskInput in deleteProjectERPTaskInputs)
      {
        DeleteProjectERPTask(
                  id: deleteProjectERPTaskInput.Id);
      }
      #endregion
      #endregion
      #region ProjectERPResponsibleEmployees
      #region  Add ProjectERPResponsibleEmployees
      foreach (var addProjectERPResponsibleEmployeeInput in addProjectERPResponsibleEmployeeInputs)
      {
        AddProjectERPResponsibleEmployee(
                       projectERPId: projectERP.Id,
                       responsibleEmployeeId: addProjectERPResponsibleEmployeeInput.ResponsibleEmployeeId,
                       isActive: addProjectERPResponsibleEmployeeInput.IsActive,
                       description: addProjectERPResponsibleEmployeeInput.Description
                  );
      }
      #endregion
      #region Delete ProjectERPResponsibleEmployees
      foreach (var deleteProjectERPResponsibleEmployeeInput in deleteProjectERPResponsibleEmployeeInputs)
      {
        DeleteProjectERPResponsibleEmployee(
                  responsibleEmployeeId: deleteProjectERPResponsibleEmployeeInput.ResponsibleEmployeeId,
                  projectERPId: projectERP.Id,
                  rowVersion: deleteProjectERPResponsibleEmployeeInput.RowVersion);
      }
      #endregion
      #endregion
      //#region ProjectERPLabelLogInput
      //#region AddProjectERPLabelLogInput
      //foreach (var addProjectERPLabelLogInput in addProjectERPLabelLogInputs)
      //{
      //    AddProjectERPLabelLog(
      //           projectERPId: projectERP.Id,
      //           projectERPLabelId: addProjectERPLabelLogInput.ProjectERPLabelId
      //        )
      //    
      //;
      //}
      //#endregion
      //#endregion
      //#region ProjectERPEventInputs
      //#region AddProjectERPEventInputs
      //foreach (var addProjectERPEventInput in addProjectERPEventInputs)
      //{
      //    AddProjectERPEvent(
      //        projectERPId: projectERP.Id,
      //        projectERPEventCategoryId: addProjectERPEventInput.ProjectERPEventCategoryId,
      //        audienceUserId: addProjectERPEventInput.AudienceUserId,
      //        projectERPEventDocumentId: addProjectERPEventInput.ProjectERPEventDocumentId,
      //        audience: addProjectERPEventInput.Audience,
      //        confidential: addProjectERPEventInput.Confidential,
      //        nextAction: addProjectERPEventInput.NextAction,
      //        nextActionDateTime: addProjectERPEventInput.NextActionDateTime,
      //        CRMDateTime: addProjectERPEventInput.CRMDateTime,
      //        type: addProjectERPEventInput.Type,
      //        announcementType: addProjectERPEventInput.AnnouncementType,
      //        description: addProjectERPEventInput.Description)
      //    
      //;
      //}
      //#endregion
      //#endregion
      #region ProjectERPDocument
      #region Delete Project ERP Documents
      foreach (var deleteProjectERPDocumentInput in deleteProjectERPDocumentInputs)
      {
        DeleteProjectERPDocument(
                  id: deleteProjectERPDocumentInput.Id);
      }
      #endregion
      #region Add Project ERP Documnets
      foreach (var addProjectERPDocumentInput in addProjectERPDocumentInputs)
      {
        if (!string.IsNullOrWhiteSpace(addProjectERPDocumentInput.FileKey))
          addProjectERPDocumentInput.UploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPDocumentInput.FileKey);
        Document document = null;
        document = App.Internals.ApplicationBase.AddDocument(
                name: addProjectERPDocumentInput.UploadFileData.FileName,
                fileStream: addProjectERPDocumentInput.UploadFileData.FileData);
        AddProjectERPDocument(
                  projectERPId: projectERP.Id,
                  documentId: document.Id,
                  projectERPDocumentTypeId: addProjectERPDocumentInput.ProjectERPDocumentTypeId,
                  description: addProjectERPDocumentInput.Description);
      }
      #endregion
      #endregion
      //#region Add Project ERP Documnets
      //foreach (var addProjectERPDocumentInput in addProjectERPDocumentInputs)
      //{
      //    if (!string.IsNullOrWhiteSpace(addProjectERPDocumentInput.FileKey))
      //        addProjectERPDocumentInput.UploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPDocumentInput.FileKey);
      //    Document document = null;
      //    document = App.Internals.ApplicationBase.AddDocument(
      //      name: addProjectERPDocumentInput.UploadFileData.FileName,
      //      fileStream: addProjectERPDocumentInput.UploadFileData.FileData)
      //  
      //;
      //    AddProjectERPDocument(
      //        projectERPId: projectERP.Id,
      //        documentId: document.Id,
      //        projectERPDocumentTypeId: addProjectERPDocumentInput.ProjectERPDocumentTypeId,
      //        description: addProjectERPDocumentInput.Description)
      //    
      //;
      //}
      //#endregion
      return projectERP;
    }
    public ProjectERP EditProjectERP(
     int id,
     byte[] rowVersion,
     TValue<string> title = null,
     TValue<string> code = null,
     TValue<int> version = null,
     TValue<ProjectERPPriority> priority = null,
     TValue<bool> isActive = null,
     TValue<int> stuffId = null,
     TValue<int> customerId = null,
     TValue<short> progress = null,
     TValue<short> projectERPPhaseId = null,
     TValue<short> projectERPTypeId = null,
     TValue<short> projectERPCategoryId = null,
     TValue<DateTime> estimateStartDateTime = null,
     TValue<DateTime> realStartDateTime = null,
     TValue<string> description = null)
    {
      var projectERP = GetProjectERP(id: id);
      if (title != null)
        projectERP.Title = title;
      if (code != null)
        projectERP.Code = code;
      if (version != null)
        projectERP.Version = version;
      if (priority != null)
        projectERP.Priority = priority;
      if (isActive != null)
        projectERP.IsActive = isActive;
      if (stuffId != null)
        projectERP.StuffId = stuffId;
      if (customerId != null)
        projectERP.CustomerId = customerId;
      if (progress != null)
        projectERP.Progress = progress;
      if (projectERPPhaseId != null)
        projectERP.ProjectERPPhaseId = projectERPPhaseId;
      if (projectERPTypeId != null)
        projectERP.ProjectERPTypeId = projectERPTypeId;
      if (projectERPCategoryId != null)
        projectERP.ProjectERPCategoryId = projectERPCategoryId;
      if (estimateStartDateTime != null)
        projectERP.EstimateStartDateTime = estimateStartDateTime;
      if (realStartDateTime != null)
        projectERP.RealStartDateTime = realStartDateTime;
      if (description != null)
        projectERP.Description = description;
      repository.Update(entity: projectERP, rowVersion: projectERP.RowVersion);
      return projectERP;
    }
    #endregion
    #region DeleteProjectERP
    public void DeleteProjectERP(int id)
    {
      var projectERP = GetProjectERP(
                id: id);
      // you can not Delete project
      repository.Delete(projectERP);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<ProjectERPResult> SortProjectERPResult(
        IQueryable<ProjectERPResult> query, SortInput<ProjectERPSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ProjectERPSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        case ProjectERPSortType.Code:
          return query.OrderBy(a => a.Code, type.SortOrder);
        case ProjectERPSortType.Priority:
          return query.OrderBy(a => a.Priority, type.SortOrder);
        case ProjectERPSortType.IsActive:
          return query.OrderBy(a => a.IsActive, type.SortOrder);
        case ProjectERPSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, type.SortOrder);
        case ProjectERPSortType.StuffName:
          return query.OrderBy(a => a.StuffName, type.SortOrder);
        case ProjectERPSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, type.SortOrder);
        case ProjectERPSortType.Progress:
          return query.OrderBy(a => a.Progress, type.SortOrder);
        case ProjectERPSortType.ProjectERPPhaseName:
          return query.OrderBy(a => a.ProjectERPPhaseName, type.SortOrder);
        case ProjectERPSortType.CreatorEmployeeFullName:
          return query.OrderBy(a => a.CreatorEmployeeFullName, type.SortOrder);
        case ProjectERPSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, type.SortOrder);
        case ProjectERPSortType.ProjectERPTypeName:
          return query.OrderBy(a => a.ProjectERPTypeName, type.SortOrder);
        case ProjectERPSortType.ProjectERPCategoryName:
          return query.OrderBy(a => a.ProjectERPCategoryName, type.SortOrder);
        case ProjectERPSortType.EstimateStartDateTime:
          return query.OrderBy(a => a.EstimateStartDateTime, type.SortOrder);
        case ProjectERPSortType.RealStartDateTime:
          return query.OrderBy(a => a.RealStartDateTime, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<ProjectERPResult> SearchProjectERPResult(
        IQueryable<ProjectERPResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.Code.Contains(searchText) ||
                    item.Title.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToProjectERPResultQuery
    public IQueryable<ProjectERPResult> ToProjectERPResultQuery(
        IQueryable<ProjectERP> projectERPs,
        IQueryable<ProjectERPResponsibleEmployee> projectERPResponsibleResult,
        IQueryable<ProjectERPLabelLog> projectERPLabelLogs)
    {
      //var projectERPLabelLogResult = from projectERPLabelLog in projectERPLabelLogs
      //                               group projectERPLabelLog by projectERPLabelLog.ProjectERPId
      //                into g
      //                               select new
      //                               {
      //                                   ProjectERPId = g.Key,
      //                                   ProjectERPLabelNames = g.Select(m => String.Join(m.ProjectERPLabel.Name,','))
      //                               };
      var projectERPResponsibleGroupResult = from projectERPResponsible in projectERPResponsibleResult
                                             group projectERPResponsible by projectERPResponsible.ProjectERPId into g
                                             select new
                                             {
                                               ProjectERPId = g.Key,
                                               ProjectERPResponsibleEmployeeFullNames = g.Select(m => String.Join(m.ResponsibleEmployee.FirstName + " " + m.ResponsibleEmployee.LastName, ','))
                                             };
      var result = from projectERP in projectERPs
                   join projectERPResponsible in projectERPResponsibleGroupResult on projectERP.Id equals projectERPResponsible.ProjectERPId into projectERPResponsibleLeft
                   from tProjectERPResponsible in projectERPResponsibleLeft.DefaultIfEmpty()
                     //join projectERPLabelLog in projectERPLabelLogResult on projectERP.Id equals projectERPLabelLog.ProjectERPId into projectERPLabelLogLeft
                     //from tProjectERPLabelLog in projectERPLabelLogLeft.DefaultIfEmpty()
                   select new ProjectERPResult()
                   {
                     Id = projectERP.Id,
                     Title = projectERP.Title,
                     Code = projectERP.Code,
                     Version = projectERP.Version,
                     Priority = projectERP.Priority,
                     IsActive = projectERP.IsActive,
                     StuffId = projectERP.StuffId,
                     StuffCode = projectERP.Stuff.Code,
                     StuffName = projectERP.Stuff.Name,
                     CustomerId = projectERP.CustomerId,
                     CustomerName = projectERP.Customer.Name,
                     Progress = projectERP.Progress,
                     ProjectERPPhaseId = projectERP.ProjectERPPhaseId,
                     ProjectERPPhaseName = projectERP.ProjectERPPhase.Name,
                     CreatorUserId = projectERP.CreatorUserId,
                     CreatorEmployeeFullName = projectERP.CreatorUser.Employee.FirstName + " " + projectERP.CreatorUser.Employee.LastName,
                     CreateDateTime = projectERP.CreateDateTime,
                     ProjectERPTypeId = projectERP.ProjectERPTypeId,
                     ProjectERPTypeName = projectERP.ProjectERPType.Name,
                     ProjectERPCategoryId = projectERP.ProjectERPCategoryId,
                     ProjectERPCategoryName = projectERP.ProjectERPCategory.Name,
                     Status = projectERP.Status,
                     EstimateStartDateTime = projectERP.EstimateStartDateTime,
                     RealStartDateTime = projectERP.RealStartDateTime,
                     Description = projectERP.Description,
                     AllProjectERPTaskCount = projectERP.ProjectERPTasks.Count(),
                     ProjectERPResponsibleEmployeeFullNames = tProjectERPResponsible.ProjectERPResponsibleEmployeeFullNames,
                     //ProjectERPLabelNames = tProjectERPLabelLog.ProjectERPLabelNames,
                     RowVersion = projectERP.RowVersion,
                     #region ICollection
                     ProjectERPResponsibleEmployees = projectERP.ProjectERPResponsibleEmployees.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPResponsibleEmployeeResult),
                     ProjectERPEvents = projectERP.ProjectERPEvents.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPEventResult),
                     ProjectERPTasks = projectERP.ProjectERPTasks.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPTaskResult),
                     ProjectERPDocuments = projectERP.ProjectERPDocuments.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPDocumentResult),
                     ProjectERPLabelLogs = projectERP.ProjectERPLabelLogs.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPLabelLogResult),
                     #endregion
                   };
      return result;
    }
    #endregion
    #region ToResult
    public Expression<Func<ProjectERP, ProjectERPResult>> ToProjectERPResult =
         projectERP => new ProjectERPResult()
         {
           Id = projectERP.Id,
           Title = projectERP.Title,
           Code = projectERP.Code,
           Version = projectERP.Version,
           Priority = projectERP.Priority,
           IsActive = projectERP.IsActive,
           StuffId = projectERP.StuffId,
           StuffCode = projectERP.Stuff.Code,
           StuffName = projectERP.Stuff.Name,
           CustomerId = projectERP.CustomerId,
           CustomerName = projectERP.Customer.Name,
           Progress = projectERP.Progress,
           ProjectERPPhaseId = projectERP.ProjectERPPhaseId,
           ProjectERPPhaseName = projectERP.ProjectERPPhase.Name,
           CreatorUserId = projectERP.CreatorUserId,
           CreatorEmployeeFullName = projectERP.CreatorUser.Employee.FirstName + " " + projectERP.CreatorUser.Employee.LastName,
           CreateDateTime = projectERP.CreateDateTime,
           ProjectERPTypeId = projectERP.ProjectERPTypeId,
           ProjectERPTypeName = projectERP.ProjectERPType.Name,
           ProjectERPCategoryId = projectERP.ProjectERPCategoryId,
           ProjectERPCategoryName = projectERP.ProjectERPCategory.Name,
           Status = projectERP.Status,
           EstimateStartDateTime = projectERP.EstimateStartDateTime,
           RealStartDateTime = projectERP.RealStartDateTime,
           Description = projectERP.Description,
           AllProjectERPTaskCount = projectERP.ProjectERPTasks.Count(),
           AllDoneProjectERPTaskCount = projectERP.ProjectERPTasks.Count(m => m.Status == ProjectERPTaskStatus.Done),
           AllDoingProjectERPTaskCount = projectERP.ProjectERPTasks.Count(m => m.Status == ProjectERPTaskStatus.Doing),
           RowVersion = projectERP.RowVersion,
           #region ICollection
           ProjectERPResponsibleEmployees = projectERP.ProjectERPResponsibleEmployees.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPResponsibleEmployeeResult),
           ProjectERPEvents = projectERP.ProjectERPEvents.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPEventResult),
           ProjectERPTasks = projectERP.ProjectERPTasks.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPTaskResult),
           ProjectERPDocuments = projectERP.ProjectERPDocuments.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPDocumentResult),
           ProjectERPLabelLogs = projectERP.ProjectERPLabelLogs.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPLabelLogResult)
           #endregion
         };
    #endregion
    #region ToComboResult
    public IQueryable<ProjectERPComboResult> ToProjectERPComboResultQuery(IQueryable<ProjectERP> query)
    {
      var result = from projectERP in query
                   select new ProjectERPComboResult()
                   {
                     Title = projectERP.Title,
                     Code = projectERP.Code,
                     Version = projectERP.Version,
                     IsActive = projectERP.IsActive,
                     StuffId = projectERP.StuffId,
                     StuffCode = projectERP.Stuff.Code,
                     StuffName = projectERP.Stuff.Name,
                     CustomerId = projectERP.CustomerId,
                     CustomerName = projectERP.Customer.Name,
                   };
      return result;
    }
    #endregion
    #region ActiveProjectERP
    public ProjectERP ActiveProjectERP(byte[] rowVersion, int id)
    {
      return EditProjectERP(
          id: id,
          rowVersion: rowVersion,
          isActive: true);
    }
    #endregion
    #region DeActive
    public ProjectERP DeActiveProjectERP(byte[] rowVersion, int id)
    {
      return EditProjectERP(
          id: id,
          rowVersion: rowVersion,
          isActive: false);
    }
    #endregion
  }
}