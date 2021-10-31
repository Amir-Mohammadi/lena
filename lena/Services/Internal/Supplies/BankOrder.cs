using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.BankOrder;
using lena.Models.Supplies.BankOrderDetail;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public BankOrder AddBankOrder(
        BankOrder bankOrder,
        TransactionBatch transactionBatch,
        string orderNumber,
        string folderCode,
        int stuffPriority,
        DateTime registerDate,
        DateTime expireDate,
        int providerId,
        byte currencyId,
        double transferCost,
        double fob,
        BankOrderStatus status,
        BankOrderType bankOrderType,
        byte bankId,
        short customhouseId,
        byte countryId,
        string description,
        short bankOrderContractTypeId)
    {

      bankOrder = bankOrder ?? repository.Create<BankOrder>();
      bankOrder.OrderNumber = orderNumber;
      bankOrder.FolderCode = folderCode;
      bankOrder.StuffPriority = stuffPriority;
      bankOrder.RegisterDate = registerDate;
      bankOrder.ExpireDate = expireDate;
      bankOrder.ProviderId = providerId;
      bankOrder.CurrencyId = currencyId;
      bankOrder.TransferCost = transferCost;
      bankOrder.FOB = fob;
      bankOrder.TotalAmount = fob + transferCost;
      bankOrder.Status = status;
      bankOrder.BankOrderType = bankOrderType;
      bankOrder.BankId = bankId;
      bankOrder.CustomhouseId = customhouseId;
      bankOrder.CountryId = countryId;
      bankOrder.BankOrderContractTypeId = bankOrderContractTypeId;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: bankOrder,
                    transactionBatch: transactionBatch,
                    description: description);
      return bankOrder;
    }
    #endregion

    #region Edit
    public BankOrder EditBankOrder(
        int id,
        byte[] rowVersion,
        TValue<string> orderNumber = null,
        TValue<string> folderCode = null,
        TValue<int> stuffPriority = null,
        TValue<DateTime> registerDate = null,
        TValue<DateTime> expireDate = null,
        TValue<int> providerId = null,
        TValue<byte> currencyId = null,
        TValue<BankOrderStatus> status = null,
        TValue<BankOrderType> bankOrderType = null,
        TValue<byte> bankId = null,
        TValue<short> customhouseId = null,
        TValue<byte> countryId = null,
        TValue<double> transferCost = null,
        TValue<double> fob = null,
        TValue<string> description = null,
        TValue<DateTime> settlementDateTime = null,
        TValue<short> bankOrderContractTypeId = null,
        TValue<bool> WithoutCurrencyTransfer = null)
    {

      var bankOrder = GetBankOrder(id: id);
      return EditBankOrder(
                    bankOrder: bankOrder,
                    rowVersion: rowVersion,
                    orderNumber: orderNumber,
                    folderCode: folderCode,
                    stuffPriority: stuffPriority,
                    registerDate: registerDate,
                    expireDate: expireDate,
                    providerId: providerId,
                    currencyId: currencyId,
                    status: status,
                    bankOrderType: bankOrderType,
                    bankId: bankId,
                    transferCost: transferCost,
                    fob: fob,
                    customhouseId: customhouseId,
                    countryId: countryId,
                    description: description,
                    settlementDateTime: settlementDateTime,
                    bankOrderContractTypeId: bankOrderContractTypeId,
                    WithoutCurrencyTransfer: WithoutCurrencyTransfer);
    }
    public BankOrder EditBankOrder(
        BankOrder bankOrder,
        byte[] rowVersion,
        TValue<string> orderNumber = null,
        TValue<string> folderCode = null,
        TValue<int> stuffPriority = null,
        TValue<DateTime> registerDate = null,
        TValue<DateTime> expireDate = null,
        TValue<int> providerId = null,
        TValue<byte> currencyId = null,
        TValue<BankOrderStatus> status = null,
        TValue<BankOrderType> bankOrderType = null,
        TValue<byte> bankId = null,
        TValue<short> customhouseId = null,
        TValue<byte> countryId = null,
        TValue<double> transferCost = null,
        TValue<double> fob = null,
        TValue<string> description = null,
        TValue<DateTime> settlementDateTime = null,
        TValue<short> bankOrderContractTypeId = null,
        TValue<bool> WithoutCurrencyTransfer = null)
    {

      if (orderNumber != null)
        bankOrder.OrderNumber = orderNumber;
      if (folderCode != null)
        bankOrder.FolderCode = folderCode;
      if (stuffPriority != null)
        bankOrder.StuffPriority = stuffPriority;
      if (registerDate != null)
        bankOrder.RegisterDate = registerDate;
      if (expireDate != null)
        bankOrder.ExpireDate = expireDate;
      if (providerId != null)
        bankOrder.ProviderId = providerId;
      if (currencyId != null)
        bankOrder.CurrencyId = currencyId;
      if (status != null)
        bankOrder.Status = status;
      if (bankOrderType != null)
        bankOrder.BankOrderType = bankOrderType;
      if (bankId != null)
        bankOrder.BankId = bankId;
      if (customhouseId != null)
        bankOrder.CustomhouseId = customhouseId;
      if (countryId != null)
        bankOrder.CountryId = countryId;
      if (bankOrderContractTypeId != null)
        bankOrder.BankOrderContractTypeId = bankOrderContractTypeId;
      if (transferCost != null)
      {
        bankOrder.TransferCost = transferCost;
        bankOrder.TotalAmount = bankOrder.TransferCost + bankOrder.FOB;
      }
      if (fob != null)
      {
        bankOrder.FOB = fob;
        bankOrder.TotalAmount = bankOrder.TransferCost + bankOrder.FOB;
      }
      if (settlementDateTime != null)
        bankOrder.SettlementDateTime = settlementDateTime;
      if (WithoutCurrencyTransfer != null)
        bankOrder.WithoutCurrencyTransfer = WithoutCurrencyTransfer;

      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: bankOrder,
                    rowVersion: rowVersion,
                    description: description);
      return retValue as BankOrder;
    }
    #endregion

    #region Get
    public BankOrder GetBankOrder(int id) => GetBankOrder(selector: e => e, id: id);
    public TResult GetBankOrder<TResult>(
        Expression<Func<BankOrder, TResult>> selector,
        int id)
    {

      var result = GetBankOrders(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new BankOrderNotFoundException(id);
      return result;
    }
    public BankOrder GetBankOrder(string code) => GetBankOrder(selector: e => e, code: code);
    public TResult GetBankOrder<TResult>(
        Expression<Func<BankOrder, TResult>> selector,
        string code)
    {

      var result = GetBankOrders(
                selector: selector,
                code: code).FirstOrDefault();
      if (result == null)
        throw new BankOrderNotFoundException(code);
      return result;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetBankOrders<TResult>(
        Expression<Func<BankOrder, TResult>> selector,
        TransactionBatch transactionBatch = null,
        TValue<int[]> employeeIds = null,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> orderNumber = null,
        TValue<string> folderCode = null,
        TValue<int> stuffPriority = null,
        TValue<DateTime> fromRegisterDate = null,
        TValue<DateTime> toRegisterDate = null,
        TValue<DateTime> fromExpireDate = null,
        TValue<DateTime> toExpireDate = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<int> providerId = null,
        TValue<int> currencyId = null,
        TValue<BankOrderStatus> status = null,
        TValue<BankOrderStatus[]> statuses = null,
        TValue<BankOrderStatus[]> notHasStatuses = null,
        TValue<int> bankId = null,
        TValue<int> customhouseId = null,
        TValue<int> countryId = null,
        TValue<int> bankOrderContractTypeId = null,
        TValue<string> description = null,
        TValue<int[]> BankOrderStatusTypes = null
        )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                    selector: e => e,
                    id: id,
                    code: code,
                    isDelete: false,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var bankOrder = baseQuery.OfType<BankOrder>();
      if (orderNumber != null)
        bankOrder = bankOrder.Where(r => r.OrderNumber == orderNumber);
      if (folderCode != null)
        bankOrder = bankOrder.Where(r => r.FolderCode == folderCode);
      if (stuffPriority != null)
        bankOrder = bankOrder.Where(r => r.StuffPriority == stuffPriority);
      if (fromRegisterDate != null)
        bankOrder = bankOrder.Where(i => i.RegisterDate >= fromRegisterDate);
      if (toRegisterDate != null)
        bankOrder = bankOrder.Where(i => i.RegisterDate <= toRegisterDate);
      if (fromExpireDate != null)
        bankOrder = bankOrder.Where(i => i.ExpireDate >= fromExpireDate);
      if (toExpireDate != null)
        bankOrder = bankOrder.Where(i => i.ExpireDate <= toExpireDate);
      if (fromDateTime != null)
        bankOrder = bankOrder.Where(i => i.DateTime >= fromDateTime);
      if (toDateTime != null)
        bankOrder = bankOrder.Where(i => i.DateTime <= toDateTime);
      if (providerId != null)
        bankOrder = bankOrder.Where(r => r.ProviderId == providerId);
      if (bankId != null)
        bankOrder = bankOrder.Where(r => r.BankId == bankId);
      if (customhouseId != null)
        bankOrder = bankOrder.Where(r => r.CustomhouseId == customhouseId);
      if (countryId != null)
        bankOrder = bankOrder.Where(r => r.CountryId == countryId);
      if (providerId != null)
        bankOrder = bankOrder.Where(r => r.ProviderId == providerId);
      if (status != null)
        bankOrder = bankOrder.Where(i => i.Status.HasFlag(status));
      if (bankOrderContractTypeId != null)
        bankOrder = bankOrder.Where(i => i.BankOrderContractTypeId == bankOrderContractTypeId);
      if (employeeIds != null)
        bankOrder = bankOrder.Where(i => employeeIds.Value.Contains(i.User.Employee.Id));
      if (statuses != null)
      {
        var s = BankOrderStatus.None;
        foreach (var item in statuses.Value)
          s = s | item;
        bankOrder = bankOrder.Where(i => (i.Status & s) > 0);
      }
      if (notHasStatuses != null)
      {
        var s = BankOrderStatus.None;
        foreach (var item in notHasStatuses.Value)
          s = s | item;
        bankOrder = bankOrder.Where(i => (i.Status & s) == 0);
      }
      if (BankOrderStatusTypes != null)
        bankOrder = bankOrder.Where(i => BankOrderStatusTypes.Value.Contains(i.CurrentBankOrderLog.BankOrderStatusTypeId));

      return bankOrder.Select(selector);
    }
    #endregion

    #region Get latestAllocationFinalizationDateTime
    public IQueryable<TResult> GetLatestAllocationFinalizationDateTime<TResult>(
        Expression<Func<Allocation, TResult>> selector
        )
    {

      var allocations = GetAllocations(e => e);

      var groupByResult = from allocation in allocations
                          group allocation by allocation.Id into g
                          select new
                          {
                            Id = g.Max(i => i.Id),
                            FinalizationDateTime = g.Max(i => i.FinalizationDateTime)
                          };
      var query = from allocation in allocations
                  join g in groupByResult on
                        allocation.Id equals g.Id
                  select allocation;


      return query.Select(selector);
    }

    #endregion

    #region Remove BankOrder
    public void RemoveBankOrder(int id, byte[] rowVersion)
    {

      var bankOrder = GetBankOrder(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: bankOrder,
                    rowVersion: rowVersion);
    }
    #endregion

    #region Delete BankOrder
    public void DeleteBankOrderProcess(
        int id,
        byte[] rowVersion)
    {


      var lading = GetLadings(selector: e => e, bankOrderId: id);
      if (lading.Any())
      {
        throw new BankOrderHasLadingException(id);
      }

      #region RemoveBankOrder
      RemoveBankOrder(
              id: id,
              rowVersion: rowVersion);
      #endregion

      #region Remove BankOrderDetails
      var bankOrderDetails = GetBankOrderDetails(selector: e => e,
          bankOrderId: id);

      foreach (var bankOrderDetail in bankOrderDetails)
      {
        DeleteBankOrderDetailProcess(bankOrderDetail.Id);
      }
      #endregion
    }
    #endregion

    #region DeleteBanKorderDetail
    public void DeleteBankOrderDetailProcess(int id)
    {

      var bankOrderDetail = GetBankOrderDetail(id: id);
      repository.Delete(bankOrderDetail);
    }
    #endregion

    #region AddProcess
    public BankOrder AddBankOrderProcess(
            TransactionBatch transactionBatch,
            BankOrder bankOrder,
            string orderNumber,
            string folderCode,
            int stuffPriority,
            DateTime registerDate,
            DateTime expireDate,
            int providerId,
            byte currencyId,
            double transferCost,
            double fob,
            BankOrderStatus status,
            BankOrderType bankOrderType,
            byte bankId,
            short customhouseId,
            byte countryId,
            AddBankOrderDetailInput[] bankOrderDetails,
            short bankOrderContractTypeId,
            string description)
    {

      #region AddTransactionBatch
      var warehouseManagement = App.Internals.WarehouseManagement;
      transactionBatch = transactionBatch ?? warehouseManagement.AddTransactionBatch();
      #endregion
      #region AddBankOrder
      bankOrder = AddBankOrder(
              bankOrder: bankOrder,
              transactionBatch: transactionBatch,
              orderNumber: orderNumber,
              folderCode: folderCode,
              stuffPriority: stuffPriority,
              registerDate: registerDate,
              expireDate: expireDate,
              providerId: providerId,
              currencyId: currencyId,
              transferCost: transferCost,
              fob: fob,
              //totalAmount: totalAmount,
              status: status,
              bankOrderType: bankOrderType,
              bankId: bankId,
              customhouseId: customhouseId,
              countryId: countryId,
              description: description,
              bankOrderContractTypeId: bankOrderContractTypeId);
      #endregion
      #region Add BankOrderDetails
      foreach (var bankOrderDetail in bankOrderDetails)
      {
        AddBankOrderDetailProcess(
                  bankOrderDetail: null,
                  bankOrderId: bankOrder.Id,
                  index: bankOrderDetail.Index,
                  price: bankOrderDetail.Price,
                  weight: bankOrderDetail.Weight,
                  fee: bankOrderDetail.Fee,
                  qty: bankOrderDetail.Qty,
                  unitId: bankOrderDetail.UnitId,
                  grossWeight: bankOrderDetail.GrossWeight,
                  stuffHSGroupId: bankOrderDetail.StuffHSGroupId,
                  description: bankOrderDetail.Description);
      }
      #endregion

      return bankOrder;
    }
    #endregion

    #region EditProcess

    public BankOrder EditBankOrderProcess(
        int id,
        byte[] rowVersion,
        string orderNumber,
        string folderCode,
        int stuffPriority,
        DateTime registerDate,
        DateTime expireDate,
        int providerId,
        byte currencyId,
        double transferCost,
        double fob,
        BankOrderStatus status,
        BankOrderType bankOrderType,
        byte bankId,
        short customhouseId,
        byte countryId,
        short bankOrderContractTypeId,
        AddBankOrderDetailInput[] addBankOrderDetails,
        EditBankOrderDetailInput[] editBankOrderDetails,
        DeleteBankOrderDetailInput[] deleteBankOrderDetails,
        string description)
    {

      #region AddTransactionBatch
      var bankOrder = GetBankOrder(id: id);
      #endregion
      #region EditBankOrder
      bankOrder = EditBankOrder(
             bankOrder: bankOrder,
             rowVersion: rowVersion,
             orderNumber: orderNumber,
             folderCode: folderCode,
             stuffPriority: stuffPriority,
             registerDate: registerDate,
             expireDate: expireDate,
             providerId: providerId,
             currencyId: currencyId,
             transferCost: transferCost,
             fob: fob,
             status: status,
             bankOrderType: bankOrderType,
             bankId: bankId,
             customhouseId: customhouseId,
             countryId: countryId,
             description: description,
             bankOrderContractTypeId: bankOrderContractTypeId);
      #endregion
      #region Add BankOrderDetails
      foreach (var bankOrderDetail in addBankOrderDetails)
      {
        AddBankOrderDetailProcess(
                      bankOrderDetail: null,
                      bankOrderId: bankOrder.Id,
                      index: bankOrderDetail.Index,
                      price: bankOrderDetail.Price,
                      weight: bankOrderDetail.Weight,
                      fee: bankOrderDetail.Fee,
                      qty: bankOrderDetail.Qty,
                      unitId: bankOrderDetail.UnitId,
                      grossWeight: bankOrderDetail.GrossWeight,
                      stuffHSGroupId: bankOrderDetail.StuffHSGroupId,
                      description: bankOrderDetail.Description);
      }
      #endregion
      #region Edit BankOrderDetails
      foreach (var editBankOrderDetail in editBankOrderDetails)
      {
        EditBankOrderDetail(
                      id: editBankOrderDetail.Id,
                      rowVersion: editBankOrderDetail.RowVersion,
                      index: editBankOrderDetail.Index,
                      stuffHSGroupId: editBankOrderDetail.StuffHSGroupId,
                      price: editBankOrderDetail.Price,
                      weight: editBankOrderDetail.Weight,
                         fee: editBankOrderDetail.Fee,
                  qty: editBankOrderDetail.Qty,
                  unitId: editBankOrderDetail.UnitId,
                  grossWeight: editBankOrderDetail.GrossWeight,
                      description: editBankOrderDetail.Description);
      }
      #endregion
      #region Delete BankOrderDetails
      foreach (var deleteBankOrderDetail in deleteBankOrderDetails)
      {
        RemoveBankOrderDetailProcess(
                  id: deleteBankOrderDetail.Id,
                  rowVersion: deleteBankOrderDetail.RowVersion);
      }
      #endregion
      return bankOrder;
    }
    #endregion

    #region Search
    public IQueryable<BankOrderResult> SearchBankOrderResult(IQueryable<BankOrderResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems,
        int? currentBankOrderStatusTypeId = null
        //int[] bankOrderStatusType = null

        )
    {

      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Code.Contains(search) ||
            item.BankName.Contains(search) ||
            item.CountryName.Contains(search) ||
            item.CurrencyTitle.Contains(search) ||
            item.CustomhouseName.Contains(search) ||
            item.ProviderName.Contains(search) ||
            item.FolderCode.Contains(search) ||
            item.OrderNumber.Contains(search) ||
            item.RegisterEmployeeFullName.Contains(search) ||
            item.CurrentBankOrderStatusTypeName.Contains(search) ||
            item.CurrentEmployeeFullName.Contains(search) ||
            item.CurrentDescription.Contains(search));

      //if (currentBankOrderStatusTypeId != null)
      //    query = query.Where(i => i.CurrentBankOrderStatusTypeId == currentBankOrderStatusTypeId);


      //if (bankOrderStatusType != null)
      //{
      //    foreach (var item in bankOrderStatusType)
      //    {
      //        query = query.Where(i => i.BankOrderStatusType.Contains(item));
      //    }

      //}

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<BankOrderResult> SortBankOrderResult(IQueryable<BankOrderResult> query,
        SortInput<BankOrderSortType> sort)
    {
      switch (sort.SortType)
      {
        case BankOrderSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case BankOrderSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case BankOrderSortType.OrderNumber:
          return query.OrderBy(a => a.OrderNumber, sort.SortOrder);
        case BankOrderSortType.FolderCode:
          return query.OrderBy(a => a.FolderCode, sort.SortOrder);
        case BankOrderSortType.RegisterDate:
          return query.OrderBy(a => a.RegisterDate, sort.SortOrder);
        case BankOrderSortType.ExpireDate:
          return query.OrderBy(a => a.ExpireDate, sort.SortOrder);
        case BankOrderSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case BankOrderSortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case BankOrderSortType.Status:
          return query.OrderBy(a => a.Status, sort.SortOrder);
        case BankOrderSortType.RegisterEmployeeFullName:
          return query.OrderBy(a => a.RegisterEmployeeFullName, sort.SortOrder);
        case BankOrderSortType.BankName:
          return query.OrderBy(a => a.BankName, sort.SortOrder);
        case BankOrderSortType.CustomhouseName:
          return query.OrderBy(a => a.CustomhouseName, sort.SortOrder);
        case BankOrderSortType.CountryName:
          return query.OrderBy(a => a.CountryName, sort.SortOrder);
        case BankOrderSortType.CurrentBankOrderStateTitle:
          return query.OrderBy(a => a.CurrentBankOrderStatusTypeName, sort.SortOrder);
        case BankOrderSortType.CurrentDateTime:
          return query.OrderBy(a => a.CurrentDateTime, sort.SortOrder);
        case BankOrderSortType.CurrentEmployeeFullName:
          return query.OrderBy(a => a.CurrentEmployeeFullName, sort.SortOrder);
        case BankOrderSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case BankOrderSortType.CurrentDescription:
          return query.OrderBy(a => a.CurrentDescription, sort.SortOrder);
        case BankOrderSortType.BankOrderContractTypeTitle:
          return query.OrderBy(a => a.BankOrderContractTypeTitle, sort.SortOrder);
        case BankOrderSortType.CustomsValue:
          return query.OrderBy(a => a.CustomsValue, sort.SortOrder);
        case BankOrderSortType.SettlementDateTime:
          return query.OrderBy(a => a.SettlementDateTime, sort.SortOrder);
        case BankOrderSortType.BankOrderType:
          return query.OrderBy(a => a.BankOrderType, sort.SortOrder);
        case BankOrderSortType.AllocationFinalizationDateTime:
          return query.OrderBy(a => a.AllocationFinalizationDateTime, sort.SortOrder);
        case BankOrderSortType.WithoutCurrencyTransfer:
          return query.OrderBy(a => a.WithoutCurrencyTransfer, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToBankOrderResultQuery
    public IQueryable<BankOrderResult> ToBankOrderResultQuery(
        IQueryable<BankOrder> bankOrders,
        IQueryable<Enactment> enactments,
        IQueryable<Allocation> allocations,
        IQueryable<BankOrderIssue> bankOrderIssues,
        IQueryable<Allocation> latestAllocations,
        DateTime dateTime
        )

    {
      var previousDateTime = DateTime.UtcNow.AddDays(-1);
      var groupByResults = from bankOrder in bankOrders
                           join allocation in allocations on bankOrder.Id equals allocation.BankOrderId
                           join bankOrderIssue in bankOrderIssues on allocation.Id equals bankOrderIssue.AllocationId

                           group bankOrderIssue by bankOrderIssue.Allocation.BankOrderId into g
                           select new
                           {
                             BankOrderId = g.Key,
                             SumBankOrderIssueCreditAmount = g.Where(r => r.FinancialDocument.IsDelete == false).Sum(m => m.FinancialDocument.CreditAmount), // حساب مبداء
                             CurrencyId = g.Where(r => r.FinancialDocument.IsDelete == false).Select(m => m.FinancialDocument.FinancialAccount.CurrencyId).FirstOrDefault(), // ارز حساب مبداء
                             CurrencyTitle = g.Where(r => r.FinancialDocument.IsDelete == false).Select(m => m.FinancialDocument.FinancialAccount.Currency.Title).FirstOrDefault(),
                             //SumBankOrderIssueDebitAmount = g.Where(r => r.FinancialDocument.IsDelete == false).Sum(m => m.FinancialDocument.DebitAmount), // حساب مقصد
                             //ToCurrencyId = g.Where(r => r.FinancialDocument.IsDelete == false).Select(m => m.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.CurrencyId).FirstOrDefault(), // ارز حساب مقصد
                             //ToCurrencyTitle = g.Where(r => r.FinancialDocument.IsDelete == false).Select(m => m.FinancialDocument.FinancialDocumentTransfer.ToFinancialAccount.Currency.Title).FirstOrDefault(),
                           };


      var result = from bankOrder in bankOrders
                   join groupByResult in groupByResults on
                   bankOrder.Id equals groupByResult.BankOrderId into tGroupByResults
                   from tGroupByResult in tGroupByResults.DefaultIfEmpty()
                   join latestAllocation in latestAllocations on
                   bankOrder.Id equals latestAllocation.BankOrderId into tLatestAllocations
                   from tLatestAllocation in tLatestAllocations.DefaultIfEmpty()

                   select new BankOrderResult()
                   {
                     Id = bankOrder.Id,
                     SumBankOrderIssueCreditAmount = tGroupByResult.SumBankOrderIssueCreditAmount,
                     BankOrderIssueCurrencyId = tGroupByResult.CurrencyId,
                     BankOrderIssueCurrencyTitle = tGroupByResult.CurrencyTitle,
                     //SumBankOrderIssueDebitAmount = tGroupByResult.SumBankOrderIssueDebitAmount,
                     //ToBankOrderIssueCurrencyId = tGroupByResult.ToCurrencyId,
                     //ToBankOrderIssueCurrencyTitle = tGroupByResult.ToCurrencyTitle,
                     //SumBankOrderCurrencySource = bankOrder.CurrencySources.Sum(m => m.Price),
                     HasEnactment = bankOrder.Enactment.ReceiveDateTime != null ? true : false,
                     EnactmentId = bankOrder.Enactment.Id,
                     Code = bankOrder.Code,
                     RowVersion = bankOrder.RowVersion,
                     BankId = bankOrder.BankId,
                     OrderNumber = bankOrder.OrderNumber,
                     RegisterEmployeeFullName = bankOrder.User.Employee.FirstName + " " + bankOrder.User.Employee.LastName,
                     FolderCode = bankOrder.FolderCode,
                     StuffPriority = bankOrder.StuffPriority,
                     BankName = bankOrder.Bank.Title,
                     ProviderName = bankOrder.Provider.Name,
                     DateTime = bankOrder.DateTime,
                     SettlementDateTime = bankOrder.SettlementDateTime,
                     CurrencyTitle = bankOrder.Currency.Title,
                     CountryName = bankOrder.Country.Title,
                     CustomhouseName = bankOrder.Customhouse.Title,
                     CountryId = bankOrder.CountryId,

                     TotalAmount = bankOrder.TotalAmount,
                     FOB = bankOrder.FOB,
                     TransferCost = bankOrder.TransferCost,

                     DepositedAmount = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete)
                              .Select(i => i.BankOrderAmount).DefaultIfEmpty(0).Sum(),

                     DepositedFOB = bankOrder.BankOrderCurrencySources.Select(m => m.FOB).DefaultIfEmpty(0).Sum(),
                     DepositedTransferCost = bankOrder.BankOrderCurrencySources.Select(m => m.TransferCost).DefaultIfEmpty(0).Sum(),

                     SumBankOrderDetailWeight = bankOrder.BankOrderDetails.Sum(m => m.Weight),
                     SumBankOrderCurrencySource = bankOrder.BankOrderCurrencySources.Sum(m => m.ActualWeight),

                     CurrencyId = bankOrder.CurrencyId,
                     CustomhouseId = bankOrder.CustomhouseId,
                     Description = bankOrder.Description,
                     ExpireDate = bankOrder.ExpireDate,
                     ProviderId = bankOrder.ProviderId,
                     RegisterDate = bankOrder.RegisterDate,
                     Status = bankOrder.Status,
                     BankOrderType = bankOrder.BankOrderType,
                     UserId = bankOrder.UserId,
                     IsDelete = bankOrder.IsDelete,
                     CustomsValue = bankOrder.Ladings.Sum(i => i.CustomsValue),
                     CurrentBankOrderStatusTypeName = bankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
                     CurrentBankOrderStatusTypeId = bankOrder.CurrentBankOrderLog.BankOrderStatusType.Id,
                     CurrentDateTime = bankOrder.CurrentBankOrderLog.DateTime,
                     CheckCurrentDateTime = bankOrder.CurrentBankOrderLog.DateTime == previousDateTime,
                     CurrentEmployeeFullName = bankOrder.CurrentBankOrderLog.User.Employee.FirstName + " " +
                                  bankOrder.CurrentBankOrderLog.User.Employee.LastName,
                     CurrentDescription = bankOrder.CurrentBankOrderLog.Description,
                     BankOrderStatusType = bankOrder.BankOrderLogs.Select(x => x.BankOrderStatusType).Select(x => x.Id)
            .AsQueryable(),
                     BankOrderContractTypeTitle = bankOrder.BankOrderContractType.Title,
                     BankOrderContractTypeId = bankOrder.BankOrderContractType.Id,
                     LadingCodes = bankOrder.Ladings.Select(m => m.Code + "  ").AsQueryable(),
                     AllocationFinalizationDateTime = tLatestAllocation.FinalizationDateTime,
                     AllocationStatus = tLatestAllocation.FinalizationDateTime < dateTime ? AllocationStatus.Expired : tLatestAllocation.Status,
                     WithoutCurrencyTransfer = bankOrder.WithoutCurrencyTransfer
                   };


      return result;
    }
    #endregion

    #region ToBankOrderResult

    public Expression<Func<BankOrder, BankOrderResult>> ToBankOrderResult =
        bankOrder => new BankOrderResult
        {
          Id = bankOrder.Id,
          Code = bankOrder.Code,
          RowVersion = bankOrder.RowVersion,
          BankId = bankOrder.BankId,
          OrderNumber = bankOrder.OrderNumber,
          RegisterEmployeeFullName = bankOrder.User.Employee.FirstName + " " + bankOrder.User.Employee.LastName,
          FolderCode = bankOrder.FolderCode,
          StuffPriority = bankOrder.StuffPriority,
          BankName = bankOrder.Bank.Title,
          ProviderName = bankOrder.Provider.Name,
          DateTime = bankOrder.DateTime,
          CurrencyTitle = bankOrder.Currency.Title,
          CountryName = bankOrder.Country.Title,
          CustomhouseName = bankOrder.Customhouse.Title,
          CountryId = bankOrder.CountryId,
          TransferCost = bankOrder.TransferCost,
          FOB = bankOrder.FOB,
          TotalAmount = bankOrder.TotalAmount,
          DepositedAmount = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete)
                .Select(i => i.BankOrderAmount).DefaultIfEmpty(0).Sum(),
          DepositedFOB = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete)
                .Select(m => m.FOB).DefaultIfEmpty(0).Sum(),
          DepositedTransferCost = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete)
                .Select(i => i.TransferCost).DefaultIfEmpty(0).Sum(),
          CurrencyId = bankOrder.CurrencyId,
          CustomhouseId = bankOrder.CustomhouseId,
          Description = bankOrder.Description,
          ExpireDate = bankOrder.ExpireDate,
          ProviderId = bankOrder.ProviderId,
          RegisterDate = bankOrder.RegisterDate,
          Status = bankOrder.Status,
          UserId = bankOrder.UserId,
          IsDelete = bankOrder.IsDelete,
          CustomsValue = bankOrder.Ladings.Sum(i => i.CustomsValue),
          CurrentBankOrderStatusTypeName = bankOrder.CurrentBankOrderLog.BankOrderStatusType.Name,
          CurrentBankOrderStatusTypeId = bankOrder.CurrentBankOrderLog.BankOrderStatusType.Id,
          CurrentDateTime = bankOrder.CurrentBankOrderLog.DateTime,
          CurrentEmployeeFullName = bankOrder.CurrentBankOrderLog.User.Employee.FirstName + " " +
                                      bankOrder.CurrentBankOrderLog.User.Employee.LastName,
          CurrentDescription = bankOrder.CurrentBankOrderLog.Description,
          BankOrderStatusType = bankOrder.BankOrderLogs.Select(x => x.BankOrderStatusType).Select(x => x.Id)
                .AsQueryable(),
          BankOrderContractTypeTitle = bankOrder.BankOrderContractType.Title,
          BankOrderContractTypeId = bankOrder.BankOrderContractType.Id
        };
    #endregion

    #region ToBankOrderComboResult
    public Expression<Func<BankOrder, BankOrderComboResult>> ToBankOrderComboResult =
        bankOrder => new BankOrderComboResult
        {
          Id = bankOrder.Id,
          Code = bankOrder.Code,
          OrderNumber = bankOrder.OrderNumber,
          FolderCode = bankOrder.FolderCode,
          BankName = bankOrder.Bank.Title,
          CurrencyId = bankOrder.CurrencyId,
          CurrencyTitle = bankOrder.Currency.Title
        };
    #endregion


    #region Activate cuurency Source 
    public BankOrder ActivateBankOrderCurrencySource(
        int bankOrderId,
        byte[] rowVersion)
    {


      return EditBankOrder(
                id: bankOrderId,
                rowVersion: rowVersion,
                WithoutCurrencyTransfer: true);


    }

    #endregion
  }
}
