using System.Linq;
using System.Reflection;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
//using Parlar.DAL.UnitOfWorks;
using lena.Domains.Enums;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase
{
  public partial class ApplicationBase
  {
    internal void InitStaticData()
    {
      _addTransactionType();
      _addtypeOfFinancialTransactionType();
      _addActions();
      _addDefaultSetting();
      _addBaseEntityConfirmTypes();
      _addExitReceiptRequestType();
    }
    private static void _addDefaultSetting()
    {
      var settingService = App.Internals.ApplicationSetting;
      settingService.AddApplicationSetting(SettingKey.UserMaxFailedLoginCount, "5");
      settingService.AddApplicationSetting(SettingKey.UserLockOutTime, "12");
      settingService.AddApplicationSetting(SettingKey.BarcodeOneColumnReport, "BarcodeOneColumnReport");
      settingService.AddApplicationSetting(SettingKey.BarcodeThreeColumnReport, "BarcodeThreeColumnReport");
      settingService.AddApplicationSetting(SettingKey.CompanyId, "0002");
      settingService.AddApplicationSetting(SettingKey.NumberOfPricesForAveraging, "5");
      settingService.AddApplicationSetting(SettingKey.MaxPublishedBomCount, "1");
      settingService.AddApplicationSetting(SettingKey.SerialBatchCount, "20");
      settingService.AddApplicationSetting(SettingKey.MinimumSerialBufferAmount, "5");
      settingService.AddApplicationSetting(SettingKey.BarcodeLabelFooterText, "Parlar.ir {Date:\"yy\"}");
      settingService.AddApplicationSetting(SettingKey.NormalBoardTime, "408");
    }
    private static void _addExitReceiptRequestType()
    {
      var orderType = StaticExitReceiptRequestTypes.OrderExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          orderType.Id,
          orderType.Title,
          orderType.AutoConfirm,
          orderType.IsActive,
          orderType.Description
          );
      var saleType = StaticExitReceiptRequestTypes.SaleExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          saleType.Id,
          saleType.Title,
          saleType.AutoConfirm,
          saleType.IsActive,
          saleType.Description
          );
      var lendingType = StaticExitReceiptRequestTypes.LendingExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          lendingType.Id,
          lendingType.Title,
          lendingType.AutoConfirm,
          lendingType.IsActive,
          lendingType.Description
          );
      var sampleType = StaticExitReceiptRequestTypes.SampleExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          sampleType.Id,
          sampleType.Title,
          sampleType.AutoConfirm,
          sampleType.IsActive,
          sampleType.Description
          );
      var giftType = StaticExitReceiptRequestTypes.GiftExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          giftType.Id,
          giftType.Title,
          giftType.AutoConfirm,
          giftType.IsActive,
          giftType.Description
          );
      var givebackType = StaticExitReceiptRequestTypes.GivebackExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          givebackType.Id,
          givebackType.Title,
          givebackType.AutoConfirm,
          givebackType.IsActive,
          givebackType.Description
          );
      var disposalOfWasteType = StaticExitReceiptRequestTypes.DisposalOfWasteExitReceiptRequest;
      App.Internals.WarehouseManagement.AddExitReceiptRequestType(
          disposalOfWasteType.Id,
          disposalOfWasteType.Title,
          disposalOfWasteType.AutoConfirm,
          disposalOfWasteType.IsActive,
          disposalOfWasteType.Description
          );
    }
    private static void _addBaseEntityConfirmTypes()
    {
      var typeOfBaseEntityConfirmType = typeof(BaseEntityConfirmType);
      var transacts = typeOfBaseEntityConfirmType.GetProperties(BindingFlags.Static | BindingFlags.Public)
          .Select(i => i.GetValue(typeOfBaseEntityConfirmType)).OfType<lena.Domains.BaseEntityConfirmType>().ToArray();
      var mod = App.Internals.Confirmation;
      var baseEntityConfirmTypes = mod.GetBaseEntityConfirmTypes(e => e).ToList();
      foreach (var type in transacts)
      {
        var baseEntityConfirmType = baseEntityConfirmTypes.FirstOrDefault(x => x.Id == type.Id);
        if (baseEntityConfirmType == null)
        {
          baseEntityConfirmType = mod.AddBaseEntityConfirmType(
                  id: type.Id,
                  departmentId: type.DepartmentId,
                  userId: type.UserId,
                  confirmType: type.ConfirmType,
                  confirmPageUrl: type.ConfirmPageUrl);
          type.Id = baseEntityConfirmType.Id;
        }
        else
        {
          baseEntityConfirmType = mod.EditBaseEntityConfirmType(
                  id: type.Id,
                  rowVersion: baseEntityConfirmType.RowVersion,
                  departmentId: type.DepartmentId,
                  userId: type.UserId,
                  confirmType: type.ConfirmType,
                  confirmPageUrl: type.ConfirmPageUrl);
        }
      }
    }
    private static void _addTransactionType()
    {
      var typeOfTransactionType = typeof(TransactionType);
      var transacts = typeOfTransactionType.GetProperties(BindingFlags.Static | BindingFlags.Public)
          .Select(i => i.GetValue(typeOfTransactionType)).OfType<lena.Domains.TransactionType>().ToArray();
      var mod = App.Internals.WarehouseManagement;
      var transactionTypes = mod.GetTransactionTypes(e => e).ToList();
      foreach (var type in transacts)
      {
        var transactionType = transactionTypes.FirstOrDefault(x => x.Id == type.Id);
        if (transactionType == null)
        {
          transactionType = mod.AddTransactionType
              (
                  id: type.Id,
                  name: type.Name,
                  factor: type.Factor,
                  transactionLevel: type.TransactionLevel,
                  rollbackTransactionTypeId: null
              );
          type.Id = transactionType.Id;
        }
        else
        {
          transactionType = mod.EditTransactionType(
                  id: transactionType.Id,
                  rowVersion: transactionType.RowVersion,
                  name: type.Name,
                  factor: type.Factor,
                  transactionLevel: type.TransactionLevel,
                  rollbackTransactionTypeId: type.RollbackTransactionTypeId,
                  transactionTypeEntity: transactionType);
        }
        type.RowVersion = transactionType.RowVersion;
      }
      //foreach (var type in transacts)
      //{
      //    if (type.RollbackTransactionType != null)
      //    {
      //        mod.EditTransactionType(
      //                rowVersion: type.RowVersion,
      //                id: type.Id,
      //                rollbackTransactionTypeId: type.RollbackTransactionType.Id)
      //            
      //;
      //    }
      //}
    }
    private static void _addtypeOfFinancialTransactionType()
    {
      var typeOfFinancialTransactionType = typeof(FinancialTransactionType);
      var transacts = typeOfStaticFinancialTransactionTypes.GetProperties(BindingFlags.Static | BindingFlags.Public)
          .Select(i => i.GetValue(typeOfFinancialTransactionType)).OfType<lena.Domains.FinancialTransactionType>().ToArray();
      var mod = App.Internals.Accounting;
      var transactionTypes = mod.GetFinancialTransactionTypes(e => e).ToList();
      foreach (var type in transacts)
      {
        var financialTransactionType = transactionTypes.FirstOrDefault(x => x.Id == type.Id);
        if (financialTransactionType == null)
        {
          financialTransactionType = mod.AddFinancialTransactionType
              (
                  id: type.Id,
                  title: type.Title,
                  factor: type.Factor,
                  transactionLevel: type.FinancialTransactionLevel,
                  rollbackFinancialTransactionTypeId: null
              );
          type.Id = financialTransactionType.Id;
        }
        else
        {
          financialTransactionType = mod.EditFinancialTransactionType(
          id: financialTransactionType.Id,
          rowVersion: financialTransactionType.RowVersion,
          title: type.Title,
          factor: type.Factor,
          financialTransactionLevel: type.FinancialTransactionLevel,
          rollbackFinancialTransactionTypeId: type.RollbackFinancialTransactionTypeId,
          financialTransactionType: financialTransactionType);
        }
        type.RowVersion = financialTransactionType.RowVersion;
      }
      //foreach (var type in transacts)
      //{
      //    if (type.RollbackFinancialTransactionType != null)
      //    {
      //        mod.EditFinancialTransactionType(
      //                rowVersion: type.RowVersion,
      //                id: type.Id,
      //                rollbackFinancialTransactionTypeId: type.RollbackFinancialTransactionTypeId)
      //            
      //;
      //    }
      //}
    }
    private static void _addActions()
    {
    }
  }
}