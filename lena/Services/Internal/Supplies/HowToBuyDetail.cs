using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.HowToBuyDetail;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<TResult> GetHowToBuyDetails<TResult>(
        Expression<Func<HowToBuyDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> order = null,
        TValue<int> howToBuyId = null,
        TValue<string> title = null)
    {

      var isIdNull = id == null;
      var isOrderNull = order == null;
      var isHowToBuyIdNull = howToBuyId == null;
      var isTransactionTypeIdNull = title == null;
      var howToBuyDetails = from item in repository.GetQuery<HowToBuyDetail>()
                            where
                                       (isIdNull || item.Id == id) &&
                                       (isOrderNull || item.Order == order) &&
                                       (isHowToBuyIdNull || item.HowToBuyId == howToBuyId) &&
                                       (isTransactionTypeIdNull || item.Title == title)
                            select item;
      return howToBuyDetails.Select(selector);
    }
    #endregion
    #region Get
    public HowToBuyDetail GetHowToBuyDetail(int id) => GetHowToBuyDetail(selector: e => e, id: id);
    public TResult GetHowToBuyDetail<TResult>(Expression<Func<HowToBuyDetail, TResult>> selector, int id)
    {

      var howToBuy = GetHowToBuyDetails(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (howToBuy == null)
        throw new HowToBuyDetailNotFoundException(id);
      return howToBuy;
    }
    #endregion
    #region Add
    public HowToBuyDetail AddHowToBuyDetail(
        short howToBuyId,
        int order,
        string title)
    {

      var howToBuy = repository.Create<HowToBuyDetail>();
      howToBuy.HowToBuyId = howToBuyId;
      howToBuy.Order = order;
      howToBuy.Title = title;
      repository.Add(howToBuy);
      return howToBuy;
    }
    #endregion
    #region Edit
    public HowToBuyDetail EditHowToBuyDetail(
        int id,
        byte[] rowVersion,
        TValue<int> order,
        TValue<string> title)
    {

      var howToBuyDetail = GetHowToBuyDetail(id);
      if (howToBuyDetail == null)
        throw new HowToBuyDetailNotFoundException(id);
      if (order != null)
        howToBuyDetail.Order = order;
      if (title != null)
        howToBuyDetail.Title = title;
      repository.Update(howToBuyDetail, rowVersion: rowVersion);
      return howToBuyDetail;
    }
    #endregion
    #region Delete
    public void DeleteHowToBuyDetail(int id)
    {

      var howToBuyDetail = GetHowToBuyDetail(id: id);
      if (howToBuyDetail == null)
        throw new HowToBuyDetailNotFoundException(id);
      repository.Delete(howToBuyDetail);
    }
    #endregion
    #region ToResult
    public Expression<Func<HowToBuyDetail, HowToBuyDetailResult>> ToHowToBuyDetailResult =
        howToBuyDetail => new HowToBuyDetailResult()
        {
          Id = howToBuyDetail.Id,
          Order = howToBuyDetail.Order,
          Title = howToBuyDetail.Title,
          HowToBuyId = howToBuyDetail.HowToBuyId,
          HowToBuyTitle = howToBuyDetail.HowToBuy.Title,
          RowVersion = howToBuyDetail.RowVersion
        };
    #endregion
    #region ToComboResult
    public Expression<Func<HowToBuyDetail, HowToBuyDetailComboResult>> ToHowToBuyDetailComboResult =
        howToBuyDetail => new HowToBuyDetailComboResult
        {
          Id = howToBuyDetail.Id,
          Title = howToBuyDetail.Title,
        };
    #endregion
    #region Sort
    public IOrderedQueryable<HowToBuyDetailResult> SortHowToBuyDetailResult(IQueryable<HowToBuyDetailResult> input, SortInput<HowToBuyDetailSortType> options)
    {
      switch (options.SortType)
      {
        case HowToBuyDetailSortType.Order:
          return input.OrderBy(i => i.Order, options.SortOrder);
        case HowToBuyDetailSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IOrderedQueryable<HowToBuyDetailComboResult> SortHowToBuyDetailComboResult(IQueryable<HowToBuyDetailComboResult> input, SortInput<HowToBuyDetailSortType> options)
    {
      switch (options.SortType)
      {
        case HowToBuyDetailSortType.Order:
          return input.OrderBy(i => i.Order, options.SortOrder);
        case HowToBuyDetailSortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<HowToBuyDetailResult> SearchHowToBuyDetailResultQuery(IQueryable<HowToBuyDetailResult> query, string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from howToBuy in query
                where howToBuy.Title.Contains(searchText) ||
                      howToBuy.HowToBuyTitle.Contains(searchText)
                select howToBuy;
      return query;
    }
    #endregion
  }
}
