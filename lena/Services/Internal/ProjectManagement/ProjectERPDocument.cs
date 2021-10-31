using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.ProjectManagement.ProjectERPDocument;
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
    public ProjectERPDocument GetProjectERPDocument(int id) => GetProjectERPDocument(selector: e => e, id: id);
    public TResult GetProjectERPDocument<TResult>(
        Expression<Func<ProjectERPDocument, TResult>> selector,
        int id)
    {

      var projectERPDocument = GetProjectERPDocuments(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPDocument == null)
        throw new ProjectERPDocumentNotFoundException(id: id);
      return projectERPDocument;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPDocuments<TResult>(
        Expression<Func<ProjectERPDocument, TResult>> selector,
        TValue<int> id = null,
        TValue<Guid> documentId = null,
        TValue<int> userId = null,
        TValue<DateTime> createDateTime = null,
        TValue<int> projectERPDocumentTypeId = null,
        TValue<int> projectERPId = null)
    {

      var query = repository.GetQuery<ProjectERPDocument>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      //if (documentId != null)
      //    query = query.Where(m => m.DocumentId == documentId);
      if (userId != null)
        query = query.Where(m => m.UserId == userId);
      if (projectERPId != null)
        query = query.Where(m => m.ProjectERPId == projectERPId);
      if (createDateTime != null)
        query = query.Where(m => m.CreateDateTime == createDateTime);
      if (projectERPDocumentTypeId != null)
        query = query.Where(m => m.ProjectERPDocumentTypeId == projectERPDocumentTypeId);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPDocument
    public ProjectERPDocument AddProjectERPDocument(
       string description,
       int projectERPId,
       short projectERPDocumentTypeId,
       Guid documentId)
    {

      var projectERPDocument = repository.Create<ProjectERPDocument>();
      projectERPDocument.UserId = App.Providers.Security.CurrentLoginData.UserId;
      projectERPDocument.DocumentId = documentId;
      projectERPDocument.CreateDateTime = DateTime.Now.ToUniversalTime();
      projectERPDocument.ProjectERPId = projectERPId;
      projectERPDocument.ProjectERPDocumentTypeId = projectERPDocumentTypeId;
      projectERPDocument.Description = description;
      repository.Add(projectERPDocument);
      return projectERPDocument;
    }

    #endregion

    #region DeleteProjectERPDocument
    public void DeleteProjectERPDocument(int id)
    {

      var projectERPDocument = GetProjectERPDocument(
                id: id);
      repository.Delete(projectERPDocument);
    }
    #endregion

    #region ToResult
    public Expression<Func<ProjectERPDocument, ProjectERPDocumentResult>> ToProjectERPDocumentResult =
         projectERPDocument => new ProjectERPDocumentResult()
         {
           Id = projectERPDocument.Id,
           UserId = projectERPDocument.UserId,
           EmployeeFullName = projectERPDocument.User.Employee.FirstName + " " + projectERPDocument.User.Employee.LastName,
           CreateDateTime = projectERPDocument.CreateDateTime,
           Description = projectERPDocument.Description,
           DocumentId = projectERPDocument.DocumentId,
           ProjectERPDocumentTypeId = projectERPDocument.ProjectERPDocumentTypeId,
           ProjectERPDocumentTypeName = projectERPDocument.ProjectERPDocumentType.Name,
           RowVersion = projectERPDocument.RowVersion
         };
    #endregion
  }
}
