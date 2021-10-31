using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Foundation;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.ApplicationBase.BaseEntityDocument;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    #region Get
    public BaseEntityDocument GetBaseEntityDocument(int id) => GetBaseEntityDocument(selector: e => e, id: id);
    public TResult GetBaseEntityDocument<TResult>(
        Expression<Func<BaseEntityDocument, TResult>> selector,
        int id)
    {

      var baseEntityDocument = GetBaseEntityDocuments(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (baseEntityDocument == null)
        throw new BaseEntityDocumentNotFoundException(id);
      return baseEntityDocument;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBaseEntityDocuments<TResult>(
        Expression<Func<BaseEntityDocument, TResult>> selector,
        TValue<int> id = null,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<DateTime> dateTime = null,
        TValue<int> baseEntityId = null,
        TValue<int> userId = null,
        TValue<Guid?> documentId = null,
        TValue<int?> baseEntityDocumentTypeId = null,
        TValue<int?> cooperatorId = null)
    {

      var query = repository.GetQuery<BaseEntityDocument>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (description != null)
        query = query.Where(x => x.Description == description);
      if (isDelete != null)
        query = query.Where(x => x.IsDelete == isDelete);
      if (dateTime != null)
        query = query.Where(x => x.DateTime == dateTime);
      if (baseEntityId != null)
        query = query.Where(x => x.BaseEntityId == baseEntityId);
      if (userId != null)
        query = query.Where(x => x.UserId == userId);
      if (documentId != null)
        query = query.Where(x => x.DocumentId == documentId);
      if (baseEntityDocumentTypeId != null)
        query = query.Where(x => x.BaseEntityDocumentTypeId == baseEntityDocumentTypeId);
      if (cooperatorId != null)
        query = query.Where(x => x.CooperatorId == cooperatorId);
      return query.Select(selector);
    }
    #endregion

    #region Gets LatestBaseEntityDocument
    public IQueryable<TResult> GetLatestBaseEntityDocuments<TResult>(
         Expression<Func<BaseEntityDocument, TResult>> selector

        )
    {

      var baseEntityDocuments = GetBaseEntityDocuments(e => e);
      var groupByResult = (from baseEntityDocument in baseEntityDocuments
                           group baseEntityDocument by baseEntityDocument.BaseEntityId into g
                           select new
                           {
                             Id = g.Max(i => i.Id)
                           });
      var query = from baseEntityDocument in baseEntityDocuments
                  join g in groupByResult on
                        baseEntityDocument.Id equals g.Id
                  select baseEntityDocument;

      return query.Select(selector);
    }
    #endregion

    #region Add

    public BaseEntityDocument AddBaseEntityDocument(
        int[] baseEntityDocumentIds,
        string description,
        int baseEntityId,
        int? baseEntityDocumentTypeId,
        int? cooperatorId,
        UploadFileData uploadFileData)
    {


      Document document = null;
      if (uploadFileData != null)
        document = App.Internals.ApplicationBase.AddDocument(
                  name: uploadFileData.FileName,
                  fileStream: uploadFileData.FileData);

      BaseEntityDocument baseEntityDocument = null;
      foreach (var baseEntityDocId in baseEntityDocumentIds)
      {
        baseEntityDocument = repository.Create<BaseEntityDocument>();
        baseEntityDocument.Description = description;
        baseEntityDocument.IsDelete = false;
        baseEntityDocument.DateTime = DateTime.UtcNow;
        baseEntityDocument.BaseEntityId = baseEntityDocId;
        baseEntityDocument.UserId = App.Providers.Security.CurrentLoginData.UserId;
        if (document != null)
          baseEntityDocument.DocumentId = document.Id;
        baseEntityDocument.BaseEntityDocumentTypeId = baseEntityDocumentTypeId;
        baseEntityDocument.CooperatorId = cooperatorId;
        repository.Add(baseEntityDocument);
      }
      return baseEntityDocument;
    }

    #endregion
    #region Edit
    public BaseEntityDocument EditBaseEntityDocument(
        int id,
        byte[] rowVersion,
        TValue<string> description = null,
        TValue<bool> isDelete = null,
        TValue<int> baseEntityId = null,
        TValue<int?> baseEntityDocumentTypeId = null,
        TValue<int?> cooperatorId = null)
    {

      var baseEntityDocument = GetBaseEntityDocument(id: id);
      return EditBaseEntityDocument(
                    baseEntityDocument: baseEntityDocument,
                    rowVersion: rowVersion,
                    description: description,
                    isDelete: isDelete,
                    baseEntityId: baseEntityId,
                    baseEntityDocumentTypeId: baseEntityDocumentTypeId,
                    cooperatorId: cooperatorId);

    }

    public BaseEntityDocument EditBaseEntityDocument(
                BaseEntityDocument baseEntityDocument,
                byte[] rowVersion,
                TValue<string> description = null,
                TValue<bool> isDelete = null,
                TValue<int> baseEntityId = null,
                TValue<string> fileKey = null,
                TValue<int?> baseEntityDocumentTypeId = null,
                TValue<int?> cooperatorId = null)
    {

      if (description != null)
        baseEntityDocument.Description = description;
      if (isDelete != null)
        baseEntityDocument.IsDelete = isDelete;
      if (baseEntityId != null)
        baseEntityDocument.BaseEntityId = baseEntityId;
      if (fileKey != null)
      {
        if (baseEntityDocument.DocumentId != null)
          App.Internals.ApplicationBase.DeleteDocument(baseEntityDocument.DocumentId.Value);

        if (!string.IsNullOrWhiteSpace(fileKey))
        {
          var uploadFileData = Core.App.Providers.Session.GetAs<UploadFileData>(fileKey);
          var document = App.Internals.ApplicationBase.AddDocument(
                        name: uploadFileData.FileName,
                        fileStream: uploadFileData.FileData);
          baseEntityDocument.DocumentId = document.Id;
        }
      }
      if (baseEntityDocumentTypeId != null)
        baseEntityDocument.BaseEntityDocumentTypeId = baseEntityDocumentTypeId;
      if (cooperatorId != null)
        baseEntityDocument.CooperatorId = cooperatorId;
      repository.Update(rowVersion: rowVersion, entity: baseEntityDocument);
      return baseEntityDocument;
    }

    #endregion
    #region Delete
    public void DeleteBaseEntityDocumentProcess(int id)
    {

      var baseEntityDocument = GetBaseEntityDocument(
                id: id);

      EditBaseEntityDocument(
                id: baseEntityDocument.Id,
                rowVersion: baseEntityDocument.RowVersion,
                isDelete: true);



      //if (baseEntityDocument.DocumentId != null)
      //    DeleteDocument(id: baseEntityDocument.DocumentId.Value)
      //        
      //;



      //repository.Delete(baseEntityDocument);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<BaseEntityDocumentResult> SortBaseEntityDocumentResult(
        IQueryable<BaseEntityDocumentResult> query,
        SortInput<BaseEntityDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case BaseEntityDocumentSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case BaseEntityDocumentSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case BaseEntityDocumentSortType.IsDelete:
          return query.OrderBy(a => a.IsDelete, sort.SortOrder);
        case BaseEntityDocumentSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case BaseEntityDocumentSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case BaseEntityDocumentSortType.BaseEntityDocumentTypeTitle:
          return query.OrderBy(a => a.BaseEntityDocumentTypeTitle, sort.SortOrder);
        case BaseEntityDocumentSortType.BaseEntityEntityDescription:
          return query.OrderBy(a => a.BaseEntityEntityDescription, sort.SortOrder);
        case BaseEntityDocumentSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case BaseEntityDocumentSortType.EntityType:
          return query.OrderBy(a => a.EntityType, sort.SortOrder);
        case BaseEntityDocumentSortType.Appendix:
          return query.OrderBy(a => a.Appendix, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<BaseEntityDocumentResult> SearchBaseEntityDocumentResult(
        IQueryable<BaseEntityDocumentResult> query,
        DateTime? fromDateTime,
        DateTime? toDateTime,
        EntityType? entityType,
        int? employeeId,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.BaseEntityDocumentTypeTitle.Contains(searchText) ||
                    item.BaseEntityEntityDescription.Contains(searchText) ||
                    item.CooperatorName.Contains(searchText) ||
                    item.Description.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText) ||
                    item.BaseEntityId.ToString().Contains(searchText)
                select item;

      if (fromDateTime != null)
        query = query.Where(i => i.DateTime >= fromDateTime);

      if (toDateTime != null)
        query = query.Where(i => i.DateTime <= toDateTime);

      if (employeeId != null)

        query = query.Where(i => i.EmployeeId >= employeeId);

      if (entityType != null)
        query = query.Where(i => i.EntityType == entityType);

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<BaseEntityDocument, BaseEntityDocumentResult>> ToBaseEntityDocumentResult =
                baseEntityDocument => new BaseEntityDocumentResult
                {
                  Id = baseEntityDocument.Id,
                  Description = baseEntityDocument.Description,
                  IsDelete = baseEntityDocument.IsDelete,
                  DateTime = baseEntityDocument.DateTime,
                  BaseEntityId = baseEntityDocument.BaseEntityId,
                  UserId = baseEntityDocument.UserId,
                  DocumentId = baseEntityDocument.DocumentId,
                  BaseEntityDocumentTypeId = baseEntityDocument.BaseEntityDocumentTypeId,
                  CooperatorId = baseEntityDocument.CooperatorId,
                  CooperatorName = baseEntityDocument.Cooperator.Name,
                  BaseEntityDocumentTypeTitle = baseEntityDocument.BaseEntityDocumentType.Title,
                  BaseEntityEntityDescription = baseEntityDocument.BaseEntity.EntityDescription,
                  EmployeeId = baseEntityDocument.User.Employee.Id,
                  EmployeeFullName = baseEntityDocument.User.Employee.FirstName + " " + baseEntityDocument.User.Employee.LastName,
                  EntityType = baseEntityDocument.BaseEntityDocumentType.EntityType,
                  Appendix = baseEntityDocument.DocumentId != null ? true : false,
                  RowVersion = baseEntityDocument.RowVersion
                };
    #endregion

  }
}