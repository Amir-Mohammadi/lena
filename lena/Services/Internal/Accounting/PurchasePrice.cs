using System;
using Microsoft.Data.SqlClient;
//using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.PurchasePrice;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region SavePurchasePricesProcess
    public void SavePurchasePricesProcess(
    int receiptId,
    AddPurchasePriceInput[] addPurchasePrices,
    EditPurchasePriceInput[] editPurchasePrices,
    DeletePurchasePriceInput[] deletePurchasePrices,
    byte[] rowVersion)
    {

      foreach (var addPurchasePrice in addPurchasePrices)
      {
        AddPurchasePriceProcess(
                      currencyId: addPurchasePrice.CurrencyId,
                      price: addPurchasePrice.Price,
                      rialPrice: addPurchasePrice.RialPrice,
                      transferCost: addPurchasePrice.TransferCost,
                      otherCost: addPurchasePrice.OtherCost,
                      dutyCost: addPurchasePrice.DutyCost,
                      discount: addPurchasePrice.Discount,
                      stuffId: addPurchasePrice.StuffId,
                      storeReceiptId: addPurchasePrice.StoreReceiptId,
                      currencyRate: addPurchasePrice.CurrencyRate);
      }

      foreach (var editPurchasePrice in editPurchasePrices)
      {
        EditPurchasePriceProcess(
                      id: editPurchasePrice.Id,
                      rowVersion: editPurchasePrice.RowVersion,
                      currencyId: editPurchasePrice.CurrencyId,
                      price: editPurchasePrice.Price,
                      rialPrice: editPurchasePrice.RialPrice,
                      transferCost: editPurchasePrice.TransferCost,
                      otherCost: editPurchasePrice.OtherCost,
                      dutyCost: editPurchasePrice.DutyCost,
                      discount: editPurchasePrice.Discount,
                      currencyRate: editPurchasePrice.CurrencyRate);
      }

      foreach (var deletePurchasePrice in deletePurchasePrices)
      {
        RemovePurchasePriceProcess(
                      id: deletePurchasePrice.Id,
                      rowVersion: deletePurchasePrice.RowVersion);
      }

      SetStatusInReceipt(
                receiptId: receiptId,
                receiptStatus: ReceiptStatus.Priced,
                rowVersion: rowVersion);

      CheckSaveReceiptPurchasePriceTask(receiptId: receiptId);
    }

    #endregion
    #region AddProcess
    public PurchasePrice AddPurchasePriceProcess(
        byte currencyId,
        int stuffId,
        int storeReceiptId,
        double currencyRate,
        double price,
        double rialPrice = 0,
        double transferCost = 0,
        double dutyCost = 0,
        double otherCost = 0,
        double discount = 0)
    {

      var oldPurchasePrices = GetPurchasePrices(
                    selector: e => e,
                    stuffId: stuffId,
                    storeReceiptId: storeReceiptId,
                    isCurrent: true,
                    isDelete: false);

      foreach (var oldPurchasePrice in oldPurchasePrices)
      {
        App.Internals.Supplies.ArchiveStuffPriceProcess(
                      stuffPrice: oldPurchasePrice,
                      rowVersion: oldPurchasePrice.RowVersion);
      }

      var purchasePrice = AddPurchasePrice(
                    currencyId: currencyId,
                    price: price,
                    rialPrice: rialPrice,
                    transferCost: transferCost,
                    dutyCost: dutyCost,
                    otherCost: otherCost,
                    discount: discount,
                    stuffId: stuffId,
                    storeReceiptId: storeReceiptId,
                    currencyRate: currencyRate);

      return purchasePrice;
    }

    #endregion
    #region Add
    public PurchasePrice AddPurchasePrice(
        byte currencyId,
        double price,
        double rialPrice,
        double transferCost,
        double dutyCost,
        double otherCost,
        double discount,
        int stuffId,
        int storeReceiptId,
        double currencyRate
        )
    {

      var purchasePrice = repository.Create<PurchasePrice>();
      purchasePrice.StoreReceiptId = storeReceiptId;
      purchasePrice.CurrencyRate = currencyRate;
      purchasePrice.RialPrice = rialPrice;
      purchasePrice.TransferCost = transferCost;
      purchasePrice.DutyCost = dutyCost;
      purchasePrice.OtherCost = otherCost;
      purchasePrice.Discount = discount;
      purchasePrice.ActiveForStoreReceipt = App.Internals.WarehouseManagement
                .GetStoreReceipt(id: storeReceiptId);
      App.Internals.Supplies.AddStuffPrice(
                    stuffPrice: purchasePrice,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    type: StuffPriceType.PurchasePrice,
                    description: null);
      return purchasePrice;
    }
    #endregion
    #region Edit
    public PurchasePrice EditPurchasePrice(
        int id,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> storeReceiptId = null,
        TValue<double> currencyRate = null,
        TValue<int?> activeForStoreReceiptId = null)
    {

      var purchasePrice = GetPurchasePrice(id: id);
      return EditPurchasePrice(
                    purchasePrice: purchasePrice,
                    rowVersion: rowVersion,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    status: status,
                    storeReceiptId: storeReceiptId,
                    currencyRate: currencyRate,
                    activeForStoreReceiptId: activeForStoreReceiptId);
    }
    public PurchasePrice EditPurchasePrice(
        PurchasePrice purchasePrice,
        byte[] rowVersion,
        TValue<byte> currencyId = null,
        TValue<double> price = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> storeReceiptId = null,
        TValue<double> currencyRate = null,
        TValue<int?> activeForStoreReceiptId = null)
    {

      if (storeReceiptId != null)
        purchasePrice.StoreReceiptId = storeReceiptId;
      if (currencyRate != null)
        purchasePrice.CurrencyRate = currencyRate;
      if (activeForStoreReceiptId != null)
      {
        if (activeForStoreReceiptId.Value != null)
        {
          purchasePrice.ActiveForStoreReceipt = App.Internals.WarehouseManagement.GetStoreReceipt(
                      id: (activeForStoreReceiptId.Value).Value);
        }
        else
        {
          if (purchasePrice.ActiveForStoreReceipt != null)
            purchasePrice.ActiveForStoreReceipt.CurrentPurchasePrice = null;
          purchasePrice.ActiveForStoreReceipt = null;
        }

      }
      App.Internals.Supplies.EditStuffPrice(
                    stuffPrice: purchasePrice,
                    rowVersion: rowVersion,
                    currencyId: currencyId,
                    price: price,
                    stuffId: stuffId,
                    status: status);
      return purchasePrice;
    }
    #endregion
    #region EditProcess
    public PurchasePrice EditPurchasePriceProcess(
        int id,
        byte[] rowVersion,
        byte currencyId,
        double price,
        double rialPrice,
        double transferCost,
        double dutyCost,
        double otherCost,
        double discount,
        double currencyRate)
    {

      var stuffPrice = App.Internals.Supplies.ArchiveStuffPriceProcess(id: id,
                    rowVersion: rowVersion);

      var purchasePrice = EditPurchasePrice(id: id,
                    rowVersion: stuffPrice.RowVersion,
                    activeForStoreReceiptId: new TValue<int?>(null));
      return AddPurchasePriceProcess(
                    currencyId: currencyId,
                    price: price,
                    rialPrice: rialPrice,
                    transferCost: transferCost,
                    dutyCost: dutyCost,
                    otherCost: otherCost,
                    discount: discount,
                    stuffId: purchasePrice.StuffId,
                    storeReceiptId: purchasePrice.StoreReceiptId,
                    currencyRate: currencyRate);

    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetPurchasePrices<TResult>(
        Expression<Func<PurchasePrice, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<StuffPriceStatus> status = null,
        TValue<int> currencyId = null,
        TValue<int> userId = null,
        TValue<int> storeReceiptId = null,
        TValue<double> currencyRate = null,
        TValue<int> receiptId = null,
        TValue<bool> isCurrent = null,
        TValue<bool> isDelete = null)
    {

      var baseQuery = App.Internals.Supplies.GetStuffPrices(
                    selector: e => e,
                    id: id,
                    stuffId: stuffId,
                    status: status,
                    currencyId: currencyId,
                    userId: userId,
                    priceType: StuffPriceType.PurchasePrice,
                    isCurrent: isCurrent,
                    isDelete: isDelete);
      var query = baseQuery.OfType<PurchasePrice>();
      if (storeReceiptId != null)
        query = query.Where(i => i.StoreReceiptId == storeReceiptId);
      if (currencyRate != null)
        query = query.Where(i => i.CurrencyRate == currencyRate);
      if (receiptId != null)
        query = query.Where(i => i.StoreReceipt.ReceiptId == receiptId);
      return query.Select(selector);
    }
    #endregion
    #region Get
    public PurchasePrice GetPurchasePrice(int id) => GetPurchasePrice(selector: e => e, id: id);
    public TResult GetPurchasePrice<TResult>(Expression<Func<PurchasePrice, TResult>> selector, int id)
    {

      var purchasePrice = GetPurchasePrices(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (purchasePrice == null)
        throw new PurchasePriceNotFoundException(id);
      return purchasePrice;
    }
    #endregion
    #region Sort
    //public IOrderedQueryable<PurchasePriceResult> SortPurchasePriceResult(IQueryable<PurchasePriceResult> input,
    //    SortInput<PurchasePriceSortType> options)
    //{
    //    switch (options.SortType)
    //    {
    //        case PurchasePriceSortType.CurrencyTitle:
    //            return input.OrderBy(i => i.CurrencyTitle, options.SortOrder);
    //        case PurchasePriceSortType.DateTime:
    //            return input.OrderBy(i => i.DateTime, options.SortOrder);
    //        case PurchasePriceSortType.EmployeeFullName:
    //            return input.OrderBy(i => i.EmployeeFullName, options.SortOrder);
    //        case PurchasePriceSortType.IsArchive:
    //            return input.OrderBy(i => i.IsArchive, options.SortOrder);
    //        case PurchasePriceSortType.Price:
    //            return input.OrderBy(i => i.Price, options.SortOrder);
    //        case PurchasePriceSortType.StuffCode:
    //            return input.OrderBy(i => i.StuffCode, options.SortOrder);
    //        case PurchasePriceSortType.StuffName:
    //            return input.OrderBy(i => i.StuffName, options.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    #endregion
    #region RemoveProcess
    public PurchasePrice RemovePurchasePriceProcess(
        int id,
        byte[] rowVersion)
    {

      var purchasePrice = GetPurchasePrice(id: id);
      return RemovePurchasePriceProcess(
                    purchasePrice: purchasePrice,
                    rowVersion: rowVersion);
    }
    public PurchasePrice RemovePurchasePriceProcess(
        PurchasePrice purchasePrice,
        byte[] rowVersion)
    {

      App.Internals.Supplies.ArchiveStuffPriceProcess(
                    stuffPrice: purchasePrice,
                    rowVersion: rowVersion);

      var storeReceipt = App.Internals.WarehouseManagement.GetStoreReceipt(id: purchasePrice.StoreReceiptId);

      App.Internals.WarehouseManagement.EditStoreReceipt(storeReceipt: storeReceipt,
                rowVersion: storeReceipt.RowVersion,
                currentPurchasePriceId: new TValue<int?>(null));





      return purchasePrice;
    }
    #endregion
    #region CheckTask
    public void CheckSaveReceiptPurchasePriceTask(Receipt receipt)
    {

      #region Get ProjectWorkItem
      var projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityScrumTask(
              baseEntityId: receipt.Id,
              scrumTaskType: ScrumTaskTypes.SaveReceiptPurchasePrice);
      if (projectWorkItem == null)
      {
        projectWorkItem = App.Internals.ScrumManagement.GetBaseEntityDoneScrumTask(
                  baseEntityId: receipt.Id,
                  scrumTaskType: ScrumTaskTypes.SaveReceiptPurchasePrice);
      }
      #endregion
      #region check PurchasePrices and DoneProjectWorkItem
      var storeReceipts = App.Internals.WarehouseManagement.GetStoreReceipts(
              selector: e => new
              {
                Id = e.Id,
                CurrentPurchasePriceId = (int?)e.CurrentPurchasePrice.Id
              },
              receiptId: receipt.Id,
              isDelete: false);

      var taskIsDone = storeReceipts.All(i => i.CurrentPurchasePriceId != null);
      if (taskIsDone && projectWorkItem != null && projectWorkItem.ScrumTaskStep != ScrumTaskStep.Done)
      {
        #region DoneTask
        App.Internals.ScrumManagement.DoneScrumTask(
                scrumTask: projectWorkItem,
                rowVersion: projectWorkItem.RowVersion);
        #endregion
      }
      else if (taskIsDone == false && projectWorkItem != null && projectWorkItem.ScrumTaskStep == ScrumTaskStep.Done)
      {
        #region  Add New SaveReceiptPurchasePrice Task
        App.Internals.ProjectManagement.AddProjectWorkItem(
                projectWorkItem: null,
                name: $"ثبت قیمت رسید ورودی {receipt.Code}",
                description: "",
                color: "",
                departmentId: (int)Departments.Accounting,
                estimatedTime: 10800,
                isCommit: false,
                scrumTaskTypeId: (int)ScrumTaskTypes.SaveReceiptPurchasePrice,
                userId: null,
                spentTime: 0,
                remainedTime: 0,
                scrumTaskStep: ScrumTaskStep.ToDo,
                projectWorkId: projectWorkItem.ScrumBackLogId,
                baseEntityId: receipt.Id);
        #endregion
      }


      #endregion

      //todo fix
      //#region SetPriceState
      //var priceStae = ReceiptStatus.NotAction;
      //if (storeReceipts.Any(i => i.CurrentPurchasePriceId != null))
      //    priceStae = ReceiptStatus.Incomplete;
      //if (storeReceipts.All(i => i.CurrentPurchasePriceId != null))
      //    priceStae = ReceiptStatus.Complated;
      //App.Internals.WarehouseManagement.EditReceipt(receipt: receipt,
      //        rowVersion: receipt.RowVersion,
      //        priceState: priceStae)
      //    
      //;

      //#endregion
    }
    public void CheckSaveReceiptPurchasePriceTask(int receiptId)
    {

      var receipt = App.Internals.WarehouseManagement.GetReceipt(id: receiptId); ; CheckSaveReceiptPurchasePriceTask(receipt: receipt);
    }


    #endregion
    #region ToResult
    //public Expression<Func<PurchasePrice, PurchasePriceResult>> ToPurchasePriceResult =>
    //    purchasePrice => new PurchasePriceResult
    //    {
    //        Id = purchasePrice.Id,
    //        CurrencyId = purchasePrice.CurrencyId,
    //        CurrencyTitle = purchasePrice.Currency.Title,
    //        DateTime = purchasePrice.DateTime,
    //        EmployeeFullName = purchasePrice.User.Employee.FirstName + " " + purchasePrice.User.Employee.LastName,
    //        EmployeeId = purchasePrice.User.Employee.Id,
    //        IsArchive = purchasePrice.IsArchive,
    //        Price = purchasePrice.Price,
    //        StuffId = purchasePrice.StuffId,
    //        StuffCode = purchasePrice.Stuff.Code,
    //        StuffName = purchasePrice.Stuff.Name,
    //        RowVersion = purchasePrice.RowVersion
    //    };
    #endregion
    #region Search
    //public IQueryable<PurchasePriceResult> SearchPurchasePriceResultQuery(
    //    IQueryable<PurchasePriceResult> query,
    //    AdvanceSearchItem[] advanceSearchItems,
    //    string searchText)
    //{
    //    if (!string.IsNullOrWhiteSpace(searchText))
    //        query = from purchasePrice in query
    //                where purchasePrice.CurrencyTitle.Contains(searchText) ||
    //                      purchasePrice.EmployeeFullName.Contains(searchText) ||
    //                      purchasePrice.StuffCode.Contains(searchText) ||
    //                      purchasePrice.StuffName.Contains(searchText)
    //                select purchasePrice;
    //    if (advanceSearchItems.Any())
    //        query = query.Where(advanceSearchItems);
    //    return query;
    //}
    #endregion
    #region SetStatusInReceipt
    public void SetStatusInReceipt(int receiptId, ReceiptStatus receiptStatus, byte[] rowVersion)
    {

      var receipt = App.Internals.WarehouseManagement.GetReceipt(id: receiptId);
      receipt.Status |= receiptStatus;
      App.Internals.WarehouseManagement.EditReceipt(receipt: receipt, rowVersion: rowVersion);

    }

    #endregion
  }
}
