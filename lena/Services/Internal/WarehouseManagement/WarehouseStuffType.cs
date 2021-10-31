//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System.Linq;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public void AddWarehouseStuffType(
        short warehouseId,
        StuffType stuffType)
    {

      var warehouseStuffType = repository.Create<WarehouseStuffType>();
      warehouseStuffType.WarehouseId = warehouseId;
      warehouseStuffType.StuffType = stuffType;
      repository.Add(warehouseStuffType);
    }
    #endregion
    #region Get
    internal WarehouseStuffType GetWarehouseStuffType(
            short warehouseId,
            StuffType stuffType)
    {

      var result = GetWarehouseStuffTypes(
                    warehouseId: warehouseId,
                    stuffType: stuffType)


                .FirstOrDefault();
      if (result == null)
        throw new WarehouseStuffTypeNotFoundException();
      return result;
    }
    #endregion
    #region Gets
    internal IQueryable<WarehouseStuffType> GetWarehouseStuffTypes(
        TValue<short> warehouseId = null,
        TValue<StuffType> stuffType = null)
    {

      var query = repository.GetQuery<WarehouseStuffType>();
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (stuffType != null)
        query = query.Where(i => i.StuffType == stuffType);
      return query;
    }
    #endregion
    #region Delete
    internal void DeleteWarehouseStuffType(
            short warehouseId,
            StuffType stuffType)
    {

      var entity = GetWarehouseStuffType(
                    warehouseId: warehouseId,
                    stuffType: stuffType);
      repository.Delete(entity);
    }
    #endregion
    internal void CheckWarehouseStuffType(
       TValue<short> warehouseId = null,
       TValue<StuffType> stuffType = null)
    {

      var result = GetWarehouseStuffTypes(
                warehouseId: warehouseId,
                stuffType: stuffType);
      if (!result.Any())
      {
        var warehouse = GetWarehouse(id: warehouseId);
        throw new WarehouseStuffTypeException(warehouse.Name);
      }
    }
  }
}