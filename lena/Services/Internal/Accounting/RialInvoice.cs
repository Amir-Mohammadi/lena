using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Services.Internals.Supplies.Exception;
//using Parlar.DAL.Repositories;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Accounting.RialInvoice;
using lena.Models.Common;
using Currency = lena.Domains.Enums.Currency;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public IQueryable<TResult> GetCalculatedRialInvoiceProcess<TResult>(
      Expression<Func<CalculatedRialInvoiceResult, TResult>> selector,
      TValue<int> receiptId = null,
      TValue<bool> updateRialRateIsUsedState = null)
    {

      List<CalculatedRialInvoiceResult> calculatedRialInvoiceResultList = new List<CalculatedRialInvoiceResult>();
      var receipt = App.Internals.WarehouseManagement.GetReceipt(
                    selector: e => e,
                    id: receiptId);
      var newShoppings = receipt.StoreReceipts.OfType<NewShopping>();
      var newShoppingsCostsAndDiscountsFinancialDocumentIds = GetNewShoppingsCostsAndDiscountsFinancialDocumentIds(
                    newShoppings: newShoppings)
                .ToArray();
      RedivideFinancialDocumentsProcess(
                    financialDocumentIds: newShoppingsCostsAndDiscountsFinancialDocumentIds,
                    throwExceptionIfThereIsNoRialRate: true,
                    isTemp: false);
      int rowCounter = 1;
      foreach (var newShopping in newShoppings)
      {
        var ladingItem = newShopping.LadingItem;
        var cargoItem = ladingItem.CargoItem;
        var purchaseOrderItem = cargoItem.PurchaseOrder;
        #region Set currency rate
        var cargoItemFinancialTransaction = cargoItem.FinancialTransactionBatch?.FinancialTransactions
            .FirstOrDefault(i => !i.IsDelete && i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.ImportToCargo.Id);
        if (cargoItemFinancialTransaction == null)
        {
          if (cargoItem.IsDelete)
            throw new CargoItemNotFoundException(id: cargoItem.Id);
          else
            throw new CargoItemHasNoFinancialTransactionException(cargoItem.Code);
        }
        double currencyRate = 1;
        int sourceCurrencyId = cargoItemFinancialTransaction.FinancialAccount.CurrencyId;
        string sourceCurrencyTitle = cargoItemFinancialTransaction.FinancialAccount.Currency.Title;
        if (sourceCurrencyId != (int)Currency.Rial)
        {
          currencyRate = GetRialRateOfFinancialTransaction(
                       financialTransaction: cargoItemFinancialTransaction,
                       updateRialRateIsUsedState: updateRialRateIsUsedState);
        }
        #endregion
        var storeReceiptUnit = newShopping.Unit;
        var cargoItemUnit = cargoItem.Unit;
        var purchaseOrderItemUnit = purchaseOrderItem.Unit;
        if (storeReceiptUnit.UnitTypeId != cargoItemUnit.UnitTypeId)
        {
          throw new CargoItemAndStoreReceiptUnitTypeDoNotMatchException(
                    cargoItemCode: cargoItem.Code,
                    cargoItemUnitName: cargoItemUnit.Name,
                    cargoItemUnitTypeName: cargoItemUnit.UnitType.Name,
                    storeReceiptCode: newShopping.Code,
                    storeReceiptUnitName: storeReceiptUnit.Name,
                    storeReceiptUnitTypeName: storeReceiptUnit.UnitType.Name);
        }
        if (storeReceiptUnit.UnitTypeId != purchaseOrderItemUnit.UnitTypeId)
        {
          throw new PurchaseOrderItemAndStoreReceiptUnitTypeDoNotMatchException(
                    purchaseOrderItemCode: purchaseOrderItem.Code,
                    purchaseOrderItemUnitName: purchaseOrderItemUnit.Name,
                    purchaseOrderItemUnitTypeName: purchaseOrderItemUnit.UnitType.Name,
                    storeReceiptCode: newShopping.Code,
                    storeReceiptUnitName: storeReceiptUnit.Name,
                    storeReceiptUnitTypeName: storeReceiptUnit.UnitType.Name);
        }
        double ladingItemUnitConversionFactor =
                  GetConversionFactor(sourceUnit: cargoItemUnit, destUnit: storeReceiptUnit);
        double cargoItemUnitConversionFactor =
                  GetConversionFactor(sourceUnit: cargoItemUnit, destUnit: storeReceiptUnit);
        double purchaseOrderItemUnitConversionFactor =
                  GetConversionFactor(sourceUnit: purchaseOrderItemUnit, destUnit: storeReceiptUnit);
        double unitTransferCostInRial = GetUnitTransferCostInRial(
                      cargoItem: cargoItem,
                      ladingItem: ladingItem,
                      ladingItemUnitConversionFactor: ladingItemUnitConversionFactor,
                      cargoItemUnitConversionFactor: cargoItemUnitConversionFactor,
                      updateRialRateIsValidState: updateRialRateIsUsedState);
        double unitDutyCostInRial = GetUnitDutyCostInRial(
                      ladingItem: ladingItem,
                      ladingItemUnitConversionFactor: ladingItemUnitConversionFactor,
                      updateRialRateIsValidState: updateRialRateIsUsedState);
        double unitPurchaseOrderCostInRial = GetUnitPurchaseOrderCostInRial(
                      purchaseOrderItem: purchaseOrderItem,
                      purchaseOrderUnitConversionFactor: purchaseOrderItemUnitConversionFactor,
                      updateRialRateIsValidState: updateRialRateIsUsedState);
        double unitPurchaseOrderDiscountInRial = GetUnitPurchaseOrderDiscountInRial(
                      purchaseOrderItem: purchaseOrderItem,
                      purchaseOrderUnitConversionFactor: purchaseOrderItemUnitConversionFactor,
                      updateRialRateIsValidState: updateRialRateIsUsedState);
        double unitPriceInSourceCurrency =
                  (cargoItemFinancialTransaction.Amount / Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount)) * cargoItemUnitConversionFactor;
        var calculatedRialInvoiceResult = new CalculatedRialInvoiceResult
        {
          Row = rowCounter,
          ReceiptId = receiptId,
          CooperatorName = receipt.Cooperator.Name,
          StoreReceiptId = newShopping.Id,
          CargoItemId = cargoItem.Id,
          CargoItemCode = cargoItem.Code,
          StuffId = newShopping.StuffId,
          StuffCode = newShopping.Stuff.Code,
          StuffName = newShopping.Stuff.Name,
          UnitId = storeReceiptUnit.Id,
          UnitName = storeReceiptUnit.Name,
          Amount = newShopping.Amount,
          UnitPriceInSourceCurrency = unitPriceInSourceCurrency,
          CurrencyRate = currencyRate,
          SourceCurrencyId = sourceCurrencyId,
          SourceCurrencyTitle = sourceCurrencyTitle,
          UnitTransferCost = unitTransferCostInRial,
          UnitDutyCost = unitDutyCostInRial,
          UnitDiscount = unitPurchaseOrderDiscountInRial,
          UnitOtherCost = unitPurchaseOrderCostInRial
        };
        calculatedRialInvoiceResultList.Add(calculatedRialInvoiceResult);
        rowCounter++;
      }
      var queryable = calculatedRialInvoiceResultList.AsQueryable();
      return queryable.Select(selector);
    }
    internal IEnumerable<int> GetNewShoppingsCostsAndDiscountsFinancialDocumentIds(IEnumerable<NewShopping> newShoppings)
    {
      var ladingCosts = newShoppings.SelectMany(i => i.LadingItem.LadingCosts);
      var ladingCostsFinancialDocumentIds = ladingCosts.Select(i => i.FinancialDocumentCost.FinancialDocument.Id);
      var cargoCosts = newShoppings.SelectMany(i => i.LadingItem.CargoItem.CargoCosts);
      var cargoCostsFinancialDocumentIds = cargoCosts.Select(i => i.FinancialDocumentCost.FinancialDocument.Id);
      var purchaseOrderCosts = newShoppings.SelectMany(i => i.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderCosts);
      var purchaseOrderCostsFinancialDocumentIds = purchaseOrderCosts.Select(i => i.FinancialDocumentCost.FinancialDocument.Id);
      var purchaseOrderDiscounts = newShoppings.SelectMany(i => i.LadingItem.CargoItem.PurchaseOrder.PurchaseOrderDiscounts);
      var purchaseOrderDiscountsFinancialDocumentIds = purchaseOrderDiscounts.Select(i => i.FinancialDocumentDiscount.FinancialDocument.Id);
      var newShoppingCostsAndDiscountsFinancialDocumentIds = ladingCostsFinancialDocumentIds
          .Union(cargoCostsFinancialDocumentIds)
          .Union(purchaseOrderCostsFinancialDocumentIds)
          .Union(purchaseOrderDiscountsFinancialDocumentIds)
          .Distinct();
      return newShoppingCostsAndDiscountsFinancialDocumentIds;
    }
    public IQueryable<GetRialInvoicesResult> GetRialInvoices(
        TValue<int> id = null,
        TValue<int> sourceCurrencyId = null,
        TValue<int> stuffId = null,
        TValue<int> userId = null)
    {

      var purchasePrices = GetPurchasePrices(
                    selector: e => e,
                    id: id,
                    currencyId: (int)Currency.Rial,
                    stuffId: stuffId,
                    userId: userId,
                    isDelete: false);
      var newShoppings = App.Internals.WarehouseManagement.GetStoreReceipts(
                    selector: e => e,
                    isDelete: false)


                .OfType<NewShopping>();
      var resultQuery = ToRialInvoiceResult(purchasePrices: purchasePrices, newShoppings: newShoppings);
      if (sourceCurrencyId != null)
        resultQuery = resultQuery.Where(i => i.SourceCurrencyId == sourceCurrencyId);
      return resultQuery;
    }
    #endregion
    #region ToResult
    public IQueryable<GetRialInvoicesResult> ToRialInvoiceResult(
        IQueryable<PurchasePrice> purchasePrices,
        IQueryable<NewShopping> newShoppings)
    {
      var resultQuery = from purchasePrice in purchasePrices
                        join newShopping in newShoppings on purchasePrice.StoreReceipt.Id equals newShopping.Id
                        let amount = Math.Round(newShopping.LadingItem.Qty, newShopping.LadingItem.CargoItem.Unit.DecimalDigitCount)
                        let unitPriceInRial = purchasePrice.RialPrice
                        let unitTransferCost = purchasePrice.TransferCost
                        let unitDutyCost = purchasePrice.DutyCost
                        let unitOtherCost = purchasePrice.OtherCost
                        let unitDiscount = purchasePrice.Discount
                        let unitNetPrice = purchasePrice.Price
                        select new GetRialInvoicesResult
                        {
                          StuffPriceId = purchasePrice.Id,
                          ReceiptId = newShopping.Receipt.Id,
                          ReceiptCode = newShopping.Receipt.Code,
                          LadingId = newShopping.LadingItem.LadingId,
                          LadingCode = newShopping.LadingItem.Lading.Code,
                          LadingItemId = newShopping.LadingItemId,
                          LadingItemCode = newShopping.LadingItem.Code,
                          CargoItemId = newShopping.LadingItem.CargoItemId,
                          CargoItemCode = newShopping.LadingItem.CargoItem.Code,
                          PurchaseOrderId = newShopping.LadingItem.CargoItem.PurchaseOrderId,
                          PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
                          SourceCurrencyId = newShopping.LadingItem.CargoItem.PurchaseOrder.CurrencyId,
                          SourceCurrencyTitle = newShopping.LadingItem.CargoItem.PurchaseOrder.Currency.Title,
                          UnitPriceInSourceCurrency = newShopping.LadingItem.CargoItem.PurchaseOrder.Price,
                          ReceiptDateTime = newShopping.Receipt.ReceiptDateTime,
                          Amount = amount,
                          UnitId = newShopping.UnitId,
                          UnitName = newShopping.Unit.Name,
                          DateTime = purchasePrice.DateTime,
                          EmployeeFullName = purchasePrice.User.Employee.FirstName + " " + purchasePrice.User.Employee.LastName,
                          EmployeeId = purchasePrice.User.Employee.Id,
                          StuffId = purchasePrice.StuffId,
                          StuffCode = purchasePrice.Stuff.Code,
                          StuffName = purchasePrice.Stuff.Name,
                          CurrencyRate = purchasePrice.CurrencyRate,
                          UnitPriceInRial = unitPriceInRial,
                          TotalGrossPrice = unitPriceInRial * amount,
                          UnitTransferCost = unitTransferCost,
                          TotalTransferCost = unitTransferCost * amount,
                          UnitDutyCost = unitDutyCost,
                          TotalDutyCost = unitDutyCost * amount,
                          UnitOtherCost = unitOtherCost,
                          TotalOtherCost = unitOtherCost * amount,
                          UnitDiscount = unitDiscount,
                          TotalDiscount = unitDiscount * amount,
                          UnitNetPrice = unitNetPrice,
                          TotalNetPrice = unitNetPrice * amount,
                          RowVersion = purchasePrice.RowVersion
                        };
      return resultQuery;
    }
    #endregion
    #region Add
    public void AddRialInvoiceProcess(
        int receiptId,
        string description)
    {

      var warehouseManagement = App.Internals.WarehouseManagement;
      var calculatedRialInvoices = GetCalculatedRialInvoiceProcess(
                    selector: e => e,
                    receiptId: receiptId,
                    updateRialRateIsUsedState: true);
      foreach (var calculatedRialInvoice in calculatedRialInvoices)
      {
        AddPurchasePriceProcess(
                      currencyId: (int)Currency.Rial,
                      currencyRate: calculatedRialInvoice.CurrencyRate,
                      stuffId: calculatedRialInvoice.StuffId,
                      storeReceiptId: calculatedRialInvoice.StoreReceiptId,
                      rialPrice: calculatedRialInvoice.UnitPriceInRial,
                      price: calculatedRialInvoice.UnitNetPrice,
                      transferCost: calculatedRialInvoice.UnitTransferCost,
                      otherCost: calculatedRialInvoice.UnitOtherCost,
                      dutyCost: calculatedRialInvoice.UnitDutyCost,
                      discount: calculatedRialInvoice.UnitDiscount);
      }
      var receipt = warehouseManagement.GetReceipt(
                    selector: e => e,
                    id: receiptId);
      warehouseManagement.EditReceipt(
                    receipt: receipt,
                    status: ReceiptStatus.Priced,
                    rowVersion: receipt.RowVersion);
    }
    #endregion
    #region Calculate
    internal double GetUnitTransferCostInRial(
       CargoItem cargoItem,
       LadingItem ladingItem,
       double cargoItemUnitConversionFactor,
       double ladingItemUnitConversionFactor,
       bool updateRialRateIsValidState)
    {

      double unitCargoItemTransferCost = GetUnitCargoItemTransferCost(
                cargoItem: cargoItem,
                cargoItemUnitConversionFactor: cargoItemUnitConversionFactor,
                updateRialRateIsValidState: updateRialRateIsValidState);
      double unitLadingItemTransferCost = GetUnitLadingItemTransferCost(
                ladingItem: ladingItem,
                ladingItemUnitConversionFactor: ladingItemUnitConversionFactor,
                updateRialRateIsValidState: updateRialRateIsValidState);
      return unitCargoItemTransferCost + unitLadingItemTransferCost;
    }
    internal double GetUnitLadingItemTransferCost(
        LadingItem ladingItem,
        double ladingItemUnitConversionFactor,
        bool updateRialRateIsValidState)
    {

      if (ladingItem.Qty == 0)
        throw new LadingItemQtyIsZeroException(ladingItemId: ladingItem.Id, ladingItemCode: ladingItem.Code);
      var ladingCosts = ladingItem.LadingCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == false);
      var ladingTransferCosts = ladingCosts.Where(i =>
                i.FinancialDocumentCost.CostType == CostType.TransferLading ||
                i.FinancialDocumentCost.CostType == CostType.TransferLadingItems);
      var ladingTransferCostsInRialSum = ladingTransferCosts.Where(i =>
                    i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId ==
                    (int)Currency.Rial)
                .Sum(i => i.Amount);
      var ladingTransferCostsInSourceCurrency = ladingTransferCosts.Where(i =>
                i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId !=
                (int)Currency.Rial);
      double ladingTransferCostsConvertedToRialSum = 0;
      foreach (var ladingTransferCost in ladingTransferCostsInSourceCurrency)
      {
        var financialTransaction = ladingTransferCost.FinancialDocumentCost.FinancialDocument
                  .FinancialTransactionBatch
                  .FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
        if (financialTransaction == null)
          throw new LadingCostHasNoFinancialTransactionException(ladingTransferCost.Id);
        var currencyRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: financialTransaction,
                      updateRialRateIsUsedState: updateRialRateIsValidState);
        ladingTransferCostsConvertedToRialSum += ladingTransferCost.Amount * currencyRate;
      }
      double unitLadingItemTransferCost =
                ((ladingTransferCostsConvertedToRialSum + ladingTransferCostsInRialSum) / Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount)) *
                ladingItemUnitConversionFactor;
      return unitLadingItemTransferCost;
    }
    internal double GetUnitCargoItemTransferCost(
        CargoItem cargoItem,
        double cargoItemUnitConversionFactor,
        bool updateRialRateIsValidState)
    {

      var cargoCosts = cargoItem.CargoCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == false);
      var cargoTransferCosts = cargoCosts.Where(i =>
                i.FinancialDocumentCost.CostType == CostType.TransferCargo ||
                i.FinancialDocumentCost.CostType == CostType.TransferCargoItems);
      var cargoTransferCostsInRialSum = cargoTransferCosts.Where(i =>
                    i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId ==
                    (int)Currency.Rial)
                .Sum(i => i.Amount);
      var cargoTransferCostsInSourceCurrency = cargoTransferCosts.Where(i =>
                i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId !=
                (int)Currency.Rial);
      double cargoTransferCostsConvertedToRialSum = 0;
      foreach (var cargoTransferCost in cargoTransferCostsInSourceCurrency)
      {
        var financialTransaction = cargoTransferCost.FinancialDocumentCost.FinancialDocument
                  .FinancialTransactionBatch.FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
        if (financialTransaction == null)
          throw new CargoCostHasNoFinancialTransactionException(cargoTransferCost.Id);
        var currencyRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: financialTransaction,
                      updateRialRateIsUsedState: updateRialRateIsValidState);
        cargoTransferCostsConvertedToRialSum += (cargoTransferCost.Amount * currencyRate);
      }
      double unitCargoItemTransferCost =
                ((cargoTransferCostsConvertedToRialSum + cargoTransferCostsInRialSum) / Math.Round(cargoItem.Qty, cargoItem.Unit.DecimalDigitCount)) *
                cargoItemUnitConversionFactor;
      return unitCargoItemTransferCost;
    }
    internal double GetUnitDutyCostInRial(
        LadingItem ladingItem,
        double ladingItemUnitConversionFactor,
        bool updateRialRateIsValidState)
    {

      var ladingCosts = ladingItem.LadingCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == false);
      var ladingDutyCosts = ladingCosts.Where(i =>
                i.FinancialDocumentCost.CostType == CostType.DutyLading ||
                i.FinancialDocumentCost.CostType == CostType.DutyLadingItems);
      var ladingDutyCostsInRialSum = ladingDutyCosts.Where(i =>
                    i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId ==
                    (int)Currency.Rial)
                .Sum(i => i.Amount);
      var ladingDutyCostsInSourceCurrency = ladingDutyCosts.Where(i =>
                i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId !=
                (int)Currency.Rial);
      double ladingDutyCostsConvertedToRialSum = 0;
      foreach (var ladingDutyCost in ladingDutyCostsInSourceCurrency)
      {
        var financialTransaction = ladingDutyCost.FinancialDocumentCost.FinancialDocument
                  .FinancialTransactionBatch
                  .FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
        if (financialTransaction == null)
          throw new LadingCostHasNoFinancialTransactionException(ladingDutyCost.Id);
        var currencyRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: financialTransaction,
                      updateRialRateIsUsedState: updateRialRateIsValidState);
        ladingDutyCostsConvertedToRialSum += ladingDutyCost.Amount * currencyRate;
      }
      double unitLadingItemDutyCost =
                ((ladingDutyCostsConvertedToRialSum + ladingDutyCostsInRialSum) / Math.Round(ladingItem.Qty, ladingItem.CargoItem.Unit.DecimalDigitCount)) *
                ladingItemUnitConversionFactor;
      return unitLadingItemDutyCost;
    }
    internal double GetUnitPurchaseOrderCostInRial(
        PurchaseOrder purchaseOrderItem,
        double purchaseOrderUnitConversionFactor,
        bool updateRialRateIsValidState)
    {

      var purchaseOrderCosts = purchaseOrderItem.PurchaseOrderCosts.Where(i =>
                i.FinancialDocumentCost.FinancialDocument.IsDelete == false);
      purchaseOrderCosts = purchaseOrderCosts.Where(i =>
                i.FinancialDocumentCost.CostType == CostType.PurchaseOrderGroup ||
                i.FinancialDocumentCost.CostType == CostType.PurchaseOrderItem);
      var purchaseOrderCostsInRialSum = purchaseOrderCosts.Where(i =>
                    i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId ==
                    (int)Currency.Rial)
                .Sum(i => i.Amount);
      var purchaseOrderCostsInSourceCurrency = purchaseOrderCosts.Where(i =>
                i.FinancialDocumentCost.FinancialDocument.FinancialAccount.CurrencyId !=
                (int)Currency.Rial);
      double purchaseOrderCostsConvertedToRialSum = 0;
      foreach (var purchaseOrderCost in purchaseOrderCostsInSourceCurrency)
      {
        var financialTransaction = purchaseOrderCost.FinancialDocumentCost.FinancialDocument
                  .FinancialTransactionBatch
                  .FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
        if (financialTransaction == null)
          throw new PurchaseOrderCostHasNoFinancialTransactionException(purchaseOrderCost.Id);
        var currencyRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: financialTransaction,
                      updateRialRateIsUsedState: updateRialRateIsValidState);
        purchaseOrderCostsConvertedToRialSum += purchaseOrderCost.Amount * currencyRate;
      }
      double unitPurchaseOrderItemDutyCost =
                ((purchaseOrderCostsConvertedToRialSum + purchaseOrderCostsInRialSum) / purchaseOrderItem.Qty) *
                purchaseOrderUnitConversionFactor;
      return unitPurchaseOrderItemDutyCost;
    }
    internal double GetUnitPurchaseOrderDiscountInRial(
        PurchaseOrder purchaseOrderItem,
        double purchaseOrderUnitConversionFactor,
        bool updateRialRateIsValidState)
    {

      var purchaseOrderDiscounts = purchaseOrderItem.PurchaseOrderDiscounts.Where(i =>
                i.FinancialDocumentDiscount.FinancialDocument.IsDelete == false);
      purchaseOrderDiscounts = purchaseOrderDiscounts.Where(i =>
                i.FinancialDocumentDiscount.DiscountType == DiscountType.PurchaseOrderGroup ||
                i.FinancialDocumentDiscount.DiscountType == DiscountType.PurchaseOrderItem);
      var purchaseOrderDiscountsInRialSum = purchaseOrderDiscounts.Where(i =>
                    i.FinancialDocumentDiscount.FinancialDocument.FinancialAccount.CurrencyId ==
                    (int)Currency.Rial)
                .Sum(i => i.Amount);
      var purchaseOrderDiscountsInSourceCurrency = purchaseOrderDiscounts.Where(i =>
                i.FinancialDocumentDiscount.FinancialDocument.FinancialAccount.CurrencyId !=
                (int)Currency.Rial);
      double purchaseOrderDiscountsConvertedToRialSum = 0;
      foreach (var purchaseOrderDiscount in purchaseOrderDiscountsInSourceCurrency)
      {
        var financialTransaction = purchaseOrderDiscount.FinancialDocumentDiscount.FinancialDocument
                  .FinancialTransactionBatch
                  .FinancialTransactions.FirstOrDefault(i =>
                      !i.IsDelete &&
                      i.FinancialTransactionTypeId == StaticFinancialTransactionTypes.Expense.Id);
        if (financialTransaction == null)
          throw new PurchaseOrderDiscountHasNoFinancialTransactionException(purchaseOrderDiscount.Id);
        var currencyRate = GetRialRateOfFinancialTransaction(
                      financialTransaction: financialTransaction,
                      updateRialRateIsUsedState: updateRialRateIsValidState);
        purchaseOrderDiscountsConvertedToRialSum += purchaseOrderDiscount.Amount * currencyRate;
      }
      double unitPurchaseOrderItemDiscount =
                ((purchaseOrderDiscountsConvertedToRialSum + purchaseOrderDiscountsInRialSum) / purchaseOrderItem.Qty) *
                purchaseOrderUnitConversionFactor;
      return unitPurchaseOrderItemDiscount;
    }
    private double GetConversionFactor(Unit sourceUnit, Unit destUnit)
    {
      double conversionFactor = 1;
      if (sourceUnit.Id != destUnit.Id)
      {
        if (sourceUnit.UnitTypeId != destUnit.UnitTypeId)
          throw new UnitTypeNotMatchException(unitName1: sourceUnit.Name, unitName2: destUnit.Name);
        if (destUnit.IsMainUnit)
        {
          conversionFactor = sourceUnit.ConversionRatio;
        }
        else
        {
          conversionFactor = 1 / destUnit.ConversionRatio;
        }
      }
      return conversionFactor;
    }
    #endregion
    #region Search
    public IQueryable<GetRialInvoicesResult> SearchRialInvoiceResults(
        IQueryable<GetRialInvoicesResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from item in query
                where
                    item.CargoItemCode.Contains(searchText) ||
                    item.LadingCode.Contains(searchText) ||
                    item.StuffCode.Contains(searchText) ||
                    item.StuffName.Contains(searchText) ||
                    item.EmployeeFullName.Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<GetRialInvoicesResult> SortRialInvoiceResults(
       IQueryable<GetRialInvoicesResult> query,
       SortInput<RialInvoiceSortType> sort)
    {
      switch (sort.SortType)
      {
        case RialInvoiceSortType.StuffPriceId:
          return query.OrderBy(a => a.StuffPriceId, sort.SortOrder);
        case RialInvoiceSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, sort.SortOrder);
        case RialInvoiceSortType.CargoItemCode:
          return query.OrderBy(a => a.CargoItemCode, sort.SortOrder);
        case RialInvoiceSortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case RialInvoiceSortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case RialInvoiceSortType.UnitName:
          return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case RialInvoiceSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case RialInvoiceSortType.SourceCurrencyTitle:
          return query.OrderBy(a => a.SourceCurrencyTitle, sort.SortOrder);
        case RialInvoiceSortType.UnitPriceInRial:
          return query.OrderBy(a => a.UnitPriceInRial, sort.SortOrder);
        case RialInvoiceSortType.TotalGrossPrice:
          return query.OrderBy(a => a.TotalGrossPrice, sort.SortOrder);
        case RialInvoiceSortType.UnitTransferCost:
          return query.OrderBy(a => a.UnitTransferCost, sort.SortOrder);
        case RialInvoiceSortType.TotalTransferCost:
          return query.OrderBy(a => a.TotalTransferCost, sort.SortOrder);
        case RialInvoiceSortType.UnitDutyCost:
          return query.OrderBy(a => a.UnitDutyCost, sort.SortOrder);
        case RialInvoiceSortType.TotalDutyCost:
          return query.OrderBy(a => a.TotalDutyCost, sort.SortOrder);
        case RialInvoiceSortType.UnitOtherCost:
          return query.OrderBy(a => a.UnitOtherCost, sort.SortOrder);
        case RialInvoiceSortType.TotalOtherCost:
          return query.OrderBy(a => a.TotalOtherCost, sort.SortOrder);
        case RialInvoiceSortType.UnitDiscount:
          return query.OrderBy(a => a.UnitDiscount, sort.SortOrder);
        case RialInvoiceSortType.TotalDiscount:
          return query.OrderBy(a => a.TotalDiscount, sort.SortOrder);
        case RialInvoiceSortType.UnitNetPrice:
          return query.OrderBy(a => a.UnitNetPrice, sort.SortOrder);
        case RialInvoiceSortType.TotalNetPrice:
          return query.OrderBy(a => a.TotalNetPrice, sort.SortOrder);
        case RialInvoiceSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case RialInvoiceSortType.ReceiptDateTime:
          return query.OrderBy(a => a.ReceiptDateTime, sort.SortOrder);
        case RialInvoiceSortType.EmployeeFullName:
          return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case RialInvoiceSortType.CurrencyRate:
          return query.OrderBy(a => a.CurrencyRate, sort.SortOrder);
        case RialInvoiceSortType.UnitPriceInSourceCurrency:
          return query.OrderBy(a => a.UnitPriceInSourceCurrency, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}