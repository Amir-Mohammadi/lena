using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.Country;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Get
    public Country GetCountry(int id) => GetCountry(selector: e => e, id: id);
    public TResult GetCountry<TResult>(
        Expression<Func<Country, TResult>> selector,
        int id)
    {

      var Country = GetCountries(selector: selector,
                id: id).FirstOrDefault();
      if (Country == null)
        throw new RecordNotFoundException(id, typeof(Country));
      return Country;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCountries<TResult>(
        Expression<Func<Country, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null
    )
    {

      var query = repository.GetQuery<Country>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (title != null)
        query = query.Where(i => i.Title == title);
      return query.Select(selector);
    }
    #endregion

    public Country AddCountry(string title)
    {

      var country = repository.Create<Country>();
      country.Title = title;
      repository.Add(country);
      return country;
    }
    public Country EditCountry(byte[] rowVersion, int id, TValue<string> title = null)
    {

      var country = GetCountry(id: id);
      if (title != null)
        country.Title = title;
      repository.Update(entity: country, rowVersion: country.RowVersion);
      return country;
    }
    public void DeleteCountry(int id)
    {

      var Country = GetCountry(id: id);
      repository.Delete(Country);
    }

    #region Sort
    public IOrderedQueryable<CountryResult> SortCountryResult(
        IQueryable<CountryResult> query, SortInput<CountrySortType> type)
    {
      switch (type.SortType)
      {
        case CountrySortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case CountrySortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region SortCombo
    public IOrderedQueryable<CountryComboResult> SortCountryComboResult(
        IQueryable<CountryComboResult> query, SortInput<CountryComboSortType> type)
    {
      switch (type.SortType)
      {
        case CountryComboSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case CountryComboSortType.Title:
          return query.OrderBy(a => a.Title, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Search
    public IQueryable<CountryResult> SearchCountryResult(
        IQueryable<CountryResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where
                item.Id.ToString().Contains(searchText) ||
                    item.Title.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region ToComboResult
    public Expression<Func<Country, CountryComboResult>> ToCountryComboResult =
        Country => new CountryComboResult()
        {
          Id = Country.Id,
          Title = Country.Title,

        };
    #endregion
    #region ToResult
    public Expression<Func<Country, CountryResult>> ToCountryResult =

         country => new CountryResult()
         {
           Id = country.Id,
           Title = country.Title,
           RowVersion = country.RowVersion
         };
    #endregion

  }
}
