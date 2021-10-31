using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.StaticData;
using lena.Models;
using lena.Models.Common;
//using System.Data.Entity.SqlServer;
using lena.Models.SaleManagement.PaymentDueDate;
//using System.Data.Entity;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region AddPaymentDueDate

    public PaymentDueDate AddPaymentDueDate(
           TValue<int> orderId = null,
           TValue<int> paymentTypeId = null,
           TValue<double> amount = null,
            TValue<DateTime> paymentDate = null
           )
    {

      var paymentDueDate = repository.Create<PaymentDueDate>();
      paymentDueDate.OrderId = orderId;
      paymentDueDate.Amount = amount;
      paymentDueDate.PaymentDate = paymentDate;
      paymentDueDate.PaymentTypeId = paymentTypeId;
      var valu = App.Internals.ApplicationBase.AddBaseEntity(
                   baseEntity: paymentDueDate,
                   transactionBatch: null,
                   financialTransactionBatch: null,
                   description: null);
      return valu as PaymentDueDate;
    }
    #endregion

    #region GetOrderItemBlock
    public PaymentDueDate GetPaymentDueDate(int id) => GetPaymentDueDate(selector: e => e, id: id);
    public TResult GetPaymentDueDate<TResult>(
        Expression<Func<PaymentDueDate, TResult>> selector,
        int id)
    {

      var paymentDueDate = GetOrderPaymentDueDates(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (paymentDueDate == null)
        throw new PaymentDueDateNotFoundException(id);
      return paymentDueDate;
    }
    public IQueryable<TResult> GetOrderPaymentDueDates<TResult>(
        Expression<Func<PaymentDueDate, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> orderId = null,
        TValue<double> amount = null,
        TValue<int> customerId = null,
        TValue<int> paymentTypeId = null,
        TValue<DateTime> paymentDate = null,
        TValue<DateTime> fromRequestDate = null,
        TValue<DateTime> toRequestDate = null,
        TValue<DateTime> dateTime = null
    )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(

                   selector: e => e,
                    id: id,
                    code: code,
                    isDelete: isDelete,
                    userId: userId,
                    transactionBatchId: transactionBatchId,
                    description: description);
      var paymentDueDates = baseQuery.OfType<PaymentDueDate>();
      if (orderId != null)
        paymentDueDates = paymentDueDates.Where(i => i.OrderId == orderId);
      if (customerId != null)
        paymentDueDates = paymentDueDates.Where(i => i.Order.CustomerId == customerId);
      if (paymentTypeId != null)
        paymentDueDates = paymentDueDates.Where(i => i.PaymentTypeId == paymentTypeId);
      if (paymentDate != null)
        paymentDueDates = paymentDueDates.Where(i => i.PaymentDate == paymentDate);
      if (amount != null)
        paymentDueDates = paymentDueDates.Where(i => i.Amount == amount);
      if (dateTime != null)
        paymentDueDates = paymentDueDates.Where(i => i.DateTime == dateTime);
      if (fromRequestDate != null)
        paymentDueDates = paymentDueDates.Where(i => i.DateTime >= fromRequestDate);
      if (toRequestDate != null)
        paymentDueDates = paymentDueDates.Where(i => i.DateTime <= toRequestDate);

      return paymentDueDates.Select(selector);
    }

    public IQueryable<PaymentDueDateResult> GetPaymentDueDates(

    TValue<bool> isDelete = null,
    TValue<int> orderId = null,
    TValue<int> userId = null,
    TValue<double> amount = null,
    TValue<int> customerId = null,
    TValue<int> paymentTypeId = null,
    TValue<DateTime> paymentDate = null,
    TValue<DateTime> fromRequestDate = null,
    TValue<DateTime> toRequestDate = null,
    TValue<DateTime> dateTime = null,
    TValue<bool> groupByCustomer = null,
    TValue<bool> groupByPaymentType = null,
    TValue<bool> groupByPaymentDate = null
  )
    {

      var paymentDueDates = GetOrderPaymentDueDates(
                    selector: e => e,
                    orderId: orderId,
                    customerId: customerId,
                    paymentTypeId: paymentTypeId,
                    paymentDate: paymentDate,
                    fromRequestDate: fromRequestDate,
                    toRequestDate: toRequestDate,
                    isDelete: isDelete
                    );
      var groupPaymentDate = groupByPaymentDate == true;
      var groupPaymentType = groupByPaymentType == true;
      var groupCustomer = groupByCustomer == true;
      if (groupPaymentDate || groupPaymentType || groupCustomer)
      {


        #region Define Key Selector
        Expression<Func<PaymentDueDate, PaymentDueDateGroupKey>> groupKeySelector =
        i => new PaymentDueDateGroupKey
        {
          CustomerId = i.Order.CustomerId,
          PaymentDate = i.PaymentDate,
          PaymentTypeId = i.PaymentTypeId,


        };
        if (groupCustomer && groupPaymentDate && groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = i.Order.CustomerId,
            PaymentDate = i.PaymentDate,
            PaymentTypeId = i.PaymentTypeId,

          };
        if (groupCustomer && groupPaymentDate && !groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = i.Order.CustomerId,
            PaymentDate = i.PaymentDate,
            PaymentTypeId = null,

          };
        if (groupCustomer && !groupPaymentDate && groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = i.Order.CustomerId,
            PaymentDate = null,
            PaymentTypeId = i.PaymentTypeId,

          };
        if (!groupCustomer && groupPaymentDate && groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = null,
            PaymentDate = i.PaymentDate,
            PaymentTypeId = i.PaymentTypeId,

          };
        if (groupCustomer && !groupPaymentDate && !groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = i.Order.CustomerId,
            PaymentDate = null,
            PaymentTypeId = null,

          };
        if (!groupCustomer && !groupPaymentDate && groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = null,
            PaymentTypeId = i.PaymentTypeId,

            PaymentDate = null,

          };
        if (!groupCustomer && groupPaymentDate && !groupPaymentType)
          groupKeySelector = i => new PaymentDueDateGroupKey
          {
            CustomerId = null,
            PaymentDate = i.PaymentDate,
            PaymentTypeId = null,

          };
        #endregion

        var paymentDueDatesQueryable = paymentDueDates.GroupBy(groupKeySelector)
        .Select(gItems => new
        {
          CustomerId = gItems.Key.CustomerId,
          PaymentDate = gItems.Key.PaymentDate,
          PaymentTypeId = gItems.Key.PaymentTypeId,
          Amount = gItems.Sum(i => i.Amount)
        });

        var customers = App.Internals.SaleManagement.GetCustomers();

        var employees = App.Internals.UserManagement.GetEmployees(
                  selector: e => e);
        var paymentTypes = App.Internals.SaleManagement.GetPaymentTypes(
                    selector: e => e);




        var rawpaymentDueDatesQuery = from pdd in paymentDueDatesQueryable
                                      join cu in customers on pdd.CustomerId equals cu.Id into temp
                                      from tbl in temp.DefaultIfEmpty()
                                      join pt in paymentTypes on pdd.PaymentTypeId equals pt.Id into kazima
                                      from kazim in kazima.DefaultIfEmpty()


                                      select new PaymentDueDateResult()
                                      {

                                        CustomerName = tbl.Name,
                                        PaymentTypeName = kazim.Name,
                                        PaymentDate = pdd.PaymentDate,
                                        Amount = pdd.Amount,
                                        OrderId = null


                                      };



        return rawpaymentDueDatesQuery;
      }
      else
        return paymentDueDates.Select(ToPaymentDueDateResult);

    }
    //public IQueryable<SendProductSummaryResult> GetSendProductSummary(
    //     int? stuffId,
    //     int? customerId,
    //     int? exitReceiptRequestTypeId,
    //     DateTime? fromDateOrder,
    //     DateTime? toDateOrder,
    //     DateTime? fromDateTransfer,
    //     DateTime? toDateTransfer,
    //   bool dividedByDate,
    //   bool dividedByCustomer)
    //{
    //    
    //        var preparingSendingItems = App.Internals.WarehouseManagement.GetPreparingSendingItems(
    //                selector: preparingSendingItem => new
    //                {
    //                    Qty = preparingSendingItem.Qty,
    //                    UnitId = preparingSendingItem.UnitId,
    //                    UnitConversionRatio = preparingSendingItem.Unit.ConversionRatio,
    //                    DateTime = preparingSendingItem.PreparingSending.SendProduct.ExitReceipt.DateTime,
    //                    StuffId = preparingSendingItem.StuffId,
    //                    CooperatorId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest
    //                        .CooperatorId,
    //                    DateTimeOrderItem =
    //                    (preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest as OrderItemBlock)
    //                    .OrderItem.DateTime,
    //                    RowVersion = preparingSendingItem.RowVersion,
    //                    ExitReceiptRequestTypeId = preparingSendingItem.PreparingSending.SendPermission.ExitReceiptRequest.ExitReceiptRequestType.Id,

    //                }
    //                ,
    //                fromDate: fromDateTransfer,
    //                toDate: toDateTransfer,
    //                cooperatorId: customerId,
    //                stuffId: stuffId,
    //                exitReceiptRequestTypeId: exitReceiptRequestTypeId)
    //            
    //;


    //        if (fromDateOrder != null)
    //            preparingSendingItems = preparingSendingItems.Where(x => x.DateTimeOrderItem >= fromDateOrder);
    //        if (toDateOrder != null)
    //            preparingSendingItems = preparingSendingItems.Where(x => x.DateTimeOrderItem <= toDateOrder);

    //        var preparingSendingItemsGroup = (from query in preparingSendingItems
    //                                          let date = dividedByDate ? query.DateTime.Date : (DateTime?)null
    //                                          let cooperatorId = dividedByCustomer ? query.CooperatorId : (int?)null
    //                                          group query by new
    //                                          {
    //                                              StuffId = query.StuffId,
    //                                              Date = date,
    //                                              CustomerId = cooperatorId,
    //                                              ExitReceiptRequestTypeId = query.ExitReceiptRequestTypeId
    //                                          }
    //            into groupedData
    //                                          select new
    //                                          {
    //                                              StuffId = groupedData.Key.StuffId,
    //                                              CustomerId = groupedData.Key.CustomerId,
    //                                              Date = groupedData.Key.Date,
    //                                              SentQty = groupedData.Sum(r => r.Qty * r.UnitConversionRatio),
    //                                              ExitReceiptRequestTypeId = groupedData.Key.ExitReceiptRequestTypeId
    //                                          });

    //        var customers = GetCooperators();
    //        var stuffs = GetStuffs(selector: e => e);
    //        var units = App.Internals.ApplicationBase.GetUnits(selector: e => e, isMainUnit: true)
    //;
    //        var exitReceiptRequestTypes = App.Internals.WarehouseManagement.GetExitReceiptRequestTypes(selector: e => e);
    //        var result = (from oib in preparingSendingItemsGroup
    //                      join exitReceiptRequestType in exitReceiptRequestTypes on oib.ExitReceiptRequestTypeId equals exitReceiptRequestType.Id
    //                      join stuff in stuffs on oib.StuffId equals stuff.Id
    //                      join unit in units on stuff.UnitTypeId equals unit.UnitTypeId
    //                      join customer in customers on oib.CustomerId equals customer.Id into allCustomer
    //                      from customerLeftJoin in allCustomer.DefaultIfEmpty()
    //                      select new SendProductSummaryResult()
    //                      {
    //                          StuffId = stuff.Id,
    //                          StuffCode = stuff.Code,
    //                          StuffName = stuff.Name,
    //                          UnitId = unit.Id,
    //                          UnitName = unit.Name,
    //                          CustomerId = customerLeftJoin.Id,
    //                          CustomerCode = customerLeftJoin.Code,
    //                          CustomerName = customerLeftJoin.Name,
    //                          SentQty = oib.SentQty,
    //                          SentDate = oib.Date,
    //                          ExitReceiptRequestTypeTitle = exitReceiptRequestType.Title
    //                      });
    //        return result;
    //    });
    //}
    #endregion
    #region To Result
    public Expression<Func<PaymentDueDate, PaymentDueDateResult>> ToPaymentDueDateResult =
        paymentDueDate => new PaymentDueDateResult
        {
          Id = paymentDueDate.Id,
          OrderId = paymentDueDate.OrderId,
          CustomerId = paymentDueDate.Order.CustomerId,
          CustomerName = paymentDueDate.Order.Customer.Name,
          PaymentTypeId = paymentDueDate.PaymentType.Id,
          PaymentTypeName = paymentDueDate.PaymentType.Name,
          PaymentDate = paymentDueDate.PaymentDate,
          Amount = paymentDueDate.Amount,
          DateTime = paymentDueDate.DateTime,
          UserId = paymentDueDate.UserId,
          EmployeeFullName = paymentDueDate.User.Employee.FirstName + "" + paymentDueDate.User.Employee.LastName,
          RowVersion = paymentDueDate.RowVersion,


        };
    #endregion

    #region SearchOrderItemBlock

    public IQueryable<PaymentDueDateResult> SearchPaymentDueDateResult(
        IQueryable<PaymentDueDateResult> query,
        string search,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PaymentDueDateResult> SortPaymentDueDateResult(IQueryable<PaymentDueDateResult> query, SortInput<PaymentDueDateSortType> sort)
    {
      switch (sort.SortType)
      {
        case PaymentDueDateSortType.OrderId:
          return query.OrderBy(a => a.OrderId, sort.SortOrder);
        case PaymentDueDateSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case PaymentDueDateSortType.PaymentTypeName:
          return query.OrderBy(a => a.PaymentTypeName, sort.SortOrder);
        case PaymentDueDateSortType.PaymentDate:
          return query.OrderBy(a => a.PaymentDate, sort.SortOrder);
        case PaymentDueDateSortType.Amount:
          return query.OrderBy(a => a.Amount, sort.SortOrder);
        case PaymentDueDateSortType.DateTime:
          return query.OrderBy(a => a.DateTime, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    #endregion
    #region EditPaymentDueDateInput

    public PaymentDueDate EditPaymentDueDate(
        byte[] rowVersion,
        int id,
        TValue<int> paymentTypeId = null,
        TValue<double> amount = null,
        TValue<DateTime> paymentDate = null
        )
    {

      var paymentDueDate = GetPaymentDueDate(id: id);

      if (paymentDate != null)
        paymentDueDate.PaymentDate = paymentDate;
      if (amount != null)
        paymentDueDate.Amount = amount;
      if (paymentTypeId != null)
        paymentDueDate.PaymentTypeId = paymentTypeId;
      var retValue = App.Internals.ApplicationBase.EditBaseEntity(
                   baseEntity: paymentDueDate,
                   rowVersion: rowVersion);
      return retValue as PaymentDueDate;

    }
    #endregion
    #region DeletePaymentDueDateInput
    public PaymentDueDate DeletePaymentDueDate(
       int id,
       byte[] rowVersion)
    {

      var paymentDueDate = GetPaymentDueDate(id: id);
      paymentDueDate.IsDelete = true;
      repository.Update(paymentDueDate, rowVersion: rowVersion);
      return paymentDueDate;
    }

    #endregion
  }
}
