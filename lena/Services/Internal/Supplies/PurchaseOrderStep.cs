using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseOrderSteps;
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
    internal PurchaseOrderStep AddPurchaseOrderStep(
        string name,
        bool isActive,
        bool allowUploadDocument,
        string description)
    {

      var entity = repository.Create<PurchaseOrderStep>();
      entity.Name = name;
      entity.Description = description;
      entity.IsActive = isActive;
      entity.AllowUploadDocument = allowUploadDocument;
      entity.UserId = App.Providers.Security.CurrentLoginData.UserId;
      entity.DateTime = DateTime.UtcNow;
      repository.Add(entity);
      return entity;
    }

    internal PurchaseOrderStepDetail AddPurchaseOrderStepDetail(
        int purchaseOrderStepId,
        int purchaseOrderId,
        string description,
        Guid? documentId = null,
        int? userId = null,
        DateTime? dateTime = null
        )
    {

      var entity = repository.Create<PurchaseOrderStepDetail>();
      entity.PurchaseOrderStepId = purchaseOrderStepId;
      entity.PurchaseOrderId = purchaseOrderId;
      entity.Description = description;
      entity.DocumentId = documentId;
      entity.UserId = userId ?? App.Providers.Security.CurrentLoginData.UserId;
      entity.DateTime = dateTime ?? DateTime.UtcNow;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    internal PurchaseOrderStep EditPurchaseOrderStep(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<bool> allowUploadDocument = null,
        TValue<string> description = null
        )
    {

      var entity = GetPurchaseOrderStep(id: id);
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
    internal void DeletePurchaseOrderStep(int id)
    {

      var entity = GetPurchaseOrderStep(id);
      repository.Delete(entity);
    }


    #endregion

    internal void UpdatePurchaseOrderStepStatus(int purchaseOrderId, int purchaseOrderStepId, string fileKey, byte[] rowVersion, string discription)
    {

      var purchaseOrderEntity = GetPurchaseOrder(purchaseOrderId);


      Guid? documentId = null;

      if (!string.IsNullOrWhiteSpace(fileKey))
      {
        var step = GetPurchaseRequestStep(purchaseOrderStepId);


        UploadFileData uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);

        var baseDocument = App.Internals.ApplicationBase.AddBaseEntityDocument(
                      description: $"تغییر مرحله سفارش خرید به {step.Name}",
                      uploadFileData: uploadFileData,
                      baseEntityDocumentTypeId: null,
                      baseEntityId: purchaseOrderEntity.Id,
                      cooperatorId: null,
                      baseEntityDocumentIds: new[] { purchaseOrderEntity.Id });
        documentId = baseDocument.DocumentId;
      }


      var newStepDetail = AddPurchaseOrderStepDetail(purchaseOrderId: purchaseOrderId, purchaseOrderStepId: purchaseOrderStepId, documentId: documentId, description: discription);

      EditPurchaseOrder(purchaseOrder: purchaseOrderEntity, rowVersion: rowVersion, purchaseOrderStepDetailId: newStepDetail.Id);
    }

    #region Get

    internal PurchaseOrderStep GetPurchaseOrderStep(int id) => GetPurchaseOrderStep(e => e, id: id);
    internal TResult GetPurchaseOrderStep<TResult>(
        Expression<Func<PurchaseOrderStep, TResult>> selector,
            int id)
    {

      var result = GetPurchaseOrderSteps(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (result == null)
        throw new PurchaseOrderStepNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetPurchaseOrderSteps<TResult>(
        Expression<Func<PurchaseOrderStep, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> description = null,
        TValue<bool> isActive = null,
        TValue<bool> allowUploadDocument = null)
    {

      var query = repository.GetQuery<PurchaseOrderStep>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (description != null)
        query = query.Where(i => i.Description == description);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (allowUploadDocument != null)
        query = query.Where(i => i.AllowUploadDocument == allowUploadDocument);
      return query.Select(selector);
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<PurchaseOrderStepResult> SortPurchaseOrderStepResult(
        IQueryable<PurchaseOrderStepResult> query,
        SortInput<PurchaseOrderStepSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseOrderStepSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case PurchaseOrderStepSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case PurchaseOrderStepSortType.IsActive:
          return query.OrderBy(a => a.IsActive, options.SortOrder);
        case PurchaseOrderStepSortType.AllowUploadDocument:
          return query.OrderBy(a => a.AllowUploadDocument, options.SortOrder);
        case PurchaseOrderStepSortType.Description:
          return query.OrderBy(a => a.Description, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToComboResult
    internal Expression<Func<PurchaseOrderStep, PurchaseOrderStepComboResult>> ToPurchaseOrderStepComboResult =
        PurchaseOrderStep => new PurchaseOrderStepComboResult
        {
          Id = PurchaseOrderStep.Id,
          Name = PurchaseOrderStep.Name,
          AllowUploadDocument = PurchaseOrderStep.AllowUploadDocument,
          RowVersion = PurchaseOrderStep.RowVersion
        };
    #endregion
    #region ToResult
    internal Expression<Func<PurchaseOrderStep, PurchaseOrderStepResult>> ToPurchaseOrderStepResult =
        PurchaseOrderStep => new PurchaseOrderStepResult()
        {
          Id = PurchaseOrderStep.Id,
          Name = PurchaseOrderStep.Name,
          IsActive = PurchaseOrderStep.IsActive,
          AllowUploadDocument = PurchaseOrderStep.AllowUploadDocument,
          Description = PurchaseOrderStep.Description,
          RowVersion = PurchaseOrderStep.RowVersion
        };
    #endregion
    #region Search
    internal IQueryable<PurchaseOrderStepResult> SearchPurchaseOrderStepResultQuery(
        IQueryable<PurchaseOrderStepResult> query,
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
