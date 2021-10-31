//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
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
    public void AddWarehouseExitReceiptRequestType(
        short warehouseId,
        int exitReceiptTypeId)
    {



      var warehouseExitReceiptType = repository.Create<WarehouseExitReceiptType>();

      warehouseExitReceiptType.WarehouseId = warehouseId;
      warehouseExitReceiptType.ExitReceiptRequestTypeId = exitReceiptTypeId;
      repository.Add(warehouseExitReceiptType);
    }
    #endregion

    #region Get
    internal WarehouseExitReceiptType GetWarehouseExitReceiptRequestType(
          short warehouseId,
          int exitReceiptRequestTypeId)
    {

      var result = GetWarehouseExitReceiptRequestTypes(
                    warehouseId: warehouseId,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId)


                .FirstOrDefault();
      if (result == null)
        throw new WarehouseExitReceiptTypeNotFoundException();
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<WarehouseExitReceiptType> GetWarehouseExitReceiptRequestTypes(
        TValue<int> warehouseId = null,
        TValue<int> exitReceiptRequestTypeId = null)
    {

      var query = repository.GetQuery<WarehouseExitReceiptType>();
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (exitReceiptRequestTypeId != null)
        query = query.Where(i => i.ExitReceiptRequestTypeId == exitReceiptRequestTypeId);
      return query;
    }
    #endregion
    #region Delete
    internal void DeleteWarehouseExitReceiptRequestType(
            short warehouseId,
            int exitReceiptRequestTypeId)
    {

      var result = GetWarehouseExitReceiptRequestType(
                    warehouseId: warehouseId,
                    exitReceiptRequestTypeId: exitReceiptRequestTypeId);
      repository.Delete(result);
    }
    #endregion

    internal void CheckWarehouseExitReceiptRequestType(
        TValue<int> warehouseId = null,
        TValue<int> exitReceiptRequestTypeId = null)
    {

      var result = GetWarehouseExitReceiptRequestTypes(warehouseId: warehouseId, exitReceiptRequestTypeId: exitReceiptRequestTypeId);

      if (!result.Any())
      {
        throw new WarehouseExitReceiptRequestTypeException(warehouseId);
      }
    }

  }
}
