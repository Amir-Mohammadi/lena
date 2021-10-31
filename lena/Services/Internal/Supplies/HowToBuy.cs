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
using lena.Models.Supplies.HowToBuy;
using lena.Models.Supplies.HowToBuyDetail;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<TResult> GetHowToBuys<TResult>(
        Expression<Func<HowToBuy, TResult>> selector,
        TValue<int> id = null,
        TValue<string> title = null)
    {

      var isIdNull = id == null;
      var isTitleNull = title == null;
      var howToBuys = from item in repository.GetQuery<HowToBuy>()
                      where
                                 (isIdNull || item.Id == id) &&
                                 (isTitleNull || item.Title == title)
                      select item;
      return howToBuys.Select(selector);
    }
    #endregion
    #region Get

    public HowToBuy GetHowToBuy(int id) => GetHowToBuy(selector: e => e, id: id);
    public TResult GetHowToBuy<TResult>(Expression<Func<HowToBuy, TResult>> selector, int id)
    {

      var howToBuy = GetHowToBuys(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (howToBuy == null)
        throw new HowToBuyNotFoundException(id);
      return howToBuy;
    }
    #endregion
    #region Add
    public HowToBuy AddHowToBuy(
        string title)
    {

      var howToBuy = repository.Create<HowToBuy>();
      howToBuy.Title = title;
      howToBuy.IsActive = true;
      repository.Add(howToBuy);
      return howToBuy;
    }
    #endregion
    #region AddProcess
    public HowToBuy AddHowToBuyProcess(
        string title,
        AddHowToBuyDetailInput[] addHowToBuyDetails)
    {

      if (addHowToBuyDetails.Length == 0)
        throw new HowToBuyDetailIsEmptyException();
      var howToBuy = AddHowToBuy(title: title);
      foreach (var item in addHowToBuyDetails)
        AddHowToBuyDetail(
                      howToBuyId: howToBuy.Id,
                      order: item.Order,
                      title: item.Title);
      return howToBuy;
    }
    #endregion
    #region EditProcess
    public HowToBuy EditHowToBuyProcess(
        byte[] rowVersion,
        int id,
        string title,
        AddHowToBuyDetailInput[] addHowToBuyDetails,
        EditHowToBuyDetailInput[] editHowToBuyDetails,
        int[] deleteHowToBuyDetails)
    {

      var howToBuy = EditHowToBuy(rowVersion: rowVersion, id: id, title: title);
      foreach (var item in addHowToBuyDetails)
        AddHowToBuyDetail(
                      howToBuyId: howToBuy.Id,
                      order: item.Order,
                      title: item.Title);
      foreach (var item in editHowToBuyDetails)
        EditHowToBuyDetail(
                      rowVersion: item.RowVersion,
                      id: item.Id,
                      order: item.Order,
                      title: item.Title);
      foreach (var item in deleteHowToBuyDetails)
        DeleteHowToBuyDetail(item);
      return howToBuy;
    }
    #endregion
    #region Edit
    public HowToBuy EditHowToBuy(
        byte[] rowVersion,
        int id,
        TValue<string> title = null,
        TValue<bool> isActive = null)
    {

      var howToBuy = GetHowToBuy(id);
      if (title != null)
        howToBuy.Title = title;
      if (isActive != null)
        howToBuy.IsActive = isActive;
      repository.Update(howToBuy, rowVersion: rowVersion);
      return howToBuy;
    }
    #endregion
    #region Delete
    public void DeleteHowToBuy(int id)
    {

      var howToBuy = GetHowToBuy(id: id);
      int[] howToBuyDetailsIds = howToBuy.HowToBuyDetails.Select(s => s.Id).ToArray();
      foreach (var item in howToBuyDetailsIds)
        DeleteHowToBuyDetail(item);
      repository.Delete(howToBuy);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<HowToBuyResult> SortHowToBuyResult(IQueryable<HowToBuyResult> input, SortInput<HowToBuySortType> options)
    {
      switch (options.SortType)
      {
        case HowToBuySortType.Title:
          return input.OrderBy(i => i.Title, options.SortOrder);
        case HowToBuySortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<HowToBuyResult> SearchHowToBuyResultQuery(IQueryable<HowToBuyResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from howToBuy in query
                where howToBuy.Title.Contains(searchText)
                select howToBuy;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;


    }
    #endregion
    #region ToResult
    public Expression<Func<HowToBuy, HowToBuyResult>> ToHowToBuyResult =>
        howToBuy => new HowToBuyResult
        {
          Id = howToBuy.Id,
          Title = howToBuy.Title,
          IsActive = howToBuy.IsActive,
          RowVersion = howToBuy.RowVersion
        };
    #endregion
    #region ToFullResult
    public Expression<Func<HowToBuy, FullHowToBuyResult>> ToHowToBuyFullResult =>
        howToBuy => new FullHowToBuyResult()
        {
          Id = howToBuy.Id,
          Title = howToBuy.Title,
          IsActive = howToBuy.IsActive,
          HowToBuyDetails = howToBuy.HowToBuyDetails.AsQueryable().Select(ToHowToBuyDetailResult),
          RowVersion = howToBuy.RowVersion
        };
    #endregion
    #region ToComboResult
    public Expression<Func<HowToBuy, HowToBuyComboResult>> ToHowToBuyComboResult =>
        howToBuy => new HowToBuyComboResult()
        {
          Id = howToBuy.Id,
          Title = howToBuy.Title
        };
    #endregion
  }
}
