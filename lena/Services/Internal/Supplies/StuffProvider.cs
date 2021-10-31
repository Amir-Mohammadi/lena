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
using lena.Models.Supplies.StuffProvider;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<StuffProvider> GetStuffProviders(
        TValue<string> description = null,
        TValue<int> stuffId = null,
        TValue<int> providerId = null,
        TValue<short> leadTime = null,
        TValue<bool> isActive = null,
        TValue<bool> isDefault = null)
    {

      var isDescriptionNull = description == null;
      var isStuffIdNull = stuffId == null;
      var isProviderIdNull = providerId == null;
      var isLeadTimeNull = leadTime == null;
      var isIsActiveNull = isActive == null;
      var isIsDefaultNull = isDefault == null;
      var stuffProviders = from item in repository.GetQuery<StuffProvider>()
                           where
                                      (isDescriptionNull || item.Description == description) &&
                                      (isStuffIdNull || item.StuffId == stuffId) &&
                                      (isProviderIdNull || item.ProviderId == providerId) &&
                                      (isLeadTimeNull || item.LeadTime == leadTime) &&
                                      (isIsActiveNull || item.IsActive == isActive) &&
                                      (isIsDefaultNull || item.IsDefault == isDefault)
                           select item;
      return stuffProviders;
    }

    public IQueryable<StuffProvider> GetStuffProvidersForPython(
        int[] stuffIds)
    {

      return repository.GetQuery<StuffProvider>().Where(r => stuffIds.Contains(r.StuffId) && r.IsActive == true && r.IsDefault == true);
    }
    #endregion
    #region Get
    public StuffProvider GetStuffProvider(int stuffId, int providerId)
    {

      var stuffProvider = GetStuffProviders(stuffId: stuffId, providerId: providerId).SingleOrDefault();
      if (stuffProvider == null)
        throw new StuffProviderNotFoundException(stuffId: stuffId, providerId: providerId);
      return stuffProvider;
    }
    #endregion
    #region Add
    public StuffProvider AddStuffProvider(
        string description,
        int stuffId,
        int providerId,
        short leadTime,
        short? instantLeadTime,
        bool isActive,
        bool isDefault)
    {

      var stuffProvider = repository.Create<StuffProvider>();
      stuffProvider.Description = description;
      stuffProvider.StuffId = stuffId;
      stuffProvider.ProviderId = providerId;
      stuffProvider.LeadTime = leadTime;
      stuffProvider.IsActive = isActive;
      stuffProvider.IsDefault = isDefault;
      stuffProvider.InstantLeadTime = instantLeadTime;
      repository.Add(stuffProvider);
      return stuffProvider;
    }
    #endregion
    #region AddProcess
    public StuffProvider AddStuffProviderProcess(
        string description,
        int stuffId,
        int providerId,
        short leadTime,
        short? instantLeadTime,
        bool isActive,
        bool isDefault)
    {


      var stuffProviders = GetStuffProviders(stuffId: stuffId);
      var isStuffProviderExist = stuffProviders.Where(m => m.ProviderId == providerId).Any();

      if (isStuffProviderExist)
      {
        throw new StuffIsProvidedByThisProviderException(stuffId: stuffId, providerId: providerId);
      }
      if (isDefault)
      {
        var isDefaultStuffProviders = stuffProviders.Where(m => m.IsDefault == true && isActive == true);

        if (isDefaultStuffProviders.Any())
          throw new DefaultStuffProviderExitst();
      }
      return AddStuffProvider(
                    description: description,
                    stuffId: stuffId,
                    providerId: providerId,
                    leadTime: leadTime,
                    instantLeadTime: instantLeadTime,
                    isActive: isActive,
                    isDefault: isDefault);
    }
    #endregion
    #region Edit
    public StuffProvider EditStuffProvider(
        byte[] rowVersion,
        TValue<string> description,
        TValue<int> stuffId,
        TValue<int> providerId,
        TValue<short> leadTime,
        TValue<short?> instantLeadTime,
        TValue<bool> isActive,
        TValue<bool> isDefault)
    {

      var stuffProvider = GetStuffProvider(stuffId: stuffId, providerId: providerId);
      if (stuffProvider == null)
        throw new StuffProviderNotFoundException(stuffId: stuffId, providerId: providerId);
      if (description != null)
        stuffProvider.Description = description;
      if (stuffId != null)
        stuffProvider.StuffId = stuffId;
      if (providerId != null)
        stuffProvider.ProviderId = providerId;
      if (leadTime != null)
        stuffProvider.LeadTime = leadTime;
      if (isActive != null)
        stuffProvider.IsActive = isActive;
      if (isDefault != null)
        stuffProvider.IsDefault = isDefault;
      if (instantLeadTime != null)
        stuffProvider.InstantLeadTime = instantLeadTime;
      repository.Update(stuffProvider, rowVersion: rowVersion);
      return stuffProvider;
    }
    #endregion
    #region EditProcess
    public StuffProvider EditStuffProviderProcess(
        byte[] rowVersion,
        TValue<string> description,
        TValue<int> stuffId,
        TValue<int> providerId,
        TValue<short> leadTime,
        TValue<short?> instantLeadTime,
        TValue<bool> isActive,
        TValue<bool> isDefault)
    {

      if (isDefault)
      {
        var stuffProviders = GetStuffProviders(
                      stuffId: stuffId,
                      providerId: providerId,
                      isDefault: true,
                      isActive: true);
        if (stuffProviders.Any(i => i.StuffId != stuffId && i.ProviderId != providerId))
          throw new DefaultStuffProviderExitst();
      }
      return EditStuffProvider(
                    rowVersion: rowVersion,
                    description: description,
                    stuffId: stuffId,
                    providerId: providerId,
                    leadTime: leadTime,
                    instantLeadTime: instantLeadTime,
                    isActive: isActive,
                    isDefault: isDefault);
    }
    #endregion
    #region Delete
    public void DeleteStuffProvider(int stuffId, int providerId)
    {

      var stuffProvider = GetStuffProvider(stuffId: stuffId, providerId: providerId);
      if (stuffProvider == null)
        throw new StuffProviderNotFoundException(stuffId: stuffId, providerId: providerId);
      repository.Delete(stuffProvider);
    }
    #endregion
    #region ToResult
    public IQueryable<StuffProviderResult> ToStuffProviderResultQuery(IQueryable<StuffProvider> query)
    {
      var resultQuery = from stuffProvider in query
                        let stuff = stuffProvider.Stuff
                        let provider = stuffProvider.Provider
                        select new StuffProviderResult()
                        {
                          Description = stuffProvider.Description,
                          StuffId = stuffProvider.StuffId,
                          StuffName = stuff.Name,
                          StuffCode = stuff.Code,
                          ProviderId = stuffProvider.ProviderId,
                          ProviderName = provider.Name,
                          ProviderCode = provider.Code,
                          LeadTime = stuffProvider.LeadTime,
                          InstantLeadTime = stuffProvider.InstantLeadTime,
                          IsActive = stuffProvider.IsActive,
                          IsDefault = stuffProvider.IsDefault,
                          RowVersion = stuffProvider.RowVersion
                        };
      return resultQuery;
    }
    public IQueryable<StuffProviderResult> SearchStuffProviderResultQuery(IQueryable<StuffProviderResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from stuffProvider in query
                where stuffProvider.StuffCode.Contains(searchText) ||
                       stuffProvider.StuffName.Contains(searchText) ||
                       stuffProvider.ProviderName.Contains(searchText) ||
                       stuffProvider.Description.Contains(searchText) ||
                       stuffProvider.ProviderCode.Contains(searchText)
                select stuffProvider;
      }

      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;


    }
    public StuffProviderResult ToStuffProviderResult(StuffProvider stuffProvider)
    {
      var stuff = stuffProvider.Stuff;
      var provider = stuffProvider.Provider;
      var result = new StuffProviderResult()
      {
        Description = stuffProvider.Description,
        StuffId = stuffProvider.StuffId,
        StuffName = stuff.Name,
        StuffCode = stuff.Code,
        ProviderId = stuffProvider.ProviderId,
        ProviderName = provider.Name,
        ProviderCode = provider.Code,
        LeadTime = stuffProvider.LeadTime,
        InstantLeadTime = stuffProvider.InstantLeadTime,
        IsActive = stuffProvider.IsActive,
        IsDefault = stuffProvider.IsDefault,
        RowVersion = stuffProvider.RowVersion
      };
      return result;
    }
    public IOrderedQueryable<StuffProviderResult> SortStuffProviderResult(IQueryable<StuffProviderResult> input, SortInput<StuffProviderSortType> options)
    {
      switch (options.SortType)
      {
        case StuffProviderSortType.Description:
          return input.OrderBy(i => i.Description, options.SortOrder);
        case StuffProviderSortType.StuffName:
          return input.OrderBy(i => i.StuffName, options.SortOrder);
        case StuffProviderSortType.StuffCode:
          return input.OrderBy(i => i.StuffCode, options.SortOrder);
        case StuffProviderSortType.ProviderName:
          return input.OrderBy(i => i.ProviderName, options.SortOrder);
        case StuffProviderSortType.ProviderCode:
          return input.OrderBy(i => i.ProviderCode, options.SortOrder);
        case StuffProviderSortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case StuffProviderSortType.IsDefault:
          return input.OrderBy(i => i.IsDefault, options.SortOrder);
        case StuffProviderSortType.LeadTime:
          return input.OrderBy(i => i.LeadTime, options.SortOrder);
        case StuffProviderSortType.InstantLeadTime:
          return input.OrderBy(i => i.InstantLeadTime, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<StuffProviderComboResult> ToStuffProviderComboResultQuery(IQueryable<StuffProvider> query)
    {
      var result = from stuffProvider in query
                   select new StuffProviderComboResult()
                   {
                     ProviderId = stuffProvider.ProviderId,
                     ProviderName = stuffProvider.Provider.Name,
                     IsDefault = stuffProvider.IsDefault,
                     LeadTime = stuffProvider.LeadTime
                   };
      return result;
    }
    #endregion
  }
}
