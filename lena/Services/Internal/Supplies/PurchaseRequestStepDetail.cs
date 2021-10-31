using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Supplies.PurchaseRequestStepDetail;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Get
    internal PurchaseRequestStepDetail GetPurchaseRequestStepDetail(int id) => GetPurchaseRequestStepDetail(e => e, id: id);
    internal TResult GetPurchaseRequestStepDetail<TResult>(
        Expression<Func<PurchaseRequestStepDetail, TResult>> selector,
            int id)
    {

      var result = GetPurchaseRequestStepDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();

      if (result == null)
        throw new PurchaseRequestStepDetailNotFoundException(id);
      return result;
    }
    #endregion

    #region AddPurchaseRequestStepDetail
    internal void AddPurchaseRequestStepDetail(int purchaseRequestId, int purchaseRequestStepId, string fileKey, byte[] rowVersion, string description)
    {

      var purchaseRequestEntity = GetPurchaseRequest(purchaseRequestId);

      Guid? documentId = null;

      if (!string.IsNullOrWhiteSpace(fileKey))
      {
        var step = GetPurchaseRequestStep(purchaseRequestStepId);


        UploadFileData uploadFileData = App.Providers.Session.GetAs<UploadFileData>(fileKey);

        var baseDocument = App.Internals.ApplicationBase.AddBaseEntityDocument(
                      description: $"ثبت مرحله درخواست خرید به {step.Name}",
                      uploadFileData: uploadFileData,
                      baseEntityDocumentTypeId: null,
                      baseEntityId: purchaseRequestEntity.Id,
                      cooperatorId: null,
                      baseEntityDocumentIds: new[] { purchaseRequestEntity.Id });
        documentId = baseDocument.DocumentId;
      }

      var newStepDetail = AddPurchaseRequestStepDetail(
                purchaseRequestId: purchaseRequestId,
                purchaseRequestStepId: purchaseRequestStepId,
                documentId: documentId,
                description: description);

      EditPurchaseRequest(
                id: purchaseRequestEntity.Id,
                rowVersion: rowVersion,
                purchaseRequestStepDetailId: newStepDetail.Id);
    }
    #endregion

    #region Gets
    internal IQueryable<TResult> GetPurchaseRequestStepDetails<TResult>(
        Expression<Func<PurchaseRequestStepDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> purchaseRequestId = null,
        TValue<int> purchaseRequestStepId = null,
        TValue<int> employeeId = null)
    {

      var query = repository.GetQuery<PurchaseRequestStepDetail>();
      if (id != null)
        query = query.Where(i => i.Id == id);

      if (purchaseRequestId != null)
        query = query.Where(i => i.PurchaseRequestId == purchaseRequestId);


      return query.Select(selector);
    }
    #endregion

    #region Sort
    internal IOrderedQueryable<PurchaseRequestStepDetailResult> SortPurchaseRequestStepDetailResult(
        IQueryable<PurchaseRequestStepDetailResult> query,
        SortInput<PurchaseRequestStepDetailSortType> options)
    {
      switch (options.SortType)
      {
        case PurchaseRequestStepDetailSortType.DateTime:
          return query.OrderBy(m => m.DateTime, options.SortOrder);
        case PurchaseRequestStepDetailSortType.Description:
          return query.OrderBy(m => m.Description, options.SortOrder);
        case PurchaseRequestStepDetailSortType.EmployeeFullName:
          return query.OrderBy(m => m.EmployeeFullName, options.SortOrder);
        case PurchaseRequestStepDetailSortType.PurchaseRequestStepName:
          return query.OrderBy(a => a.PurchaseRequestStepName, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToResult
    internal Expression<Func<PurchaseRequestStepDetail, PurchaseRequestStepDetailResult>> ToPurchaseRequestStepDetailResult =
        PurchaseRequestStepDetail => new PurchaseRequestStepDetailResult()
        {
          Id = PurchaseRequestStepDetail.Id,
          DateTime = PurchaseRequestStepDetail.DateTime,
          DocumentId = PurchaseRequestStepDetail.DocumentId,
          Description = PurchaseRequestStepDetail.Description,
          PurchaseRequestId = PurchaseRequestStepDetail.PurchaseRequestId,
          PurchaseRequestCode = PurchaseRequestStepDetail.PurchaseRequest.Code,
          PurchaseRequestStepId = PurchaseRequestStepDetail.PurchaseRequestStep.Id,
          PurchaseRequestStepName = PurchaseRequestStepDetail.PurchaseRequestStep.Name,
          UserId = PurchaseRequestStepDetail.UserId,
          EmployeeId = PurchaseRequestStepDetail.User.Employee.Id,
          EmployeeFullName = PurchaseRequestStepDetail.User.Employee.FirstName + " " + PurchaseRequestStepDetail.User.Employee.LastName,
          AllowUploadDocument = PurchaseRequestStepDetail.PurchaseRequestStep.AllowUploadDocument,
          RowVersion = PurchaseRequestStepDetail.RowVersion
        };
    #endregion

    #region Search
    internal IQueryable<PurchaseRequestStepDetailResult> SearchPurchaseRequestStepDetailResultQuery(
        IQueryable<PurchaseRequestStepDetailResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Description.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion
  }
}
