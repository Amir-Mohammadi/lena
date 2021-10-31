using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
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
    public PriceAnnunciationItem AddPriceAnnunciationItem(
        double price,
        byte currencyId,
        int stuffId,
        double? count,
        int priceAnnunciationId
    )
    {

      var priceAnnunciationItem = repository.Create<PriceAnnunciationItem>();

      priceAnnunciationItem.Price = price;
      priceAnnunciationItem.CurrencyId = currencyId;
      priceAnnunciationItem.StuffId = stuffId;
      priceAnnunciationItem.Count = count;
      priceAnnunciationItem.PriceAnnunciationId = priceAnnunciationId;
      priceAnnunciationItem.Status = PriceAnnunciationItemStatus.NotAction;
      repository.Add(priceAnnunciationItem);
      return priceAnnunciationItem;
    }
    #endregion

    #region Edit
    public PriceAnnunciationItem EditPriceAnnunciationItem(
        int id,
        TValue<double> price = null,
        TValue<byte> currencyId = null,
        TValue<PriceAnnunciationItemStatus> status = null,
        TValue<string> description = null,
        TValue<int> stuffId = null,
        TValue<double> count = null

        )
    {

      var priceAnnunciationItem = GetPriceAnnunciationItem(id: id);
      if (status != null)
        priceAnnunciationItem.Status = status;
      if (description != null)
        priceAnnunciationItem.Description = description;
      if (price != null)
        priceAnnunciationItem.Price = price;
      if (currencyId != null)
        priceAnnunciationItem.CurrencyId = currencyId;
      if (count != null)
        priceAnnunciationItem.Count = count;
      if (stuffId != null)
        priceAnnunciationItem.StuffId = stuffId;
      if (status != null)
      {
        priceAnnunciationItem.ConfirmerUserId = App.Providers.Security.CurrentLoginData.UserId;
        priceAnnunciationItem.ConfirmationDateTime = DateTime.UtcNow;
      }
      repository.Update(rowVersion: priceAnnunciationItem.RowVersion, entity: priceAnnunciationItem);
      return priceAnnunciationItem;
    }
    #endregion

    #region ToResult
    Expression<Func<PriceAnnunciationItem, PriceAnnunciationItemResult>> ToPriceAnnunciationItem =
        (priceAnnunciationItem) => new PriceAnnunciationItemResult
        {
          Id = priceAnnunciationItem.Id,
          RowVersion = priceAnnunciationItem.RowVersion,
          PriceAnnunciation = priceAnnunciationItem.Price,
          CurrencyName = priceAnnunciationItem.Currency.Title,
          CurrencyId = priceAnnunciationItem.Currency.Id,
          StuffId = priceAnnunciationItem.Stuff.Id,
          StuffCode = priceAnnunciationItem.Stuff.Code,
          StuffName = priceAnnunciationItem.Stuff.Name,
          Description = priceAnnunciationItem.Description,
          Status = priceAnnunciationItem.Status,
          ConfirmerUserName = priceAnnunciationItem.ConfirmerUser.Employee.FirstName + " " + priceAnnunciationItem.ConfirmerUser.Employee.LastName,
          ConfirmationDateTime = priceAnnunciationItem.ConfirmationDateTime,
          Count = priceAnnunciationItem.Count,
          TotalPrice = priceAnnunciationItem.Count != null ? priceAnnunciationItem.Price * priceAnnunciationItem.Count : 0
        };
    #endregion

    #region ToComboResult
    public Expression<Func<PriceAnnunciationItem, PriceAnnunciationItemComboResult>> ToPriceAnnunciationItemComboResult =
        (priceAnnunciationItem) => new PriceAnnunciationItemComboResult
        {
          Id = priceAnnunciationItem.Id,
          Price = priceAnnunciationItem.Price,
          CurrencyName = priceAnnunciationItem.Currency.Title,
          StuffId = priceAnnunciationItem.StuffId,
          CooperatorId = priceAnnunciationItem.PriceAnnunciation.CooperatorId
        };
    #endregion

    #region Gets
    internal IQueryable<TResult> GetPriceAnnunciationItems<TResult>(
         Expression<Func<PriceAnnunciationItem, TResult>> selector,
         TValue<int> id = null,
         TValue<int> stuffId = null,
         TValue<int> cooperatorId = null,
         TValue<int> priceAnnunciationId = null)
    {

      var baseQuery = repository.GetQuery<PriceAnnunciationItem>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (stuffId != null)
        baseQuery = baseQuery.Where(i => i.StuffId == stuffId);
      if (cooperatorId != null)
        baseQuery = baseQuery.Where(i => i.PriceAnnunciation.CooperatorId == cooperatorId);
      if (priceAnnunciationId != null)
        baseQuery = baseQuery.Where(i => i.PriceAnnunciationId == priceAnnunciationId);
      return baseQuery.Select(selector);
    }
    #endregion

    #region GetsCombo
    internal IQueryable<TResult> GetPriceAnnunciationItemsCombo<TResult>(
         Expression<Func<PriceAnnunciationItem, TResult>> selector,
         TValue<int> id = null,
         TValue<int> stuffId = null,
         TValue<int> cooperatorId = null,
         TValue<PriceAnnunciationItemStatus> status = null,
         TValue<PriceAnnunciationStatus> priceAnnunciationStatus = null,
         TValue<int> priceAnnunciationId = null)
    {

      var baseQuery = repository.GetQuery<PriceAnnunciationItem>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (stuffId != null)
        baseQuery = baseQuery.Where(i => i.StuffId == stuffId);
      if (cooperatorId != null)
        baseQuery = baseQuery.Where(i => i.PriceAnnunciation.CooperatorId == cooperatorId);
      if (status != null)
        baseQuery = baseQuery.Where(i => i.Status == status);
      if (priceAnnunciationStatus != null)
        baseQuery = baseQuery.Where(i => i.PriceAnnunciation.Status == priceAnnunciationStatus);
      if (priceAnnunciationId != null)
        baseQuery = baseQuery.Where(i => i.PriceAnnunciationId == priceAnnunciationId);
      return baseQuery.Select(selector);
    }
    #endregion

    #region Get
    public PriceAnnunciationItem GetPriceAnnunciationItem(int id)
    {

      var priceAnnunciationItem = GetPriceAnnunciationItems(selector: e => e, id: id).FirstOrDefault();
      return priceAnnunciationItem;
    }
    #endregion

    #region EditProcess
    public PriceAnnunciationItem EditPriceAnnunciationItemProcess(
        int id,
        TValue<PriceAnnunciationItemStatus> status,
        TValue<string> description = null
        )
    {

      var saleManagements = App.Internals.SaleManagement;

      var editPriceAnnunciationItem = EditPriceAnnunciationItem(
                id: id,
                status: status,
                description: description
               );

      ResetPriceAnnunciationStatus(id);


      return editPriceAnnunciationItem;
    }
    #endregion

    #region ResetStatus
    public void ResetPriceAnnunciationStatus(int priceAnnunciationItemId)
    {

      var priceAnnunciationItem = GetPriceAnnunciationItem(id: priceAnnunciationItemId);
      var priceAnnunciation = priceAnnunciationItem.PriceAnnunciation;
      var priceAnnunciationItems = priceAnnunciation.PriceAnnunciationItems;

      if (priceAnnunciationItems.All(x => x.Status != PriceAnnunciationItemStatus.NotAction))
      {
        if (priceAnnunciationItems.Any(x => x.Status == PriceAnnunciationItemStatus.Rejected))
        {
          priceAnnunciation.Status = PriceAnnunciationStatus.Rejected;
        }
        else
        {
          priceAnnunciation.Status = PriceAnnunciationStatus.Accepted;
        }
      }

      else if (priceAnnunciationItems.Any(x => x.Status != PriceAnnunciationItemStatus.NotAction))
      {
        priceAnnunciation.Status = PriceAnnunciationStatus.InProgress;
      }
      else
      {
        priceAnnunciation.Status = PriceAnnunciationStatus.NotAction;
      }

      EditPriceAnnunciation(id: priceAnnunciation.Id, status: priceAnnunciation.Status);
    }
    #endregion

    #region Delete
    public void DeletePriceAnnunciationItem(int id)
    {


      var priceAnnunciationItem = App.Internals.SaleManagement.GetPriceAnnunciationItem(id: id);

      repository.Delete(priceAnnunciationItem);
    }
    #endregion
  }
}
