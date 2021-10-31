//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {

    #region Gets
    public IQueryable<TResult> GetCities<TResult>(
        Expression<Func<City, TResult>> selector,
        TValue<byte> countryId = null,
        TValue<short> id = null
    )
    {

      var query = repository.GetQuery<City>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (countryId != null)
        query = query.Where(i => i.CountryId == countryId);
      return query.Select(selector);
    }
    #endregion
    #region ToComboResult
    public Expression<Func<City, CityComboResult>> ToCityComboResult =
        city => new CityComboResult()
        {
          Id = city.Id,
          CityTitle = city.Title,
          CountryId = city.CountryId,
        };
    #endregion
  }
}
