using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Models.Production.RepairProductionFault;
using lena.Models.Production.ProductionStuffDetail;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public RepairProductionFault GetRepairProductionFault(int id) => GetRepairProductionFault(selector: e => e, id: id);
    public TResult GetRepairProductionFault<TResult>(
        Expression<Func<RepairProductionFault, TResult>> selector,
        int id)
    {

      var repairProductionFault = GetRepairProductionFaults(
                   selector: selector,
                   id: id)


               .FirstOrDefault();
      if (repairProductionFault == null)
        throw new RepairProductionFaultNotFoundException(id);
      return repairProductionFault;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetRepairProductionFaults<TResult>(
            Expression<Func<RepairProductionFault, TResult>> selector,
            TValue<int> id = null,
            TValue<string> code = null,
            TValue<bool> isDelete = null,
            TValue<int> userId = null,
            TValue<int> transactionBatchId = null,
            TValue<string> description = null,
            TValue<int> productionFaultTypeId = null,
            TValue<int> repairProductionId = null
            )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                            selector: e => e,
                            id: id,
                            code: code,
                            description: description,
                            isDelete: isDelete,
                            transactionBatchId: transactionBatchId,
                            userId: userId);
      var query = baseQuery.OfType<RepairProductionFault>();
      if (repairProductionId != null) query = query.Where(x => x.RepairProductionId == repairProductionId);
      if (productionFaultTypeId != null) query = query.Where(x => x.ProductionFaultTypeId == productionFaultTypeId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public RepairProductionFault AddRepairProductionFault(
                TransactionBatch transactionBatch,
                string description,
                int productionFaultTypeId,
                int repairProductionId
                )
    {

      var repairProductionFault = repository.Create<RepairProductionFault>();
      repairProductionFault.ProductionFaultTypeId = productionFaultTypeId;
      repairProductionFault.RepairProductionId = repairProductionId;



      App.Internals.ApplicationBase.AddBaseEntity(
                transactionBatch: transactionBatch,
                baseEntity: repairProductionFault,
                description: description
                );
      return repairProductionFault;
    }
    #endregion
    #region Edit
    public RepairProductionFault EditRepairProductionFault(
        int id,
        byte[] rowVersion,
        TValue<string> code = null,
        TValue<bool> isDelete = null,
        TValue<int> userId = null,
        TValue<int> transactionBatchId = null,
        TValue<string> description = null,
        TValue<int> productionFaultTypeId = null,
        TValue<int> repairProductionId = null
        )
    {

      var repairProductionFault = GetRepairProductionFault(id: id);
      return EditRepairProductionFault(
                 repairProductionFault: repairProductionFault,
                rowVersion: rowVersion,
                             code: code,
                 description: description,
                 isDelete: isDelete,
                 userId: userId,
                 productionFaultTypeId: productionFaultTypeId,
                 repairProductionId: repairProductionId,
                 transactionBatchId: transactionBatchId

                );

    }

    public RepairProductionFault EditRepairProductionFault(
                RepairProductionFault repairProductionFault,
                byte[] rowVersion,
                TValue<string> code = null,
                TValue<bool> isDelete = null,
                TValue<int> userId = null,
                TValue<int> transactionBatchId = null,
                TValue<string> description = null,
                TValue<int> productionFaultTypeId = null,
                TValue<int> repairProductionId = null
                )
    {


      if (productionFaultTypeId != null) repairProductionFault.ProductionFaultTypeId = productionFaultTypeId;
      if (repairProductionId != null) repairProductionFault.RepairProductionId = repairProductionId;

      App.Internals.ApplicationBase.EditBaseEntity(
                  baseEntity: repairProductionFault,
                  rowVersion: rowVersion,
                  isDelete: isDelete,
                  description: description);
      return repairProductionFault;
    }

    #endregion
    #region Delete
    public void DeleteRepairProductionFault(int id)
    {

      var repairProductionFault = GetRepairProductionFault(id: id);
      repository.Delete(repairProductionFault);
    }
    #endregion


    #region ToResult

    public Expression<Func<RepairProductionFault, RepairProductionFaultResult>> ToRepairProductionFaultResult =
        entity => new RepairProductionFaultResult()
        {
          Id = entity.Id,
          RepairProductionId = entity.RepairProductionId,
          ProductionFaultTypeTitle = entity.ProductionFaultType.Title,
          ProductionFaultTypeId = entity.ProductionFaultTypeId,
          RepairProductionStuffDetails = entity.RepairProductionStuffDetails.AsQueryable().Select(App.Internals.Production.ToRepairProductionStuffDetailResult),
          RowVersion = entity.RowVersion
        };


    public Expression<Func<RepairProductionStuffDetail, ProductionStuffDetailResult>> ToRepairProductionStuffDetailResult =
        entity => new ProductionStuffDetailResult()
        {
          Id = entity.Id,
          ProductionId = entity.ProductionId,
          ProductionOperationId = entity.ProductionOperationId,
          ProductionOperationName = entity.ProductionOperation.Operation.Title,
          BillOfMaterialDetailType = entity.BillOfMaterialDetailType,
          StuffId = entity.StuffId,
          StuffSerialCode = entity.StuffSerialCode,
          Qty = entity.Qty,
          UnitId = entity.UnitId,
          WarehouseId = entity.WarehouseId,
          Type = entity.Type,
          DetachedQty = entity.DetachedQty,
          StuffName = entity.Stuff.Name,
          StuffCode = entity.Stuff.Code,
          UnitName = entity.Unit.Name,
          WarehouseName = entity.Warehouse.Name,
          Serial = entity.StuffSerial.Serial,
          RowVersion = entity.RowVersion
        };



    #endregion


  }
}
