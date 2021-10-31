using System;
//using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.QualityControl.GeneralQualityControlFaultReport;
using lena.Models.QualityControl.ProductionFaultSeparationDayReport;
using lena.Models.QualityControl.QualityControFaultsReport;
using lena.Models.QualityControl.ReworkProductionOperationReport;
using lena.Models.QualityControl.StuffProductionFaultPercentegeReport;
using System.Collections.Generic;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {

    #region Gets
    public IQueryable<QualityControFaultsReportResult> GetQualityControFaultsReports(
        //Expression<Func<lena.Domains.StuffProductionFaultType, TResult> selector,
        TValue<int> id = null,
        TValue<int> stuffId = null,
        TValue<string> StuffName = null,
        TValue<string> StuffCode = null,
        TValue<string> ProductionFaultId = null,
        TValue<string> ProductionFaultTitle = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<DateTime> dateTime = null)
    {


      var productionFaultTypes = App.Internals.Production.GetProductionFaultTypes(
                e => e);

      var stuffs = App.Internals.SaleManagement.GetStuffs(
                e => e);

      var repairProductionFaults = App.Internals.Production.GetRepairProductionFaults(
                e => e);


      if (fromDate != null)

        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime >= fromDate);

      if (toDate != null)
        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime <= toDate);

      var groupNew = (from repairProductionFault in repairProductionFaults
                      group repairProductionFault by
                            new
                            {
                              StuffId = repairProductionFault.RepairProduction.Production.StuffSerialStuffId,
                              ProductionFaultTypeId = repairProductionFault.ProductionFaultTypeId,
                            }

        into newGroupResult

                      select new
                      {
                        StuffId = newGroupResult.Key.StuffId,
                        ProductionFaultTypeId = newGroupResult.Key.ProductionFaultTypeId,
                        ProductionFaultTypeTitle = newGroupResult.FirstOrDefault().ProductionFaultType.Title,
                        ProductionFaultTypeCount = newGroupResult.Count()
                      });

      var query = (
                from q in groupNew
                join stuff in stuffs on q.StuffId equals stuff.Id
                select new QualityControFaultsReportResult()
                {
                  StuffId = q.StuffId,
                  StuffName = stuff.Name,
                  StuffCode = stuff.Code,
                  ProductionFaultTypeId = q.ProductionFaultTypeId,
                  ProductionFaultTypeTitle = q.ProductionFaultTypeTitle,
                  ProductionFaultTypeCount = q.ProductionFaultTypeCount,
                }
                );

      return query;
    }
    #endregion


    //#region Search
    //public IQueryable<QualityControFaultsReportResult> SearchQualityControFaultResult(
    //IQueryable<QualityControFaultsReportResult> query,
    //string search,
    //DateTime? fromConfirmationDate = null,
    //DateTime? toConfirmationDate = null,
    //DateTime? fromDate = null,
    //DateTime? toDate = null)
    //{
    //    if (!string.IsNullOrEmpty(search))
    //        query = from item in query
    //                where
    //                    item.StuffCode.Contains(search) ||
    //                    item.StuffName.Contains(search)
    //                select item;

    //    return query;
    //}
    //#endregion


    #region Gets ReworkProductionOperation
    public IQueryable<ReworkProductionOperationResult> GetReworkProductionOperations(
            TValue<int> employeeId = null,
            TValue<int> operationId = null,
            TValue<string> operationCode = null,
            TValue<bool> groupByEmployee = null,
            TValue<DateTime> fromDate = null,
            TValue<DateTime> toDate = null)
    {

      var productionOperations = App.Internals.Production.GetProductionOperations(e =>
            new
            {
              Id = e.Id,
              EmployeeId = (int?)null,
              IsFaultCause = e.IsFaultCause,
              OperationId = e.OperationId,
              ReworkProductionOperationId = (int?)e.ReworkFaildProductionOperation.Id,
              FaildProductionOperationId = (int?)e.FaildProductionOperation.Id,
              ProductionOperationEmployeeGroupId = e.ProductionOperationEmployeeGroupId,
              ProductionOperationEmployeeGroup = e.ProductionOperationEmployeeGroup

            }
            ,
                fromDateTime: fromDate,
                toDateTime: toDate,
                operationId: operationId);

      var employees = App.Internals.UserManagement.GetEmployees(
                e => e);

      var operations = App.Internals.Planning.GetOperations(e => e,
                id: operationId);

      if (groupByEmployee)
      {
        productionOperations = from productionOperation in productionOperations
                               from productionOpertaionEmployee in productionOperation.ProductionOperationEmployeeGroup.ProductionOperationEmployees
                               select new
                               {
                                 Id = productionOperation.Id,
                                 EmployeeId = (int?)productionOpertaionEmployee.EmployeeId,
                                 IsFaultCause = productionOperation.IsFaultCause,
                                 OperationId = productionOperation.OperationId,
                                 ReworkProductionOperationId = productionOperation.ReworkProductionOperationId,
                                 FaildProductionOperationId = productionOperation.FaildProductionOperationId,
                                 ProductionOperationEmployeeGroupId = productionOperation.ProductionOperationEmployeeGroupId,
                                 ProductionOperationEmployeeGroup = productionOperation.ProductionOperationEmployeeGroup

                               };
      }
      //var productionOperationCount = from productionOperation in productionOperations
      //                               group productionOperation by new
      //                               {
      //                                   OperaionId = productionOperation.OperationId,
      //                                   ProductionOperationEmployeeId = productionOperation.EmployeeId
      //                               } into g
      //                               select new
      //                               {
      //                                   ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
      //                                   OperationId = g.Key.OperaionId,
      //                                   TotalProductionOperationCount = g.Count(),
      //                                   TotalNotFaildProductionOperationCount = g.Count(i => i.FaildProductionOperationId == null && i.ReworkProductionOperationId == null),
      //                                   TotalIsFaultCausedProductionOperationCount = g.Count(i => i.IsFaultCause == true),
      //                                   TotalReworkProductionOperationCount = g.Count(i => i.ReworkProductionOperationId != null)
      //                               };


      var totalProductionOps = from productionOperation in productionOperations
                               group productionOperation by new
                               {
                                 OperaionId = productionOperation.OperationId,
                                 ProductionOperationEmployeeId = productionOperation.EmployeeId
                               } into g
                               select new
                               {
                                 ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
                                 OperationId = g.Key.OperaionId,
                                 TotalProductionOperationCount = g.Count()
                               };

      var notFaildProductionOps = from productionOperation in productionOperations.Where(i => i.FaildProductionOperationId == null && i.ReworkProductionOperationId == null)
                                  group productionOperation by new
                                  {
                                    OperaionId = productionOperation.OperationId,
                                    ProductionOperationEmployeeId = productionOperation.EmployeeId
                                  } into g
                                  select new
                                  {
                                    ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
                                    OperationId = g.Key.OperaionId,

                                    TotalNotFaildProductionOperationCount = g.Count(),

                                  };

      var faultCausedProductionOps = from productionOperation in productionOperations.Where(i => i.IsFaultCause == true)
                                     group productionOperation by new
                                     {
                                       OperaionId = productionOperation.OperationId,
                                       ProductionOperationEmployeeId = productionOperation.EmployeeId
                                     } into g
                                     select new
                                     {
                                       ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
                                       OperationId = g.Key.OperaionId,
                                       TotalIsFaultCausedProductionOperationCount = g.Count()
                                     };

      var reworkProductionOps = from productionOperation in productionOperations.Where(i => i.ReworkProductionOperationId != null)
                                group productionOperation by new
                                {
                                  OperaionId = productionOperation.OperationId,
                                  ProductionOperationEmployeeId = productionOperation.EmployeeId
                                } into g
                                select new
                                {
                                  ProductionOperationEmployeeId = g.Key.ProductionOperationEmployeeId,
                                  OperationId = g.Key.OperaionId,
                                  TotalReworkProductionOperationCount = g.Count(i => i.ReworkProductionOperationId != null)
                                };

      var finalProductionOperationCount = from totalProductionOp in totalProductionOps
                                          join notFaildProductionOp in notFaildProductionOps
                                                on new { totalProductionOp.OperationId, totalProductionOp.ProductionOperationEmployeeId } equals
                                                new { notFaildProductionOp.OperationId, notFaildProductionOp.ProductionOperationEmployeeId } into tnfp
                                          from nfp in tnfp.DefaultIfEmpty()
                                          join faultCausedProductionOp in faultCausedProductionOps
                                                on new { totalProductionOp.OperationId, totalProductionOp.ProductionOperationEmployeeId } equals
                                                new { faultCausedProductionOp.OperationId, faultCausedProductionOp.ProductionOperationEmployeeId } into tfcp
                                          from fcp in tfcp.DefaultIfEmpty()
                                          join reworkProductionOp in reworkProductionOps
                                                on new { totalProductionOp.OperationId, totalProductionOp.ProductionOperationEmployeeId } equals
                                                new { reworkProductionOp.OperationId, reworkProductionOp.ProductionOperationEmployeeId } into trp
                                          from rp in trp.DefaultIfEmpty()
                                          select new
                                          {
                                            OperationId = totalProductionOp.OperationId,
                                            ProductionOperationEmployeeId = totalProductionOp.ProductionOperationEmployeeId,
                                            TotalProductionOperationCount = (int?)totalProductionOp.TotalProductionOperationCount,
                                            TotalNotFaildProductionOperationCount = (int?)nfp.TotalNotFaildProductionOperationCount,
                                            TotalReworkProductionOperationCount = (int?)rp.TotalReworkProductionOperationCount,
                                            TotalIsFaultCausedProductionOperationCount = (int?)fcp.TotalIsFaultCausedProductionOperationCount
                                          };


      var query = (from q1 in finalProductionOperationCount
                   join operation in operations on q1.OperationId equals operation.Id
                   join employee in employees on
                         q1.ProductionOperationEmployeeId equals employee.Id into q
                   from productionOpertaionEmployee in q.DefaultIfEmpty()

                   select new ReworkProductionOperationResult()
                   {
                     OperationId = q1.OperationId,
                     OperationCode = operation.Code,
                     OperationTitle = operation.Title,
                     ProductionOperationEmployeeId = q1.ProductionOperationEmployeeId,
                     ProductionOperationEmployeeFullName = productionOpertaionEmployee.FirstName + " " + productionOpertaionEmployee.LastName,
                     TotalProductionOperationCount = q1.TotalProductionOperationCount ?? 0,
                     TotalNotFaildProductionOperationCount = q1.TotalNotFaildProductionOperationCount ?? 0,
                     TotalReworkProductionOperationCount = q1.TotalReworkProductionOperationCount ?? 0,
                     TotalIsFaultCausedProductionOperationCount = q1.TotalIsFaultCausedProductionOperationCount ?? 0
                   });

      // query = query.Where(m => m.TotalReworkProductionOperationCount > 0);
      if (employeeId != null)
      {
        query = query.Where(m => m.ProductionOperationEmployeeId == employeeId);
      }

      return query;
    }
    #endregion

    #region Sort ReworkProductionOperation
    public IOrderedQueryable<ReworkProductionOperationResult> SortReworkProductionOperation(
        IQueryable<ReworkProductionOperationResult> query,
        SortInput<ReworkProductionOperationSortType> sort)

    {
      switch (sort.SortType)
      {
        case ReworkProductionOperationSortType.OperationId:
          return query.OrderBy(a => a.OperationId, sort.SortOrder);
        case ReworkProductionOperationSortType.OperationCode:
          return query.OrderBy(a => a.OperationCode, sort.SortOrder);
        case ReworkProductionOperationSortType.OperationTitle:
          return query.OrderBy(a => a.OperationTitle, sort.SortOrder);
        case ReworkProductionOperationSortType.TotalProductionOperationCount:
          return query.OrderBy(a => a.TotalProductionOperationCount, sort.SortOrder);
        case ReworkProductionOperationSortType.TotalNotFaildProductionOperationCount:
          return query.OrderBy(a => a.TotalNotFaildProductionOperationCount, sort.SortOrder);
        case ReworkProductionOperationSortType.TotalReworkProductionOperationCount:
          return query.OrderBy(a => a.TotalReworkProductionOperationCount, sort.SortOrder);
        case ReworkProductionOperationSortType.TotalIsFaultCausedProductionOperationCount:
          return query.OrderBy(a => a.TotalIsFaultCausedProductionOperationCount, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<ReworkProductionOperationResult> SearchReworkProductionOperation(
        IQueryable<ReworkProductionOperationResult> query,
        string searchText
      )
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.OperationCode.Contains(searchText) ||
                 item.OperationTitle.Contains(searchText)
                select item;
      return query;
    }
    #endregion


    #region Gets ProductionStuffDetailsFaultCategoryPercentageReport
    public IQueryable<ProductionStuffDetailFaultCategoryPercentegeReportResult> GetProductionStuffDetailFaultCategoryPercentegeReports(
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<DateTime> dateTime = null)
    {

      var repairProductionStuffDetails = App.Internals.Production.GetRepairProductionStuffDetails(e => e,
                fromDate: fromDate,
                toDate: toDate);

      var stuffCategories = App.Internals.SaleManagement.GetStuffCategories(e => e);

      var groupResult = from repairProductionStuffDetail in repairProductionStuffDetails
                        group repairProductionStuffDetail by repairProductionStuffDetail.Stuff.StuffCategory.Id into g
                        select new
                        {
                          ProductionStuffDetailFaultCategoryId = g.Key,
                          ProductionStuffDetailFaultCategorySum = g.Sum(m => m.Qty)
                        };

      var query = from item in groupResult
                  join repairProductionStuffDetail in repairProductionStuffDetails on
                        item.ProductionStuffDetailFaultCategoryId equals repairProductionStuffDetail.Stuff.StuffCategory.Id
                  select new ProductionStuffDetailFaultCategoryPercentegeReportResult()
                  {
                    ProductionStuffDetailFaultCategoryId = item.ProductionStuffDetailFaultCategoryId,
                    ProductionStuffDetailFaultCategoryName = repairProductionStuffDetail.Stuff.StuffCategory.Name,
                    ProductionStuffDetailFaultCategoryCount = item.ProductionStuffDetailFaultCategorySum,
                    ProductionFaultTypeStuffId = repairProductionStuffDetail.StuffId,
                    ProductionFaultTypeStuffName = repairProductionStuffDetail.Stuff.Name
                  };
      return query;


    }

    #endregion

    #region Gets ProductionStuffDetailPercentegeReport
    public IQueryable<ProductionStuffDetailFaultPercentegeReportResult> GetProductionStuffDetailPercentegeReports(
        TValue<DateTime> fromDate = null,
    TValue<DateTime> toDate = null,
    TValue<DateTime> dateTime = null)
    {

      var serialprofiles = App.Internals.WarehouseManagement.GetSerialProfiles(e => e);

      var repairProductionStuffDetails = App.Internals.Production.GetRepairProductionStuffDetails(e => e,
                fromDate: fromDate,
                toDate: toDate);

      var productionStuffDetails = App.Internals.Production.GetProductionStuffDetails();

      var cooperators = App.Internals.SaleManagement.GetCooperators(
                selector: e => e);

      var providers = App.Internals.SaleManagement.GetProviders();

      var productionStuffDetailsQuery = from productionStuffDetail in productionStuffDetails
                                        group productionStuffDetail by productionStuffDetail.StuffId into g
                                        select new
                                        {
                                          stuffId = g.Key,
                                          consume = g.Sum(x => x.Qty)
                                        };// مقدار مصرفی               


      var groupResult = from repairProductionStuffDetail in repairProductionStuffDetails
                        let stuffSerial = repairProductionStuffDetail.StuffSerial
                        let serialProfile = repairProductionStuffDetail.StuffSerial.SerialProfile
                        join provider in providers on serialProfile.CooperatorId equals provider.Id


                        group repairProductionStuffDetail by
                               new
                               {
                                 StuffId = repairProductionStuffDetail.StuffId,
                                 CooperatorId = provider.Id

                               } into g
                        select new
                        {
                          ProductionFaultStuffDetailId = g.Key.StuffId,
                          ProductionFaultStuffDetailCount = g.Sum(x => x.Qty),
                          CooperatorId = g.Key.CooperatorId
                        };

      var query = from item in groupResult
                  join repairProductionStuffDetail in repairProductionStuffDetails on
                        item.ProductionFaultStuffDetailId equals repairProductionStuffDetail.StuffId
                  join q in productionStuffDetailsQuery on
                        item.ProductionFaultStuffDetailId equals q.stuffId
                  join cooperator in cooperators on
                        item.CooperatorId equals cooperator.Id

                  select new ProductionStuffDetailFaultPercentegeReportResult()
                  {
                    FaultProductionStuffDetailId = item.ProductionFaultStuffDetailId,
                    FaultProductionStuffDetailCode = repairProductionStuffDetail.Stuff.Code,
                    FaultProductionStuffDetailName = repairProductionStuffDetail.Stuff.Name,
                    FaultProductionStuffDetailCount = item.ProductionFaultStuffDetailCount,
                    FaultProductionStuffDetailConsumedQty = q.consume,
                    ProviderId = item.CooperatorId,
                    ProviderName = cooperator.Name
                  };

      return query;
    }
    #endregion

    #region Gets ProductionFaultSeparationDayReports
    public IQueryable<ProductionFaultSeparationDayResult> GetProductionFaultSeparationDayReports(
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<DateTime> dateTime = null)
    {


      var productionFaultTypes = App.Internals.Production.GetProductionFaultTypes(
       e => e);

      var stuffs = App.Internals.SaleManagement.GetStuffs(
                e => e);

      var repairProductionFaults = App.Internals.Production.GetRepairProductionFaults(
                e => e);


      if (fromDate != null)

        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime >= fromDate);

      if (toDate != null)
        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime <= toDate);

      var groupNew = (from repairProductionFault in repairProductionFaults
                      group repairProductionFault by
                            new
                            {
                              StuffId = repairProductionFault.RepairProduction.Production.StuffSerialStuffId,
                              ProductionFaultTypeId = repairProductionFault.ProductionFaultTypeId,
                              DateTime = repairProductionFault.DateTime.Date
                            }

        into newGroupResult

                      select new
                      {
                        StuffId = newGroupResult.Key.StuffId,
                        ProductionFaultTypeId = newGroupResult.Key.ProductionFaultTypeId,
                        ProductionFaultTypeTitle = newGroupResult.FirstOrDefault().ProductionFaultType.Title,
                        ProductionFaultTypeCount = newGroupResult.Count(),
                        DateTime = newGroupResult.Key.DateTime.Date
                      });


      var query = (
                from q in groupNew
                join stuff in stuffs on q.StuffId equals stuff.Id
                select new ProductionFaultSeparationDayResult()
                {
                  StuffId = q.StuffId,
                  StuffName = stuff.Name,
                  StuffCode = stuff.Code,
                  ProductionFaultTypeId = q.ProductionFaultTypeId,
                  ProductionFaultTypeTitle = q.ProductionFaultTypeTitle,
                  ProductionFaultTypeCount = q.ProductionFaultTypeCount,
                  DateTime = q.DateTime

                }
                );

      var x = query.ToArray();

      return query;
    }
    #endregion

    #region Gets ProductionFaultSeparationDayReports
    public IQueryable<GeneralQualityControlFaultReportResult> GetGeneralQualityControlFaultReports(
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<DateTime> dateTime = null)
    {


      var productionFaultTypes = App.Internals.Production.GetProductionFaultTypes(
                 e => e);

      var stuffs = App.Internals.SaleManagement.GetStuffs(
                e => e);

      var repairProductionFaults = App.Internals.Production.GetRepairProductionFaults(
                e => e);

      if (fromDate != null)

        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime >= fromDate);

      if (toDate != null)
        repairProductionFaults = repairProductionFaults
              .Where(i => i.DateTime <= toDate);

      var groupBy = (from repairProductionFault in repairProductionFaults
                     group repairProductionFault by
                           new
                           {
                             StuffId = repairProductionFault.RepairProduction.Production.StuffSerialStuffId,
                             ProductionFaultTypeId = repairProductionFault.ProductionFaultTypeId,
                           }
        into groupResult
                     select new
                     {
                       StuffId = groupResult.Key.StuffId,
                       ProductionFaultTypeId = groupResult.Key.ProductionFaultTypeId,
                       ProductionFaultTypeTitle = groupResult.FirstOrDefault().ProductionFaultType.Title,
                       ProductionFaultTypeCount = groupResult.Count()
                     });

      var query = (
                from q in groupBy
                join stuff in stuffs on q.StuffId equals stuff.Id
                select new GeneralQualityControlFaultReportResult()
                {
                  StuffId = q.StuffId,
                  StuffName = stuff.Name,
                  StuffCode = stuff.Code,
                  ProductionFaultTypeId = q.ProductionFaultTypeId,
                  ProductionFaultTypeTitle = q.ProductionFaultTypeTitle,
                  ProductionFaultTypeCount = q.ProductionFaultTypeCount,
                }
                );

      return query;
    }
    #endregion

  }
}
