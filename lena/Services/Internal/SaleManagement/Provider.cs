using System;
using System.Linq;
using lena.Services.Common;

using lena.Domains;
using lena.Models.Common;
using lena.Domains;
using lena.Models;
using lena.Models.Common;
using lena.Models.SaleManagement.Provider;
using lena.Services.Core;
using lena.Services.Internals.Supplies.Exception;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public void DeleteProvider(int id)
    {
      var cooperatorFinancialAccount = App.Internals.Accounting.GetCooperatorFinancialAccounts(
                selector: e => e,
                cooperatorId: id);
      if (cooperatorFinancialAccount.Any())
      {
        throw new CooperatorHasFinancialAcountException(id);
      }
      var provider = GetProvider(id);
      repository.Delete(provider);
    }
    public Cooperator AddProvider(
        string name,
        string detailedCode,
        ProviderType providerType,
        short cityId)
    {
      var provider = repository.Create<Cooperator>();
      provider.ProviderType = providerType;
      provider.CityId = cityId;
      AddCooperator(
                provider,
                name: name,
                detailedCode: detailedCode,
                cooperatorType: CooperatorType.Provider);
      return provider;
    }
    public IQueryable<Cooperator> GetProviders(
        TValue<int> id = null,
        TValue<ProviderType> providerType = null,
        TValue<bool> confirmationDetailedCode = null,
        TValue<string> name = null,
        TValue<string> Code = null)
    {
      var cooperators = GetCooperators(
                selector: e => e,
                id: id,
                name: name,
                Code: Code,
                providerType: providerType,
                confirmationDetailedCode: confirmationDetailedCode,
                cooperatorType: CooperatorType.Provider);
      return cooperators;
    }
    public Cooperator GetProvider(int id)
    {
      var provider = GetProviders(id: id).FirstOrDefault();
      if (provider == null)
        throw new ProviderNotfoundException();
      return provider;
    }
    public Cooperator EditProvider(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> detailedCode = null,
        TValue<ProviderType> providerType = null,
        TValue<string> code = null,
        TValue<short> cityId = null)
    {
      var provider = GetProvider(id: id);
      EditCooperator(
                cooperator: provider,
                rowVersion: rowVersion,
                detailedCode: detailedCode,
                name: name,
                cityId: cityId,
                providerType: providerType,
                code: code);
      repository.Update(entity: provider, rowVersion: provider.RowVersion);
      return provider;
    }
    public IQueryable<ProviderComboResult> ToProviderComboList(IQueryable<Cooperator> cooperators)
    {
      return from provider in cooperators
             select new ProviderComboResult()
             {
               Id = provider.Id,
               Name = provider.Name,
               Code = provider.Code,
               DetailedCode = provider.DetailedCode,
               ConfirmationDetailedCode = provider.ConfirmationDetailedCode,
               Type = provider.ProviderType
             };
    }
    public IQueryable<ProviderResult> ToProviderResultList(IQueryable<Cooperator> query)
    {
      return from provider in query
             select new ProviderResult()
             {
               Id = provider.Id,
               Name = provider.Name,
               Code = provider.Code,
               Type = provider.ProviderType,
               DetailedCode = provider.DetailedCode,
               ConfirmationDetailedCode = provider.ConfirmationDetailedCode,
               CityId = provider.CityId,
               CountryId = provider.City.CountryId,
               CityTitle = provider.City.Title,
               CountryTitle = provider.City.Country.Title,
               RowVersion = provider.RowVersion
             };
    }
    public ProviderResult ToProviderResult(Cooperator provider)
    {
      return new ProviderResult()
      {
        Id = provider.Id,
        Code = provider.Code,
        Name = provider.Name,
        Type = provider.ProviderType,
        DetailedCode = provider.DetailedCode,
        ConfirmationDetailedCode = provider.ConfirmationDetailedCode,
        CityId = provider.CityId,
        CountryId = provider.City.CountryId,
        CityTitle = provider.City.Title,
        CountryTitle = provider.City.Country.Title,
        RowVersion = provider.RowVersion
      };
    }
    public IOrderedQueryable<ProviderResult> SortProviderResult(
        IQueryable<ProviderResult> input,
        SortInput<ProviderSortType> options)
    {
      switch (options.SortType)
      {
        case ProviderSortType.Id:
          return input.OrderBy(res => res.Id, options.SortOrder);
        case ProviderSortType.Name:
          return input.OrderBy(res => res.Name, options.SortOrder);
        case ProviderSortType.Code:
          return input.OrderBy(res => res.Code, options.SortOrder);
        case ProviderSortType.DetailedCode:
          return input.OrderBy(res => res.DetailedCode, options.SortOrder);
        case ProviderSortType.ConfirmationDetailedCode:
          return input.OrderBy(res => res.ConfirmationDetailedCode, options.SortOrder);
        case ProviderSortType.CountryTitle:
          return input.OrderBy(res => res.CountryTitle, options.SortOrder);
        case ProviderSortType.CityTitle:
          return input.OrderBy(res => res.CityTitle, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ProviderResult> SearchProviderResult(
        IQueryable<ProviderResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Code.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #region ConfirmationProviderDetailedCode
    public Cooperator ConfirmationProviderDetailedCode(
        int id,
        string detailedCode,
        byte[] rowVersion)
    {
      var provider = GetProvider(id: id);
      if (detailedCode != null)
        provider.DetailedCode = detailedCode;
      provider.ConfirmationDetailedCode = true;
      repository.Update(provider, provider.RowVersion);
      return provider;
    }
    #endregion
    #region DisapprovalProviderDetailedCode
    public Cooperator DisapprovalProviderDetailedCode(
        int id,
        byte[] rowVersion,
        TValue<string> detailedCode = null)
    {
      var provider = GetProvider(id: id);
      if (detailedCode != null)
        provider.DetailedCode = detailedCode;
      provider.ConfirmationDetailedCode = false;
      repository.Update(provider, rowVersion);
      return provider;
    }
    #endregion
  }
}