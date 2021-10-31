using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseRequestSteps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    internal PurchaseRequestStep AddPurchaseRequestStep(
        string name,
        bool isActive,
        bool allowUploadDocument,
        string description)
    {

      var entity = repository.Create<PurchaseRequestStep>();
      entity.Name = name;
      entity.Description = description;
      entity.IsActive = isActive;
      entity.AllowUploadDocument = allowUploadDocument;
      entity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      entity.DateTime = DateTime.UtcNow;
      repository.Add(entity);
      return entity;
    }
    #endregion

    internal PurchaseRequestStepDetail AddPurchaseRequestStepDetail(
        int purchaseRequestStepId,
        int purchaseRequestId,
        string description,
        Guid? documentId = null,
        int? userId = null,
        DateTime? dateTime = null
        )
    {

      var entity = repository.Create<PurchaseRequestStepDetail>();
      entity.PurchaseRequestStepId = purchaseRequestStepId;
      entity.Description = description;
      entity.PurchaseRequestId = purchaseRequestId;
      entity.DocumentId = documentId;
      entity.UserId = userId ?? App.Providers.Security.CurrentLoginData.UserId;
      entity.DateTime = dateTime ?? DateTime.UtcNow;
      repository.Add(entity);
      return entity;
    }
    #region Edit
    internal PurchaseRequestStep EditPurchaseRequestStep(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<bool> allowUploadDocument = null,
        TValue<string> description = null
        )
    {

      var entity = GetPurchaseRequestStep(id: id);
      if (name != null)
        entity.Name = name;
      if (description != null)
        entity.Description = description;
      if (isActive != null)
        entity.IsActive = isActive;
      if (allowUploadDocument != null)
        entity.AllowUploadDocument = allowUploadDocument;
      repository.Update(entity, rowVersion);
      return entity;
    }
    #endregion
    #region Delete
    internal void DeletePurchaseRequestStep(int id)
    {

      var entity = GetPurchaseRequestStep(id);
      repository.Delete(entity);
    }
    #endregion

    internal void UpdatePurchaseRequestStepStatus(int purchaseRequestId, int purchaseRequestStepId, string fileKey, byte[] rowVersion, string description)
    {



      var purchaseRequestEntity = GetPurchaseRequest(purchaseRequestId);

      Guid? documentId = null;

      if (!string.IsNullOrWhiteSpace(fileKey))
      {
        var step = GetPurchaseRequestStep(purchaseRequestStepId);


        UploadFileData uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);

        var baseDocument = App.Internals.ApplicationBase.AddBaseEntityDocument(
                      description: $"تغییر مرحله درخواست خرید به {step.Name}",
                      uploadFileData: uploadFileData,
                      baseEntityDocumentTypeId: null,
                      baseEntityId: purchaseRequestEntity.Id,
                      cooperatorId: null,
                      baseEntityDocumentIds: new[] { purchaseRequestEntity.Id });
        documentId = baseDocument.DocumentId;
      }

      var newStepDetail = AddPurchaseRequestStepDetail(purchaseRequestId: purchaseRequestId, purchaseRequestStepId: purchaseRequestStepId, documentId: documentId, description: description);

      EditPurchaseRequest(id: purchaseRequestEntity.Id, rowVersion: rowVersion, purchaseRequestStepDetailId: newStepDetail.Id);
    }
    #region Get

    internal PurchaseRequestStep GetPurchaseRequestStep(int id) => GetPurchaseRequestStep(e => e, id: id);
    internal TResult GetPurchaseRequestStep<TResult>(
        Expression<Func<PurchaseRequestStep, TResult>> selector,
            int id)
    {

      var result = GetPurchaseRequestSteps(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (result == null)
        throw new PurchaseRequestStepNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetPurchaseRequestSteps<TResult>(
        Expression<Func<PurchaseRequestStep, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null)
    {

      var query = repository.GetQuery<PurchaseRequestStep>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query.Select(selector);
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<PurchaseRequestStepResult> SortPurchaseRequestStepResult(
        IQueryable<PurchaseRequestStepResult> query,
        SortInput<PurchaseRequestStepSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseRequestStepSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case PurchaseRequestStepSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case PurchaseRequestStepSortType.AllowUploadDocument:
          return query.OrderBy(a => a.AllowUploadDocument, options.SortOrder);
        case PurchaseRequestStepSortType.IsActive:
          return query.OrderBy(a => a.IsActive, options.SortOrder);
        case PurchaseRequestStepSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToComboResult
    internal Expression<Func<PurchaseRequestStep, PurchaseRequestStepComboResult>> ToPurchaseRequestStepComboResult =
        PurchaseRequestStep => new PurchaseRequestStepComboResult
        {
          Id = PurchaseRequestStep.Id,
          Name = PurchaseRequestStep.Name,
          AllowUploadDocument = PurchaseRequestStep.AllowUploadDocument,
          RowVersion = PurchaseRequestStep.RowVersion
        };
    #endregion
    #region ToResult
    internal Expression<Func<PurchaseRequestStep, PurchaseRequestStepResult>> ToPurchaseRequestStepResult =
        PurchaseRequestStep => new PurchaseRequestStepResult()
        {
          Id = PurchaseRequestStep.Id,
          Name = PurchaseRequestStep.Name,
          IsActive = PurchaseRequestStep.IsActive,
          AllowUploadDocument = PurchaseRequestStep.AllowUploadDocument,
          Description = PurchaseRequestStep.Description,
          RowVersion = PurchaseRequestStep.RowVersion
        };
    #endregion
    #region Search
    internal IQueryable<PurchaseRequestStepResult> SearchPurchaseRequestStepResultQuery(
        IQueryable<PurchaseRequestStepResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Name.Contains(searchText) ||
                    item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion
  }
}
