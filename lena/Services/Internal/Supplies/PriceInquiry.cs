using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Supplies;
using lena.Models.Supplies.PriceInquiry;
using lena.Models.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public PriceInquiry AddPriceInquiry(
        int stuffId,
        int cooperatorId,
        byte currencyId,
        int? number,
        double? price,
        string description,
        DateTime? priceAnnunciationDateTime)
    {

      var priceInquiry = repository.Create<PriceInquiry>();

      priceInquiry.StuffId = stuffId;
      priceInquiry.CooperatorId = cooperatorId;
      priceInquiry.CurrencyId = currencyId;
      priceInquiry.Number = number;
      priceInquiry.Price = price;
      priceInquiry.PriceAnnunciationDateTime = priceAnnunciationDateTime;
      priceInquiry.UserId = App.Providers.Security.CurrentLoginData.UserId;
      priceInquiry.CreateDateTime = DateTime.UtcNow;
      priceInquiry.Description = description;
      repository.Add(priceInquiry);
      return priceInquiry;
    }
    #endregion
    #region Delete
    public void DeletePriceInquiry(int id)
    {

      var priceInquiry = GetPriceInquiry(id: id);
      repository.Delete(priceInquiry);
    }
    #endregion
    #region Edit
    public PriceInquiry EditPriceInquiry(
        int id,
        TValue<int> stuffId = null,
        TValue<int> cooperatorId = null,
        TValue<byte> currencyId = null,
        TValue<int?> number = null,
        TValue<double?> price = null,
        TValue<string> description = null,
        TValue<DateTime?> priceAnnunciationDateTime = null
        )
    {

      var priceInquiry = GetPriceInquiry(id: id);
      if (stuffId != null)
        priceInquiry.StuffId = stuffId;
      if (cooperatorId != null)
        priceInquiry.CooperatorId = cooperatorId;
      if (currencyId != null)
        priceInquiry.CurrencyId = currencyId;
      if (number != null)
        priceInquiry.Number = number;
      if (price != null)
        priceInquiry.Price = price;
      if (priceAnnunciationDateTime != null)
        priceInquiry.PriceAnnunciationDateTime = priceAnnunciationDateTime;
      if (description != null)
        priceInquiry.Description = description;

      repository.Update(priceInquiry, priceInquiry.RowVersion);
      return priceInquiry;
    }
    #endregion

    #region Get
    public PriceInquiry GetPriceInquiry(int id) => GetPriceInquiry(selector: e => e, id: id);
    internal TResult GetPriceInquiry<TResult>(
    Expression<Func<PriceInquiry, TResult>> selector,
    int id)
    {


      var priceInquiry = GetPriceInquiries(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (priceInquiry == null)
        throw new PriceInquiryNotFoundException(id: id);
      return priceInquiry;

    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetPriceInquiries<TResult>(
        Expression<Func<PriceInquiry, TResult>> selector,
        TValue<int?> id = null,
        TValue<int?> stuffId = null,
        TValue<int?> cooperatorId = null,
        TValue<byte?> currencyId = null,
        TValue<int?> employeeId = null)
    {


      var priceInquiry = repository.GetQuery<PriceInquiry>();

      if (id != null)
        priceInquiry = priceInquiry.Where(i => i.Id == id);
      if (stuffId != null)
        priceInquiry = priceInquiry.Where(i => i.StuffId == stuffId);
      if (cooperatorId != null)
        priceInquiry = priceInquiry.Where(i => i.CooperatorId == cooperatorId);
      if (currencyId != null)
        priceInquiry = priceInquiry.Where(i => i.CurrencyId == currencyId);
      if (employeeId != null)
        priceInquiry = priceInquiry.Where(i => i.User.Employee.Id == employeeId);

      return priceInquiry.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<PriceInquiry, PriceInquiryResult>> ToPriceInquiryResult =
     priceInquiry => new PriceInquiryResult
     {
       Id = priceInquiry.Id,
       StuffId = priceInquiry.StuffId,
       StuffName = priceInquiry.Stuff.Name,
       StuffCode = priceInquiry.Stuff.Code,
       CooperatorId = priceInquiry.CooperatorId,
       CooperatorName = priceInquiry.Cooperator.Name,
       CurrencyId = priceInquiry.CurrencyId,
       CurrencyTitle = priceInquiry.Currency.Title,
       UserId = priceInquiry.User.Id,
       UserFullName = priceInquiry.User.Employee.FirstName + " " + priceInquiry.User.Employee.LastName,
       Number = priceInquiry.Number,
       Price = priceInquiry.Price,
       PriceAnnunciationDateTime = priceInquiry.PriceAnnunciationDateTime,
       CreateDateTime = priceInquiry.CreateDateTime,
       Description = priceInquiry.Description,
     };

    #endregion

    #region Search
    public IQueryable<PriceInquiryResult> SearchPriceInquiry(IQueryable<PriceInquiryResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {

      if (!string.IsNullOrEmpty(searchText))
      {
        query = query.Where(item =>
            item.StuffCode.Contains(searchText) ||
            item.StuffName.Contains(searchText) ||
            item.CooperatorName.Contains(searchText) ||
            item.CurrencyTitle.Contains(searchText) ||
            item.UserFullName.Contains(searchText) ||
            item.Number.ToString().Contains(searchText) ||
            item.Price.ToString().Contains(searchText) ||
            item.Description.Contains(searchText));
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region Sort
    public IOrderedQueryable<PriceInquiryResult> SortPriceInquiryResult(IQueryable<PriceInquiryResult> query,
        SortInput<PriceInquirySortType> sort)
    {
      switch (sort.SortType)
      {
        case PriceInquirySortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PriceInquirySortType.StuffName:
          return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case PriceInquirySortType.StuffCode:
          return query.OrderBy(a => a.StuffCode, sort.SortOrder);
        case PriceInquirySortType.CooperatorName:
          return query.OrderBy(a => a.CooperatorName, sort.SortOrder);
        case PriceInquirySortType.CurrencyTitle:
          return query.OrderBy(a => a.CurrencyTitle, sort.SortOrder);
        case PriceInquirySortType.UserFullName:
          return query.OrderBy(a => a.UserFullName, sort.SortOrder);
        case PriceInquirySortType.Number:
          return query.OrderBy(a => a.Number, sort.SortOrder);
        case PriceInquirySortType.Price:
          return query.OrderBy(a => a.Price, sort.SortOrder);
        case PriceInquirySortType.PriceAnnunciationDateTime:
          return query.OrderBy(a => a.PriceAnnunciationDateTime, sort.SortOrder);
        case PriceInquirySortType.CreateDateTime:
          return query.OrderBy(a => a.CreateDateTime, sort.SortOrder);
        case PriceInquirySortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}
