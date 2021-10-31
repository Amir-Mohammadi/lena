using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.ProjectManagement.ProjectERPTask;
using lena.Models.ProjectManagement.ProjectERPTaskDependency;
using lena.Models.ProjectManagement.ProjectERPTaskLabelLog;
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
    public ProjectERPTask GetProjectERPTask(int id) => GetProjectERPTask(selector: e => e, id: id);
    public TResult GetProjectERPTask<TResult>(
        Expression<Func<ProjectERPTask, TResult>> selector,
        int id)
    {

      var projectERPTask = GetProjectERPTasks(selector: selector,
                id: id).FirstOrDefault();
      //if (projectERPTask == null)
      //    throw new ProjectERPTaskNotFoundException(id: id);
      return projectERPTask;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPTasks<TResult>(
        Expression<Func<ProjectERPTask, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<string> output = null,
        TValue<DateTime?> startDateTime = null,
        TValue<DateTime?> dueDateTime = null,
        TValue<int> estimateTime = null,
        TValue<int?> durationMinute = null,
        TValue<int?> progressPercentage = null,
        TValue<ProjectERPTaskPriority> priority = null,
        TValue<int> projectERPId = null,
        TValue<int?> assigneeEmployeeId = null,
        TValue<short> projectERPTaskCategoryId = null,
        TValue<int?> parentTaskId = null)
    {

      var query = repository.GetQuery<ProjectERPTask>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (projectERPTaskCategoryId != null)
        query = query.Where(m => m.ProjectERPTaskCategoryId == projectERPTaskCategoryId);
      if (projectERPId != null)
        query = query.Where(m => m.ProjectERPId == projectERPId);
      if (title != null)
        query = query.Where(m => m.Title == title);
      if (output != null)
        query = query.Where(m => m.Output == output);
      if (startDateTime != null)
        query = query.Where(m => m.StartDateTime == startDateTime);
      if (dueDateTime != null)
        query = query.Where(m => m.DueDateTime == dueDateTime);
      if (estimateTime != null)
        query = query.Where(m => m.EstimateTime == estimateTime);
      if (durationMinute != null)
        query = query.Where(m => m.DurationMinute == durationMinute);
      if (progressPercentage != null)
        query = query.Where(m => m.ProgressPercentage == progressPercentage);
      if (assigneeEmployeeId != null)
        query = query.Where(m => m.AssigneeEmployeeId == assigneeEmployeeId);
      if (priority != null)
        query = query.Where(m => m.Priority == priority);
      if (parentTaskId != null)
        query = query.Where(m => m.ParentTaskId == parentTaskId);
      if (description != null)
        query = query.Where(m => m.Description == description);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPTask
    public ProjectERPTask AddProjectERPTaskProcess(
        string title,
        string description,
        string output,
        ProjectERPTaskStatus? status,
        DateTime? startDateTime,
        DateTime? dueDateTime,
        int estimateTime,
        int? durationMinute,
        int? progressPercentage,
        ProjectERPTaskPriority priority,
        int projectERPId,
        int? assigneeEmployeeId,
        short projectERPTaskCategoryId,
        int? parentTaskId,  // برای پیاده سازی ساختار WBS
        string[] fileKeies,
        AddProjectERPTaskLabelLogInput[] addProjectERPTaskLabelLogInputs,
        AddProjectERPTaskDependencyInput[] addProjectERPTaskDependencyInputs)
    {


      var projectERPTask = AddProjectERPTask(
                    title: title,
                    description: description,
                    output: output,
                    status: status,
                    startDateTime: startDateTime,
                    dueDateTime: dueDateTime,
                    estimateTime: estimateTime,
                    durationMinute: durationMinute,
                    progressPercentage: progressPercentage,
                    priority: priority,
                    projectERPId: projectERPId,
                    assigneeEmployeeId: assigneeEmployeeId,
                    projectERPTaskCategoryId: projectERPTaskCategoryId,
                    parentTaskId: parentTaskId);

      #region Add ProjectERPTaskDocumnet


      foreach (var fileKey in fileKeies)
      {

        UploadFileData uploadFileData = null;
        if (!string.IsNullOrWhiteSpace(fileKey))
          uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);

        Document document = null;
        document = App.Internals.ApplicationBase.AddDocument(
                name: uploadFileData.FileName,
                fileStream: uploadFileData.FileData);

        AddProjectERPTaskDocument(
                  projectERPTaskId: projectERPTask.Id,
                  documentId: document.Id,
                  description: ""
                  );
      }

      #endregion

      #region ProjectERPTaskLabelLog

      //#region Delete
      //foreach (var projectERPTaskLabelLog in projectERPTask.ProjectERPTaskLabelLogs)
      //{

      //    DeleteProjectERPTaskLabelLog(projectERPTaskLabelLog.ProjectERPTaskId, projectERPTaskLabelLog.ProjectERPLabelId)
      //        
      //;
      //}
      //#endregion

      #region Add
      foreach (var addProjectERPTaskLabelLogInput in addProjectERPTaskLabelLogInputs)
      {
        AddProjectERPTaskLabelLog(
                     projectERPTaskId: projectERPTask.Id,
                     projectERPLabelId: addProjectERPTaskLabelLogInput.ProjectERPLabelId
                  );
      }
      #endregion

      #endregion

      #region Add ProjectERPTaskDependency
      foreach (var addProjectERPTaskDependencyInput in addProjectERPTaskDependencyInputs)
      {
        AddProjectERPTaskDependency(
                      projectERPTaskId: projectERPTask.Id,
                      lagMinutues: addProjectERPTaskDependencyInput.LagMinutues,
                      dependencyType: addProjectERPTaskDependencyInput.DependencyType,
                      predecessorProjectERPTaskId: addProjectERPTaskDependencyInput.PredecessorProjectERPTaskId
                  );
      }
      #endregion

      return projectERPTask;

    }

    public ProjectERPTask AddProjectERPTask(
     string title,
     string description,
     string output,
     DateTime? startDateTime,
     DateTime? dueDateTime,
     int estimateTime,
     int? durationMinute,
     int? progressPercentage,
     ProjectERPTaskPriority priority,
     int projectERPId,
     int? assigneeEmployeeId,
     short projectERPTaskCategoryId,
     int? parentTaskId,
     ProjectERPTaskStatus? status)
    {

      var projectERPTask = repository.Create<ProjectERPTask>();

      projectERPTask.Title = title;
      projectERPTask.Description = description;
      projectERPTask.Output = output;
      projectERPTask.Status = status ?? ProjectERPTaskStatus.Open;
      projectERPTask.StartDateTime = startDateTime;
      projectERPTask.DueDateTime = dueDateTime;
      projectERPTask.EstimateTime = estimateTime;
      projectERPTask.DurationMinute = durationMinute;
      projectERPTask.ProgressPercentage = progressPercentage;
      projectERPTask.Priority = priority;
      projectERPTask.ProjectERPId = projectERPId;
      projectERPTask.AssigneeEmployeeId = assigneeEmployeeId;
      projectERPTask.ProjectERPTaskCategoryId = projectERPTaskCategoryId;
      projectERPTask.ParentTaskId = parentTaskId;
      projectERPTask.CreateDateTime = DateTime.UtcNow;
      projectERPTask.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(projectERPTask);
      return projectERPTask;
    }

    #endregion

    #region EditProjectERPTask
    public ProjectERPTask EditProjectERPTaskProcess(
     int id,
     byte[] rowVersion,

     //string[] fileKeies,
     AddProjectERPTaskLabelLogInput[] addProjectERPTaskLabelLogInputs,
     AddProjectERPTaskDocumentInput[] addProjectERPTaskDocumentInputs,
     DeleteProjectERPTaskDocumentInput[] deleteProjectERPTaskDocumentInputs,

     AddProjectERPTaskDependencyInput[] addProjectERPTaskDependencyInputs,
     DeleteProjectERPTaskDependencyInput[] deleteProjectERPTaskDependencyInputs,

     TValue<string> title = null,
     TValue<string> description = null,
     TValue<string> output = null,
     TValue<ProjectERPTaskStatus> status = null,
     TValue<DateTime?> startDateTime = null,
     TValue<DateTime?> dueDateTime = null,
     TValue<int> estimateTime = null,
     TValue<int?> durationMinute = null,
     TValue<int?> progressPercentage = null,
     TValue<ProjectERPTaskPriority> priority = null,
     TValue<int> projectERPId = null,
     TValue<int?> assigneeEmployeeId = null,
     TValue<short> projectERPTaskCategoryId = null,
     TValue<int?> parentTaskId = null)
    {


      var projectERPTaskResult = EditProjectERPTask(
                     id: id,
                     title: title,
                     description: description,
                     output: output,
                     status: status,
                     startDateTime: startDateTime,
                     dueDateTime: dueDateTime,
                     estimateTime: estimateTime,
                     durationMinute: durationMinute,
                     progressPercentage: progressPercentage,
                     priority: priority,
                     projectERPId: projectERPId,
                     assigneeEmployeeId: assigneeEmployeeId,
                     projectERPTaskCategoryId: projectERPTaskCategoryId,
                     parentTaskId: parentTaskId,
                     rowVersion: rowVersion);

      var projectERPTask = GetProjectERPTasks(
                selector: App.Internals.ProjectManagement.ToProjectERPTaskResult,
                id: id)


            .FirstOrDefault();


      #region ProjectERPTaskDependency
      #region Delete
      foreach (var deleteProjectERPTaskDependencyInput in deleteProjectERPTaskDependencyInputs)
      {

        DeleteProjectERPTaskDependency(
                  id: deleteProjectERPTaskDependencyInput.Id,
                  rowVersion: deleteProjectERPTaskDependencyInput.RowVersion);
      }
      #endregion

      #region Add
      foreach (var addProjectERPTaskDependencyInput in addProjectERPTaskDependencyInputs)
      {

        AddProjectERPTaskDependency(
                      projectERPTaskId: projectERPTaskResult.Id,
                      lagMinutues: addProjectERPTaskDependencyInput.LagMinutues,
                      dependencyType: addProjectERPTaskDependencyInput.DependencyType,
                      predecessorProjectERPTaskId: addProjectERPTaskDependencyInput.PredecessorProjectERPTaskId
                  );
      }

      #endregion

      #endregion

      #region ProjectERPTaskLabelLog
      #region Delete
      foreach (var projectERPTaskLabelLog in projectERPTask.ProjectERPTaskLabelLogs)
      {

        DeleteProjectERPTaskLabelLog(projectERPTaskLabelLog.ProjectERPTaskId, projectERPTaskLabelLog.ProjectERPLabelId);
      }
      #endregion

      #region Add
      foreach (var addProjectERPTaskLabelLogInput in addProjectERPTaskLabelLogInputs)
      {
        AddProjectERPTaskLabelLog(
                     projectERPTaskId: addProjectERPTaskLabelLogInput.ProjectERPTaskId,
                     projectERPLabelId: addProjectERPTaskLabelLogInput.ProjectERPLabelId
                  );
      }
      #endregion

      #endregion

      #region ProjectERPTaskDocument

      #region Delete Project ERP Documents
      foreach (var deleteProjectERPTaskDocumentInput in deleteProjectERPTaskDocumentInputs)
      {
        DeleteProjectERPTaskDocument(
                  id: deleteProjectERPTaskDocumentInput.Id);
      }
      #endregion

      #region Add Project ERP Documnets
      foreach (var addProjectERPTaskDocumentInput in addProjectERPTaskDocumentInputs)
      {
        if (!string.IsNullOrWhiteSpace(addProjectERPTaskDocumentInput.FileKey))
        {
          Document document = null;
          addProjectERPTaskDocumentInput.UploadFileData = App.Providers.Session.GetAs<UploadFileData>(addProjectERPTaskDocumentInput.FileKey);
          document = App.Internals.ApplicationBase.AddDocument(
                  name: addProjectERPTaskDocumentInput.UploadFileData.FileName,
                  fileStream: addProjectERPTaskDocumentInput.UploadFileData.FileData);

          AddProjectERPTaskDocument(
                    projectERPTaskId: projectERPTask.Id,
                    documentId: document.Id,
                    description: addProjectERPTaskDocumentInput.Description);
        }
      }
      #endregion
      #endregion

      //#region Add ProjectERPTaskDocumnet

      //foreach (var fileKey in fileKeies)
      //{

      //    UploadFileData uploadFileData = null;
      //    if (!string.IsNullOrWhiteSpace(fileKey))
      //        uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);

      //    Document document = null;
      //    document = App.Internals.ApplicationBase.AddDocument(
      //      name: uploadFileData.FileName,
      //      fileStream: uploadFileData.FileData)
      //  
      //;

      //    AddProjectERPTaskDocument(
      //        projectERPTaskId: projectERPTask.Id,
      //        documentId: document.Id,
      //        description: ""
      //        )
      //    
      //;
      //}

      //#endregion

      return projectERPTaskResult;


    }

    public ProjectERPTask EditProjectERPTask(
         int id,
         byte[] rowVersion,

         TValue<string> title = null,
         TValue<string> description = null,
         TValue<string> output = null,
         TValue<ProjectERPTaskStatus> status = null,
         TValue<DateTime?> startDateTime = null,
         TValue<DateTime?> dueDateTime = null,
         TValue<int> estimateTime = null,
         TValue<int?> durationMinute = null,
         TValue<int?> progressPercentage = null,
         TValue<ProjectERPTaskPriority> priority = null,
         TValue<int> projectERPId = null,
         TValue<int?> assigneeEmployeeId = null,
         TValue<short> projectERPTaskCategoryId = null,
         TValue<int?> parentTaskId = null)
    {

      var projectERPTask = GetProjectERPTask(id: id);



      if (title != null)
        projectERPTask.Title = title;
      if (description != null)
        projectERPTask.Description = description;
      if (output != null)
        projectERPTask.Output = output;
      if (status != null)
        projectERPTask.Status = status;
      if (startDateTime != null)
        projectERPTask.StartDateTime = startDateTime;
      if (dueDateTime != null)
        projectERPTask.DueDateTime = dueDateTime;
      if (estimateTime != null)
        projectERPTask.EstimateTime = estimateTime;
      if (durationMinute != null)
        projectERPTask.DurationMinute = durationMinute;
      if (progressPercentage != null)
        projectERPTask.ProgressPercentage = progressPercentage;
      if (priority != null)
        projectERPTask.Priority = priority;
      if (projectERPId != null)
        projectERPTask.ProjectERPId = projectERPId;
      if (assigneeEmployeeId != null)
        projectERPTask.AssigneeEmployeeId = assigneeEmployeeId;
      if (projectERPTaskCategoryId != null)
        projectERPTask.ProjectERPTaskCategoryId = projectERPTaskCategoryId;
      if (parentTaskId != null)
        projectERPTask.ParentTaskId = parentTaskId;

      repository.Update(entity: projectERPTask, rowVersion: projectERPTask.RowVersion);
      return projectERPTask;
    }
    #endregion

    #region DeleteProjectERPTask
    public void DeleteProjectERPTask(
        int id)
    {

      var projectERPTask = GetProjectERPTask(id: id);
      repository.Delete(projectERPTask);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPTaskResult> SortProjectERPTaskResult(
        IQueryable<ProjectERPTaskResult> query, SortInput<ProjectERPTaskSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPTaskSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ProjectERPTaskSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        case ProjectERPTaskSortType.Description:
          return query.OrderBy(a => a.Description, type.SortOrder);
        case ProjectERPTaskSortType.Output:
          return query.OrderBy(a => a.Output, type.SortOrder);
        case ProjectERPTaskSortType.StartDateTime:
          return query.OrderBy(a => a.StartDateTime, type.SortOrder);
        case ProjectERPTaskSortType.DueDateTime:
          return query.OrderBy(a => a.DueDateTime, type.SortOrder);
        case ProjectERPTaskSortType.EstimateTime:
          return query.OrderBy(a => a.EstimateTime, type.SortOrder);
        case ProjectERPTaskSortType.DurationMinute:
          return query.OrderBy(a => a.DurationMinute, type.SortOrder);
        case ProjectERPTaskSortType.ProgressPercentage:
          return query.OrderBy(a => a.ProgressPercentage, type.SortOrder);
        case ProjectERPTaskSortType.Priority:
          return query.OrderBy(a => a.Priority, type.SortOrder);
        case ProjectERPTaskSortType.ProjectERPId:
          return query.OrderBy(a => a.ProjectERPId, type.SortOrder);
        case ProjectERPTaskSortType.AssigneeEmployeeId:
          return query.OrderBy(a => a.AssigneeEmployeeId, type.SortOrder);
        case ProjectERPTaskSortType.ProjectERPTaskCategoryId:
          return query.OrderBy(a => a.ProjectERPTaskCategoryId, type.SortOrder);
        case ProjectERPTaskSortType.ParentTaskId:
          return query.OrderBy(a => a.ParentTaskId, type.SortOrder);
        case ProjectERPTaskSortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, type.SortOrder);
        case ProjectERPTaskSortType.CreatorUserId:
          return query.OrderBy(a => a.CreatorUserId, type.SortOrder);
        case ProjectERPTaskSortType.CreatorEmployeeFullName:
          return query.OrderBy(a => a.CreatorEmployeeFullName, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPTaskResult> SearchProjectERPTaskResult(
        IQueryable<ProjectERPTaskResult> query,
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
    public Expression<Func<ProjectERPTask, ProjectERPTaskResult>> ToProjectERPTaskResult =
         projectERPTask => new ProjectERPTaskResult()
         {
           Id = projectERPTask.Id,
           Title = projectERPTask.Title,
           Description = projectERPTask.Description,
           Output = projectERPTask.Output,
           Status = projectERPTask.Status,
           StartDateTime = projectERPTask.StartDateTime,
           DueDateTime = projectERPTask.DueDateTime,
           CreateDateTime = projectERPTask.CreateDateTime,
           CreatorUserId = projectERPTask.CreatorUserId,
           CreatorEmployeeFullName = projectERPTask.CreatorUser.Employee.FirstName + " " + projectERPTask.CreatorUser.Employee.LastName,
           EstimateTime = projectERPTask.EstimateTime,
           DurationMinute = projectERPTask.DurationMinute,
           ProgressPercentage = projectERPTask.ProgressPercentage,
           Priority = projectERPTask.Priority,
           ProjectERPId = projectERPTask.ProjectERPId,
           AssigneeEmployeeId = projectERPTask.AssigneeEmployeeId,
           AssigneeEmployeeFullName = projectERPTask.AssigneeEmployee.FirstName + " " + projectERPTask.AssigneeEmployee.LastName,
           ProjectERPTaskCategoryId = projectERPTask.ProjectERPTaskCategoryId,
           ProjectERPTaskCategoryName = projectERPTask.ProjectERPTaskCategory.Name,
           ParentTaskId = projectERPTask.ParentTaskId,
           RowVersion = projectERPTask.RowVersion,

           ProjectERPTaskDocumentCount = projectERPTask.ProjectERPTaskDocuments.Count(),

           #region ICollection
           ProjectERPTaskDependencies = projectERPTask.ProjectERPTaskDependencies.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPTaskDependencyResult),
           ProjectERPTaskDocuments = projectERPTask.ProjectERPTaskDocuments.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPTaskDocumentResult),
           ProjectERPTaskLabelLogs = projectERPTask.ProjectERPTaskLabelLogs.AsQueryable().Select(App.Internals.ProjectManagement.ToProjectERPTaskLabelLogResult),
           #endregion
         };
    #endregion

  }
}
