using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.ProjectManagement.ProjectERPTaskDocument;
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
    public ProjectERPTaskDocument GetProjectERPTaskDocument(int id) => GetProjectERPTaskDocument(selector: e => e, id: id);
    public TResult GetProjectERPTaskDocument<TResult>(
        Expression<Func<ProjectERPTaskDocument, TResult>> selector,
        int id)
    {

      var projectERPTaskDocument = GetProjectERPTaskDocuments(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPTaskDocument == null)
        throw new ProjectERPTaskDocumentNotFoundException(id: id);
      return projectERPTaskDocument;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPTaskDocuments<TResult>(
        Expression<Func<ProjectERPTaskDocument, TResult>> selector,
        TValue<int> id = null,
        TValue<Guid> documentId = null,
        TValue<int> creatorUserId = null,
        TValue<DateTime> createDateTime = null,
        TValue<int> projectERPTaskId = null)
    {

      var query = repository.GetQuery<ProjectERPTaskDocument>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      //if (documentId != null)
      //    query = query.Where(m => m.DocumentId == documentId);
      if (creatorUserId != null)
        query = query.Where(m => m.CreatorUserId == creatorUserId);
      if (projectERPTaskId != null)
        query = query.Where(m => m.ProjectERPTaskId == projectERPTaskId);
      if (createDateTime != null)
        query = query.Where(m => m.CreateDateTime == createDateTime);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPTaskDocument
    public ProjectERPTaskDocument AddProjectERPTaskDocument(
       string description,
       int projectERPTaskId,
       Guid documentId)
    {

      var projectERPTaskDocument = repository.Create<ProjectERPTaskDocument>();
      projectERPTaskDocument.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      projectERPTaskDocument.DocumentId = documentId;
      projectERPTaskDocument.CreateDateTime = DateTime.Now.ToUniversalTime();
      projectERPTaskDocument.ProjectERPTaskId = projectERPTaskId;
      projectERPTaskDocument.Description = description;
      repository.Add(projectERPTaskDocument);
      return projectERPTaskDocument;
    }

    #endregion

    #region DeleteProjectERPTaskDocument
    public void DeleteProjectERPTaskDocument(int id)
    {

      var projectERPTaskDocument = GetProjectERPTaskDocument(
                id: id);
      repository.Delete(projectERPTaskDocument);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPTaskDocument, ProjectERPTaskDocumentResult>> ToProjectERPTaskDocumentResult =
         projectERPTaskDocument => new ProjectERPTaskDocumentResult()
         {
           Id = projectERPTaskDocument.Id,
           UserId = projectERPTaskDocument.CreatorUserId,
           EmployeeFullName = projectERPTaskDocument.CreatorUser.Employee.FirstName + " " + projectERPTaskDocument.CreatorUser.Employee.LastName,
           CreateDateTime = projectERPTaskDocument.CreateDateTime,
           Description = projectERPTaskDocument.Description,
           //DocumentId = projectERPTaskDocument.DocumentId,
           RowVersion = projectERPTaskDocument.RowVersion
         };
    #endregion
  }
}
