using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using lena.Services.Common;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.QualityControl.Exception;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Stuff;
using lena.Models.ApplicationBase.StuffDocument;
using lena.Services.Core;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public void DeleteStuffDocument(int stuffDcoumentId)
    {
      var stuffDocument = GetStuffDocument(stuffDocumentId: stuffDcoumentId);
      repository.Delete(stuffDocument);
      Core.App.Internals.ApplicationBase.DeleteDocument(stuffDocument.DocumentId);
    }
    public StuffDocument GetStuffDocument(int stuffDocumentId)
    {
      var stuffDocument =
                GetStuffDocuments(id: stuffDocumentId)

                    .FirstOrDefault();
      if (stuffDocument == null)
        throw new StuffDocumentNotFoundException(stuffDocumentId);
      return stuffDocument;
    }
    public IQueryable<StuffDocument> GetStuffDocuments(
        TValue<int> id = null,
        TValue<StuffDocumentType> stuffDocumentType = null,
        TValue<int> stuffId = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<int> userId = null,
        TValue<DateTime> dateTime = null)
    {
      var isIdNull = id == null;
      var isStuffDocumentTypeNull = stuffDocumentType == null;
      var isStuffIdNull = stuffId == null;
      var isTitleNull = title == null;
      var isDescriptionNull = description == null;
      var isUserIdNull = userId == null;
      var isDateTimeNull = dateTime == null;
      var stuffDocuments = from item in repositpory.GetQuery<StuffDocument>()
                           where (isIdNull || item.Id == id) &&
                                       (isStuffDocumentTypeNull || item.StuffDocumentType == stuffDocumentType) &&
                                       (isStuffIdNull || item.StuffId == stuffId) &&
                                       (isTitleNull || item.Title == title) &&
                                       (isDescriptionNull || item.Description == description) &&
                                       (isUserIdNull || item.UserId == userId) &&
                                       (isDateTimeNull || item.DateTime == dateTime)
                           select item;
      return stuffDocuments;

    }
    public IQueryable<TResult> GetLatestStuffDocument<TResult>(
        Expression<Func<StuffDocument, TResult>> selector,
        TValue<StuffDocumentType> stuffDocumentType = null,
        TValue<int> stuffId = null)
    {
      var query = GetStuffDocuments(
                stuffId: stuffId,
                stuffDocumentType: stuffDocumentType
                );
      var groupQuery = from stuffDocument in query
                       group stuffDocument by new { stuffDocument.StuffId, stuffDocument.StuffDocumentType } into gItem
                       select gItem.Max(i => i.Id);
      query = from stuffDocument in query
              join gquery in groupQuery on stuffDocument.Id equals gquery
              select stuffDocument;
      return query.Select(selector);

    }
    public StuffDocument AddStuffDocument(
        int stuffId,
        StuffDocumentType stuffDocumentType,
        string title,
        string description,
        UploadFileData uploadFileData)
    {
      if (uploadFileData == null)
        throw new StuffDocumentUploadFileNotFoundException();
      var stuffDocuments = GetStuffDocuments(
                stuffId: stuffId,
                stuffDocumentType: stuffDocumentType);
      if (stuffDocumentType == StuffDocumentType.QualityControlDocument && stuffDocuments.Any())
        throw new QualityControlDocumentExistException(stuffId);
      var document = Core.App.Internals.ApplicationBase.AddDocument(
                name: uploadFileData.FileName,
                fileStream: uploadFileData.FileData);
      var stuffDocument = repositpory.Create<StuffDocument>();
      stuffDocument.StuffDocumentType = stuffDocumentType;
      stuffDocument.StuffId = stuffId;
      stuffDocument.Title = title;
      stuffDocument.Description = description;
      stuffDocument.DocumentId = document.Id;
      stuffDocument.UserId = App.Providers.Security.CurrentLoginData.UserId;
      stuffDocument.DateTime = DateTime.UtcNow.ToUniversalTime();
      stuffDocument.FileName = uploadFileData.FileName;
      repositpory.Add(stuffDocument);
      return stuffDocument;
    }
    public StuffDocument EditStuffDocument(
        byte[] rowVersion,
        int id,
        TValue<StuffDocumentType> stuffDocumentType = null,
        TValue<string> title = null,
        TValue<string> description = null,
        UploadFileData uploadFileData = null,
        TValue<string> fileName = null)
    {
      var stuffDocument = GetStuffDocument(stuffDocumentId: id);
      #region document
      Guid? documentId;
      if (uploadFileData != null)
      {
        documentId = App.Internals.ApplicationBase.AddDocument(
                  name: uploadFileData.FileName,
                  fileStream: uploadFileData.FileData)

              .Id;
      }
      else
      {
        documentId = stuffDocument.DocumentId;
      }
      #endregion
      var editStuffDocument = EditStuffDocument(
           stuffDocument: stuffDocument,
           rowVersion: rowVersion,
           stuffDocumentType: stuffDocumentType,
           title: title,
           description: description,
           documentId: documentId,
           fileName: uploadFileData.FileName);
      return editStuffDocument;
    }
    public StuffDocument EditStuffDocument(
        StuffDocument stuffDocument,
        byte[] rowVersion,
        TValue<StuffDocumentType> stuffDocumentType = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<Guid> documentId = null,
        TValue<string> fileName = null)
    {
      var getAllStuffDocuments = GetStuffDocuments(stuffId: stuffDocument.StuffId);
      if (getAllStuffDocuments.Any(r => r.StuffDocumentType == StuffDocumentType.QualityControlDocument && r.Id != stuffDocument.Id) == true &&
                stuffDocumentType == StuffDocumentType.QualityControlDocument)
        throw new QualityControlDocumentExistException(stuffDocument.StuffId);
      if (stuffDocumentType != null)
        stuffDocument.StuffDocumentType = stuffDocumentType;
      if (title != null)
        stuffDocument.Title = title;
      if (description != null)
        stuffDocument.Description = description;
      if (fileName != null)
        stuffDocument.FileName = fileName;
      if (documentId != null)
        stuffDocument.DocumentId = documentId;
      repository.Update(entity: stuffDocument, rowVersion: rowVersion);
      return stuffDocument;

    }
    public IQueryable<StuffDocumentResult> ToStuffDocumentResultQuery(IQueryable<StuffDocument> stuffDocuments)
    {
      return from item in stuffDocuments
             select new StuffDocumentResult
             {
               Id = item.Id,
               StuffDocumentType = item.StuffDocumentType,
               StuffId = item.StuffId,
               DocumentTitle = item.Title,
               DocumentDescription = item.Description,
               DocumentId = item.DocumentId,
               DateTime = item.DateTime,
               UserId = item.UserId,
               FileName = item.FileName,
               UserFullName = item.User.UserName,
               EmployeeId = item.User.Employee.Id,
               EmployeeFullName = item.User.Employee.FirstName + " " + item.User.Employee.LastName,
               RowVersion = item.RowVersion
             };
    }
    public IQueryable<StuffDocumentResult> SearchStuffDocumentResultQuery(IQueryable<StuffDocumentResult> query, string searchText)
    {
      if (searchText == null)
        return query;
      return from item in query
             where item.DocumentTitle.Contains(searchText)
             select item;
    }
    public StuffDocumentResult ToStuffDocumentResult(StuffDocument stuffDocument)
    {
      return new StuffDocumentResult
      {
        Id = stuffDocument.Id,
        StuffDocumentType = stuffDocument.StuffDocumentType,
        StuffId = stuffDocument.StuffId,
        DocumentTitle = stuffDocument.Title,
        DocumentId = stuffDocument?.DocumentId,
        DocumentDescription = stuffDocument.Description,
        UserId = stuffDocument.UserId,
        DateTime = stuffDocument.DateTime,
        UserFullName = stuffDocument.User.UserName,
        EmployeeId = stuffDocument.User.Employee.Id,
        EmployeeFullName = stuffDocument.User.Employee.FirstName + " " + stuffDocument.User.Employee.LastName,
        RowVersion = stuffDocument.RowVersion
      };
    }
    public IOrderedQueryable<StuffDocumentResult> SortStuffDocumentResult(IQueryable<StuffDocumentResult> query, SortInput<StuffDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffDocumentSortType.Title:
          return query.OrderBy(a => a.DocumentTitle, sort.SortOrder);
        case StuffDocumentSortType.StuffDocumentType:
          return query.OrderBy(a => a.StuffDocumentType, sort.SortOrder);
        case StuffDocumentSortType.Description:
          return query.OrderBy(a => a.DocumentDescription, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}