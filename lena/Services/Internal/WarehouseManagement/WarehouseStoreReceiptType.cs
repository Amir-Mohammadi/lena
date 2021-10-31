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
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public void AddWarehouseStoreReceiptType(
        short warehouseId,
        StoreReceiptType storeReceiptType)
    {



      var warehouseStoreReceiptType = repository.Create<WarehouseStoreReceiptType>();
      warehouseStoreReceiptType.WarehouseId = warehouseId;
      warehouseStoreReceiptType.StoreReceiptType = storeReceiptType;
      repository.Add(warehouseStoreReceiptType);
    }
    #endregion
    #region Get
    internal WarehouseStoreReceiptType GetWarehouseStoreReceiptType(
          short warehouseId,
          StoreReceiptType storeReceiptType)
    {

      var result = GetWarehouseStoreReceiptTypes(
                    warehouseId: warehouseId,
                    storeReceiptType: storeReceiptType)


                .FirstOrDefault();
      if (result == null)
        throw new WarehouseStoreReceiptTypeNotFoundException();
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<WarehouseStoreReceiptType> GetWarehouseStoreReceiptTypes(
        TValue<int> warehouseId = null,
        TValue<StoreReceiptType> storeReceiptType = null)
    {

      var query = repository.GetQuery<WarehouseStoreReceiptType>();
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (storeReceiptType != null)
        query = query.Where(i => i.StoreReceiptType == storeReceiptType);
      return query;
    }
    #endregion
    #region Delete
    internal void DeleteWarehouseStoreReceiptType(
            short warehouseId,
            StoreReceiptType storeReceiptType)
    {

      var result = GetWarehouseStoreReceiptType(
                    warehouseId: warehouseId,
                    storeReceiptType: storeReceiptType);
      repository.Delete(result);
    }
    #endregion
    internal void CheckWarehouseStoreReceiptType(
        TValue<int> warehouseId = null,
        TValue<StoreReceiptType> storeReceiptType = null)
    {

      var result = GetWarehouseStoreReceiptTypes(
                warehouseId: warehouseId,
                storeReceiptType: storeReceiptType);
      if (!result.Any())
      {
        throw new WarehouseStoreReceiptTypeException(warehouseId);
      }
    }
  }
}