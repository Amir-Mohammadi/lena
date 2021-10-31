using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
//using Parlar.DAL.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.EquivalentStuffDetail;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<EquivalentStuffDetail> GetEquivalentStuffDetails(
        TValue<int> id = null,
        TValue<int> equivalentStuffId = null,
        TValue<int> stuffId = null,
        TValue<int?> semiProductBillOfMaterialVersion = null,
        TValue<double> value = null,
        TValue<int> unitId = null
        )
    {

      var equivalentStuffDetail = repository.GetQuery<EquivalentStuffDetail>();

      if (id != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.Id == id);
      if (equivalentStuffId != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.EquivalentStuffId == equivalentStuffId);
      if (stuffId != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.StuffId == stuffId);
      if (semiProductBillOfMaterialVersion != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.SemiProductBillOfMaterialVersion == semiProductBillOfMaterialVersion);
      if (value != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.Value == value);
      if (unitId != null)
        equivalentStuffDetail = equivalentStuffDetail.Where(i => i.UnitId == unitId);

      return equivalentStuffDetail;
    }


    public EquivalentStuffDetail GetEquivalentStuffDetail(int id)
    {


      var equivalentStuffDetail = GetEquivalentStuffDetails(id: id).SingleOrDefault();
      if (equivalentStuffDetail == null)
        throw new EquivalentStuffDetailNotFoundException(id: id);
      return equivalentStuffDetail;
    }

    public EquivalentStuffDetail AddEquivalentStuffDetail(
        int equivalentStuffId,
        int stuffId,
        short? semiProductBillOfMaterialVersion,
        double forQty,
        double value,
        byte unitId)
    {

      var equivalentStuffDetail = repository.Create<EquivalentStuffDetail>();
      equivalentStuffDetail.EquivalentStuffId = equivalentStuffId;
      equivalentStuffDetail.StuffId = stuffId;
      equivalentStuffDetail.SemiProductBillOfMaterialVersion = semiProductBillOfMaterialVersion;
      equivalentStuffDetail.Value = value;
      equivalentStuffDetail.ForQty = forQty;
      equivalentStuffDetail.UnitId = unitId;
      repository.Add(equivalentStuffDetail);
      return equivalentStuffDetail;
    }
    public EquivalentStuffDetail EditEquivalentStuffDetail(
        byte[] rowVersion,
        int id,
        TValue<byte> unitId = null,
        TValue<int> stuffId = null,
        TValue<double> value = null,
        TValue<double> forQty = null,
        TValue<int> equivalentStuffId = null,
        TValue<short?> semiProductBillOfMaterialVersion = null)
    {

      var equivalentStuffDetail = GetEquivalentStuffDetail(id: id);
      equivalentStuffDetail.EquivalentStuffId = equivalentStuffId;
      equivalentStuffDetail.StuffId = stuffId;
      equivalentStuffDetail.SemiProductBillOfMaterialVersion = semiProductBillOfMaterialVersion;
      equivalentStuffDetail.Value = value;
      equivalentStuffDetail.ForQty = forQty;
      equivalentStuffDetail.UnitId = unitId;
      repository.Update(entity: equivalentStuffDetail, rowVersion: rowVersion);
      return equivalentStuffDetail;
    }
    public void DeleteEquivalentStuffDetail(int id)
    {

      var equivalentStuffDetail = GetEquivalentStuffDetail(id: id);
      repository.Delete(equivalentStuffDetail);
    }

    public EquivalentStuffDetailResult ToEquivalentStuffDetailResult(EquivalentStuffDetail i)
    {
      return new EquivalentStuffDetailResult
      {
        StuffId = i.StuffId,
        StuffName = i.Stuff.Name,
        Value = i.Value,
        UnitId = i.UnitId,
        UnitName = i.Unit.Name,
        BillOfMaterialVersion = i.SemiProductBillOfMaterialVersion,
        RowVersion = i.RowVersion
      };
    }
    public FullEquivalentStuffDetailResult ToFullEquivalentStuffDetailResult(EquivalentStuffDetail i)
    {
      return new FullEquivalentStuffDetailResult
      {
        Id = i.Id,
        StuffId = i.StuffId,
        StuffCode = i.Stuff.Code,
        StuffName = i.Stuff.Name,
        Value = i.Value,
        UnitId = i.UnitId,
        UnitName = i.Unit.Name,
        ForQty = i.ForQty,
        SemiProductBillOfMaterialVersion = i.SemiProductBillOfMaterialVersion,
        BillOfMaterials = ToBillOfMaterialComboResult(GetBillOfMaterials(stuffId: i.StuffId)).ToArray(),
        Units = App.Internals.WarehouseManagement.GetStuffUnits(stuffId: i.StuffId).ToArray(),
        RowVersion = i.RowVersion
      };
    }

    public IQueryable<EquivalentStuffDetailResult> ToEquivalentStuffDetailResultQuery(
        IQueryable<EquivalentStuffDetail> query)
    {
      return from item in query
             let unit = item.Unit
             let stuff = item.Stuff
             select new EquivalentStuffDetailResult
             {
               EquivalentStuffDetailId = item.Id,
               StuffId = item.StuffId,
               StuffCode = item.Stuff.Code,
               StuffName = stuff.Name,
               Value = item.Value,
               UnitId = item.UnitId,
               UnitName = unit.Name,
               BillOfMaterialVersion = item.SemiProductBillOfMaterialVersion,
               RowVersion = item.RowVersion
             };
    }

    public IQueryable<EquivalentStuffDetailResult> SortEquivalentStuffDetails(
        IQueryable<EquivalentStuffDetailResult> query, SortInput<EquivalentStuffDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case EquivalentStuffDetailSortType.StuffId:
          query = query.OrderBy(x => x.StuffId, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.StuffName:
          query = query.OrderBy(x => x.StuffName, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.Value:
          query = query.OrderBy(x => x.Value, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.UnitId:
          query = query.OrderBy(x => x.UnitId, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.UnitName:
          query = query.OrderBy(x => x.UnitName, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.BillOfMaterialVersion:
          query = query.OrderBy(x => x.BillOfMaterialVersion, sort.SortOrder);
          break;
        case EquivalentStuffDetailSortType.EquivalentStuffDetailId:
          query = query.OrderBy(x => x.EquivalentStuffDetailId, sort.SortOrder);
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      return query;
    }
  }
}
