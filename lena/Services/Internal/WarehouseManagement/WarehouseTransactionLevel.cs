//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Add
    public void AddWarehouseTransactionLevel(
        short warehouseId,
        TransactionLevel transactionLevel)
    {



      var warehouseTransactionLevel = repository.Create<WarehouseTransactionLevel>();

      warehouseTransactionLevel.WareHouserId = warehouseId;
      warehouseTransactionLevel.TransactionLevel = transactionLevel;
      repository.Add(warehouseTransactionLevel);
    }
    #endregion

    #region Get
    internal WarehouseTransactionLevel GetWarehouseTransactionLevel(
          short warehouseId,
          TransactionLevel transactionLevel)
    {

      var result = GetWarehouseTransactionLevels(
                    warehouseId: warehouseId,
                    transactionLevel: transactionLevel)


                .FirstOrDefault();
      if (result == null)
        throw new WarehouseTransactionLevelNotFoundException();
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<WarehouseTransactionLevel> GetWarehouseTransactionLevels(
        TValue<short> warehouseId = null,
        TValue<TransactionLevel> transactionLevel = null)
    {

      var query = repository.GetQuery<WarehouseTransactionLevel>();
      if (warehouseId != null)
        query = query.Where(i => i.WareHouserId == warehouseId);
      if (transactionLevel != null)
        query = query.Where(i => i.TransactionLevel == transactionLevel);
      return query;
    }
    #endregion
    #region Delete
    internal void DeleteWarehouseTransactionLevel(
            short warehouseId,
            TransactionLevel transactionLevel)
    {

      var result = GetWarehouseTransactionLevel(
                    warehouseId: warehouseId,
                    transactionLevel: transactionLevel);
      repository.Delete(result);
    }
    #endregion

    internal void CheckWarehouseTransactionLevel(
       TValue<short> warehouseId = null,
       TValue<TransactionLevel> transactionLevel = null)
    {

      var result = GetWarehouseTransactionLevels(warehouseId: warehouseId, transactionLevel: transactionLevel);

      if (!result.Any())
      {
        var ThisWarehouse = GetWarehouse(warehouseId);
        throw new WarehouseTransactionLevelException(ThisWarehouse.Name);
      }
    }
  }
}
