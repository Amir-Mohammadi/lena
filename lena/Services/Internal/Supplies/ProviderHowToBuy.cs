using System;
using System.Linq;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.ProviderHowToBuy;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<ProviderHowToBuy> GetProviderHowToBuys(
        TValue<string> description = null,
        TValue<short> howToBuyId = null,
        TValue<int> providerId = null,
        TValue<short> leadTime = null,
        TValue<bool> isActive = null,
        TValue<bool> isDefault = null)
    {

      var isDescriptionNull = description == null;
      var isHowToBuyIdNull = howToBuyId == null;
      var isProviderIdNull = providerId == null;
      var isLeadTimeNull = leadTime == null;
      var isIsActiveNull = isActive == null;
      var isIsDefaultNull = isDefault == null;
      var providerHowToBuys = from item in repository.GetQuery<ProviderHowToBuy>()
                              where
                                         (isDescriptionNull || item.Description == description) &&
                                         (isHowToBuyIdNull || item.HowToBuyId == howToBuyId) &&
                                         (isProviderIdNull || item.ProviderId == providerId) &&
                                         (isLeadTimeNull || item.LeadTime == leadTime) &&
                                         (isIsActiveNull || item.IsActive == isActive) &&
                                         (isIsDefaultNull || item.IsDefault == isDefault)
                              select item;
      return providerHowToBuys;
    }
    #endregion
    #region Get
    public ProviderHowToBuy GetProviderHowToBuy(short howToBuyId, int providerId)
    {

      var stuffProvider = GetProviderHowToBuys(howToBuyId: howToBuyId, providerId: providerId).SingleOrDefault();
      if (stuffProvider == null)
        throw new ProviderHowToBuyNotFoundException(howToBuyId: howToBuyId, providerId: providerId);
      return stuffProvider;
    }
    #endregion
    #region Add
    public ProviderHowToBuy AddProviderHowToBuy(
        string description,
        short howToBuyId,
        int providerId,
        short leadTime,
        bool isActive,
        bool isDefault)
    {

      var stuffProvider = repository.Create<ProviderHowToBuy>();
      stuffProvider.Description = description;
      stuffProvider.HowToBuyId = howToBuyId;
      stuffProvider.ProviderId = providerId;
      stuffProvider.LeadTime = leadTime;
      stuffProvider.IsActive = isActive;
      stuffProvider.IsDefault = isDefault;
      repository.Add(stuffProvider);
      return stuffProvider;
    }
    #endregion
    #region AddProcess
    public ProviderHowToBuy AddProviderHowToBuyProcess(
        string description,
        short howToBuyId,
        int providerId,
        short leadTime,
        bool isActive,
        bool isDefault)
    {

      if (isDefault)
      {
        var providerHowToBuys = GetProviderHowToBuys(
                      howToBuyId: howToBuyId,
                      isDefault: true,
                      isActive: true);
        if (providerHowToBuys.Any())
          throw new DefaultProviderHowToBuyExitst();
      }
      return AddProviderHowToBuy(
                    description: description,
                    howToBuyId: howToBuyId,
                    providerId: providerId,
                    leadTime: leadTime,
                    isActive: isActive,
                    isDefault: isDefault);
    }
    #endregion
    #region Edit
    public ProviderHowToBuy EditProviderHowToBuy(
        byte[] rowVersion,
        TValue<string> description,
        TValue<short> howToBuyId,
        TValue<int> providerId,
        TValue<short> leadTime,
        TValue<bool> isActive,
        TValue<bool> isDefault)
    {

      var providerHowToBuy = GetProviderHowToBuy(howToBuyId: howToBuyId, providerId: providerId);
      if (providerHowToBuy == null)
        throw new ProviderHowToBuyNotFoundException(howToBuyId: howToBuyId, providerId: providerId);
      if (description != null)
        providerHowToBuy.Description = description;
      if (howToBuyId != null)
        providerHowToBuy.HowToBuyId = howToBuyId;
      if (providerId != null)
        providerHowToBuy.ProviderId = providerId;
      if (leadTime != null)
        providerHowToBuy.LeadTime = leadTime;
      if (isActive != null)
        providerHowToBuy.IsActive = isActive;
      if (isDefault != null)
        providerHowToBuy.IsDefault = isDefault;
      repository.Update(providerHowToBuy, rowVersion: rowVersion);
      return providerHowToBuy;
    }
    #endregion
    #region EditProcess
    public ProviderHowToBuy EditProviderHowToBuyProcess(
        byte[] rowVersion,
        TValue<string> description,
        TValue<short> howToBuyId,
        TValue<int> providerId,
        TValue<short> leadTime,
        TValue<bool> isActive,
        TValue<bool> isDefault)
    {

      if (isDefault)
      {
        var providerHowToBuys = GetProviderHowToBuys(
                      howToBuyId: howToBuyId,
                      providerId: providerId,
                      isDefault: true,
                      isActive: true);
        if (providerHowToBuys.Any(i => i.HowToBuyId != howToBuyId && i.ProviderId != providerId))
          throw new DefaultProviderHowToBuyExitst();
      }
      return EditProviderHowToBuy(
                    rowVersion: rowVersion,
                    description: description,
                    howToBuyId: howToBuyId,
                    providerId: providerId,
                    leadTime: leadTime,
                    isActive: isActive,
                    isDefault: isDefault);
    }
    #endregion
    #region Delete
    public void DeleteProviderHowToBuy(short howToBuyId, int providerId)
    {

      var providerHowToBuy = GetProviderHowToBuy(howToBuyId: howToBuyId, providerId: providerId);
      if (providerHowToBuy == null)
        throw new ProviderHowToBuyNotFoundException(howToBuyId: howToBuyId, providerId: providerId);
      repository.Delete(providerHowToBuy);
    }
    #endregion
    #region ToResult
    public IQueryable<ProviderHowToBuyResult> ToProviderHowToBuyResultQuery(IQueryable<ProviderHowToBuy> query)
    {
      var resultQuery = from providerHowToBuy in query
                        let howToBuy = providerHowToBuy.HowToBuy
                        let provider = providerHowToBuy.Provider
                        select new ProviderHowToBuyResult()
                        {
                          Description = providerHowToBuy.Description,
                          HowToBuyId = providerHowToBuy.HowToBuyId,
                          HowToBuyName = howToBuy.Title,
                          ProviderId = providerHowToBuy.ProviderId,
                          ProviderName = provider.Name,
                          ProviderCode = provider.Code,
                          LeadTime = providerHowToBuy.LeadTime,
                          IsActive = providerHowToBuy.IsActive,
                          IsDefault = providerHowToBuy.IsDefault,
                          RowVersion = providerHowToBuy.RowVersion
                        };
      return resultQuery;
    }
    public IQueryable<ProviderHowToBuyResult> SearchProviderHowToBuyResultQuery(IQueryable<ProviderHowToBuyResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from providerHowToBuy in query
                where providerHowToBuy.HowToBuyName.Contains(searchText) ||
                       providerHowToBuy.ProviderName.Contains(searchText) ||
                       providerHowToBuy.Description.Contains(searchText) ||
                       providerHowToBuy.ProviderCode.Contains(searchText)
                select providerHowToBuy;
      }

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    public ProviderHowToBuyResult ToProviderHowToBuyResult(ProviderHowToBuy providerHowToBuy)
    {
      var howToBuy = providerHowToBuy.HowToBuy;
      var provider = providerHowToBuy.Provider;
      var result = new ProviderHowToBuyResult()
      {
        Description = providerHowToBuy.Description,
        HowToBuyId = providerHowToBuy.HowToBuyId,
        HowToBuyName = howToBuy.Title,
        ProviderId = providerHowToBuy.ProviderId,
        ProviderName = provider.Name,
        ProviderCode = provider.Code,
        LeadTime = providerHowToBuy.LeadTime,
        IsActive = providerHowToBuy.IsActive,
        IsDefault = providerHowToBuy.IsDefault,
        RowVersion = providerHowToBuy.RowVersion
      };
      return result;
    }
    public IOrderedQueryable<ProviderHowToBuyResult> SortProviderHowToBuyResult(IQueryable<ProviderHowToBuyResult> input, SortInput<ProviderHowToBuySortType> options)
    {
      switch (options.SortType)
      {
        case ProviderHowToBuySortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case ProviderHowToBuySortType.HowToBuyName:
          return input.OrderBy(i => i.HowToBuyName, options.SortOrder);
        case ProviderHowToBuySortType.ProviderName:
          return input.OrderBy(i => i.ProviderName, options.SortOrder);
        case ProviderHowToBuySortType.ProviderCode:
          return input.OrderBy(i => i.ProviderCode, options.SortOrder);
        case ProviderHowToBuySortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case ProviderHowToBuySortType.IsDefault:
          return input.OrderBy(i => i.IsDefault, options.SortOrder);
        case ProviderHowToBuySortType.LeadTime:
          return input.OrderBy(i => i.LeadTime, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
