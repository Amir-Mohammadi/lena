using System;
using System.Collections.Generic;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Core.Exceptions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.Planning.BillOfMaterial;
using lena.Models.Planning.BillOfMaterialDetail;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    public IQueryable<BillOfMaterial> GetBillOfMaterials(
        TValue<int> stuffId = null,
        TValue<int> version = null,
        TValue<string> title = null,
        TValue<BillOfMaterialVersionType> billOfMaterialVersionType = null,
        TValue<bool> isActive = null,
        TValue<bool> isPublished = null,
        TValue<System.DateTime> createDate = null,
        TValue<double> value = null,
        TValue<int> unitId = null,
        TValue<int> detailStuffId = null,
        TValue<int[]> detailStuffIds = null,
        TValue<int> productionStepId = null,
        TValue<int> equivalentStuffId = null,
        TValue<string> description = null)
    {
      var query = repository.GetQuery<BillOfMaterial>();
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);
      if (detailStuffId != null)
      {
        var detailQuery = GetBillOfMaterialDetails(stuffId: detailStuffId);
        var stuffQuery = detailQuery
                  .Select(i => new { StuffId = i.BillOfMaterialStuffId, Version = i.BillOfMaterialVersion })
                  .Distinct();
        query = from bom in query
                join stuffItem in stuffQuery on new { StuffId = bom.StuffId, Version = bom.Version }
                      equals new { stuffItem.StuffId, stuffItem.Version }
                select bom;
      }
      if (detailStuffIds != null)
      {
        foreach (var detailStuffsId in detailStuffIds.Value)
        {
          var detailQuery = GetBillOfMaterialDetails(stuffId: detailStuffsId);
          var stuffQuery = detailQuery
                .Select(i => new { StuffId = i.BillOfMaterialStuffId, Version = i.BillOfMaterialVersion })
                .Distinct();
          query = from bom in query
                  join stuffItem in stuffQuery on new { StuffId = bom.StuffId, Version = bom.Version }
                        equals new { stuffItem.StuffId, stuffItem.Version }
                  select bom;
        }
      }
      if (version != null)
        query = query.Where(x => x.Version == version);
      if (title != null)
        query = query.Where(x => x.Title == title);
      if (billOfMaterialVersionType != null)
        query = query.Where(x => x.BillOfMaterialVersionType == billOfMaterialVersionType);
      if (equivalentStuffId != null)
      {
        var equivalentDetailQuery = GetEquivalentStuffDetails(stuffId: equivalentStuffId);
        var filterBomQuery = equivalentDetailQuery.Select(i => new
        {
          StuffId = i.EquivalentStuff.BillOfMaterialDetail.BillOfMaterialStuffId,
          Version = i.EquivalentStuff.BillOfMaterialDetail.BillOfMaterialVersion
        }).Distinct();
        query = from bom in query
                join item in filterBomQuery on new { StuffId = bom.StuffId, Version = bom.Version }
                      equals new { item.StuffId, item.Version }
                select bom;
      }
      if (isActive != null)
        query = query.Where(x => x.IsActive == isActive);
      if (isPublished != null)
        query = query.Where(x => x.IsPublished == isPublished);
      if (createDate != null)
        query = query.Where(x => x.CreateDate == createDate);
      if (value != null)
        query = query.Where(x => x.Value == value);
      if (unitId != null)
        query = query.Where(x => x.UnitId == unitId);
      if (productionStepId != null)
        query = query.Where(x => x.ProductionStepId == productionStepId);
      if (description != null)
        query = query.Where(x => x.Description == description);
      return query;
    }
    public IQueryable<BillOfMaterialComparisonResult> GetBillOfMaterialsComparison(
       bool beRecursive,
       int stuffId1,
       int version1,
       int stuffId2,
       int version2)
    {
      #region GetBom1Details
      var billOfMaterialDetails1 = GetBillOfMaterialDetailsRecursive(
              beRecursive: beRecursive,
              stuffId: stuffId1,
              version: version1);
      var groupedResult1 = from item in billOfMaterialDetails1
                           group item by new { item.StuffId, item.Version }
                into gItems
                           select new
                           {
                             StuffId = gItems.Key.StuffId,
                             Version = gItems.Key.Version,
                             Value = gItems.Sum(i => i.Value * i.UnitConversionRatio)
                           };
      #endregion
      #region GetBom2Details
      var billOfMaterialDetails2 = GetBillOfMaterialDetailsRecursive(
              beRecursive: beRecursive,
              stuffId: stuffId2,
              version: version2);
      var groupedResult2 = from item in billOfMaterialDetails2
                           group item by new { item.StuffId, item.Version }
                into gItems
                           select new
                           {
                             StuffId = gItems.Key.StuffId,
                             Version = gItems.Key.Version,
                             Value = gItems.Sum(i => i.Value * i.UnitConversionRatio)
                           };
      #endregion
      #region Full Outer Join
      var leftOuterJoin = from first in groupedResult1
                          join last in groupedResult2
                                on new { StuffId = (int?)first.StuffId } equals new { StuffId = (int?)last.StuffId } into temp
                          from last in temp.DefaultIfEmpty()
                          select new
                          {
                            StuffId = first.StuffId,
                            Version1 = first.Version,
                            Value1 = (double?)first.Value,
                            Version2 = last != null ? last.Version : null,
                            Value2 = last != null ? (double?)last.Value : null,
                          };
      var rightOuterJoin = from first in groupedResult2
                           join last in groupedResult1
                                     on new { StuffId = (int?)first.StuffId } equals new { StuffId = (int?)last.StuffId } into temp
                           from last in temp.DefaultIfEmpty()
                           select new
                           {
                             StuffId = first.StuffId,
                             Version1 = last != null ? last.Version : null,
                             Value1 = last != null ? (double?)last.Value : null,
                             Version2 = first.Version,
                             Value2 = (double?)first.Value,
                           };
      var query = leftOuterJoin.Union(rightOuterJoin);
      #endregion
      var stuffIds = query.Select(i => i.StuffId).ToArray();
      #region Get Stuffs
      var stuffs = App.Internals.SaleManagement.GetStuffs(
              selector: e => e,
              ids: stuffIds)
          .ToList();
      #endregion
      #region Get MainUnits
      var mainUnits = App.Internals.ApplicationBase.GetUnits(
              selector: e => e
              , isMainUnit: true)
          .ToList();
      #endregion
      #region Union
      var result = from item in query
                   join stuff in stuffs on item.StuffId equals stuff.Id
                   join unit in mainUnits on stuff.UnitTypeId equals unit.UnitTypeId
                   select new BillOfMaterialComparisonResult
                   {
                     StuffId = item.StuffId,
                     StuffCode = stuff.Code,
                     StuffNoun = stuff.Noun,
                     StuffName = stuff.Name,
                     Version1 = item.Version1,
                     Value1 = item.Value1,
                     Version2 = item.Version2,
                     Value2 = item.Value2,
                     UnitId = unit.Id,
                     UnitName = unit.Name,
                   };
      #endregion
      return result;
    }
    public bool IsPackingStuff(int stuffId, short billOfMaterialVersion)
    {
      var bom = GetBillOfMaterial(stuffId: stuffId, version: billOfMaterialVersion);
      return bom.BillOfMaterialVersionType == BillOfMaterialVersionType.Packing;
    }
    public BillOfMaterial GetBillOfMaterial(int stuffId, short version)
    {
      var billOfMaterial = GetBillOfMaterials(stuffId: stuffId, version: version).SingleOrDefault();
      if (billOfMaterial == null)
        throw new BillOfMaterialNotFoundException(stuffId: stuffId, version: version);
      return billOfMaterial;
    }
    public IQueryable<FlatBillOfMaterialRawResult> GetFlatBillOfMaterial(int? rootId, int? parentId, int? childId, int? version, StuffType? stuffType)
    {
      var query = repository.CreateContextQuery<FlatBillOfMaterialRawResult>("[GetFlatBom]()");
      if (rootId != null)
        query = query.Where(i => i.Root == rootId);
      if (parentId != null)
        query = query.Where(i => i.Parent == parentId);
      if (childId != null)
        query = query.Where(i => i.Child == childId);
      if (version != null)
        query = query.Where(i => i.Version == version);
      if (stuffType != null)
        query = query.Where(i => i.TypeId == (int)stuffType);
      return query;
    }
    public IQueryable<BillOfMaterial> GetBillOfMaterialListForPython(GetBillOfMaterialInput[] getBillOfMaterials)
    {
      var billOfMaterials = GetBillOfMaterials();
      var inputBomKeys = getBillOfMaterials.Select(i => i.StuffId + "*" + i.Version).ToArray();
      billOfMaterials = billOfMaterials.Where(i =>
                inputBomKeys.Contains(i.StuffId + "*" + i.Version));
      return billOfMaterials;
    }
    public BillOfMaterial AddBillOfMaterial(
        int stuffId,
        string title,
        BillOfMaterialVersionType billOfMaterialVersionType,
        double value,
        byte unitId,
        int productionStepId,
        int qtyPerBox,
        string description)
    {
      var billOfMaterial = repository.Create<BillOfMaterial>();
      billOfMaterial.StuffId = stuffId;
      billOfMaterial.Version = GetBillOfMaterialNewVersion(stuffId);
      billOfMaterial.Title = title;
      billOfMaterial.BillOfMaterialVersionType = billOfMaterialVersionType;
      billOfMaterial.IsActive = false;
      billOfMaterial.IsPublished = false;
      billOfMaterial.Value = value;
      billOfMaterial.UnitId = unitId;
      billOfMaterial.ProductionStepId = productionStepId;
      billOfMaterial.Description = description;
      billOfMaterial.UserId = App.Providers.Security.CurrentLoginData.UserId;
      billOfMaterial.CreateDate = DateTime.Now.ToUniversalTime();
      billOfMaterial.QtyPerBox = qtyPerBox;
      repository.Add(billOfMaterial);
      return billOfMaterial;
    }
    private short GetBillOfMaterialNewVersion(int stuffId)
    {
      var query = GetBillOfMaterials(stuffId: stuffId);
      var maxVersion = (short)(query.Any() ? query.Max(i => i.Version) : 0);
      return (short)(maxVersion + 1);
    }
    public BillOfMaterial AddBillOfMaterialProcess(
        int stuffId,
        string title,
        BillOfMaterialVersionType billOfMaterialVersionType,
        double value,
        byte unitId,
        int qtyPerBox,
        int productionStepId,
        string description,
        AddBillOfMaterialDetailInput[] billOfMaterialDetails,
        bool copyDocuments,
        int? SourceVersionToCopyDocuments
        )
    {
      #region CheckConstraint
      if (billOfMaterialDetails.Any(m => m.StuffId == stuffId))
      {
        if (billOfMaterialVersionType != BillOfMaterialVersionType.Packing)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
          throw new CannotConsumeTheSameStuffInProductionBillOfMaterialException(stuffCode: stuff.Code);
        }
      }
      #endregion
      var billOfMaterial = AddBillOfMaterial(
              stuffId: stuffId,
              title: title,
              billOfMaterialVersionType: billOfMaterialVersionType,
              value: value,
              unitId: unitId,
              productionStepId: productionStepId,
              description: description,
              qtyPerBox: qtyPerBox);
      //Add Documents from another version
      if (copyDocuments)
      {
        var documents = GetBillOfMaterialDocuments(
                  billOfMaterialStuffId: billOfMaterial.StuffId,
                  billOfMaterialVersion: SourceVersionToCopyDocuments,
                  isDelete: false)
              .ToList();
        documents.ForEach(sourceBomDoc =>
              {
                var docFile = App.Internals.ApplicationBase.
                      GetDocument(sourceBomDoc.DocumentId);
                var fileData = new UploadFileData()
                {
                  FileName = docFile.Name,
                  FileData = docFile.FileStream
                };
                AddBillOfMaterialDocument(
                          billOfMaterialVersion: billOfMaterial.Version,
                          billOfMaterialStuffId: billOfMaterial.StuffId,
                          billOfMaterialDocumentTypeId: sourceBomDoc.BillOfMaterialDocumentTypeId,
                          description: sourceBomDoc.Description,
                          title: sourceBomDoc.Title,
                          uploadFileData: fileData
                          );
              });
      }
      #region Add BillOfMaterialDetails
      foreach (var billOfMaterialDetail in billOfMaterialDetails)
      {
        AddBillOfMaterialDetailProcess(index: billOfMaterialDetail.Index,
                      billOfMaterialStuffId: billOfMaterial.StuffId,
                      billOfMaterialVersion: billOfMaterial.Version,
                      stuffId: billOfMaterialDetail.StuffId,
                      semiProductBillOfMaterialVersion: billOfMaterialDetail.SemiProductBillOfMaterialVersion,
                      description: billOfMaterialDetail.Description,
                      billOfMaterialDetailType: billOfMaterialDetail.BillOfMaterialDetailType,
                      reservable: billOfMaterialDetail.Reservable,
                      unitId: billOfMaterialDetail.UnitId,
                      value: billOfMaterialDetail.Value,
                      forQty: billOfMaterialDetail.ForQty,
                      isPackingMaterial: billOfMaterialDetail.IsPackingMaterial,
                      equivalentStuffs: billOfMaterialDetail.EquivalentStuffs);
      }
      #endregion
      return billOfMaterial;
    }
    public BillOfMaterial EditBillOfMaterial(
        byte[] rowVersion,
        int stuffId,
        short version,
        TValue<string> title = null,
        TValue<BillOfMaterialVersionType> billOfMaterialVersionType = null,
        TValue<bool> isActive = null,
        TValue<bool> isPublished = null,
        TValue<double> value = null,
        TValue<byte> unitId = null,
        TValue<int> productionStepId = null,
        TValue<string> description = null,
        TValue<int> qtyPerBox = null,
        TValue<int> latestBillOfMaterialPublishRequestId = null
        )
    {
      var bom = GetBillOfMaterial(stuffId, version);
      if (isPublished != null && isPublished && !bom.IsActive)
        throw new CannotPublishNotActiveBillOfMaterialException(stuffId, version);
      //if (isActive != null && isActive)
      //{
      //    var productionPlanDetailes = GetProductionPlanDetails();
      //    var productionPlanCount = productionPlanDetailes.Count(x =>
      //        x.BillOfMaterialStep.BillOfMaterialStuffId == stuffId &&
      //        x.BillOfMaterialStep.BillOfMaterialVersion == version);
      //    if (productionPlanCount > 0)
      //        throw new BillOfMaterialUsedInProductionPlanningException(stuffId, version, "فرمول تولید مورد نظر قبلا در برنامه ریزی تولید استفاده شده است!");
      //}
      var billOfMaterial = GetBillOfMaterial(stuffId: stuffId, version: version);
      if (billOfMaterial == null)
        throw new BillOfMaterialNotFoundException(
                  stuffId: stuffId,
                  version: version);
      if (title != null)
        billOfMaterial.Title = title;
      if (billOfMaterialVersionType != null)
        billOfMaterial.BillOfMaterialVersionType = billOfMaterialVersionType;
      if (isActive != null)
        billOfMaterial.IsActive = isActive;
      if (isPublished != null)
        billOfMaterial.IsPublished = isPublished;
      if (value != null)
        billOfMaterial.Value = value;
      if (unitId != null)
        billOfMaterial.UnitId = unitId;
      if (productionStepId != null)
        billOfMaterial.ProductionStepId = productionStepId;
      if (description != null)
        billOfMaterial.Description = description;
      if (qtyPerBox != null)
        billOfMaterial.QtyPerBox = qtyPerBox;
      var latestBillOfMaterialPublishRequest = App.Internals.Planning.GetBillOfMaterialPublishRequests(
                e => e,
                id: latestBillOfMaterialPublishRequestId)
                .FirstOrDefault();
      if (latestBillOfMaterialPublishRequestId != null)
      {
        billOfMaterial.LatestBillOfMaterialPublishRequest = latestBillOfMaterialPublishRequest;
      }
      repository.Update(billOfMaterial, rowVersion: rowVersion);
      #region Prevent published bom exceed from Max count
      if (isPublished)
      {
        var publishedCount = GetPublishedBillOfMaterials(stuffId)
                  .Count();
        var maxPublishedBomCount = App.Internals.ApplicationSetting.GetMaxPublishedBomCount();
        if (publishedCount > maxPublishedBomCount)
          throw new MaxPublishedBomCountExceedException(stuffId: stuffId, stuffCode: bom.Stuff.Code, publishedCount: publishedCount);
      }
      #endregion
      #region Prevent published packing and price bom 
      if (billOfMaterial.IsPublished == true && billOfMaterial.BillOfMaterialVersionType != BillOfMaterialVersionType.Production)
        throw new NotProductionPublishedBomException();
      #endregion
      return billOfMaterial;
    }
    public IQueryable<BillOfMaterial> GetPublishedBillOfMaterials(int stuffId)
    {
      var billOfMaterials = GetBillOfMaterials(
                    stuffId: stuffId,
                    isPublished: true);
      return billOfMaterials;
    }
    public BillOfMaterial GetPublishedBillOfMaterial(int stuffId, bool ignoreNotFoundException = false)
    {
      var billOfMaterials = GetPublishedBillOfMaterials(stuffId: stuffId);
      var billOfMaterial = billOfMaterials.FirstOrDefault();
      if (billOfMaterials.Count() > 1)
      {
        var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
        throw new AmbiguousPublishedBillOfMaterialException(stuffCode: stuff.Code);
      }
      if (billOfMaterial == null)
      {
        var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
        if (!ignoreNotFoundException)
          throw new PublishedBillOfMaterialNotFoundException(stuffCode: stuff.Code);
        var bom = GetBillOfMaterials(stuffId: stuffId)
                  .OrderByDescending(x => x.IsActive)
                  .ThenByDescending(x => x.Version)
                  .FirstOrDefault();
        if (bom == null)
          throw new StuffDoesNotHaveAnyActiveBillOfMaterialException(stuffCode: stuff.Code);
        if (!bom.IsActive)
          bom.NotHasAnyActiveAndPublishedVersion = true;
        return bom;
      }
      return billOfMaterial;
    }
    public BillOfMaterial EditBillOfMaterialProcess(
        byte[] rowVersion,
        int stuffId,
        short version,
        string title,
        BillOfMaterialVersionType billOfMaterialVersionType,
        double value,
        byte unitId,
        int qtyPerBox,
        int productionStepId,
        string description,
        AddBillOfMaterialDetailInput[] addBillOfMaterialDetailsInput,
        EditBillOfMaterialDetailInput[] editBillOfMaterialDetailsInput,
        int[] deleteBillOfMaterialDetails)
    {
      #region CheckConstraint
      var billOfMaterialDetails = addBillOfMaterialDetailsInput.Select(m => m.StuffId).Union(editBillOfMaterialDetailsInput.Select(m => m.StuffId));
      if (billOfMaterialDetails.Any(m => m == stuffId))
      {
        if (billOfMaterialVersionType != BillOfMaterialVersionType.Packing)
        {
          var stuff = App.Internals.SaleManagement.GetStuff(id: stuffId);
          throw new CannotConsumeTheSameStuffInProductionBillOfMaterialException(stuffCode: stuff.Code);
        }
      }
      #endregion
      #region EditBillOfMaterial
      var bom = GetBillOfMaterial(
              stuffId: stuffId,
              version: version);
      var bomStatus = CheckBillOfMaterialIsUsedInItems(
                    stuffId: stuffId,
                    version: version);
      if (bomStatus.IsUsedInProductionProcess && bom.BillOfMaterialVersionType != BillOfMaterialVersionType.Price &&
                (bom.Value != value ||
                bom.UnitId != unitId ||
                bom.BillOfMaterialVersionType != billOfMaterialVersionType ||
                bom.StuffId != stuffId ||
                bom.ProductionStepId != productionStepId ||
                bom.QtyPerBox != qtyPerBox ||
                addBillOfMaterialDetailsInput.Any() ||
                deleteBillOfMaterialDetails.Any()))
        throw new BillOfMaterialUsedInProductionProcessException(bom.Stuff.Code, stuffId, version);
      var billOfMaterial = EditBillOfMaterial(
                    rowVersion: rowVersion,
                    stuffId: stuffId,
                    version: version,
                    billOfMaterialVersionType: billOfMaterialVersionType,
                    title: title,
                    productionStepId: productionStepId,
                    value: value,
                    unitId: unitId,
                    qtyPerBox: qtyPerBox,
                    description: description);
      #endregion
      #region AddBillOfMaterialDetails
      if (addBillOfMaterialDetailsInput != null)
        foreach (var billOfMaterialDetail in addBillOfMaterialDetailsInput)
        {
          AddBillOfMaterialDetailProcess(index: billOfMaterialDetail.Index,
                        stuffId: billOfMaterialDetail.StuffId,
                        semiProductBillOfMaterialVersion: billOfMaterialDetail.SemiProductBillOfMaterialVersion,
                        description: billOfMaterialDetail.Description,
                        billOfMaterialDetailType: billOfMaterialDetail.BillOfMaterialDetailType,
                        billOfMaterialStuffId: billOfMaterial.StuffId,
                        billOfMaterialVersion: billOfMaterial.Version,
                        reservable: billOfMaterialDetail.Reservable,
                        unitId: billOfMaterialDetail.UnitId,
                        value: billOfMaterialDetail.Value,
                        forQty: billOfMaterialDetail.ForQty,
                        isPackingMaterial: billOfMaterialDetail.IsPackingMaterial,
                        equivalentStuffs: billOfMaterialDetail.EquivalentStuffs);
        }
      #endregion
      #region EditBillOfMaterialDetails
      if (editBillOfMaterialDetailsInput != null)
      {
        var editItemsInputInOldBom = editBillOfMaterialDetailsInput.Where(bomdInput =>
                  bom.BillOfMaterialDetails.Any(bomd => bomd.StuffId == bomdInput.StuffId)).ToList();
        if (bomStatus.IsUsedInProductionProcess)
        {
          if (editBillOfMaterialDetailsInput.Length != editItemsInputInOldBom.Count)
            throw new BillOfMaterialUsedInProductionProcessException(bom.Stuff.Code, bom.StuffId, bom.Version);
          foreach (var editItem in editItemsInputInOldBom)
          {
            var oldItem = bom.BillOfMaterialDetails.FirstOrDefault(x => x.StuffId == editItem.StuffId);
            if (bom.BillOfMaterialVersionType != BillOfMaterialVersionType.Price && (oldItem.Value != editItem.Value || oldItem.Value != editItem.Value || oldItem.ForQty != editItem.ForQty
                       || oldItem.SemiProductBillOfMaterialVersion != editItem.SemiProductBillOfMaterialVersion))
              throw new BillOfMaterialUsedInProductionProcessException(bom.Stuff.Code, bom.StuffId, bom.Version);
          }
        }
        foreach (var billOfMaterialDetail in editBillOfMaterialDetailsInput)
        {
          EditBillOfMaterialDetailProcess(
                        id: billOfMaterialDetail.Id,
                        rowVersion: billOfMaterialDetail.RowVersion,
                        index: billOfMaterialDetail.Index,
                        stuffId: billOfMaterialDetail.StuffId,
                        semiProductBillOfMaterialVersion: ((TValue<short?>)billOfMaterialDetail.SemiProductBillOfMaterialVersion) ?? new TValue<short?>(null),
                        description: billOfMaterialDetail.Description,
                        billOfMaterialDetailType: billOfMaterialDetail.BillOfMaterialDetailType,
                        billOfMaterialStuffId: billOfMaterial.StuffId,
                        billOfMaterialVersion: billOfMaterial.Version,
                        reservable: billOfMaterialDetail.Reservable,
                        unitId: billOfMaterialDetail.UnitId,
                        value: billOfMaterialDetail.Value,
                        forQty: billOfMaterialDetail.ForQty,
                        isPackingMaterial: billOfMaterialDetail.IsPackingMaterial,
                        addEquivalentStuffInputs: billOfMaterialDetail.AddEquivalentStuffs,
                        editEquivalentStuffInputs: billOfMaterialDetail.EditEquivalentStuffs,
                        deleteEquivalentStuffs: billOfMaterialDetail.DeleteEquivalentStuffs);
        }
      }
      #endregion
      #region DeleteBillOfMaterialDetails
      if (deleteBillOfMaterialDetails != null)
        foreach (var billOfMaterialDetail in deleteBillOfMaterialDetails)
        {
          DeleteBillOfMaterialDetailProcess(id: billOfMaterialDetail);
        }
      #endregion
      return billOfMaterial;
    }
    public void DeleteBillOfMaterialProcess(int stuffId, short version)
    {
      var billOfMaterial = GetBillOfMaterial(stuffId: stuffId, version: version);
      var schedules = GetProductionSchedules(ToProductionScheduleResult);
      var workPlans = GetWorkPlans(billOfMaterialStuffId: stuffId, billOfMaterialVersion: version);
      var workPlansSteps = GetWorkPlanSteps(selector: e => e);
      if (bomUsedInScheduleCount > 0)
        throw new ProductionScheduleHasDependencyToBomException(stuffId);
      var publishRequestCount = billOfMaterial.BillOfMaterialPublishRequests.Count();
      if (publishRequestCount > 0)
        throw new BillOfMaterialPublishRequestHasDependencyToBomException(stuffId);
      var workPlanCount = billOfMaterial.WorkPlans.Count();
      if (workPlanCount > 0)
        throw new WorkPlanHasDependencyToBomException(stuffId);
      var billOfMaterialList = billOfMaterial.BillOfMaterialDetails.ToList();
      foreach (var billOfMaterialDetail in billOfMaterialList)
        DeleteBillOfMaterialDetailProcess(billOfMaterialDetail.Id);
      repository.Delete(billOfMaterial);
    }
    public BillOfMaterial ActiveBillOfMaterial(
        byte[] rowVersion,
        int stuffId,
        short version)
    {
      return EditBillOfMaterial(
          rowVersion: rowVersion,
          stuffId: stuffId,
          version: version,
          isActive: true);
    }
    public BillOfMaterial DeactiveBillOfMaterial(byte[] rowVersion, int stuffId, short version)
    {
      var bom = GetBillOfMaterial(stuffId, version);
      if (bom.IsPublished)
        throw new CannotDeactivePublishedBillOfMaterialException(stuffId, version);
      var schedules = GetProductionSchedules(
                    selector: productionSchedule => new
                    {
                      SemiProductStuffId = productionSchedule.ProductionPlanDetail.BillOfMaterialStuffId,
                      SemiProductVersion = productionSchedule.ProductionPlanDetail.BillOfMaterialVersion,
                      IsPublished = productionSchedule.IsPublished,
                      Status = productionSchedule.Status
                    },
                    isDelete: false);
      schedules = schedules.Where(i =>
                i.SemiProductStuffId == stuffId &&
                i.SemiProductVersion == version &&
                i.Status != ProductionScheduleStatus.Produced);
      var chechProductionPlanning = App.Internals.ApplicationSetting.GetCkeckProductionPlanningWhenDeactivateBom();
      if (chechProductionPlanning && schedules.Any())
        throw new InCompleteProductionScheduleException(stuffId, version);
      return EditBillOfMaterial(rowVersion: rowVersion, stuffId: stuffId, version: version, isActive: false);
    }
    public BillOfMaterial PublishBillOfMaterial(byte[] rowVersion, int stuffId, short version)
    {
      return EditBillOfMaterial(
          rowVersion: rowVersion,
          stuffId: stuffId,
          version: version,
          isPublished: true);
    }
    public BillOfMaterial UnPublishBillOfMaterial(byte[] rowVersion, int stuffId, short version)
    {
      return EditBillOfMaterial(
          rowVersion: rowVersion,
          stuffId: stuffId,
          version: version,
          isPublished: false);
    }
    public IQueryable<BillOfMaterialResult> ToBillOfMaterialResultQuery(IQueryable<BillOfMaterial> query)
    {
      var resultQuery = from billOfMaterial in query
                        let product = billOfMaterial.Stuff
                        let unit = billOfMaterial.Unit
                        let user = billOfMaterial.User
                        let employee = billOfMaterial.User.Employee
                        select new BillOfMaterialResult()
                        {
                          StuffId = billOfMaterial.StuffId,
                          Title = billOfMaterial.Title,
                          Version = billOfMaterial.Version,
                          UnitId = billOfMaterial.UnitId,
                          UnitName = billOfMaterial.Unit.Name,
                          ProductionStepId = billOfMaterial.ProductionStepId,
                          ProductionStepName = billOfMaterial.ProductionStep.Name,
                          StuffName = product.Name,
                          StuffCode = product.Code,
                          IsActive = billOfMaterial.IsActive,
                          IsPublished = billOfMaterial.IsPublished,
                          CreateDate = billOfMaterial.CreateDate,
                          UserId = billOfMaterial.UserId,
                          UserName = (employee != null) ? employee.FirstName + " " + employee.LastName : "",
                          Description = billOfMaterial.Description,
                          Value = billOfMaterial.Value,
                          BillOfMaterialVersionType = billOfMaterial.BillOfMaterialVersionType,
                          QtyPerBox = billOfMaterial.QtyPerBox,
                          RowVersion = billOfMaterial.RowVersion
                        };
      return resultQuery;
    }
    public BillOfMaterialResult ToBillOfMaterialResult(BillOfMaterial billOfMaterial)
    {
      var product = billOfMaterial.Stuff;
      var user = billOfMaterial.User;
      var employee = user.Employee;
      var result = new BillOfMaterialResult()
      {
        StuffId = billOfMaterial.StuffId,
        Title = billOfMaterial.Title,
        Version = billOfMaterial.Version,
        UnitId = billOfMaterial.UnitId,
        UnitName = billOfMaterial.Unit.Name,
        ProductionStepId = billOfMaterial.ProductionStepId,
        ProductionStepName = billOfMaterial.ProductionStep.Name,
        StuffName = product.Name,
        StuffCode = product.Code,
        IsActive = billOfMaterial.IsActive,
        IsPublished = billOfMaterial.IsPublished,
        CreateDate = billOfMaterial.CreateDate,
        UserId = billOfMaterial.UserId,
        UserName = (employee != null) ? employee.FirstName + " " + employee.LastName : "",
        Description = billOfMaterial.Description,
        Value = billOfMaterial.Value,
        BillOfMaterialVersionType = billOfMaterial.BillOfMaterialVersionType,
        QtyPerBox = billOfMaterial.QtyPerBox,
        RowVersion = billOfMaterial.RowVersion
      };
      return result;
    }
    public IQueryable<BillOfMaterialComboResult> ToBillOfMaterialComboResult(IQueryable<BillOfMaterial> billOfMaterial)
    {
      return from item in billOfMaterial
             select new BillOfMaterialComboResult
             {
               StuffId = item.StuffId,
               ProductionStepId = item.ProductionStepId,
               ProductionStepName = item.ProductionStep.Name,
               BillOfMaterialVersionType = item.BillOfMaterialVersionType,
               Title = item.Title,
               Version = item.Version,
               IsActive = item.IsActive,
               QtyPerBox = item.QtyPerBox,
               IsPublished = item.IsPublished
             };
    }
    public IOrderedQueryable<BillOfMaterialResult> SortBillOfMaterialResult(IQueryable<BillOfMaterialResult> input, SortInput<BillOfMaterialSortType> options)
    {
      switch (options.SortType)
      {
        case BillOfMaterialSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case BillOfMaterialSortType.BillOfMaterialVersionType:
          return input.OrderBy(i => i.BillOfMaterialVersionType, options.SortOrder);
        case BillOfMaterialSortType.CreateDate:
          return input.OrderBy(i => i.CreateDate, options.SortOrder);
        case BillOfMaterialSortType.UserName:
          return input.OrderBy(i => i.UserName, options.SortOrder);
        case BillOfMaterialSortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case BillOfMaterialSortType.IsPublished:
          return input.OrderBy(i => i.IsPublished, options.SortOrder);
        case BillOfMaterialSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case BillOfMaterialSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case BillOfMaterialSortType.UnitName:
          return input.OrderBy(i => i.UnitName, options.SortOrder);
        case BillOfMaterialSortType.ProductionStepName:
          return input.OrderBy(i => i.ProductionStepName, options.SortOrder);
        case BillOfMaterialSortType.Value:
          return input.OrderBy(i => i.Value, options.SortOrder);
        case BillOfMaterialSortType.Version:
          return input.OrderBy(i => i.Version, options.SortOrder);
        case BillOfMaterialSortType.QtyPerBox:
          return input.OrderBy(i => i.QtyPerBox, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<BillOfMaterialEquivalentStuffResult> SortBillOfMaterialEquivalentStuffResult(IQueryable<BillOfMaterialEquivalentStuffResult> query, SortInput<BillOfMaterialEquivalentStuffSortType> options)
    {
      switch (options.SortType)
      {
        case BillOfMaterialEquivalentStuffSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, options.SortOrder);
        case BillOfMaterialEquivalentStuffSortType.StuffName:
          return query.OrderBy(i => i.StuffName, options.SortOrder);
        case BillOfMaterialEquivalentStuffSortType.Version:
          return query.OrderBy(i => i.Version, options.SortOrder);
        case BillOfMaterialEquivalentStuffSortType.Value:
          return query.OrderBy(i => i.Value, options.SortOrder);
        case BillOfMaterialEquivalentStuffSortType.UnitName:
          return query.OrderBy(i => i.UnitName, options.SortOrder);
        case BillOfMaterialEquivalentStuffSortType.ForQty:
          return query.OrderBy(i => i.ForQty, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<BillOfMaterialComparisonResult> SortBillOfMaterialComparisonResult(
        IQueryable<BillOfMaterialComparisonResult> query,
        SortInput<BillOfMaterialComparisonSortType> options)
    {
      switch (options.SortType)
      {
        case BillOfMaterialComparisonSortType.StuffCode:
          return query.OrderBy(i => i.StuffCode, options.SortOrder);
        case BillOfMaterialComparisonSortType.StuffName:
          return query.OrderBy(i => i.StuffName, options.SortOrder);
        case BillOfMaterialComparisonSortType.StuffNoun:
          return query.OrderBy(i => i.StuffNoun, options.SortOrder);
        case BillOfMaterialComparisonSortType.UnitName:
          return query.OrderBy(i => i.UnitName, options.SortOrder);
        case BillOfMaterialComparisonSortType.Value1:
          return query.OrderBy(i => i.Value1, options.SortOrder);
        case BillOfMaterialComparisonSortType.Value2:
          return query.OrderBy(i => i.Value2, options.SortOrder);
        case BillOfMaterialComparisonSortType.Status:
          return query.OrderBy(i => i.Status, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<BillOfMaterialComparisonResult> SearchBillOfMaterialComparisonResult(
        IQueryable<BillOfMaterialComparisonResult> query,
        string searchText)
    {
      if (searchText != null)
        query = from item in query
                where item.StuffCode.ToLower().Contains(searchText.ToLower()) ||
                      item.StuffName.ToLower().Contains(searchText.ToLower()) ||
                      item.StuffNoun.ToLower().Contains(searchText.ToLower()) ||
                      item.UnitName.ToLower().Contains(searchText.ToLower())
                select item;
      return query;
    }
    public FullBillOfMaterialResult ToFullBillOfMaterialResult(BillOfMaterial billOfMaterial)
    {
      var result = new FullBillOfMaterialResult
      {
        BillOfMaterialDetails = billOfMaterial.BillOfMaterialDetails.ToList().Select(ToFullBillOfMaterialDetailResult).ToArray(),
        Description = billOfMaterial.Description,
        UnitId = billOfMaterial.UnitId,
        Version = billOfMaterial.Version,
        QtyPerBox = billOfMaterial.QtyPerBox,
        StuffId = billOfMaterial.StuffId,
        Title = billOfMaterial.Title,
        ProductionStepId = billOfMaterial.ProductionStepId,
        ProductionStepName = billOfMaterial.ProductionStep.Name,
        StuffCode = billOfMaterial.Stuff.Code,
        Value = billOfMaterial.Value,
        IsActive = billOfMaterial.IsActive,
        IsPublished = billOfMaterial.IsPublished,
        CreateDate = billOfMaterial.CreateDate,
        BillOfMaterialVersionType = billOfMaterial.BillOfMaterialVersionType,
        StuffName = billOfMaterial.Stuff.Name,
        UnitName = billOfMaterial.Unit.Name,
        UnitConversionRatio = billOfMaterial.Unit.ConversionRatio,
        Units = App.Internals.WarehouseManagement.GetStuffUnits(stuffId: billOfMaterial.StuffId).ToArray(),
        RowVersion = billOfMaterial.RowVersion
      };
      return result;
    }
    #region
    public CheckBillOfMaterialIsUsedItemsResult CheckBillOfMaterialIsUsedInItems(
        TValue<int> stuffId = null,
        TValue<int> version = null)
    {
      var productions = App.Internals.Production.GetProductions(e => e,
                stuffSerialStuffId: stuffId,
                version: version);
      var productionPlans = GetProductionPlans(e => e,
               billOfMaterialStuffId: stuffId,
               billOfMaterialVersion: version);
      var productionSchedules = GetProductionSchedules(e => e,
               stuffId: stuffId,
               version: version);
      var orderItems = App.Internals.SaleManagement.GetOrderItems(e => e,
                productPackBillOfMaterialStuffId: stuffId,
                productPackBillOfMaterialVersion: (int?)version);
      var workPlans = GetWorkPlans(
                billOfMaterialStuffId: stuffId,
                billOfMaterialVersion: version);
      var checkBillOfMaterialUsedItems = new CheckBillOfMaterialIsUsedItemsResult();
      checkBillOfMaterialUsedItems.HasProduction = productions.Any();
      checkBillOfMaterialUsedItems.HasProductionPlan = productionPlans.Any();
      checkBillOfMaterialUsedItems.HasProductionSchedule = productionSchedules.Any();
      checkBillOfMaterialUsedItems.HasOrderItem = orderItems.Any();
      checkBillOfMaterialUsedItems.HasWorkPlan = workPlans.Any();
      return checkBillOfMaterialUsedItems;
    }
    #endregion
    #region ToCheckCanBeDeletedBillOfMaterialResult
    public CheckBillOfMaterialIsUsedItemsResult ToCheckBillOfMaterialIsUsedInItemsResult(BillOfMaterial billOfMaterial, CheckBillOfMaterialIsUsedItemsResult checkBillOfMaterialIsUsedItems)
    {
      var result = new CheckBillOfMaterialIsUsedItemsResult
      {
        StuffId = billOfMaterial.StuffId,
        Version = billOfMaterial.Version,
        HasProduction = checkBillOfMaterialIsUsedItems.HasProduction,
        HasProductionPlan = checkBillOfMaterialIsUsedItems.HasProductionPlan,
        HasProductionSchedule = checkBillOfMaterialIsUsedItems.HasProductionSchedule,
        HasOrderItem = checkBillOfMaterialIsUsedItems.HasOrderItem,
      };
      return result;
    }
    #endregion
    public IQueryable<FullBillOfMaterialResult> ToFullBillOfMaterialResultForPython(IQueryable<BillOfMaterial> billOfMaterials)
    {
      return from item in billOfMaterials
             select new FullBillOfMaterialResult
             {
               BillOfMaterialDetails = item.BillOfMaterialDetails.ToList().Select(ToFullBillOfMaterialDetailResult).ToArray(),
               Description = item.Description,
               UnitId = item.UnitId,
               ProductionStepId = item.ProductionStepId,
               ProductionStepName = item.ProductionStep.Name,
               Version = item.Version,
               StuffId = item.StuffId,
               Title = item.Title,
               StuffCode = item.Stuff.Code,
               Value = item.Value,
               IsActive = item.IsActive,
               IsPublished = item.IsPublished,
               CreateDate = item.CreateDate,
               BillOfMaterialVersionType = item.BillOfMaterialVersionType,
               StuffName = item.Stuff.Name,
               UnitName = item.Unit.Name,
               UnitConversionRatio = item.Unit.ConversionRatio,
               Units = null,
               RowVersion = item.RowVersion,
             };
    }
    public FullBillOfMaterialForTreeViewResult GetFullBillOfMaterialForTreeViewResult(BillOfMaterial billOfMaterial, double factor, int level)
    {
      var result = new FullBillOfMaterialForTreeViewResult
      {
        BillOfMaterialDetails = billOfMaterial.BillOfMaterialDetails.ToList()
                    .Select(i => GetFullBillOfMaterialForTreeViewResult(i, factor, level + 1)).ToArray(),
        Description = billOfMaterial.Description,
        Version = billOfMaterial.Version,
        StuffId = billOfMaterial.StuffId,
        StuffName = billOfMaterial.Stuff.Name,
        ProductionStepId = billOfMaterial.ProductionStepId,
        ProductionStepName = billOfMaterial.ProductionStep.Name,
        Value = billOfMaterial.Value * factor,
        Level = level + 1,
        Title = billOfMaterial.Title,
        UnitId = billOfMaterial.UnitId,
        StuffCode = billOfMaterial.Stuff.Code,
        UnitName = billOfMaterial.Unit.Name,
        NotHasAnyActiveAndPublishedVersion = billOfMaterial.NotHasAnyActiveAndPublishedVersion,
        //HasEqualStuff = billOfMaterial.de
      };
      return result;
    }
    public FullBillOfMaterialForTreeViewResult GetFullBillOfMaterialForTreeViewResult(BillOfMaterialDetail billOfMaterialDetail, double factor, int level)
    {
      var result = new FullBillOfMaterialForTreeViewResult();
      if (billOfMaterialDetail.Stuff.StuffType != StuffType.RawMaterial &&
                billOfMaterialDetail.Stuff.StuffType != StuffType.General)
      {
        BillOfMaterial semiProductBillOfMaterial = null;
        if (billOfMaterialDetail.SemiProductBillOfMaterialVersion != null)
          semiProductBillOfMaterial = billOfMaterialDetail.SemiProductBillOfMaterial;
        else
          semiProductBillOfMaterial = GetPublishedBillOfMaterial(billOfMaterialDetail.StuffId, ignoreNotFoundException: true);
        var nextfactor = factor * billOfMaterialDetail.Value * billOfMaterialDetail.Unit.ConversionRatio /
                               (semiProductBillOfMaterial.Value * semiProductBillOfMaterial.Unit.ConversionRatio);
        result = GetFullBillOfMaterialForTreeViewResult(semiProductBillOfMaterial, nextfactor, level + 1);
      }
      result.EquivalentStuffsCount = billOfMaterialDetail.EquivalentStuffs.Count;
      result.StuffId = billOfMaterialDetail.StuffId;
      result.StuffCode = billOfMaterialDetail.Stuff.Code;
      result.StuffName = billOfMaterialDetail.Stuff.Name;
      result.Value = billOfMaterialDetail.Value * factor;
      result.UnitId = billOfMaterialDetail.UnitId;
      result.Level = level + 1;
      result.UnitName = billOfMaterialDetail.Unit.Name;
      result.BillOfMaterialDetailType = billOfMaterialDetail.BillOfMaterialDetailType;
      result.ForQty = billOfMaterialDetail.ForQty * factor;
      return result;
    }
    public void ConvertBomTreeToFlatBom(List<BillOfMaterialTreeResult> output, FullBillOfMaterialForTreeViewResult input, int rootStuffId, bool addSemiProductToResult)
    {
      var item = new BillOfMaterialTreeResult();
      if (rootStuffId != input.StuffId || addSemiProductToResult)
      {
        item.StuffId = input.StuffId;
        item.StuffCode = input.StuffCode;
        item.StuffName = input.StuffName;
        item.Version = input.Version;
        item.Title = input.Title;
        item.Value = input.Value;
        item.UnitId = input.UnitId;
        item.UnitName = input.UnitName;
        item.ParentStuffId = output.Count == 0 ? null : (int?)rootStuffId;
        output.Add(item);
      }
      if (input.BillOfMaterialDetails != null)
        input.BillOfMaterialDetails.ToList().ForEach(element =>
        {
          ConvertBomTreeToFlatBom(output, element, rootStuffId, addSemiProductToResult);
        });
    }
    public IQueryable<BillOfMaterialResult> SearchOrderItem(
       IQueryable<BillOfMaterialResult> query,
       string searchText,
       AdvanceSearchItem[] advanceSearchItems
       )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from billOfMaterial in query
                where billOfMaterial.Title.Contains(searchText) ||
                      billOfMaterial.StuffCode.Contains(searchText) ||
                      billOfMaterial.StuffName.Contains(searchText) ||
                      billOfMaterial.Description.Contains(searchText) ||
                      billOfMaterial.UserName.Contains(searchText)
                select billOfMaterial;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
  }
}