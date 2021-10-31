using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Services.Internals.Production.Exception;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.WarehouseManagement.SerialBuffer;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public RepairProductionStuffDetail GetRepairProductionStuffDetail(int id) => GetRepairProductionStuffDetail(selector: e => e, id: id);
    public TResult GetRepairProductionStuffDetail<TResult>(
        Expression<Func<RepairProductionStuffDetail, TResult>> selector,
        int id)
    {

      var repairProductionStuffDetail = GetRepairProductionStuffDetails(
                   selector: selector,
                   id: id)


               .FirstOrDefault();
      if (repairProductionStuffDetail == null)
        throw new RepairProductionStuffDetailNotFoundException(id);
      return repairProductionStuffDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRepairProductionStuffDetails<TResult>(
            Expression<Func<RepairProductionStuffDetail, TResult>> selector,
            TValue<int> id = null,
            TValue<int> productionId = null,
            TValue<int> productionOperationId = null,
            TValue<BillOfMaterialDetailType> billOfMaterialType = null,
            TValue<ProductionStuffDetailType> productionStuffDetailType = null,
            TValue<int> stuffId = null,
            TValue<long?> stuffSerialCode = null,
            TValue<double> qty = null,
            TValue<int> unitId = null,
            TValue<string> serial = null,
            TValue<string> productionSerial = null,
            TValue<int> repairProductoinFaultId = null,
            TValue<RepairProductionStuffDetailType> repairProductionStuffDetailType = null,
            TValue<DateTime> fromDate = null,
            TValue<DateTime> toDate = null

            )
    {

      var baseQuery = GetProductionStuffDetails(
                id: id,
                billOfMaterialDetailType: billOfMaterialType,
                productionId: productionId,
                productionOperationId: productionOperationId,
                productionSerial: productionSerial,
                productionStuffDetailType: productionStuffDetailType,
                qty: qty,
                unitId: unitId,
                serial: serial,
                stuffId: stuffId,
                stuffSerialCode: stuffSerialCode
                );
      var query = baseQuery.OfType<RepairProductionStuffDetail>();
      if (repairProductoinFaultId != null) query = query.Where(x => x.RepairProductionFaultId == repairProductoinFaultId);
      if (fromDate != null) query = query.Where(x => x.RepairProductionFault.DateTime >= fromDate);
      if (toDate != null) query = query.Where(x => x.RepairProductionFault.DateTime <= toDate);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public RepairProductionStuffDetail AddRepairProductionStuffDetailProcess(
                lena.Domains.Production production,
                ProductionOperation productionOperation,
                int stuffId,
                double qty,
                byte unitId,
                short warehouseId,
                int repairProductoinFaultId,
                int? parentProductionOperationId,
                short? billOfMaterialVersion,
                double unitConversionRatio,
                TransactionBatch transactionBatch,
                SerialBufferMinResult consumeSerialBufferResult,
                SerialBufferMinResult produceSerialBufferResult,
                SerialBuffer consumeSerialBuffer,
                SerialBuffer produceSerialBuffer,
                ProductionStuffDetailType productionStuffDetailType
                )
    {


      var repairProductionStuffDetail = repository.Create<RepairProductionStuffDetail>();
      repairProductionStuffDetail.RepairProductionFaultId = repairProductoinFaultId;
      AddProductionStuffDetailProcess(
                productionStuffDetailConcrete: repairProductionStuffDetail,
                production: production,
                productionOperation: productionOperation,
                parentProductionOperationId: parentProductionOperationId,
                productionStuffDetailType: productionStuffDetailType,
                qty: qty,
                stuffId: stuffId,
                consumeSerialBufferResult: consumeSerialBufferResult,
                consumeSerialBuffer: consumeSerialBuffer,
                produceSerialBufferResult: produceSerialBufferResult,
                produceSerialBuffer: produceSerialBuffer,
                transactionBatch: transactionBatch,
                billOfMaterialVersion: billOfMaterialVersion,
                unitConversionRatio: unitConversionRatio,
                unitId: unitId,
                warehouseId: warehouseId,
                produceSerialBufferRowVersion: produceSerialBufferResult?.RowVersion,
                consumeSerialBufferRowVersion: consumeSerialBufferResult?.RowVersion
                );
      return repairProductionStuffDetail;
    }
    #endregion
    #region Edit
    public RepairProductionStuffDetail EditRepairProductionStuffDetail(
        int id,
        byte[] rowVersion,
            TValue<int> productionId = null,
            TValue<int> productionOperationId = null,
            TValue<BillOfMaterialDetailType> billOfMaterialType = null,
            TValue<ProductionStuffDetailType> productionStuffDetailType = null,
            TValue<int> stuffId = null,
            TValue<int?> stuffSerialCode = null,
            TValue<double> qty = null,
            TValue<byte> unitId = null,
            TValue<string> serial = null,
            TValue<string> productionSerial = null,
            TValue<int> repairProductoinFaultId = null,
            TValue<RepairProductionStuffDetailType> repairProductionStuffDetailType = null
        )
    {

      var repairProductionStuffDetail = GetRepairProductionStuffDetail(id: id);
      return EditRepairProductionStuffDetail(
                  repairProductionStuffDetail: repairProductionStuffDetail,
                rowVersion: rowVersion,
                billOfMaterialType: billOfMaterialType,
                productionId: productionId,
                productionOperationId: productionOperationId,
                productionSerial: productionSerial,
                productionStuffDetailType: productionStuffDetailType,
                qty: qty,
                unitId: unitId,
                serial: serial,
                stuffId: stuffId,
                stuffSerialCode: stuffSerialCode);

    }

    public RepairProductionStuffDetail EditRepairProductionStuffDetail(
                RepairProductionStuffDetail
      repairProductionStuffDetail,
                byte[] rowVersion,
                TValue<int> productionId = null,
                TValue<int> productionOperationId = null,
                TValue<BillOfMaterialDetailType> billOfMaterialType = null,
                TValue<ProductionStuffDetailType> productionStuffDetailType = null,
                TValue<int> stuffId = null,
                TValue<int?> stuffSerialCode = null,
                TValue<double> qty = null,
                TValue<byte> unitId = null,
                TValue<string> serial = null,
                TValue<string> productionSerial = null,
                TValue<int> repairProductoinFaultId = null,
                TValue<RepairProductionStuffDetailType> repairProductionStuffDetailType = null)
    {



      if (repairProductoinFaultId != null) repairProductionStuffDetail.RepairProductionFaultId = repairProductoinFaultId;
      EditProductionStuffDetail(productionStuffDetail: repairProductionStuffDetail,
                rowVersion: rowVersion,
                     billOfMaterialDetailType: billOfMaterialType,
                productionId: productionId,
                productionOperationId: productionOperationId,
                productionStuffDetailType: productionStuffDetailType,
                qty: qty,
                unitId: unitId,
                stuffId: stuffId,
                stuffSerialCode: stuffSerialCode);
      return repairProductionStuffDetail;
    }

    #endregion
    #region Delete
    public void DeleteRepairProductionStuffDetail(int id)
    {

      var repairProductionStuffDetail = GetRepairProductionStuffDetail(id: id);
      repository.Delete(repairProductionStuffDetail);
    }
    #endregion
    #region Sort
    //public IOrderedQueryable<RepairProductionStuffDetailResult> SortRepairProductionStuffDetailResult(
    //    IQueryable<RepairProductionStuffDetailResult> query,
    //    SortInput<RepairProductionStuffDetailSortType> sort)
    //{
    //    switch (sort.SortType)
    //    {
    //        case RepairProductionStuffDetailSortType.: return query.OrderBy(a => a., sort.SortOrder);
    //        default:
    //            throw new ArgumentOutOfRangeException();
    //    }
    //}
    #endregion
    #region Search
    //        public IQueryable<RepairProductionStuffDetailResult> SearchRepairProductionStuffDetailResult(
    //            IQueryable<RepairProductionStuffDetailResult> query,
    //            string searchText)
    //        {
    //            if (!string.IsNullOrEmpty(searchText))
    //                query = from item in query
    //                        where item.Title.Contains(searchText) ||
    //                        select item;
    //            return query;
    //        }
    //        #endregion
    //        #region ToResult
    //        public Expression<Func<RepairProductionStuffDetail, RepairProductionStuffDetailResult>> ToRepairProductionStuffDetailResult =
    //                      repairProductionStuffDetail => new RepairProductionStuffDetail Result
    //{
    //                =   repairProductionStuffDetail.,

    //                RowVersion = repairProductionStuffDetail.RowVersion
    //    };
    #endregion

  }
}
