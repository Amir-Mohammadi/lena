using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffSerial;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Core;
using lena.Services.Internals.WarehouseManagement.Exception;
//using lena.Services.Common.Helpers;
//using System.Data.Entity.Infrastructure;
//using System.Data.Entity.SqlServer;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    internal StuffSerial AddStuffSerial(
        int stuffId,
        short? billOfMaterialVersion,
        int serialProfileCode,
        int order,
        long code,
        double initQty,
        byte initUnitId,
        string stuffCode,
        long batchNo,
        int? partitionStuffSerialId,
        bool isPacking,
        StuffSerialStatus stuffSerialStatus,
        string qualityControlDescription,
        DateTime? warehouseEnterTime = null,
        int? issueConfirmerUserId = null,
        int? issueUserId = null,
        int? productionOrderId = null
        )
    {
      var stuffSerial = repository.Create<StuffSerial>();
      stuffSerial.ProductionOrderId = productionOrderId;
      stuffSerial.Code = code;
      stuffSerial.StuffId = stuffId;
      stuffSerial.BillOfMaterialVersion = billOfMaterialVersion;
      stuffSerial.SerialProfileCode = serialProfileCode;
      stuffSerial.Order = order;
      stuffSerial.InitQty = initQty;
      stuffSerial.PartitionedQty = 0;
      stuffSerial.InitUnitId = initUnitId;
      stuffSerial.BatchNo = batchNo;
      stuffSerial.PartitionStuffSerialId = partitionStuffSerialId;
      stuffSerial.Status = stuffSerialStatus;
      stuffSerial.IsPacking = isPacking;
      stuffSerial.QualityControlDescription = qualityControlDescription;
      stuffSerial.WarehouseEnterTime = warehouseEnterTime;
      stuffSerial.IssueConfirmerUserId = issueConfirmerUserId;
      stuffSerial.IssueUserId = issueUserId;
      stuffSerial.Serial = GenerateSerial(
                stuffId: stuffId,
                serialProfileCode: serialProfileCode,
                code: code);
      stuffSerial.CRC = CRCHelper.CalculateCRC(stuffSerial.Serial);
      repository.Add(stuffSerial);
      return stuffSerial;
    }
    #endregion
    #region Edit
    internal StuffSerial EditStuffSerial(
        int stuffId,
        long code,
        byte[] rowVersion,
        TValue<short> billOfMaterialVersion = null,
        TValue<int> serialProfileCode = null,
        TValue<int> order = null,
        TValue<double> initQty = null,
        TValue<byte> initUnitId = null,
        TValue<string> stuffCode = null,
        TValue<long> batchNo = null,
        TValue<int> partitionStuffSerialId = null,
        TValue<bool> isPacking = null,
        TValue<StuffSerialStatus> stuffSerialStatus = null,
        TValue<string> qualityControlDescription = null,
        TValue<int> issueUserId = null,
        TValue<int> issueConfirmerUserId = null,
        TValue<int> assetId = null,
        TValue<DateTime> warehouseEnterTime = null
        )
    {
      var stuffSerial = GetStuffSerial(code: code, stuffId: stuffId);
      if (assetId != null)
      {
        var asset = GetAsset(id: assetId);
        stuffSerial.Asset = asset;
      }
      if (billOfMaterialVersion != null)
        stuffSerial.BillOfMaterialVersion = billOfMaterialVersion;
      if (serialProfileCode != null)
        stuffSerial.SerialProfileCode = serialProfileCode;
      if (order != null)
        stuffSerial.Order = order;
      if (initQty != null)
        stuffSerial.InitQty = initQty;
      if (initUnitId != null)
        stuffSerial.InitUnitId = initUnitId;
      if (batchNo != null)
        stuffSerial.BatchNo = batchNo;
      if (partitionStuffSerialId != null)
        stuffSerial.PartitionStuffSerialId = partitionStuffSerialId;
      if (stuffSerialStatus != null)
        stuffSerial.Status = stuffSerialStatus;
      if (isPacking != null)
        stuffSerial.IsPacking = isPacking;
      if (qualityControlDescription != null)
        stuffSerial.QualityControlDescription = qualityControlDescription;
      if (issueUserId != null)
        stuffSerial.IssueUserId = issueUserId;
      if (issueConfirmerUserId != null)
        stuffSerial.IssueConfirmerUserId = issueConfirmerUserId;
      if (warehouseEnterTime != null)
        stuffSerial.WarehouseEnterTime = warehouseEnterTime;
      repository.Update(stuffSerial, rowVersion);
      return stuffSerial;
    }
    //internal StuffSerial ModifySerialStuffFirstEnterUserInfo(int stuffId , long stuffCode, int userId , DateTime warehouseEnterTime , byte[] rowVersion)
    //{
    //    
    //        return stuffSerial;
    //    });
    //}
    #endregion
    #region SetQualityControlDescription
    internal StuffSerial SetStuffSerialQualityControlDescription(StuffSerial stuffSerial, byte[] rowVersion, string qualityControlDescription)
    {
      stuffSerial.QualityControlDescription = qualityControlDescription;
      repository.Update(stuffSerial, rowVersion);
      return stuffSerial;
    }
    #endregion
    #region SetStuffSerialPartitionedQty
    internal StuffSerial SetStuffSerialPartitionedQty(StuffSerial stuffSerial, byte[] rowVersion, double partitionedQty)
    {
      stuffSerial.PartitionedQty = partitionedQty;
      repository.Update(stuffSerial, rowVersion);
      return stuffSerial;
    }
    #endregion
    #region ModifySerialStatus
    private StuffSerial ModifySerialStatus(StuffSerial stuffSerial, byte[] rowVersion, StuffSerialStatus status)
    {
      if (stuffSerial == null)
      {
        throw new StuffSerialNotFoundException("");
      }
      stuffSerial.Status = status;
      repository.Update(stuffSerial, rowVersion);
      return stuffSerial;
    }
    internal StuffSerial ModifySerialStatus(long code, int stuffId, byte[] rowVersion,
        StuffSerialStatus status)
    {
      var stuffSerial = GetStuffSerial(selector: e => e, code: code, stuffId: stuffId); ; return ModifySerialStatus(stuffSerial: stuffSerial, rowVersion: rowVersion, status: status);
    }
    internal StuffSerial ModifySerialStatus(string serial, byte[] rowVersion,
        StuffSerialStatus status)
    {
      var stuffSerial = GetStuffSerial(selector: e => e, serial: serial); ; return ModifySerialStatus(stuffSerial: stuffSerial, rowVersion: rowVersion, status: status);
    }
    #endregion
    #region ModifiySerialLastActivity
    internal StuffSerial ModifySerialStuffLastActivity(StuffSerial stuffSerial)
    {
      stuffSerial.LastModified = DateTime.Now.ToUniversalTime();
      stuffSerial.LastModifiedUserId = App.Providers.Security.CurrentLoginData.UserId;
      repository.Update(stuffSerial, stuffSerial.RowVersion);
      return stuffSerial;
    }
    #endregion
    #region SetStuffSerialBillOfMaterialVersion
    private StuffSerial SetStuffSerialBillOfMaterialVersion(
        StuffSerial stuffSerial,
        byte[] rowVersion,
        short billOfMaterialVersion)
    {
      if (stuffSerial == null)
      {
        throw new StuffSerialNotFoundException("");
      }
      stuffSerial.BillOfMaterialVersion = billOfMaterialVersion;
      repository.Update(stuffSerial, rowVersion);
      return stuffSerial;
    }
    internal StuffSerial SetStuffSerialBillOfMaterialVersion(
        long code,
        int stuffId,
        byte[] rowVersion,
        short billOfMaterialVersion)
    {
      var stuffSerial = GetStuffSerial(selector: e => e, code: code, stuffId: stuffId);
      return SetStuffSerialBillOfMaterialVersion(
                stuffSerial: stuffSerial,
                rowVersion: rowVersion,
                billOfMaterialVersion: billOfMaterialVersion);
    }
    internal StuffSerial SetStuffSerialBillOfMaterialVersion(
        string serial,
        byte[] rowVersion,
        short billOfMaterialVersion)
    {
      var stuffSerial = GetStuffSerial(selector: e => e, serial: serial);
      return SetStuffSerialBillOfMaterialVersion(
                    stuffSerial: stuffSerial,
                    rowVersion: rowVersion,
                    billOfMaterialVersion: billOfMaterialVersion);
    }
    #endregion
    #region GenerateSerial
    internal string GenerateSerial(int stuffId, int serialProfileCode, long code)
    {
      var result = GetSerialProfile(
                    selector: e => new
                    {
                      StuffCode = e.Stuff.Code,
                      code = e.Code,
                      CooperatorCode = e.Cooperator.Code,
                      CooperatorId = e.CooperatorId
                    },
                    stuffId: stuffId,
                    code: serialProfileCode);
      return $"{result.StuffCode}{result.CooperatorCode}" + code.ToString("000000");
    }
    #endregion
    #region GetMaxOrder
    internal int GetMaxStuffSerialOrder(int stuffId)
    {
      var query = GetStuffSerials(
                    e => e,
                    stuffId: stuffId);
      var maxCode = query.Any() ? query.Max(i => i.Order) : 0;
      return maxCode;
    }
    #endregion
    #region GetMaxBatchNo
    internal long GetMaxStuffSerialBatchNo(int stuffId)
    {
      var dt = DateTime.UtcNow;
      var pc = new PersianCalendar();// DateTime.Now;
      var firstDateTimeOfYear = pc.ToDateTime(pc.GetYear(dt), 1, 1, 0, 0, 0, 0);
      var query = GetStuffSerials(
                    e => e,
                    stuffId: stuffId,
                    fromDateTime: firstDateTimeOfYear);
      var maxCode = query.Any() ? query.Max(i => i.BatchNo) : 0;
      return maxCode;
    }
    #endregion
    #region GetCountSerialInBatchNo
    internal int GetCountSerialInBatchNo(long batchNo)
    {
      var query = GetStuffSerials(
                    e => e,
                    batchNo: batchNo);
      var count = query.Any() ? query.Count() : 0;
      return count;
    }
    #endregion
    #region GetMaxCode
    internal long GetMaxStuffSerialCode(int stuffId)
    {
      var query = GetStuffSerials(
                    e => e.Code,
                    stuffId: stuffId);
      var maxCode = query.Any() ? query.Max(i => i) : 0;
      return maxCode;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetStuffSerials<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        TValue<long> code = null,//stuffSerialCode
        TValue<int> stuffId = null,
        TValue<string> stuffCode = null,
        TValue<int> billofMaterialVersion = null,
        TValue<long> batchNo = null,
        TValue<int> serialProfileCode = null,
        TValue<int> cooperatorId = null,
        TValue<string> cooperatorCode = null,
        TValue<int> order = null,
        TValue<string> fromSerial = null,
        TValue<string> toSerial = null,
        TValue<int> step = null,
        TValue<int> fromOrder = null,
        TValue<int> toOrder = null,
        TValue<string> serial = null,
        TValue<double> initQty = null,
        TValue<int> initUnitId = null,
        TValue<string> storReceiptCode = null,
        TValue<string> crc = null,
        TValue<string[]> serials = null,
        TValue<DateTime> dateTime = null,
        TValue<DateTime> fromDateTime = null,
        TValue<DateTime> toDateTime = null,
        TValue<StuffSerialStatus> status = null,
        TValue<StuffSerialStatus[]> statuses = null,
        TValue<int> productionOrderId = null,
        TValue<int?[]> stuffIds = null)
    {
      var query = repository.GetQuery<StuffSerial>();
      if (!string.IsNullOrWhiteSpace(serial))
      {
        serial = App.Internals.WarehouseManagement.CheckCrcAndGetSerial(serial);
        query = query.Where(i => i.Serial == serial);
      }
      if (productionOrderId != null)
        query = query.Where(i => i.ProductionOrderId == productionOrderId);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (stuffId != null)
        query = query.Where(i => i.StuffId == stuffId);
      if (!string.IsNullOrWhiteSpace(stuffCode))
        query = query.Where(i => i.Stuff.Code == stuffCode);
      if (serialProfileCode != null)
        query = query.Where(i => i.SerialProfileCode == serialProfileCode);
      if (order != null)
        query = query.Where(i => i.Order == order);
      if (cooperatorId != null)
        query = query.Where(i => i.SerialProfile.CooperatorId == cooperatorId);
      if (!string.IsNullOrWhiteSpace(cooperatorCode))
        query = query.Where(i => i.SerialProfile.Cooperator.Code == cooperatorCode);
      if (!string.IsNullOrWhiteSpace(crc))
        query = query.Where(i => i.CRC == crc);
      if (initQty != null)
        query = query.Where(i => i.InitQty == initQty);
      if (initUnitId != null)
        query = query.Where(i => i.InitUnitId == initUnitId);
      if (!string.IsNullOrWhiteSpace(storReceiptCode))
      {
        var storeRecipt = App.Internals.WarehouseManagement.GetStoreReceipts(
                  e => e,
                  code: storReceiptCode,
                  isDelete: false);
        if (storeRecipt.Any())
        {
          int id = storeRecipt.FirstOrDefault().Id;
          var store = App.Internals.WarehouseManagement.GetStoreReceiptSerialProfile(e => e, storeReceiptId: id);
          query = query.Where(i => i.SerialProfileCode == store.Code && i.StuffId == store.StuffId);
        }
        //query = query.Where(i => (i.SerialProfile as StoreReceiptSerialProfile).StoreReceipt.Code== storReceiptCode);
      }
      if (serials != null)
        query = query.Where(i => serials.Value.Contains(i.Serial));
      if (toDateTime != null)
        query = query.Where(i => i.SerialProfile.DateTime <= toDateTime);
      if (dateTime != null)
        query = query.Where(i => i.SerialProfile.DateTime == dateTime);
      if (fromDateTime != null)
        query = query.Where(i => i.SerialProfile.DateTime >= fromDateTime);
      if (toDateTime != null)
        query = query.Where(i => i.SerialProfile.DateTime <= toDateTime);
      if (batchNo != null)
        query = query.Where(i => i.BatchNo == batchNo);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (statuses != null)
        query = query.Where(i => statuses.Value.Contains(i.Status));
      if (!string.IsNullOrWhiteSpace(fromSerial))
      {
        var fromSerial1 = CheckCrcAndGetSerial(fromSerial);
        query = query.Where(i => i.Serial.CompareTo(fromSerial1) >= 0);
      }
      if (!string.IsNullOrWhiteSpace(toSerial))
      {
        var toSerial1 = CheckCrcAndGetSerial(toSerial);
        query = query.Where(i => i.Serial.CompareTo(toSerial1) <= 0);
      }
      if (fromOrder != null)
        query = query.Where(i => i.Order >= fromOrder);
      if (toOrder != null)
        query = query.Where(i => i.Order <= toOrder);
      if (step != null)     // بهتر است این فیلتر به خاطر toList داشتن آخر از همه اعمال شود
      {
        var queryCount = query.Count();
        var maximumReprintAmount = App.Internals.ApplicationSetting.GetMaximumReprintAmount();
        if (queryCount > maximumReprintAmount)
        {
          throw new SerialQuantityIsMoreThanMaximumException(maximumReprintAmount);
        }
        query = query.OrderBy(x => x.Code);
        var queryList = query.ToList();
        var queryCodeList = new List<long>();
        for (int j = 0; j < queryCount; j += step)
        {
          queryCodeList.Add(queryList[j].Code);
        }
        var queryCodes = queryCodeList.ToArray();
        query = from stuffSerial in query
                where queryCodes.Contains(stuffSerial.Code)
                select stuffSerial;
      }
      if (stuffIds != null)
        query = query.Where(i => stuffIds.Value.Contains(i.StuffId));
      return query.Select(selector);
    }
    internal IQueryable<StuffSerialWithOperationInfoResult> GetStuffSerialsWithLastOperationInfo<StuffSerialWithOperationInfoResult>()
    {
      return repository.CreateContextQuery<StuffSerialWithOperationInfoResult>("[GetStuffSerialsWithLastOperationInfo]()");
    }
    #endregion
    #region Check CRC And Get Serail
    internal string CheckCrcAndGetSerial(string serial)
    {
      if (serial == null || serial.Length < 9)
      {
        throw new SerialLengthIsNotValidException(serial);
      }
      if (!string.IsNullOrWhiteSpace(serial))
      {
        serial = serial.Trim();
        var serialWithoutCrc = "";
        var hasCrc = HasCrc(serial);
        if (hasCrc == false)
        {
          serialWithoutCrc = serial;
        }
        else
        {
          var crc = serial.Substring(0, 4);
          serialWithoutCrc = serial.Remove(0, 4);
          if (CheckCrc(serialWithoutCrc, crc) == false)
            throw new SerialCrcException(serial: serial);
        }
        if (string.IsNullOrWhiteSpace(serialWithoutCrc))
        {
          throw new StuffSerialNotFoundException(serial: serial);
        }
        return serialWithoutCrc;
      }
      return null;
    }
    public bool HasCrc(string serial)
    {
      var hasCrc = true;
      var cooperatorCode = serial.Substring(4, 4);
      var cooperators = App.Internals.SaleManagement.GetCooperators(
                selector: e => e,
                Code: cooperatorCode);
      if (cooperators.Any())
      {
        hasCrc = false;
        var stuffCode = serial.Substring(0, 4);
        var stuffs = App.Internals.SaleManagement.GetStuffs(selector: e => e.Id, code: stuffCode);
        if (stuffs.Any())
          hasCrc = false;
        else
          hasCrc = true;
      }
      return hasCrc;
    }
    public static bool CheckCrc(string serial, string crc)
    {
      return crc == CRCHelper.CalculateCRC(serial);
      //return true;
    }
    #endregion
    #region Get
    internal StuffSerial GetStuffSerial(string serial) => GetStuffSerial(
        selector: e => e, serial: serial);
    internal TResult GetStuffSerial<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        string serial)
    {
      var serialProfile = GetStuffSerials(
                        selector: selector,
                        serial: serial)
                    .FirstOrDefault();
      if (serialProfile == null)
        throw new StuffSerialNotFoundException(serial: serial);
      return serialProfile;
    }
    internal StuffSerialForSerialCountingResult GetStuffSerialForSerialTagCounting(
       string serial,
       int stockCheckingId,
       short warehouseId,
       int tagTypeId)
    {
      var stuffSerial = GetStuffSerials(selector: i => new StuffSerialForSerialCountingResult
      {
        Code = i.Code,
        StuffId = i.StuffId,
        BillOfMaterialVersion = i.BillOfMaterialVersion,
        StuffType = i.Stuff.StuffType,
        StuffCode = i.Stuff.Code,
        StuffName = i.Stuff.Name,
        StuffNoun = i.Stuff.Noun,
        StuffTitle = i.Stuff.Title,
        SerialProfileCode = i.SerialProfileCode,
        BatchNo = i.BatchNo,
        DateTime = i.SerialProfile.DateTime,
        CooperatorId = i.SerialProfile.CooperatorId,
        CooperatorName = i.SerialProfile.Cooperator.Name,
        Order = i.Order,
        Serial = i.Serial,
        InitQty = i.InitQty,
        PartitionedQty = i.PartitionedQty,
        Qty = i.InitQty - i.PartitionedQty,
        InitUnitId = i.InitUnitId,
        InitUnitName = i.InitUnit.Name,
        InitUnitConversionRatio = i.InitUnit.ConversionRatio,
        Status = i.Status,
        QualityControlDescription = i.QualityControlDescription,
        UserName = i.SerialProfile.User.UserName,
        FullEmployeeName = i.SerialProfile.User.Employee.FirstName + " " + i.SerialProfile.User.Employee.LastName,
        RowVersion = i.RowVersion
      },
                serial: serial)
                .FirstOrDefault();
      if (stuffSerial == null)
        throw new StuffSerialNotFoundException(serial: serial);
      //var serialProfile = GetStuffSerials(
      //        selector: selector,
      //        serial: serial)
      //    
      //    
      //    .FirstOrDefault();
      var countBoxStockChecking = GetStockCheckingTags(
             selector: i => new
             {
               Id = i.Id,
               StockCheckingId = i.StockCheckingId,
               WarehouseId = i.WarehouseId,
               StuffId = i.StuffId,
               StuffSerialCode = i.StuffSerialCode,
               Amount = i.Amount * i.Unit.ConversionRatio
             },
             stockCheckingId: stockCheckingId,
             warehouseId: warehouseId,
             stuffId: stuffSerial.StuffId,
             tagTypeId: tagTypeId)
         .Count();
      var countBoxStocks = App.Internals.WarehouseManagement.GetWarehouseInventories(
                        warehouseId: warehouseId,
                        groupBySerial: true,
                        stuffId: stuffSerial.StuffId)
                    .Where(x => x.TotalAmount > 0).Count();
      stuffSerial.CountBoxStockChecking = countBoxStockChecking;
      stuffSerial.CountBoxStocks = countBoxStocks;
      return stuffSerial;
    }
    internal StuffSerial GetStuffSerial(long code, int stuffId) => GetStuffSerial(
        selector: e => e,
        stuffId: stuffId,
        code: code);
    internal TResult GetStuffSerial<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        long code,
        int stuffId)
    {
      var serialProfile = GetStuffSerials(
                    selector: selector,
                    code: code,
                    stuffId: stuffId)
                .FirstOrDefault();
      if (serialProfile == null)
        throw new StuffSerialNotFoundException(code: code, stuffId: stuffId);
      return serialProfile;
    }
    #endregion
    #region GetStuffSerialProductionOrderProcess
    internal TResult GetStuffSerialProductionOrderProcess<TResult>(
    Expression<Func<lena.Domains.Production, TResult>> selector,
    string serial)
    {
      var production = App.Internals.Production.GetProductions(
                    selector: selector,
                    serial: serial)
                .FirstOrDefault();
      return production;
    }
    #endregion
    #region GetStuffSerialProcess
    internal TResult GetStuffSerialProcess<TResult>(
    Expression<Func<StuffSerial, TResult>> selector,
    string serial)
    {
      var serialProfile = GetStuffSerials(
                    selector: selector,
                    serial: serial)
                .FirstOrDefault();
      if (serialProfile == null)
        throw new StuffSerialNotFoundException(serial: serial);
      return serialProfile;
    }
    #endregion
    #region AddStuffSerials
    internal IQueryable<TResult> AddStuffSerials<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        SerialProfile serialProfile,
        int? partitionStuffSerialId,
        int stuffId,
        int? productionOrderId,
        short? billOfMaterialVersion,
        double qty,
        byte unitId,
        bool isPacking,
        double qtyPerBox,
        int? boxCount,
        string qualityControlDescription = null,
        StuffSerialStatus? stuffSerialStatus = null,
        DateTime? warehouseEnterTime = null,
        int? issueConfirmerUserId = null,
        int? issueUserId = null
        )
    {
      #region GetOrder
      int order = 0;
      if (partitionStuffSerialId.HasValue)
      {
        #region GetStuffSerialOrderOrder
        order = GetPartitionStuffSerial(
        selector: e => e.MainStuffSerial.Order,
        id: partitionStuffSerialId.Value);
        #endregion
      }
      else
      {
        #region GetMaxOrder
        order = GetMaxStuffSerialOrder(stuffId);
        order++;
        #endregion
      }
      var maxBatchNo = GetMaxStuffSerialBatchNo(stuffId); ; var countSerialInBatchNo = GetCountSerialInBatchNo(maxBatchNo);
      var serialBatchCount = App.Internals.ApplicationSetting.GetSerialBatchCount();
      var batchNo = maxBatchNo;
      #endregion
      #region AddStuffSerials
      var remainedQty = qty;
      if (boxCount != null)
      {
        if (boxCount * qtyPerBox < qty)
        {
          int boxCountTemp = boxCount.Value;
          remainedQty = boxCountTemp * qtyPerBox;
        }
      }
      var stuffSerials = new List<StuffSerial>();
      var requestedPrintCount = qty / qtyPerBox;
      #region GetMaxSerialCode
      var serialCode = GetMaxStuffSerialCode(stuffId: stuffId);
      #endregion
      while (remainedQty > 0)
      {
        countSerialInBatchNo++;
        if (countSerialInBatchNo > serialBatchCount)
        {
          batchNo++;
          countSerialInBatchNo = 1;
        }
        serialCode++;
        var initQty = remainedQty > qtyPerBox ? qtyPerBox : remainedQty;
        var serialStatus = StuffSerialStatus.WithoutOperation;
        if (stuffSerialStatus != null)
        {
          serialStatus = stuffSerialStatus.Value;
        }
        var productionOrderStuffSerial = GetStuffSerials(e => e, productionOrderId: productionOrderId);
        var printedCount = productionOrderStuffSerial.Count();
        if (productionOrderId != null)
        {
          var productionOrder = App.Internals.Production.GetProductionOrder(id: productionOrderId.Value);
          double permittedCount = productionOrder.Qty + productionOrder.ToleranceQty;
          if (printedCount + requestedPrintCount > permittedCount)
          {
            throw new TheNumberOfProductionOrdersIsLessThanTheRequestedNumber(permittedCount);
          }
          if (stuffId != productionOrder.WorkPlanStep.WorkPlan.BillOfMaterial.StuffId)
          {
            throw new StuffCodeIsNotBelongToTheProductionOrderException();
          }
        }
        var stuffSerial = AddStuffSerial(
                      productionOrderId: productionOrderId,
                      stuffId: stuffId,
                      billOfMaterialVersion: billOfMaterialVersion,
                      serialProfileCode: serialProfile.Code,
                      code: serialCode,
                      order: order,
                      isPacking: isPacking,
                      initQty: initQty,
                      initUnitId: unitId,
                      stuffCode: serialProfile.Stuff.Code,
                      batchNo: batchNo,
                      partitionStuffSerialId: partitionStuffSerialId,
                      stuffSerialStatus: serialStatus,
                      qualityControlDescription: qualityControlDescription,
                      warehouseEnterTime: warehouseEnterTime,
                      issueConfirmerUserId: issueConfirmerUserId,
                      issueUserId: issueUserId);
        stuffSerials.Add(stuffSerial);
        remainedQty = remainedQty - initQty;
        requestedPrintCount--;
      }
      #endregion
      var serials = stuffSerials.Select(i => i.Serial).ToArray();
      return GetStuffSerials(
                    selector: selector,
                    serials: serials);
    }
    #endregion
    #region AddProductionStuffSerials
    internal IQueryable<StuffSerialResult> AddProductionStuffSerials(
        int stuffId,
        int? productionOrderId,
        short? billOfMaterialVersion,
        double qty,
        byte unitId,
        double qtyPerBox,
        bool isPacking,
        SerialType serialType)
    {
      #region GetCompanyId
      var companyId = App.Internals.ApplicationSetting.GetCompanyId();
      #endregion
      #region AddProductionSerialProfile
      var productionSerialProfile = AddProductionSerialProfile(
          productionSerialProfile: null,
          stuffId: stuffId,
          cooperatorId: companyId);
      #endregion
      #region GetBillOfMareialVersion and StuffSerialStatus
      StuffSerialStatus? stuffSerialStatus = null;
      if (serialType == SerialType.ReturnOfSale)
      {
        billOfMaterialVersion = App.Internals.Planning.GetPublishedBillOfMaterial(stuffId: stuffId)
                  .Version;
        stuffSerialStatus = StuffSerialStatus.Completed;
      }
      #endregion
      #region AddStuffSerials
      var stuffSerials = AddStuffSerials(
          selector: e => e,
          serialProfile: productionSerialProfile,
          stuffId: stuffId,
          productionOrderId: productionOrderId,
          billOfMaterialVersion: billOfMaterialVersion,
          qty: qty,
          isPacking: isPacking,
          unitId: unitId,
          qtyPerBox: qtyPerBox,
          boxCount: null,
          partitionStuffSerialId: null,
          stuffSerialStatus: stuffSerialStatus);
      #endregion
      #region Simulate Serial Production For Return of sale type
      if (serialType == SerialType.ReturnOfSale)
      {
        foreach (var returnOfSaleSerial in stuffSerials)
        {
          var lastProduction = App.Internals.Production.GetProductions(selector: e => e,
                        stuffSerialStuffId: stuffId,
                        status: ProductionStatus.Produced)
                    .OrderByDescending(i => i.Id)
                    .FirstOrDefault();
          if (lastProduction == null)
          {
            throw new CannotFindAnyProductionRecordsForThisReturnOfSaleStuffException();
          }
          var lastProductionSerial = GetStuffSerial(
                        stuffId: lastProduction.StuffSerialStuffId,
                        code: lastProduction.StuffSerialCode);
          SetStuffSerialBillOfMaterialVersion(
                    stuffId: returnOfSaleSerial.StuffId,
                    code: returnOfSaleSerial.Code,
                    billOfMaterialVersion: lastProductionSerial.BillOfMaterialVersion.Value,
                    rowVersion: returnOfSaleSerial.RowVersion);
          var lastProductionOrderOfThisProduct = lastProduction.ProductionOrderId;
          var virtualProduction = App.Internals.Production.AddProduction(
                        description: "SIM RETURN OF SALE",
                        productionOrderId: lastProductionOrderOfThisProduct,
                        stuffSerialStuffId: returnOfSaleSerial.StuffId,
                        stuffSerialCode: returnOfSaleSerial.Code); ; var productionOrder = App.Internals.Production.GetProductionOrder(lastProductionOrderOfThisProduct);
          var sequences = App.Internals.Planning.GetOperationSequences(
                        selector: e => new { e.OperationId },
                        workPlanStepId: productionOrder.WorkPlanStepId);
          productionOrder.WorkPlanStep.OperationSequences.ToList();
          foreach (var operation in sequences)
          {
            var transactionBatch = App.Internals.WarehouseManagement.AddTransactionBatch();
            var stuffSerial = virtualProduction.StuffSerial;
            var productionOperationEmployeeGroup = App.Internals.Production.GetOrAddProductionOperationEmployeeGroup(employeeIds: new int[] { App.Providers.Storage.SystemEmplyeeId });
            var productionOperation = App.Internals.Production.AddProductionOperation(
                          transactionBatchId: transactionBatch.Id,
                          description: "SIM OPERATION",
                          operationId: operation.OperationId,
                          productionId: virtualProduction.Id,
                          productionOperationStatus: ProductionOperationStatus.Done,
                          productionOperatorId: null,
                          productionOperationEmployeeGroupId: productionOperationEmployeeGroup.Id,
                          qty: (stuffSerial.InitQty - stuffSerial.PartitionedQty),
                          productionTerminalId: App.Providers.Storage.SystemProductionTerminalId,
                          time: 0);
          }
        }
      }
      #endregion
      var serials = stuffSerials.Select(App.Internals.WarehouseManagement.ToStuffSerialResult);
      return serials;
    }
    #endregion
    #region AddStoreReceiptStuffSerials
    internal IQueryable<TResult> AddStoreReceiptStuffSerials<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        int storeReceiptId,
        int stuffId,
        short? billOfMaterialVersion,
        double qty,
        int? productionOrderId,
        byte unitId,
        double qtyPerBox)
    {
      #region Get StoreReceipt
      var storeReceipt = GetStoreReceipt(storeReceiptId);
      #endregion
      #region AddStoreReceiptSerialProfile
      var storeReceiptSerialProfile = AddStoreReceiptSerialProfile(
          storeReceiptSerialProfile: null,
          stuffId: stuffId,
          storeReceiptId: storeReceiptId,
          cooperatorId: storeReceipt.CooperatorId);
      #endregion
      #region AddStuffSerials
      var stuffSerials = AddStuffSerials(
      selector: e => e,
      productionOrderId: productionOrderId,
      serialProfile: storeReceiptSerialProfile,
      partitionStuffSerialId: null,
      stuffId: stuffId,
      billOfMaterialVersion: billOfMaterialVersion,
      qty: qty,
      unitId: unitId,
      isPacking: false,
      boxCount: null,
      qtyPerBox: qtyPerBox,
      warehouseEnterTime: DateTime.UtcNow,
      issueUserId: App.Providers.Security.CurrentLoginData.UserId);
      #endregion
      return stuffSerials.Select(selector);
    }
    #endregion
    #region PrintStuffSerials
    internal void PrintStuffSerials(
        IQueryable<StuffSerialResult> stuffSerialsList,
        SerialPrintType printType,
        int printerId,
        string reportName = null,
        bool printFooterText = false,
        bool printVersion = false,
        string billOfMaterialVersion = null
        )
    {
      #region PrintBarcodes
      //#if !DEBUG
      if (printType != SerialPrintType.CustomTemplate)
        App.Internals.PrinterManagment.PrintBarcodesWithResult(
                      stuffSerials: stuffSerialsList,
                      printerId: printerId,
                      printType: printType,
                      printFooterText: printFooterText,
                      printVersion: printVersion,
                      billOfMaterialVersion: billOfMaterialVersion);
      else
      {
        App.Internals.ReportManagement.Print(
                  reportName: reportName,
                  reportData: stuffSerialsList,
                  printerId: printerId,
                  numberOfCopies: 1,
                  reportParams: null);
      }
      //#endif
      #endregion
    }
    #endregion
    #region PrintStuffSerial
    internal StuffSerial PrintStuffSerial(
        SerialPrintType printType,
        int printerId,
        int stuffId,
        string serial,
        string reportName,
        bool printBarcodeFooter,
        bool printVersion = false,
        string billOfMaterialVersion = null) => PrintStuffSerial(
        selector: e => e,
        printType: printType,
        printerId: printerId,
        stuffId: stuffId,
        serial: serial,
        reportName: reportName,
        printBarcodeFooter: printBarcodeFooter,
        printVersion: printVersion,
        billOfMaterialVersion: billOfMaterialVersion);
    internal TResult PrintStuffSerial<TResult>(
        Expression<Func<StuffSerial, TResult>> selector,
        SerialPrintType printType,
        int printerId,
        int stuffId,
        string serial,
        string reportName,
        bool printBarcodeFooter,
        bool printVersion = false,
        string billOfMaterialVersion = null)
    {
      var stuffSerial = GetStuffSerial(
                    selector: selector,
                    serial: serial);
      if (stuffSerial == null)
        throw new StuffSerialNotFoundException(serial: serial);
      IQueryable<StuffSerial> stuffSerials = stuffSerial as IQueryable<StuffSerial>;
      if (printType != SerialPrintType.CustomTemplate)
        App.Internals.PrinterManagment.PrintBarcodes(
                      stuffSerials: stuffSerials,
                      printerId: printerId,
                      printType: printType,
                      printFooterText: printBarcodeFooter,
                      printVersion: printVersion,
                      billOfMaterialVersion: billOfMaterialVersion
                      );
      else
      {
        var result = stuffSerials.Select(ToStuffSerialResult).ToList();
        App.Internals.ReportManagement.Print(
                  reportName: reportName,
                  reportData: result,
                  printerId: printerId,
                  numberOfCopies: 1,
                  reportParams: null);
      }
      return stuffSerial;
    }
    #endregion
    #region GetStuffSerialStock
    public StuffSerialStockResult GetStuffSerialStock(
        int? warehouseId,
        int? version,
        string serial)
    {
      #region GetWarehouseInventories
      var serialInventory = GetWarehouseInventories(
              warehouseId: warehouseId,
              groupByBillOfMaterialVersion: true,
              billOfMaterialVersion: version,
              groupBySerial: true,
              serial: serial)
       .ToList();
      #endregion
      var result = serialInventory.Select(
          i => new StuffSerialStockResult
          {
            StuffId = i.StuffId,
            StuffName = i.StuffName,
            StuffCode = i.StuffCode,
            Version = i.BillOfMaterialVersion,
            Serial = serial,
            StuffSerialCode = i.StuffSerialCode.Value,
            Amount = i.TotalAmount.Value,
            UnitId = i.UnitId,
            UnitName = i.UnitName,
            WarehouseId = warehouseId,
            WarehouseInventories = serialInventory
          })
          .FirstOrDefault();
      if (result == null)
      {
        var stuffSerial = App.Internals.WarehouseManagement.GetStuffSerial(
                  serial: serial);
        var stuff = App.Internals.SaleManagement.GetStuff(stuffSerial.StuffId);
        var unit = stuff.UnitType.Units.SingleOrDefault(i => i.IsMainUnit);
        var sum = serialInventory.Sum(i => i.TotalAmount) ?? 0;
        return new StuffSerialStockResult()
        {
          StuffId = stuff.Id,
          StuffName = stuff.Name,
          StuffCode = stuff.Code,
          Version = version ?? stuffSerial.BillOfMaterialVersion,
          Serial = serial,
          StuffSerialCode = stuffSerial.Code,
          Amount = sum,
          UnitId = unit.Id,
          UnitName = unit.Name,
          UnitConversionRatio = unit.ConversionRatio,
          WarehouseId = warehouseId,
          WarehouseInventories = serialInventory
        };
      }
      return result;
    }
    #endregion
    #region ToResult
    internal Expression<Func<StuffSerial, StuffSerialResult>> ToStuffSerialResult =
     entity => new StuffSerialResult
     {
       LinkedSerial = entity.LinkSerial.LinkedSerial,
       Code = entity.Code,
       StuffId = entity.StuffId,
       BillOfMaterialVersion = entity.BillOfMaterialVersion,
       StuffType = entity.Stuff.StuffType,
       StuffCode = entity.Stuff.Code,
       StuffName = entity.Stuff.Name,
       StuffNoun = entity.Stuff.Noun,
       StuffTitle = entity.Stuff.Title,
       SerialProfileCode = entity.SerialProfileCode,
       BatchNo = entity.BatchNo,
       DateTime = entity.SerialProfile.DateTime,
       CooperatorId = entity.SerialProfile.CooperatorId,
       CooperatorCode = entity.SerialProfile.Cooperator.Code,
       CooperatorName = entity.SerialProfile.Cooperator.Name,
       Order = entity.Order,
       Serial = entity.Serial,
       InitQty = entity.InitQty,
       PartitionedQty = entity.PartitionedQty,
       Qty = entity.InitQty - entity.PartitionedQty,
       InitUnitId = entity.InitUnitId,
       InitUnitName = entity.InitUnit.Name,
       InitUnitConversionRatio = entity.InitUnit.ConversionRatio,
       Status = entity.Status,
       QualityControlDescription = entity.QualityControlDescription,
       UserName = entity.SerialProfile.User.UserName,
       FullEmployeeName = entity.SerialProfile.User.Employee.FirstName + " " + entity.SerialProfile.User.Employee.LastName,
       CRC = entity.CRC,
       FullSerial = (entity.CRC ?? "") + entity.Serial,
       RowVersion = entity.RowVersion,
       LastOperationId = null,
       LastOperationName = null,
       LastOperationOperatorName = null,
       ProductOrderId = entity.ProductionOrderId,
       ProductOrderCode = entity.ProductionOrder.Code
     };
    internal Expression<Func<lena.Domains.Production, StuffSerialProductionOrderResult>> ToStuffSerialProductionOrderResult =
    entity => new StuffSerialProductionOrderResult
    {
      ProductionOrderId = entity.ProductionOrderId,
      ProductionOrderCode = entity.ProductionOrder.Code,
      WorkPlanTitle = entity.ProductionOrder.WorkPlanStep.WorkPlan.Title,
      WorkPlanVersion = entity.ProductionOrder.WorkPlanStep.WorkPlan.Version,
      StartDateTime = entity.ProductionOrder.CalendarEvent.DateTime,
      ToDateTime = entity.ProductionOrder.CalendarEvent.DateTime.AddSeconds(entity.ProductionOrder.CalendarEvent.Duration),
      SupervisorEmployeeFullName = entity.ProductionOrder.Employee.FirstName + " " + entity.ProductionOrder.Employee.LastName,
    };
    #endregion
    #region Search
    public IQueryable<StuffSerialResult> SearchStuffSerailResult(
        IQueryable<StuffSerialResult> query,
        AdvanceSearchItem[] advanceSearchItems,
        int? cooperatorId,
        int? warehouseId,
        int? billOfMaterialVersion,
        string fromSerial,
        string toSerial,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from stuffSerial in query
                where stuffSerial.Serial.Contains(searchText) ||
                      stuffSerial.StuffCode.Contains(searchText) ||
                      stuffSerial.StuffName.Contains(searchText) ||
                      stuffSerial.ProductOrderCode.Contains(searchText) ||
                      stuffSerial.FullEmployeeName.Contains(searchText) ||
                      stuffSerial.InitUnitName.Contains(searchText)
                select stuffSerial;
      if (cooperatorId != null)
        query = query.Where(i => i.CooperatorId == cooperatorId);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      if (billOfMaterialVersion != null)
        query = query.Where(i => i.BillOfMaterialVersion == billOfMaterialVersion);
      //if (fromSerial != null)
      //    query = query.Where(i => i.Serial >= fromSerial);
      //if (toSerial != null)
      //    query = query.Where(i => i.Serial <= toSerial);
      //CacheSystem          
      if (warehouseId != null)
        query = query.Where(i => i.WarehouseId == warehouseId);
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffSerialResult> SortStuffSerial(IQueryable<StuffSerialResult> input,
        SortInput<StuffSerialSortType> options)
    {
      switch (options.SortType)
      {
        case StuffSerialSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case StuffSerialSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffSerialSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case StuffSerialSortType.BillOfMaterialVersion:
          return input.OrderBy(i => i.BillOfMaterialVersion, options.SortOrder);
        case StuffSerialSortType.BatchNo:
          return input.OrderBy(i => i.BatchNo, options.SortOrder);
        case StuffSerialSortType.DateTime:
          return input.OrderBy(i => i.DateTime, options.SortOrder);
        case StuffSerialSortType.Serial:
          return input.OrderBy(i => i.Serial, options.SortOrder);
        case StuffSerialSortType.Order:
          return input.OrderBy(i => i.Order, options.SortOrder);
        case StuffSerialSortType.Status:
          return input.OrderBy(i => i.Status, options.SortOrder);
        case StuffSerialSortType.CooperatorName:
          return input.OrderBy(i => i.CooperatorName, options.SortOrder);
        case StuffSerialSortType.InitQty:
          return input.OrderBy(i => i.InitQty, options.SortOrder);
        case StuffSerialSortType.InitUnitName:
          return input.OrderBy(i => i.InitUnitName, options.SortOrder);
        case StuffSerialSortType.WarehouseName:
          return input.OrderBy(i => i.WarehouseName, options.SortOrder);
        case StuffSerialSortType.FullSerial:
          return input.OrderBy(i => i.FullSerial, options.SortOrder);
        case StuffSerialSortType.CRC:
          return input.OrderBy(i => i.CRC, options.SortOrder);
        case StuffSerialSortType.FullEmployeeName:
          return input.OrderBy(i => i.FullEmployeeName, options.SortOrder);
        case StuffSerialSortType.ProductOrderCode:
          return input.OrderBy(i => i.ProductOrderCode, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region GetBillOfMaterialVersionOfSerial
    public short? GetBillOfMaterialVersionOfSerial(int stuffId, long? stuffSerialCode)
    {
      if (stuffSerialCode == null)
      {
        return default(short?);
      }
      #region GetFromStuffSerial
      var billOfMaterialVersion = GetStuffSerials(
          selector: e => e.BillOfMaterialVersion,
          stuffId: stuffId,
          code: stuffSerialCode)
      .FirstOrDefault();
      #endregion
      #region GetFromProduction
      if (billOfMaterialVersion == null)
      {
        billOfMaterialVersion = App.Internals.Production.GetProductions(
                                        e => (short?)e.ProductionOrder.WorkPlanStep.WorkPlan.BillOfMaterialVersion,
                                        stuffSerialCode: stuffSerialCode,
                                        stuffSerialStuffId: stuffId)
                                     .FirstOrDefault();
      }
      #endregion
      #region GetFromStock
      if (billOfMaterialVersion == null)
      {
        #region GetStuffWarehouseTransactions
        var transactions = App.Internals.WarehouseManagement.GetWarehouseTransactions(
        selector: e => e,
        stuffId: stuffId,
        stuffSerialCodes: new long?[] { stuffSerialCode });
        billOfMaterialVersion = transactions.FirstOrDefault()?.BillOfMaterialVersion;
        #endregion
      }
      #endregion
      #region GetPublishVersion
      if (billOfMaterialVersion == null)
      {
        var bom = App.Internals.Planning.GetBillOfMaterials(stuffId: stuffId, isPublished: true)
               .FirstOrDefault();
        if (bom != null)
          billOfMaterialVersion = bom.Version;
      }
      #endregion
      return billOfMaterialVersion;
    }
    #endregion
  }
}