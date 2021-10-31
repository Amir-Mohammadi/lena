using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.AutomaticReceiptRegistration;
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
    #region Sort
    public IOrderedQueryable<AutomaticReceiptRegistrationResult> SortAutoReceiptRegistrationResult(IQueryable<AutomaticReceiptRegistrationResult> query, SortInput<AutomaticReceiptRegistrationSortType> sort)
    {
      switch (sort.SortType)
      {
        case AutomaticReceiptRegistrationSortType.LadingCode:
          return query.OrderBy(a => a.LadingCode, sort.SortOrder);
        case AutomaticReceiptRegistrationSortType.LadingDateTime:
          return query.OrderBy(a => a.LadingDateTime, sort.SortOrder);
        case AutomaticReceiptRegistrationSortType.ProviderType:
          return query.OrderBy(a => a.ProviderType, sort.SortOrder);
        case AutomaticReceiptRegistrationSortType.ProviderName:
          return query.OrderBy(a => a.ProviderName, sort.SortOrder);
        case AutomaticReceiptRegistrationSortType.EntranceDateTime:
          return query.OrderBy(a => a.EntranceDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToResult
    public IQueryable<AutomaticStoreReceiptResult> ToAutomaticStoreReceiptResult(IQueryable<StoreReceipt> storeReceiptQuery, IQueryable<NewShopping> newShoppingQuery)
    {

      var result = from storeReceipt in storeReceiptQuery
                   join newShopping in newShoppingQuery on storeReceipt.Id equals newShopping.Id
                   select new AutomaticStoreReceiptResult
                   {
                     Id = storeReceipt.Id,
                     StoreReceiptType = storeReceipt.StoreReceiptType,
                     Code = storeReceipt.Code,
                     ReceiptId = storeReceipt.ReceiptId,
                     ReceiptCode = storeReceipt.Receipt.Code,
                     ReceiptStatus = storeReceipt.ReceiptId == null ? ReceiptStatus.NoReceipt : storeReceipt.Receipt.Status,
                     TransportDateTime = storeReceipt.InboundCargo.TransportDateTime,
                     StuffId = storeReceipt.StuffId,
                     StuffName = storeReceipt.Stuff.Name,
                     StuffCode = storeReceipt.Stuff.Code,
                     Amount = storeReceipt.Amount,
                     UnitId = storeReceipt.UnitId,
                     UnitName = storeReceipt.Unit.Name,
                     BoxNo = newShopping.BoxNo,
                     InboundCargoCode = newShopping.InboundCargo.Code,
                     InboundCargoId = newShopping.InboundCargoId,
                     LandingId = newShopping.LadingItem.LadingId,
                     LadingCode = newShopping.LadingItem.Lading.Code,
                     QtyPerBox = newShopping.QtyPerBox,
                     PurchaseOrderCode = newShopping.LadingItem.CargoItem.PurchaseOrder.Code,
                     CargoCode = newShopping.LadingItem.CargoItem.Cargo.Code,
                     CargoItemId = (int?)newShopping.LadingItem.CargoItemId,
                     CargoItemCode = newShopping.LadingItem.CargoItem.Code,
                     ProviderName = storeReceipt.Cooperator.Name,
                     ProviderId = storeReceipt.CooperatorId,
                     QualityControlStatus = storeReceipt.ReceiptQualityControls.Select(m => m.Status),
                     RowVersion = storeReceipt.RowVersion
                   };
      return result;
    }

    #endregion

    #region search
    public IQueryable<AutomaticReceiptRegistrationResult> SearchGroupedStoreReceiptResult(
       IQueryable<AutomaticReceiptRegistrationResult> query,
       AdvanceSearchItem[] advanceSearchItems,
       string searchText,
      TValue<ProviderType> providerType = null)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = query.Where(item =>
            item.LadingCode.Contains(searchText) ||
            item.ProviderName.Contains(searchText));

      if (providerType != null)
        query = query.Where(i => i.ProviderType == providerType);

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);

      return query;
    }
    #endregion
  }
}
