using System;
//using System.Data.Entity;
using System.Linq;
//using System.Web.UI.WebControls;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.BillOfMaterialDocument;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public void DeleteBillOfMaterialDocument(int billOfMaterialDcoumentId)
    {
      var billOfMaterialDocument = GetBillOfMaterialDocument(billOfMaterialDocumentId: billOfMaterialDcoumentId);
      //repository.Delete(billOfMaterialDocument);
      //Core.App.Internals.ApplicationBase.DeleteDocument(billOfMaterialDocument.DocumentId)
      //    
      //;
      if (billOfMaterialDocument.IsDelete != true)
      {
        billOfMaterialDocument.IsDelete = true;
        billOfMaterialDocument.DateOfDelete = DateTime.UtcNow;
        billOfMaterialDocument.DeleteUserId = App.Providers.Security.CurrentLoginData.UserId;
      }
      repository.Update(entity: billOfMaterialDocument, rowVersion: billOfMaterialDocument.RowVersion);
    }
    public BillOfMaterialDocument GetBillOfMaterialDocument(int billOfMaterialDocumentId)
    {
      var billOfMaterialDocument =
                GetBillOfMaterialDocuments(id: billOfMaterialDocumentId)

                    .FirstOrDefault();
      if (billOfMaterialDocument == null)
        throw new BillOfMaterialDocumentNotFoundException(billOfMaterialDocumentId);
      return billOfMaterialDocument;
    }
    public IQueryable<BillOfMaterialDocument> GetBillOfMaterialDocuments(
        TValue<int> id = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialDocumentTypeId = null,
        TValue<string> title = null,
        TValue<string> description = null,
        TValue<Guid> documentId = null,
        TValue<bool> isDelete = null)
    {
      var query = repositpory.GetQuery<BillOfMaterialDocument>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (billOfMaterialStuffId != null)
        query = query.Where(i => i.BillOfMaterialStuffId == billOfMaterialStuffId);
      if (billOfMaterialDocumentTypeId != null)
        query = query.Where(i => i.BillOfMaterialDocumentTypeId == billOfMaterialDocumentTypeId);
      if (title != null)
        query = query.Where(i => i.Title == title);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (documentId != null)
        query = query.Where(i => i.DocumentId == documentId);
      if (isDelete != null)
        query = query.Where(i => i.IsDelete == isDelete);
      return query;
    }
    public BillOfMaterialDocument AddBillOfMaterialDocument(
        short billOfMaterialVersion,
        int billOfMaterialStuffId,
        int billOfMaterialDocumentTypeId,
        string title,
        string description,
        UploadFileData uploadFileData)
    {
      var document = Core.App.Internals.ApplicationBase.AddDocument(
                name: uploadFileData.FileName,
                fileStream: uploadFileData.FileData);
      var billOfMaterialDocument = repository.Create<BillOfMaterialDocument>();
      billOfMaterialDocument.BillOfMaterialVersion = billOfMaterialVersion;
      billOfMaterialDocument.BillOfMaterialStuffId = billOfMaterialStuffId;
      billOfMaterialDocument.BillOfMaterialDocumentTypeId = billOfMaterialDocumentTypeId;
      billOfMaterialDocument.Title = title;
      billOfMaterialDocument.FileName = uploadFileData.FileName;
      billOfMaterialDocument.Description = description;
      billOfMaterialDocument.DocumentId = document.Id;
      billOfMaterialDocument.UserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Add(billOfMaterialDocument);
      return billOfMaterialDocument;
    }
    public BillOfMaterialDocument EditBillOfMaterialDocument(
        byte[] rowVersion,
        int id,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> billOfMaterialDocumentTypeId = null,
        TValue<string> title = null,
        TValue<string> description = null)
    {
      var billOfMaterialDocument =
                GetBillOfMaterialDocument(billOfMaterialDocumentId: id);
      if (billOfMaterialVersion != null)
        billOfMaterialDocument.BillOfMaterialVersion = billOfMaterialVersion;
      if (billOfMaterialStuffId != null)
        billOfMaterialDocument.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (billOfMaterialDocumentTypeId != null)
        billOfMaterialDocument.BillOfMaterialDocumentTypeId = billOfMaterialDocumentTypeId;
      if (title != null)
        billOfMaterialDocument.Title = title;
      if (description != null)
        billOfMaterialDocument.Description = description;
      repositpory.Update(entity: billOfMaterialDocument, rowVersion: rowVersion);
      return billOfMaterialDocument;
    }
    public IQueryable<BillOfMaterialDocumentResult> ToBillOfMaterialDocumentResultQuery(IQueryable<BillOfMaterialDocument> billOfMaterialDocuments)
    {
      var documents = App.Internals.ApplicationBase.GetDocumentResults();
      var query = from item in billOfMaterialDocuments
                  where item.IsDelete == false
                  join doc in documents on item.DocumentId equals doc.Id into g1
                  from document in g1.DefaultIfEmpty()
                  let bom = item.BillOfMaterial
                  let bomProduct = item.BillOfMaterial.Stuff
                  select new BillOfMaterialDocumentResult
                  {
                    Id = item.Id,
                    BillOfMaterialTitle = bom.Title,
                    BillOfMaterialStuffId = bom.StuffId,
                    BillOfMaterialDocumentTypeId = item.BillOfMaterialDocumentTypeId,
                    BillOfMaterialDocumentTypeTitle = item.BillOfMaterialDocumentType.Title,
                    BilOfMaterialStuffCode = bomProduct.Code,
                    DocumentId = item.DocumentId,
                    DocumentTitle = item.Title,
                    DocumentDescription = item.Description,
                    CreationTime = document.CreationTime,
                    FileName = item.FileName ?? document.Name.ToLower().Replace(document.Id + "_", "").Replace(document.Id.ToString(), ""),
                    UserId = item.UserId,
                    EmployeeFullName = item.User.Employee.FirstName + " " + item.User.Employee.LastName,
                    RowVersion = item.RowVersion,
                    IsDelete = item.IsDelete
                  };
      return query;
    }
    public IQueryable<BillOfMaterialDocumentResult> SearchBillOfMaterialDocumentResultQuery(
        AdvanceSearchItem[] advanceSearchItems,
        IQueryable<BillOfMaterialDocumentResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where
                      item.BillOfMaterialDocumentTypeTitle.Contains(searchText) ||
                      item.DocumentDescription.Contains(searchText) ||
                      item.BillOfMaterialDocumentTypeTitle.Contains(searchText) ||
                      item.FileName.Contains(searchText) ||
                      item.DocumentTitle.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public BillOfMaterialDocumentResult ToBillOfMaterialDocumentResult(BillOfMaterialDocument billOfMaterialDocument)
    {
      return new BillOfMaterialDocumentResult
      {
        Id = billOfMaterialDocument.Id,
        BillOfMaterialTitle = billOfMaterialDocument.BillOfMaterial.Title,
        BillOfMaterialStuffId = billOfMaterialDocument.BillOfMaterialStuffId,
        BillOfMaterialDocumentTypeId = billOfMaterialDocument.BillOfMaterialDocumentTypeId,
        BillOfMaterialDocumentTypeTitle = billOfMaterialDocument.BillOfMaterialDocumentType.Title,
        BilOfMaterialStuffCode = billOfMaterialDocument.BillOfMaterial.Stuff.Code,
        DocumentId = billOfMaterialDocument.DocumentId,
        DocumentTitle = billOfMaterialDocument.Title,
        DocumentDescription = billOfMaterialDocument.Description,
        RowVersion = billOfMaterialDocument.RowVersion
      };
    }
    public IOrderedQueryable<BillOfMaterialDocumentResult> SortBillOfMaterialDocumentResult(IQueryable<BillOfMaterialDocumentResult> query, SortInput<BillOfMaterialDocumentSortType> sort)
    {
      switch (sort.SortType)
      {
        case BillOfMaterialDocumentSortType.BillOfMaterialTitle:
          return query.OrderBy(a => a.BillOfMaterialTitle, sort.SortOrder);
        case BillOfMaterialDocumentSortType.BilOfMaterialStuffCode:
          return query.OrderBy(a => a.BilOfMaterialStuffCode, sort.SortOrder);
        case BillOfMaterialDocumentSortType.DocumentTitle:
          return query.OrderBy(a => a.DocumentTitle, sort.SortOrder);
        case BillOfMaterialDocumentSortType.BillOfMaterialDocumentTypeTitle:
          return query.OrderBy(a => a.BillOfMaterialDocumentTypeTitle, sort.SortOrder);
        case BillOfMaterialDocumentSortType.DocumentDescription:
          return query.OrderBy(a => a.DocumentDescription, sort.SortOrder);
        case BillOfMaterialDocumentSortType.FileName:
          return query.OrderBy(a => a.FileName, sort.SortOrder);
        case BillOfMaterialDocumentSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case BillOfMaterialDocumentSortType.CreationTime:
          return query.OrderBy(a => a.CreationTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}