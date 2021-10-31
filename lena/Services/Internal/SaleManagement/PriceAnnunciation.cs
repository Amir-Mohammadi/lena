using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains;
using lena.Models.SaleManagement.PriceAnnunciation;
using lena.Models.SaleManagement.PriceAnnunciationItem;
using System.Linq.Expressions;
using lena.Domains.Enums;


using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public PriceAnnunciation AddPriceAnnunciation(
        DateTime validityFromDate,
        DateTime? validityToDate,
        int cooperatorId,
        string description,
        Guid? documentId
    )
    {

      var priceAnnunciation = repository.Create<PriceAnnunciation>();

      priceAnnunciation.FromDate = validityFromDate;
      priceAnnunciation.ToDate = validityToDate;
      priceAnnunciation.CooperatorId = cooperatorId;
      priceAnnunciation.DocumentId = documentId;
      priceAnnunciation.Status = PriceAnnunciationStatus.NotAction;
      priceAnnunciation.RegisterarUserId = App.Providers.Security.CurrentLoginData.UserId;
      priceAnnunciation.RegisterDateTime = DateTime.UtcNow;
      priceAnnunciation.Description = description;
      repository.Add(priceAnnunciation);
      return priceAnnunciation;
    }
    #endregion

    #region Edit
    public PriceAnnunciation EditPriceAnnunciation(
        int id,
        TValue<int> cooperatorId = null,
        TValue<DateTime> validityFromDate = null,
        TValue<DateTime> validityToDate = null,
        TValue<PriceAnnunciationStatus> status = null,
        TValue<Guid> documentId = null
        )
    {

      var priceAnnunciation = GetPriceAnnunciation(id: id);
      if (status != null)
        priceAnnunciation.Status = status;
      if (documentId != null)
        priceAnnunciation.DocumentId = documentId;
      if (validityFromDate != null)
        priceAnnunciation.FromDate = validityFromDate;
      if (validityToDate != null)
        priceAnnunciation.ToDate = validityToDate;
      if (cooperatorId != null)
        priceAnnunciation.CooperatorId = cooperatorId;
      repository.Update(rowVersion: priceAnnunciation.RowVersion, entity: priceAnnunciation);
      return priceAnnunciation;
    }
    #endregion

    #region AddProcess
    public PriceAnnunciation AddPriceAnnunciationProcess(
        int cooperatorId,
        UploadFileData uploadFileData,
        DateTime validityFromDate,
        DateTime? validityToDate,
        string description,
        AddPriceAnnunciationItemInput[] priceAnnunciationItems
    )
    {

      var saleManagements = App.Internals.SaleManagement;
      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
              name: uploadFileData.FileName,
              fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }

      var priceAnnunciation = AddPriceAnnunciation(
                documentId: documentId,
                cooperatorId: cooperatorId,
                validityFromDate: validityFromDate,
                description: description,
                validityToDate: validityToDate
               );

      foreach (var item in priceAnnunciationItems)
      {
        saleManagements.AddPriceAnnunciationItem(
                  price: item.Price,
                  currencyId: item.CurrencyId,
                  stuffId: item.StuffId,
                  count: item.Count,
                  priceAnnunciationId: priceAnnunciation.Id);
      }
      return priceAnnunciation;
    }
    #endregion

    #region EditProcess
    public PriceAnnunciation EditPriceAnnunciationProcess(
        int id,
        UploadFileData uploadFileData,
        DateTime validityFromDate,
        DateTime? validityToDate,
        int cooperatorId,
        AddPriceAnnunciationItemInput[] addPriceAnnunciationItems,
        EditPriceAnnunciationItemInput[] editPriceAnnunciationItems,
        int[] deletePriceAnnunciationItems,
        TValue<PriceAnnunciationStatus> status = null
    )
    {


      var priceAnnunciation = GetPriceAnnunciation(id: id);
      if (priceAnnunciation.Status != PriceAnnunciationStatus.NotAction)
      {
        throw new CantEditPriceAnnunciationException(id: id);
      }

      var saleManagements = App.Internals.SaleManagement;
      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
              name: uploadFileData.FileName,
              fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }

      var editPriceAnnunciation = EditPriceAnnunciation(
                id: id,
                documentId: documentId,
                validityFromDate: validityFromDate,
                validityToDate: validityToDate,
                cooperatorId: cooperatorId,
                status: status
               );

      foreach (var item in addPriceAnnunciationItems)
      {
        saleManagements.AddPriceAnnunciationItem(
                  currencyId: item.CurrencyId,
                  price: item.Price,
                  stuffId: item.StuffId,
                  count: item.Count,
                  priceAnnunciationId: editPriceAnnunciation.Id
                  );
      }

      foreach (var item in editPriceAnnunciationItems)
      {
        saleManagements.EditPriceAnnunciationItem(
                  id: item.Id,
                  currencyId: item.CurrencyId,
                  price: item.Price,
                  stuffId: item.StuffId,
                  count: item.Count
                  );
      }

      foreach (var itemId in deletePriceAnnunciationItems)
      {
        saleManagements.DeletePriceAnnunciationItem(itemId);
      }

      return priceAnnunciation;
    }
    #endregion

    #region Gets
    internal IQueryable<TResult> GetPriceAnnunciations<TResult>(
         Expression<Func<PriceAnnunciation, TResult>> selector,
         TValue<int> id = null,
         TValue<int> cooperatorId = null,
         TValue<int> employeeId = null,
         TValue<System.DateTime> validityFromDate = null,
         TValue<System.DateTime> validityToDate = null
         )
    {

      var baseQuery = repository.GetQuery<PriceAnnunciation>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (cooperatorId != null)
        baseQuery = baseQuery.Where(i => i.Cooperator.Id == cooperatorId);
      if (employeeId != null)
        baseQuery = baseQuery.Where(i => i.RegisterarUser.Employee.Id == employeeId);
      if (validityFromDate != null)
        baseQuery = baseQuery.Where(i => i.FromDate >= validityFromDate);
      if (validityToDate != null)
        baseQuery = baseQuery.Where(i => i.ToDate <= validityToDate);
      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public IQueryable<PriceAnnunciationResult> ToPriceAnnunciationResultQuery(
       IQueryable<PriceAnnunciation> priceAnnunciations
       )
    {


      var saleManagements = App.Internals.SaleManagement;
      var priceAnnunciationItems = saleManagements.GetPriceAnnunciationItems(selector: e => e);

      var resultQuery = from priceAnnunciation in priceAnnunciations
                        select new PriceAnnunciationResult
                        {
                          Id = priceAnnunciation.Id,
                          RowVersion = priceAnnunciation.RowVersion,
                          FromDate = priceAnnunciation.FromDate,
                          ToDate = priceAnnunciation.ToDate,
                          RegisterarUserName = priceAnnunciation.RegisterarUser.Employee.FirstName + " " + priceAnnunciation.RegisterarUser.Employee.LastName,
                          RegisterDateTime = priceAnnunciation.RegisterDateTime,
                          DocumentId = priceAnnunciation.DocumentId,
                          PriceAnnunciationItems = priceAnnunciation.PriceAnnunciationItems.AsQueryable().Select(App.Internals.SaleManagement.ToPriceAnnunciationItem),
                          Status = priceAnnunciation.Status,
                          CooperatorName = priceAnnunciation.Cooperator.Name,
                          CooperatorId = priceAnnunciation.CooperatorId,
                          Description = priceAnnunciation.Description
                        };

      return resultQuery;
    }
    #endregion

    #region ToReportResult
    public IQueryable<PriceAnnunciationResult> ToReportPriceAnnunciationResultQuery(
       IQueryable<PriceAnnunciation> priceAnnunciations
       )
    {


      var saleManagements = App.Internals.SaleManagement;
      var priceAnnunciationItems = saleManagements.GetPriceAnnunciationItems(selector: e => e);

      var resultQuery = from priceAnnunciation in priceAnnunciations
                        select new PriceAnnunciationResult
                        {
                          Id = priceAnnunciation.Id,
                          FromDate = priceAnnunciation.FromDate,
                          ToDate = priceAnnunciation.ToDate,
                          RegisterarUserName = priceAnnunciation.RegisterarUser.Employee.FirstName + " " + priceAnnunciation.RegisterarUser.Employee.LastName,
                          RegisterDateTime = priceAnnunciation.RegisterDateTime,
                          Status = priceAnnunciation.Status,
                          CooperatorName = priceAnnunciation.Cooperator.Name
                        };

      return resultQuery;
    }
    #endregion

    #region Search
    public IQueryable<PriceAnnunciationResult> SearchPriceAnnunciation(IQueryable<PriceAnnunciationResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.RegisterarUserName.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<PriceAnnunciationResult> SortPriceAnnunciationResult(IQueryable<PriceAnnunciationResult> query,
        SortInput<PriceAnnunciationSortType> sort)
    {
      switch (sort.SortType)
      {
        case PriceAnnunciationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PriceAnnunciationSortType.RegisterarUserName:
          return query.OrderBy(a => a.RegisterarUserName, sort.SortOrder);
        case PriceAnnunciationSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        case PriceAnnunciationSortType.ValidityFromDate:
          return query.OrderBy(a => a.FromDate, sort.SortOrder);
        case PriceAnnunciationSortType.ValidityToDate:
          return query.OrderBy(a => a.ToDate, sort.SortOrder);
        case PriceAnnunciationSortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case PriceAnnunciationSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Get
    public PriceAnnunciation GetPriceAnnunciation(int id)
    {

      var priceAnnunciation = GetPriceAnnunciations(selector: e => e, id: id).FirstOrDefault();

      return priceAnnunciation;
    }
    #endregion

    #region Delete
    public void DeletePriceAnnunciation(int id)
    {


      var priceAnnunciation = App.Internals.SaleManagement.GetPriceAnnunciation(id: id);

      repository.Delete(priceAnnunciation);
    }
    #endregion

    #region DeleteProcess
    public void DeletePriceAnnunciationProcess(int id)
    {

      var priceAnnunciation = App.Internals.SaleManagement.GetPriceAnnunciation(id: id);

      if (priceAnnunciation.Status != PriceAnnunciationStatus.NotAction)
      {
        throw new CantDeletePriceAnnunciationException(id: id);
      }

      var priceAnnunciationItems = App.Internals.SaleManagement.GetPriceAnnunciationItems(selector: e => e, priceAnnunciationId: id);
      foreach (var item in priceAnnunciationItems)
      {
        App.Internals.SaleManagement.DeletePriceAnnunciationItem(item.Id);
      }

      repository.Delete(priceAnnunciation);
    }
    #endregion

    #region GetFull       
    public PriceAnnunciationResult GetFullPriceAnnunciation(
       int id)
    {

      var saleManagements = App.Internals.SaleManagement;
      var priceAnnunciations = saleManagements.GetPriceAnnunciations(
                selector: e => e,
                id: id);
      var priceAnnunciationResults = saleManagements.ToPriceAnnunciationResultQuery(priceAnnunciations: priceAnnunciations)

                .FirstOrDefault();
      return priceAnnunciationResults;
    }
    #endregion
  }
}
