using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Domains;
using lena.Models;
using System.Linq.Expressions;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Services.Internals.Supplies.Exception;
//using lena.Services.Core.Foundation.Service.Internal.Action;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Get
    public Allocation GetAllocation(int id) => GetAllocation(selector: e => e, id: id);
    public TResult GetAllocation<TResult>(
        Expression<Func<Allocation, TResult>> selector,
        int id)
    {

      var allocation = GetAllocations(selector: selector,
                id: id).FirstOrDefault();
      if (allocation == null)
        throw new AllocationNotFoundException(id: id);
      return allocation;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetAllocations<TResult>(
        Expression<Func<Allocation, TResult>> selector,
        TValue<int> id = null,
        TValue<int> duration = null,
        TValue<double> amount = null,
        TValue<byte> currencyId = null,
        TValue<int> bankOrderId = null,
        TValue<AllocationStatus> status = null,
        TValue<DateTime> receivedDateTime = null,
        TValue<DateTime> finalizationDateTime = null,
        TValue<int> statisticalRegistrationCertificate = null,
        TValue<string> description = null
    )
    {

      var query = repository.GetQuery<Allocation>();
      if (id != null)
        query = query.Where(m => m.Id == id);
      if (description != null)
        query = query.Where(m => m.Description == description);
      if (status != null)
        query = query.Where(m => m.Status == status);
      if (amount != null)
        query = query.Where(m => m.Amount == amount);
      if (currencyId != null)
        query = query.Where(m => m.CurrencyId == currencyId);
      if (duration != null)
        query = query.Where(m => m.Duration == duration);
      if (bankOrderId != null)
        query = query.Where(m => m.BankOrderId == bankOrderId);
      if (receivedDateTime != null)
        query = query.Where(m => m.ReceivedDateTime == receivedDateTime);
      if (finalizationDateTime != null)
        query = query.Where(m => m.FinalizationDateTime == finalizationDateTime);
      if (statisticalRegistrationCertificate != null)
        query = query.Where(m => m.StatisticalRegistrationCertificate == statisticalRegistrationCertificate);
      return query.Select(selector);
    }
    #endregion

    #region AddAllocation

    public Allocation AddAllocationProcess(
        int duration,
        double amount,
        byte currencyId,
        int bankOrderId,
        DateTime beginningDateTime,
        DateTime receivedDateTime,
        int statisticalRegistrationCertificate,
        UploadFileData uploadFileData,
        string description)
    {


      var bankOrder = GetBankOrder(id: bankOrderId);
      if (bankOrder != null && bankOrder.Enactment == null)
        throw new BankOrderNotHaveEnactmentException(bankOrderId: bankOrderId);



      Document document = null;
      if (uploadFileData != null)
        document = App.Internals.ApplicationBase.AddDocument(
                  name: uploadFileData.FileName,
                  fileStream: uploadFileData.FileData);
      var allocation = AddAllocation(
                duration: duration,
                amount: amount,
                documentId: document?.Id,
                currencyId: currencyId,
                bankOrderId: bankOrderId,
                beginningDateTime: beginningDateTime,
                receivedDateTime: receivedDateTime,
                statisticalRegistrationCertificate: statisticalRegistrationCertificate,
                description: description);

      #region Check Alloocation Amount             

      var depositAmount = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete).Select(i => i.BankOrderAmount).DefaultIfEmpty(0).Sum();
      var remainingAmount = bankOrder.TotalAmount - depositAmount; // مانده حواله

      if (amount > remainingAmount)
        throw new AllocationAmountIsBiggerThanBankOrderIssueRemainingAmount(bankOrderId: bankOrderId);
      #endregion

      return allocation;
    }

    public Allocation AddAllocation(
       int duration,
       double amount,
       byte currencyId,
       int bankOrderId,
       DateTime beginningDateTime,
       DateTime receivedDateTime,
       int statisticalRegistrationCertificate,
       string description,
       Nullable<Guid> documentId)
    {

      var allocation = repository.Create<Allocation>();
      allocation.Amount = amount;
      allocation.DocumentId = documentId ?? null;
      allocation.Duration = duration;
      allocation.CurrencyId = currencyId;
      allocation.BankOrderId = bankOrderId;
      allocation.Description = description;
      allocation.BeginningDateTime = beginningDateTime;
      allocation.ReceivedDateTime = receivedDateTime;
      allocation.DateTime = DateTime.Now.ToUniversalTime();
      allocation.FinalizationDateTime = receivedDateTime.AddDays(duration);
      allocation.Status = allocation.FinalizationDateTime < DateTime.Now.ToUniversalTime() ? AllocationStatus.Expired : AllocationStatus.Observed;
      allocation.UserId = App.Providers.Security.CurrentLoginData.UserId;
      allocation.StatisticalRegistrationCertificate = statisticalRegistrationCertificate;
      repository.Add(allocation);
      return allocation;
    }

    #endregion

    #region EditAllocation
    public Allocation EditAllocationProcess(
        int id,
        byte[] rowVersion,
        TValue<int> duration = null,
        TValue<double> amount = null,
        TValue<byte> currencyId = null,
        TValue<AllocationStatus> status = null,
        TValue<DateTime> receivedDateTime = null,
        TValue<DateTime> beginningDateTime = null,
        TValue<UploadFileData> uploadFileData = null,
        TValue<int> statisticalRegistrationCertificate = null,
        TValue<string> description = null)
    {


      var allocation = EditAllocation(
                id: id,
                amount: amount,
                status: status,
                duration: duration,
                currencyId: currencyId,
                uploadFileData: uploadFileData,
                receivedDateTime: receivedDateTime,
                beginningDateTime: beginningDateTime,
                statisticalRegistrationCertificate: statisticalRegistrationCertificate,
                description: description);


      #region Check Alloocation Amount
      var allocationRes = GetAllocation(id: id);
      var bankOrder = GetBankOrder(id: allocationRes.BankOrderId);

      var depositAmount = bankOrder.FinancialDocumentBankOrders.Where(i => !i.FinancialDocument.IsDelete).Select(i => i.BankOrderAmount).DefaultIfEmpty(0).Sum();
      var remainingAmount = bankOrder.TotalAmount - depositAmount; // مانده حواله

      if (amount > remainingAmount)
        throw new AllocationAmountIsBiggerThanBankOrderIssueRemainingAmount(bankOrderId: bankOrder.Id);
      #endregion

      return allocation;

    }

    public Allocation EditAllocation(
     int id,
     TValue<int> duration = null,
     TValue<double> amount = null,
     TValue<byte> currencyId = null,
     TValue<AllocationStatus> status = null,
     TValue<DateTime> receivedDateTime = null,
     TValue<DateTime> beginningDateTime = null,
     TValue<UploadFileData> uploadFileData = null,
     TValue<int> statisticalRegistrationCertificate = null,
     TValue<string> description = null)
    {

      var allocation = GetAllocation(id: id);
      if (description != null)
        allocation.Description = description;

      if (statisticalRegistrationCertificate != null)
        allocation.StatisticalRegistrationCertificate = statisticalRegistrationCertificate;

      if (beginningDateTime != null)
      {
        allocation.BeginningDateTime = beginningDateTime;
      }

      if (receivedDateTime != null)
      {
        allocation.ReceivedDateTime = receivedDateTime;
        allocation.FinalizationDateTime = receivedDateTime.Value.AddDays(duration);
      }

      if (duration != null)
      {
        allocation.Duration = duration;
        allocation.FinalizationDateTime = allocation.ReceivedDateTime.AddDays(duration);
        var dateTimeNow = DateTime.Now.ToUniversalTime();
        if (allocation.FinalizationDateTime < dateTimeNow)
          allocation.Status = AllocationStatus.Expired;
        else
          allocation.Status = AllocationStatus.Observed;
      }

      if (status != null)
      {
        var dateTimeNow = DateTime.Now.ToUniversalTime();
        if (allocation.FinalizationDateTime < dateTimeNow)
          allocation.Status = AllocationStatus.Expired;
        else
          allocation.Status = status;
      }

      if (currencyId != null)
        allocation.CurrencyId = currencyId;

      if (uploadFileData != null)
      {
        if (allocation.DocumentId != null)
          App.Internals.ApplicationBase.DeleteDocument(allocation.DocumentId.Value);
        var document = App.Internals.ApplicationBase.AddDocument(
                      name: uploadFileData.Value.FileName,
                      fileStream: uploadFileData.Value.FileData);
        allocation.DocumentId = document.Id;
      }

      if (amount != null)
        allocation.Amount = amount;
      allocation.DateTime = DateTime.Now.ToUniversalTime();

      repository.Update(entity: allocation, rowVersion: allocation.RowVersion);
      return allocation;
    }
    #endregion

    #region DeleteAllocation
    public void DeleteAllocation(int id)
    {

      var allocation = GetAllocation(id: id);
      if (allocation.Status != AllocationStatus.Observed)
        throw new CanNotDeleteAllocationException(id: id);
      repository.Delete(allocation);
    }
    #endregion

    #region ConfirmAllocation
    public Allocation ConfirmAllocation(
        int id,
        byte[] rowVersion)
    {


      #region Check Allocation Has Document
      var allocation = GetAllocation(id: id);

      if (allocation.DocumentId == null)

        throw new AllocationDoNotHaveDocumentException(id: id);
      #endregion

      return EditAllocation(
              id: id,
              status: AllocationStatus.Confirmed);
    }
    #endregion

    #region Sort
    public IOrderedQueryable<AllocationResult> SortAllocationResult(
        IQueryable<AllocationResult> query, SortInput<AllocationSortType> type)
    {
      switch (type.SortType)
      {
        case AllocationSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case AllocationSortType.BankOrderNumber:
          return query.OrderBy(a => a.BankOrderNumber, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<AllocationResult> SearchAllocationResult(
        IQueryable<AllocationResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.BankOrderNumber.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region ToAllocationResultQuery
    public IQueryable<AllocationResult> ToAllocationResultQuery(
        IQueryable<Allocation> allocations,
        DateTime dateTime)
    {
      var result = from allocation in allocations
                   select new AllocationResult()
                   {
                     Id = allocation.Id,
                     UserId = allocation.UserId,
                     DateTime = allocation.DateTime,
                     Description = allocation.Description,
                     BankOrderId = allocation.BankOrderId,
                     DocumentId = allocation.DocumentId,
                     BankOrderNumber = allocation.BankOrder.OrderNumber,
                     Amount = allocation.Amount,
                     CurrencyId = allocation.CurrencyId,
                     CurrencyTitle = allocation.Currency.Title,
                     Duration = allocation.Duration,
                     Status = allocation.FinalizationDateTime < dateTime ? AllocationStatus.Expired : allocation.Status,
                     ReceivedDateTime = allocation.ReceivedDateTime,
                     BeginningDateTime = allocation.BeginningDateTime,
                     FinalizationDateTime = allocation.FinalizationDateTime,
                     StatisticalRegistrationCertificate = allocation.StatisticalRegistrationCertificate,
                     EmployeeFullName = allocation.User.Employee.FirstName + " " + allocation.User.Employee.LastName,
                     HasBankOrderIssue = allocation.BankOrderIssues.Any() ? true : false,
                     BankOrderIssueResult = allocation.BankOrderIssues.AsQueryable().Select(App.Internals.Supplies.ToBankOrderIssueResult),
                     RowVersion = allocation.RowVersion

                   };

      return result;
    }
    #endregion

    #region ToResult
    public Expression<Func<Allocation, AllocationResult>> ToAllocationResult =
         allocation => new AllocationResult()
         {
           Id = allocation.Id,
           UserId = allocation.UserId,
           DateTime = allocation.DateTime,
           Description = allocation.Description,
           BankOrderId = allocation.BankOrderId,
           BankOrderNumber = allocation.BankOrder.OrderNumber,
           Amount = allocation.Amount,
           CurrencyId = allocation.CurrencyId,
           CurrencyTitle = allocation.Currency.Title,
           Duration = allocation.Duration,
           Status = allocation.Status,
           ReceivedDateTime = allocation.ReceivedDateTime,
           BeginningDateTime = allocation.BeginningDateTime,
           FinalizationDateTime = allocation.FinalizationDateTime,
           StatisticalRegistrationCertificate = allocation.StatisticalRegistrationCertificate,
           EmployeeFullName = allocation.User.Employee.FirstName + " " + allocation.User.Employee.LastName,
           RowVersion = allocation.RowVersion
         };
    #endregion
  }
}
