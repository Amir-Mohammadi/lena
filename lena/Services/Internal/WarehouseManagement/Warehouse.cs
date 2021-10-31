using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Services.Internals.UserManagement;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.WarehouseManagement.SendProduct;
using lena.Models.WarehouseManagement.Warehouse;
using lena.Services.Core;
using lena.Models.UserManagement.SecurityAction;
using System.Collections.Generic;
using lena.Domains.Enums;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal Warehouse AddWarehouseProcess(
        string name,
        bool isActive,
        short departmentId,
        bool fifo,
        int? displayOrder,
        WarehouseType warehouseType,
        TValue<StoreReceiptType[]> storeReceiptTypes = null,
        TValue<int[]> exitReceiptTypeIds = null,
        TValue<TransactionLevel[]> transactionLevels = null,
        TValue<StuffType[]> stuffTypes = null)
    {
      var warehouse = AddWarehouse(
                name: name,
                isActive: isActive,
                departmentId: departmentId,
                fifo: fifo,
                displayOrder: displayOrder,
                warehouseType: warehouseType);
      var warehouseSecurityActions = Models.StaticData.WarehouseSecurityAction.WarehouseSecurityActions();
      foreach (var warehouseSecurityAction in warehouseSecurityActions)
      {
        AddWarehouseSecurityAction(warehouseSecurityAction, name);
      }
      if (storeReceiptTypes != null)
      {
        for (int i = 0; i < storeReceiptTypes.Value.Length; i++)
        {
          AddWarehouseStoreReceiptType(
                    warehouseId: warehouse.Id,
                    storeReceiptType: storeReceiptTypes.Value[i]);
        }
      }
      if (exitReceiptTypeIds != null)
      {
        for (int i = 0; i < exitReceiptTypeIds.Value.Length; i++)
        {
          AddWarehouseExitReceiptRequestType(
                    warehouseId: warehouse.Id,
                    exitReceiptTypeId: exitReceiptTypeIds.Value[i]);
        }
      }
      if (transactionLevels != null)
      {
        for (int i = 0; i < transactionLevels.Value.Length; i++)
        {
          AddWarehouseTransactionLevel(
                    warehouseId: warehouse.Id,
                    transactionLevel: transactionLevels.Value[i]);
        }
      }
      if (stuffTypes != null)
      {
        foreach (var stuffType in stuffTypes.Value)
        {
          AddWarehouseStuffType(
                    warehouseId: warehouse.Id,
                    stuffType: stuffType);
        }
      }
      return warehouse;
    }
    internal SecurityAction AddWarehouseSecurityAction(
        SecurityActionResult warehouseSecurityAction,
        string name
        )
    {
      var actionParameters = new List<AddActionParameterInput>().ToArray();
      var addSecurityAction = App.Internals.UserManagement.AddSecurityActionProcess(
            name: warehouseSecurityAction.Name + name,
            actionName: warehouseSecurityAction.ActionName,
            securityActionGroupId: warehouseSecurityAction.SecurityActionGroupId,
            addActionParameterInputs: actionParameters);
      return addSecurityAction;
    }
    internal Warehouse AddWarehouse(
        string name,
        bool isActive,
        short departmentId,
        bool fifo,
        int? displayOrder,
        WarehouseType warehouseType)
    {
      var warehouse = repository.Create<Warehouse>();
      warehouse.Name = name;
      warehouse.IsActive = isActive;
      warehouse.IsDeleted = false;
      warehouse.FIFO = fifo;
      warehouse.DepartmentId = departmentId;
      warehouse.DisplayOrder = displayOrder;
      warehouse.WarehouseType = warehouseType;
      repository.Add(warehouse);
      return warehouse;
    }
    #endregion
    #region Edit
    internal Warehouse EditWarehouseProcess(
        short id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null,
        TValue<short> departmentId = null,
        TValue<bool> isDeleted = null,
        TValue<bool> fifo = null,
        TValue<int?> displayOrder = null,
        TValue<WarehouseType> warehouseType = null,
        TValue<StoreReceiptType[]> storeReceiptTypes = null,
        TValue<int[]> exitReceiptTypeIds = null,
        TValue<TransactionLevel[]> transactionLevels = null,
        TValue<StuffType[]> stuffTypes = null,
        TValue<StoreReceiptType[]> deleteStoreReceiptTypes = null,
        TValue<int[]> deleteExitReceiptTypeIds = null,
        TValue<TransactionLevel[]> deleteTransactionLevels = null,
        TValue<StuffType[]> deleteStuffTypes = null)
    {
      var warehouse = GetWarehouse(id: id);
      warehouse = EditWarehouse(
                warehouse: warehouse,
                rowVersion: warehouse.RowVersion,
                name: name,
                isActive: isActive,
                departmentId: departmentId,
                isDeleted: isDeleted,
                fifo: fifo,
                displayOrder: displayOrder,
                warehouseType: warehouseType);
      #region Add New Items
      if (storeReceiptTypes != null)
      {
        for (int i = 0; i < storeReceiptTypes.Value.Length; i++)
        {
          AddWarehouseStoreReceiptType(
                    warehouseId: warehouse.Id,
                    storeReceiptType: storeReceiptTypes.Value[i]);
        }
      }
      if (exitReceiptTypeIds != null)
      {
        for (int i = 0; i < exitReceiptTypeIds.Value.Length; i++)
        {
          AddWarehouseExitReceiptRequestType(
                    warehouseId: warehouse.Id,
                    exitReceiptTypeId: exitReceiptTypeIds.Value[i]);
        }
      }
      if (transactionLevels != null)
      {
        for (int i = 0; i < transactionLevels.Value.Length; i++)
        {
          AddWarehouseTransactionLevel(
                    warehouseId: warehouse.Id,
                    transactionLevel: transactionLevels.Value[i]);
        }
      }
      if (stuffTypes != null)
      {
        foreach (var stuffType in stuffTypes.Value)
        {
          AddWarehouseStuffType(
                    warehouseId: warehouse.Id,
                    stuffType: stuffType);
        }
      }
      #endregion
      #region Delete old items
      if (deleteStoreReceiptTypes != null)
      {
        for (int i = 0; i < deleteStoreReceiptTypes.Value.Length; i++)
        {
          DeleteWarehouseStoreReceiptType(
                    warehouseId: warehouse.Id,
                    storeReceiptType: deleteStoreReceiptTypes.Value[i]);
        }
      }
      if (deleteExitReceiptTypeIds != null)
      {
        for (int i = 0; i < deleteExitReceiptTypeIds.Value.Length; i++)
        {
          DeleteWarehouseExitReceiptRequestType(
                    warehouseId: warehouse.Id,
                    exitReceiptRequestTypeId: deleteExitReceiptTypeIds.Value[i]);
        }
      }
      if (deleteTransactionLevels != null)
      {
        for (int i = 0; i < deleteTransactionLevels.Value.Length; i++)
        {
          DeleteWarehouseTransactionLevel(
                    warehouseId: warehouse.Id,
                    transactionLevel: deleteTransactionLevels.Value[i]);
        }
      }
      if (deleteStuffTypes != null)
      {
        foreach (var stuffType in deleteStuffTypes.Value)
        {
          DeleteWarehouseStuffType(
                    warehouseId: warehouse.Id,
                    stuffType: stuffType);
        }
      }
      #endregion
      return warehouse;
    }
    internal Warehouse EditWarehouse(
       short id,
       byte[] rowVersion,
       TValue<string> name = null,
       TValue<bool> isActive = null,
       TValue<short> departmentId = null,
       TValue<bool> isDeleted = null,
       TValue<bool> fifo = null,
       TValue<int?> displayOrder = null,
       TValue<WarehouseType> warehouseType = null)
    {
      var warehouse = GetWarehouse(id: id);
      return EditWarehouse(
               warehouse: warehouse,
               rowVersion: rowVersion,
               name: name,
               isActive: isActive,
               departmentId: departmentId,
               isDeleted: isDeleted,
               fifo: fifo,
               displayOrder: displayOrder,
               warehouseType: warehouseType);
    }
    internal Warehouse EditWarehouse(
       Warehouse warehouse,
       byte[] rowVersion,
       TValue<string> name = null,
       TValue<bool> isActive = null,
       TValue<short> departmentId = null,
       TValue<bool> isDeleted = null,
       TValue<bool> fifo = null,
       TValue<int?> displayOrder = null,
       TValue<WarehouseType> warehouseType = null)
    {
      if (name != null)
        warehouse.Name = name;
      if (departmentId != null)
        warehouse.DepartmentId = departmentId;
      if (isActive != null)
        warehouse.IsActive = isActive;
      if (isDeleted != null)
        warehouse.IsDeleted = isDeleted;
      if (fifo != null)
        warehouse.FIFO = fifo;
      if (displayOrder != null)
        warehouse.DisplayOrder = displayOrder;
      if (warehouseType != null)
        warehouse.WarehouseType = warehouseType;
      repository.Update(warehouse, rowVersion);
      return warehouse;
    }
    #endregion
    #region Delete
    internal void DeleteWarehouse(byte[] rowVersion, short id)
    {
      EditWarehouse(
                rowVersion: rowVersion,
                id: id,
                isDeleted: true);
    }
    #endregion
    #region Restore
    internal void RestoreWarehouse(byte[] rowVersion, short id)
    {
      EditWarehouse(rowVersion: rowVersion, id: id, isDeleted: false);
    }
    #endregion
    #region Get
    internal Warehouse GetWarehouse(short id) => GetWarehouse(e => e, id: id);
    internal TResult GetWarehouse<TResult>(
        Expression<Func<Warehouse, TResult>> selector,
        short id)
    {
      var result = GetWarehouses(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (result == null)
        throw new WarehouseNotFoundException(id);
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetWarehouses<TResult>(
        Expression<Func<Warehouse, TResult>> selector,
        TValue<short> id = null,
        TValue<string> name = null,
        TValue<int> departmentId = null,
        TValue<bool> isDeleted = null,
        TValue<bool> fifo = null,
        TValue<int?> displayOrder = null,
        TValue<WarehouseType> warehouseType = null)
    {
      var query = repository.GetQuery<Warehouse>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (isDeleted != null)
        query = query.Where(i => i.IsDeleted == isDeleted);
      if (fifo != null)
        query = query.Where(i => i.FIFO == fifo);
      if (displayOrder != null)
        query = query.Where(i => i.DisplayOrder == displayOrder);
      if (warehouseType != null)
        query = query.Where(i => i.WarehouseType.HasFlag(warehouseType));
      return query.Select(selector);
    }
    #endregion
    #region Sort
    internal IOrderedQueryable<WarehouseResult> SortWarehouseResult(
        IQueryable<WarehouseResult> query,
        SortInput<WarehouseSortType> options)
    {
      switch (options.SortType)
      {
        case WarehouseSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case WarehouseSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case WarehouseSortType.IsActive:
          return query.OrderBy(a => a.IsActive, options.SortOrder);
        case WarehouseSortType.IsDelete:
          return query.OrderBy(a => a.IsDelete, options.SortOrder);
        case WarehouseSortType.DepartmentName:
          return query.OrderBy(a => a.DepartmentName, options.SortOrder);
        case WarehouseSortType.FIFO:
          return query.OrderBy(a => a.FIFO, options.SortOrder);
        case WarehouseSortType.DisplayOrder:
          return query.OrderBy(a => a.DisplayOrder, options.SortOrder);
        case WarehouseSortType.WarehouseType:
          return query.OrderBy(a => a.WarehouseType, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SortCombo
    internal IOrderedQueryable<WarehouseComboResult> SortWarehouseComboResult(
        IQueryable<WarehouseComboResult> query,
        SortInput<WarehouseSortType> options)
    {
      switch (options.SortType)
      {
        case WarehouseSortType.Id:
          return query.OrderBy(a => a.Id, options.SortOrder);
        case WarehouseSortType.Name:
          return query.OrderBy(a => a.Name, options.SortOrder);
        case WarehouseSortType.DisplayOrder:
          return query.OrderBy(a => a.DisplayOrder, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToComboResult
    internal Expression<Func<Warehouse, WarehouseComboResult>> ToWarehouseComboResult =
        warehouse => new WarehouseComboResult
        {
          Id = warehouse.Id,
          Name = warehouse.Name,
          DisplayOrder = warehouse.DisplayOrder
        };
    #endregion
    #region ToResult
    internal Expression<Func<Warehouse, WarehouseResult>> ToWarehouseResult =
        warehouse => new WarehouseResult()
        {
          Id = warehouse.Id,
          Name = warehouse.Name,
          IsActive = warehouse.IsActive,
          IsDelete = warehouse.IsDeleted,
          DepartmentId = warehouse.DepartmentId,
          DepartmentName = warehouse.Department.Name,
          FIFO = warehouse.FIFO,
          DisplayOrder = warehouse.DisplayOrder,
          StoreReceiptTypes = warehouse.WarehouseStoreReceiptTypes.AsQueryable().Select(x => x.StoreReceiptType),
          TransactionLevels = warehouse.WarehouseTransactionLevels.AsQueryable().Select(x => x.TransactionLevel),
          StuffTypes = warehouse.WarehouseStuffTypes.AsQueryable().Select(x => x.StuffType),
          ExitReciptRequestType = warehouse.WarehouseExitReceiptTypes.AsQueryable().Select(x => x.ExitReceiptRequestTypeId),
          WarehouseType = warehouse.WarehouseType,
          RowVersion = warehouse.RowVersion
        };
    #endregion
    #region Search
    internal IQueryable<WarehouseResult> SearchWarehouseResultQuery(
        IQueryable<WarehouseResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                    item.Name.Contains(searchText) ||
                    item.DepartmentName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    internal void CheckWarehouseType(
        TValue<int> warehouseId = null,
        TValue<WarehouseType> warehouseType = null)
    {
      var query = repository.GetQuery<Warehouse>();
      if (warehouseId != null)
        query = query.Where(i => i.Id == warehouseId);
      if (warehouseType != null)
        query = query.Where(i => i.WarehouseType.HasFlag(warehouseType));
      if (!query.Any())
      {
        throw new WarehouseTypeException(warehouseId);
      }
    }
  }
}