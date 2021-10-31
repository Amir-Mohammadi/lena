using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
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
    public ProjectERPEventDocument GetProjectERPEventDocument(int id) => GetProjectERPEventDocument(selector: e => e, id: id);
    public TResult GetProjectERPEventDocument<TResult>(
        Expression<Func<ProjectERPEventDocument, TResult>> selector,
        int id)
    {

      var projectERPEventDocument = GetProjectERPEventDocuments(selector: selector,
                id: id).FirstOrDefault();
      if (projectERPEventDocument == null)
        throw new ProjectERPEventDocumentNotFoundException(id: id);
      return projectERPEventDocument;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetProjectERPEventDocuments<TResult>(
        Expression<Func<ProjectERPEventDocument, TResult>> selector,
        TValue<int> id = null,
        TValue<Guid> documentId = null,
        TValue<int> creatorUserId = null,
        TValue<DateTime> creationDateTime = null,
        TValue<int> projectERPEventDocumentTypeId = null,
        TValue<int> projectERPEventId = null)
    {

      var query = repository.GetQuery<ProjectERPEventDocument>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      //if (documentId != null)
      //    query = query.Where(m => m.DocumentId == documentId);
      if (creatorUserId != null)
        query = query.Where(m => m.CreatorUserId == creatorUserId);
      if (projectERPEventId != null)
        query = query.Where(m => m.ProjectERPEventId == projectERPEventId);
      if (creationDateTime != null)
        query = query.Where(m => m.CreationDateTime == creationDateTime);
      if (documentId != null)
        query = query.Where(m => m.DocumentId == documentId);
      return query.Select(selector);
    }
    #endregion

    #region AddProjectERPEventDocument
    public ProjectERPEventDocument AddProjectERPEventDocument(
       string description,
       int projectERPEventId,
       Guid documentId)
    {

      var projectERPEventDocument = repository.Create<ProjectERPEventDocument>();
      projectERPEventDocument.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      projectERPEventDocument.DocumentId = documentId;
      projectERPEventDocument.CreationDateTime = DateTime.Now.ToUniversalTime();
      projectERPEventDocument.ProjectERPEventId = projectERPEventId;
      projectERPEventDocument.Description = description;
      repository.Add(projectERPEventDocument);
      return projectERPEventDocument;
    }

    #endregion

    #region DeleteProjectERPEventDocument
    public void DeleteProjectERPEventDocument(int id)
    {

      var projectERPEventDocument = GetProjectERPEventDocument(
                id: id);
      repository.Delete(projectERPEventDocument);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ProjectERPEventDocumentResult> SortProjectERPEventDocumentResult(
    IQueryable<ProjectERPEventDocumentResult> query, SortInput<ProjectERPEventDocumentSortType> type)
    {
      switch (type.SortType)
      {
        case ProjectERPEventDocumentSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ProjectERPEventDocumentResult> SearchProjectERPEventDocumentResult(
        IQueryable<ProjectERPEventDocumentResult> query,
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
    public Expression<Func<ProjectERPEventDocument, ProjectERPEventDocumentResult>> ToProjectERPEventDocumentResult =
         projectERPEventDocument => new ProjectERPEventDocumentResult()
         {
           Id = projectERPEventDocument.Id,
           CreatorUserId = projectERPEventDocument.CreatorUserId,
           CreatorEmployeeFullName = projectERPEventDocument.CreatorUser.Employee.FirstName + " " + projectERPEventDocument.CreatorUser.Employee.LastName,
           CreationDateTime = projectERPEventDocument.CreationDateTime,
           Description = projectERPEventDocument.Description,
           DocumentId = projectERPEventDocument.DocumentId,
           RowVersion = projectERPEventDocument.RowVersion
         };
    #endregion

    #region ToResultQuery
    public IQueryable<ProjectERPEventDocumentResult> ToProjectERPEventDocumentResultQuery(
        IQueryable<ProjectERPEventDocument> projectERPEventDocuments,
        IQueryable<Document> documents
        )
    {
      var query = from projectERPEventDocument in projectERPEventDocuments
                  join documnet in documents
                  on projectERPEventDocument.DocumentId equals documnet.Id
                  select new ProjectERPEventDocumentResult
                  {
                    Id = projectERPEventDocument.Id,
                    CreatorUserId = projectERPEventDocument.CreatorUserId,
                    CreatorEmployeeFullName = projectERPEventDocument.CreatorUser.Employee.FirstName + " " + projectERPEventDocument.CreatorUser.Employee.LastName,
                    CreationDateTime = projectERPEventDocument.CreationDateTime,
                    Description = projectERPEventDocument.Description,
                    DocumentId = projectERPEventDocument.DocumentId,
                    RowVersion = projectERPEventDocument.RowVersion,

                    FileName = documnet.Name,
                    FileSize = documnet.FileSize,
                    FileType = documnet.FileType
                  };

      return query;
    }
    #endregion
  }
}
