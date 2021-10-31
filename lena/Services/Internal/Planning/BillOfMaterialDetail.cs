using System;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Planning.BillOfMaterialDetail;
using lena.Models.Planning.EquivalentStuff;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<BillOfMaterialDetail> GetBillOfMaterialDetails(
        TValue<int> id = null,
        TValue<int> index = null,
        TValue<int?> semiProductBillOfMaterialVersion = null,
        TValue<string> description = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<int> billOfMaterialVersion = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<int> stuffId = null,
        TValue<bool> reservable = null,
        TValue<int> unitId = null,
        TValue<double> value = null)
    {

      var query = repository.GetQuery<BillOfMaterialDetail>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (index != null)
        query = query.Where(i => i.Index == index);
      if (semiProductBillOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterial.Version == billOfMaterialVersion);
      if (description != null)
        query = query.Where(i => i.BillOfMaterial.Description == description);
      if (billOfMaterialDetailType != null)
        query = query.Where(i => i.BillOfMaterialDetailType == billOfMaterialDetailType);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterial.Version == billOfMaterialVersion);
      if (billOfMaterialStuffId != null)
        query = query.Where(i => i.BillOfMaterial.StuffId == billOfMaterialStuffId);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (reservable != null)
        query = query.Where(i => i.Reservable == reservable);
      if (unitId != null)
        query = query.Where(i => i.UnitId == unitId);
      if (value != null)
        query = query.Where(i => i.Value == value);
      return query;
    }
    public BillOfMaterialDetail GetBillOfMaterialDetail(int id)
    {

      var billOfMaterialDetail = GetBillOfMaterialDetails(id: id).SingleOrDefault();
      if (billOfMaterialDetail == null)
        throw new BillOfMaterialDetailNotFoundException(id);
      return billOfMaterialDetail;
    }
    public IQueryable<BillOfMaterialDetail> GetBillOfMaterialDetailsByStuff(int stuffId, int versionId)
    {

      var billOfMaterialDetail = GetBillOfMaterialDetails(billOfMaterialStuffId: stuffId, billOfMaterialVersion: versionId);
      if (billOfMaterialDetail == null)
        throw new BillOfMaterialDetailNotFoundException(0, stuffId, versionId);
      return billOfMaterialDetail;
    }

    public IQueryable<BillOfMaterialDetailMiniResult> GetBillOfMaterialDetailsRecursive(int stuffId, int version, bool beRecursive)
    {

      var query = GetBillOfMaterialDetails(
                    billOfMaterialStuffId: stuffId,
                    billOfMaterialVersion: version,
                    billOfMaterialDetailType: BillOfMaterialDetailType.Material);

      var billOfMaterialDetails = (from item in query
                                   select new BillOfMaterialDetailMiniResult
                                   {
                                     Id = item.Id,
                                     StuffId = item.StuffId,
                                     Version = item.SemiProductBillOfMaterialVersion,
                                     BillOfMaterialDetailType = item.BillOfMaterialDetailType,
                                     Value = item.Value,
                                     UnitConversionRatio = item.Unit.ConversionRatio,
                                     StuffType = item.Stuff.StuffType
                                   }).ToList();

      if (beRecursive)
      {
        var semiProducts = billOfMaterialDetails
                  .Where(i =>
                  i.StuffType == StuffType.Product ||
                  i.StuffType == StuffType.ProductPack ||
                  i.StuffType == StuffType.SemiProduct
                  ).ToList();


        foreach (var semiProduct in semiProducts)
        {
          if (semiProduct.Version == null)
          {
            var allVersions = GetBillOfMaterials(semiProduct.StuffId);

            var publishedVersion = allVersions.FirstOrDefault(v => v.IsPublished);

            if (publishedVersion != null)
              semiProduct.Version = publishedVersion.Version;
            else
            {
              var firstItem = allVersions.FirstOrDefault();
              if (firstItem != null)
                semiProduct.Version = firstItem.Version;
              else
                throw new BillOfMaterialNotFoundException(semiProduct.StuffId, 1);
            }
          }

          var details = GetBillOfMaterialDetailsRecursive(
                        beRecursive: beRecursive,
                        stuffId: semiProduct.StuffId,
                        version: semiProduct.Version.Value)


                    .ToList();
          billOfMaterialDetails.AddRange(details);
        }
      }

      return billOfMaterialDetails.AsQueryable();
    }


    public BillOfMaterialDetail AddBillOfMaterialDetail(
        int index,
        int stuffId,
        short? semiProductBillOfMaterialVersion,
        BillOfMaterialDetailType billOfMaterialDetailType,
        string description,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        bool reservable,
        byte unitId,
        double value,
        double forQty,
        bool isPackingMaterial)
    {

      var billOfMaterialDetail = repository.Create<BillOfMaterialDetail>();
      billOfMaterialDetail.Index = index;
      billOfMaterialDetail.StuffId = stuffId;
      billOfMaterialDetail.SemiProductBillOfMaterialVersion = semiProductBillOfMaterialVersion;
      billOfMaterialDetail.BillOfMaterialDetailType = billOfMaterialDetailType;
      billOfMaterialDetail.Description = description;
      billOfMaterialDetail.BillOfMaterialStuffId = billOfMaterialStuffId;
      billOfMaterialDetail.BillOfMaterialVersion = billOfMaterialVersion;
      billOfMaterialDetail.Reservable = reservable;
      billOfMaterialDetail.UnitId = unitId;
      billOfMaterialDetail.Value = value;
      billOfMaterialDetail.ForQty = forQty;
      billOfMaterialDetail.IsPackingMaterial = isPackingMaterial;
      repository.Add(billOfMaterialDetail);
      return billOfMaterialDetail;
    }
    public BillOfMaterialDetail AddBillOfMaterialDetailProcess(
        int index,
        int stuffId,
        short? semiProductBillOfMaterialVersion,
        string description,
        BillOfMaterialDetailType billOfMaterialDetailType,
        int billOfMaterialStuffId,
        short billOfMaterialVersion,
        bool reservable,
        byte unitId,
        double value,
        double forQty,
        bool isPackingMaterial,
        AddEquivalentStuffInput[] equivalentStuffs)
    {

      var billOfMaterialDetail = AddBillOfMaterialDetail(
                    index: index,
                    stuffId: stuffId,
                    semiProductBillOfMaterialVersion: semiProductBillOfMaterialVersion,
                    description: description,
                    billOfMaterialDetailType: billOfMaterialDetailType,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    reservable: reservable,
                    unitId: unitId,
                    value: value,
                    forQty: forQty,
                    isPackingMaterial: isPackingMaterial);
      foreach (var equivalentStuff in equivalentStuffs)
        AddEquivalentStuffProcess(billOfMaterialDetailId: billOfMaterialDetail.Id,
                  title: equivalentStuff.Title,
                  description: equivalentStuff.Description,
                  isActive: equivalentStuff.IsActive,
                  equivalentStuffType: equivalentStuff.EquivalentStuffType,
                  equivalentStuffDetails: equivalentStuff.EquivalentStuffDetails);
      return billOfMaterialDetail;
    }
    public void DeleteBillOfMaterialDetailProcess(int id)
    {

      var billOfMaterialDetail = GetBillOfMaterialDetail(id: id);
      var equivalentStuffs = billOfMaterialDetail.EquivalentStuffs.ToList();
      foreach (var equivalentStuff in equivalentStuffs)
        DeleteEquivalentStuffProcess(equivalentStuff.Id);
      repository.Delete(billOfMaterialDetail);
    }
    public BillOfMaterialDetail EditBillOfMaterialDetail(byte[] rowVersion,
        int id,
        TValue<int> index = null,
        TValue<int> stuffId = null,
        TValue<short?> semiProductBillOfMaterialVersion = null,
        TValue<string> description = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<bool> reservable = null,
        TValue<byte> unitId = null,
        TValue<double> value = null,
        TValue<double> forQty = null,
        TValue<bool> isPackingMaterial = null)
    {

      var billOfMaterialDetail = GetBillOfMaterialDetail(id: id);
      if (index != null)
        billOfMaterialDetail.Index = index;
      if (stuffId != null)
        billOfMaterialDetail.StuffId = stuffId;
      if (billOfMaterialDetailType != null)
        billOfMaterialDetail.BillOfMaterialDetailType = billOfMaterialDetailType;
      if (semiProductBillOfMaterialVersion != null)
        billOfMaterialDetail.SemiProductBillOfMaterialVersion = semiProductBillOfMaterialVersion;
      if (description != null)
        billOfMaterialDetail.Description = description;
      if (billOfMaterialStuffId != null)
        billOfMaterialDetail.BillOfMaterialStuffId = billOfMaterialStuffId;
      if (billOfMaterialVersion != null)
        billOfMaterialDetail.BillOfMaterialVersion = billOfMaterialVersion;
      if (reservable != null)
        billOfMaterialDetail.Reservable = reservable;
      if (unitId != null)
        billOfMaterialDetail.UnitId = unitId;
      if (value != null)
        billOfMaterialDetail.Value = value;
      if (forQty != null)
        billOfMaterialDetail.ForQty = forQty;
      if (isPackingMaterial != null)
        billOfMaterialDetail.IsPackingMaterial = isPackingMaterial;
      repository.Update(billOfMaterialDetail, rowVersion: rowVersion);
      return billOfMaterialDetail;
    }
    public BillOfMaterialDetail EditBillOfMaterialDetailProcess(
        int id,
        byte[] rowVersion,
        TValue<int> index = null,
        TValue<int> stuffId = null,
        TValue<short?> semiProductBillOfMaterialVersion = null,
        TValue<string> description = null,
        TValue<BillOfMaterialDetailType> billOfMaterialDetailType = null,
        TValue<int> billOfMaterialStuffId = null,
        TValue<short> billOfMaterialVersion = null,
        TValue<bool> reservable = null,
        TValue<byte> unitId = null,
        TValue<double> value = null,
        TValue<double> forQty = null,
        TValue<bool> isPackingMaterial = null,
        TValue<AddEquivalentStuffInput[]> addEquivalentStuffInputs = null,
        TValue<EditEquivalentStuffInput[]> editEquivalentStuffInputs = null,
        TValue<int[]> deleteEquivalentStuffs = null)
    {

      var billOfMaterialDetail = EditBillOfMaterialDetail(
                    id: id,
                    rowVersion: rowVersion,
                    index: index,
                    stuffId: stuffId,
                    semiProductBillOfMaterialVersion: semiProductBillOfMaterialVersion,
                    description: description,
                    billOfMaterialDetailType: billOfMaterialDetailType,
                    billOfMaterialStuffId: billOfMaterialStuffId,
                    billOfMaterialVersion: billOfMaterialVersion,
                    reservable: reservable,
                    unitId: unitId,
                    value: value,
                    forQty: forQty,
                    isPackingMaterial: isPackingMaterial);

      if (addEquivalentStuffInputs != null)
        foreach (var equivalentStuff in addEquivalentStuffInputs.Value)
          AddEquivalentStuffProcess(billOfMaterialDetailId: billOfMaterialDetail.Id,
                        title: equivalentStuff.Title,
                        description: equivalentStuff.Description,
                        isActive: equivalentStuff.IsActive,
                        equivalentStuffType: equivalentStuff.EquivalentStuffType,
                        equivalentStuffDetails: equivalentStuff.EquivalentStuffDetails);

      if (editEquivalentStuffInputs != null)
      {
        foreach (var equivalentStuff in editEquivalentStuffInputs.Value)
          EditEquivalentStuffProcess(
                        id: equivalentStuff.Id,
                        rowVersion: equivalentStuff.RowVersion,
                        billOfMaterialDetailId: billOfMaterialDetail.Id,
                        title: equivalentStuff.Title,
                        description: equivalentStuff.Description,
                        isActive: equivalentStuff.IsActive,
                        equivalentStuffType: equivalentStuff.EquivalentStuffType,
                        addEquivalentStuffDetails: equivalentStuff.AddEquivalentStuffDetails,
                        editEquivalentStuffDetails: equivalentStuff.EditEquivalentStuffDetails,
                        deleteEquivalentStuffDetails: equivalentStuff.DeleteEquivalentStuffDetails);
      }


      if (deleteEquivalentStuffs != null)
        foreach (var equivalentStuffId in deleteEquivalentStuffs.Value)
          DeleteEquivalentStuffProcess(id: equivalentStuffId);


      return billOfMaterialDetail;
    }

    public FullBillOfMaterialDetailResult ToFullBillOfMaterialDetailResult(BillOfMaterialDetail billOfMaterialDetail)
    {
      return new FullBillOfMaterialDetailResult
      {
        Id = billOfMaterialDetail.Id,
        Index = billOfMaterialDetail.Index,
        Value = billOfMaterialDetail.Value,
        Reservable = billOfMaterialDetail.Reservable,
        UnitId = billOfMaterialDetail.UnitId,
        UnitConversionRatio = billOfMaterialDetail.Unit.ConversionRatio,
        //UnitName = billOfMaterialDetail.Unit.Name,
        StuffName = billOfMaterialDetail.Stuff.Name,
        BillOfMaterialDetailType = billOfMaterialDetail.BillOfMaterialDetailType,
        StuffId = billOfMaterialDetail.StuffId,
        StuffCode = billOfMaterialDetail.Stuff.Code,
        SemiProductBillOfMaterialVersion = billOfMaterialDetail.SemiProductBillOfMaterialVersion,
        BillOfMaterials = ToBillOfMaterialComboResult(GetBillOfMaterials(stuffId: billOfMaterialDetail.StuffId)).ToArray(),
        Units = App.Internals.WarehouseManagement.GetStuffUnits(stuffId: billOfMaterialDetail.StuffId).ToArray(),
        EquivalentStuffs = billOfMaterialDetail.EquivalentStuffs.ToList().Select(ToFullEquivalentStuffResult).ToArray(),
        ForQty = billOfMaterialDetail.ForQty,
        IsPackingMaterial = billOfMaterialDetail.IsPackingMaterial,
        Description = billOfMaterialDetail.Description,
        RowVersion = billOfMaterialDetail.RowVersion,
      };
    }
    public BillOfMaterialDetailResult ToBillOfMaterialDetailResult(BillOfMaterialDetail billOfMaterialDetail)
    {
      return new BillOfMaterialDetailResult
      {
        Id = billOfMaterialDetail.Id,
        BillOfMaterialStuffId = billOfMaterialDetail.BillOfMaterialStuffId,
        BillOfMaterialVersion = billOfMaterialDetail.BillOfMaterialVersion,
        Index = billOfMaterialDetail.Index,
        Value = billOfMaterialDetail.Value,
        Reservable = billOfMaterialDetail.Reservable,
        UnitId = billOfMaterialDetail.UnitId,
        UnitName = billOfMaterialDetail.Unit.Name,
        StuffCode = billOfMaterialDetail.Stuff.Code,
        StuffName = billOfMaterialDetail.Stuff.Name,
        BillOfMaterialDetailType = billOfMaterialDetail.BillOfMaterialDetailType,
        StuffId = billOfMaterialDetail.StuffId,
        SemiProductBillOfMaterialVersion = billOfMaterialDetail.SemiProductBillOfMaterialVersion,
        StuffNoun = billOfMaterialDetail.Stuff.Noun,
        ForQty = billOfMaterialDetail.ForQty,
        IsPackingMaterial = billOfMaterialDetail.IsPackingMaterial,
        RowVersion = billOfMaterialDetail.RowVersion
      };
    }
    public IQueryable<BillOfMaterialDetailResult> ToBillOfMaterialDetailResultQuery(IQueryable<BillOfMaterialDetail> query)
    {
      var result = from billOfMaterialDetail in query
                   select new BillOfMaterialDetailResult
                   {
                     Id = billOfMaterialDetail.Id,
                     BillOfMaterialStuffId = billOfMaterialDetail.BillOfMaterialStuffId,
                     BillOfMaterialStuffCode = billOfMaterialDetail.BillOfMaterial.Stuff.Code,
                     BillOfMaterialStuffType = billOfMaterialDetail.BillOfMaterial.Stuff.StuffType,
                     BillOfMaterialVersion = billOfMaterialDetail.BillOfMaterialVersion,
                     Index = billOfMaterialDetail.Index,
                     Value = billOfMaterialDetail.Value,
                     Reservable = billOfMaterialDetail.Reservable,
                     UnitId = billOfMaterialDetail.UnitId,
                     UnitName = billOfMaterialDetail.Unit.Name,
                     StuffCode = billOfMaterialDetail.Stuff.Code,
                     StuffName = billOfMaterialDetail.Stuff.Name,
                     StuffType = billOfMaterialDetail.Stuff.StuffType,
                     BillOfMaterialDetailType = billOfMaterialDetail.BillOfMaterialDetailType,
                     StuffId = billOfMaterialDetail.StuffId,
                     SemiProductBillOfMaterialVersion = billOfMaterialDetail.SemiProductBillOfMaterialVersion,
                     StuffNoun = billOfMaterialDetail.Stuff.Noun,
                     ForQty = billOfMaterialDetail.ForQty,
                     IsPackingMaterial = billOfMaterialDetail.IsPackingMaterial,
                     RowVersion = billOfMaterialDetail.RowVersion
                   };
      return result;
    }

    public IOrderedQueryable<BillOfMaterialDetailResult> SortBillOfMaterialDetailResult(IQueryable<BillOfMaterialDetailResult> input, SortInput<BillOfMaterialDetailSortType> options)
    {
      switch (options.SortType)
      {
        case BillOfMaterialDetailSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case BillOfMaterialDetailSortType.Index:
          return input.OrderBy(i => i.Index, options.SortOrder);
        case BillOfMaterialDetailSortType.StuffId:
          return input.OrderBy(i => i.StuffId, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
