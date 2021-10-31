using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.StuffPurchaseCategory;
using System;
using System.Linq;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Gets
    public IQueryable<TResult> GetStuffPurchaseCategoryDetails<TResult>(
        Expression<Func<StuffPurchaseCategoryDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> stuffPurchaseCategoryId = null,
        TValue<int> applicatorUserGroupId = null,
        TValue<int> applicatorConfirmerUserGroupId = null
        )
    {

      var query = repository.GetQuery<StuffPurchaseCategoryDetail>();

      if (id != null)
        query = query.Where(i => i.Id == id);
      if (applicatorUserGroupId != null)
        query = query.Where(i => i.ApplicatorUserGroupId == applicatorUserGroupId);
      if (applicatorConfirmerUserGroupId != null)
        query = query.Where(i => i.ApplicatorConfirmerUserGroupId == applicatorConfirmerUserGroupId);
      if (stuffPurchaseCategoryId != null)
        query = query.Where(i => i.StuffPurchaseCategoryId == stuffPurchaseCategoryId);


      return query.Select(selector);
    }
    #endregion
    #region Get

    public StuffPurchaseCategoryDetail GetStuffPurchaseCategoryDetail(int id) => GetStuffPurchaseCategoryDetail(selector: e => e, id: id);
    public TResult GetStuffPurchaseCategoryDetail<TResult>(Expression<Func<StuffPurchaseCategoryDetail, TResult>> selector, int id)
    {

      var stuffPurchaseCategory = GetStuffPurchaseCategoryDetails(
                    selector: selector,
                    id: id)


                .SingleOrDefault();
      if (stuffPurchaseCategory == null)
        throw new StuffPurchaseCategoryDetailNotFoundException(id);
      return stuffPurchaseCategory;
    }
    #endregion
    #region Add
    public StuffPurchaseCategoryDetail AddStuffPurchaseCategoryDetail(
       int stuffPurchaseCategoryId,
       int applicatorUserGroupId,
       int applicatorConfirmerUserGroupId,
       int requestConfirmerUserGroupId
        )
    {

      var entity = repository.Create<StuffPurchaseCategoryDetail>();
      entity.StuffPurchaseCategoryId = stuffPurchaseCategoryId;
      entity.ApplicatorUserGroupId = applicatorUserGroupId;
      entity.ApplicatorConfirmerUserGroupId = applicatorConfirmerUserGroupId;
      entity.RequestConfirmerUserGroupId = requestConfirmerUserGroupId;

      repository.Add(entity);
      return entity;
    }
    #endregion


    #region Edit
    public StuffPurchaseCategoryDetail EditStuffPurchaseCategoryDetail(
        byte[] rowVersion,
        int id,
        TValue<int> stuffPurchaseCategoryId = null,
        TValue<int> applicatorUserGroupId = null,
        TValue<int> applicatorConfirmerUserGroupId = null,
        TValue<int> requestConfirmerUserGroupId = null
        )
    {

      var entity = GetStuffPurchaseCategoryDetail(id);

      if (stuffPurchaseCategoryId != null)
        entity.StuffPurchaseCategoryId = stuffPurchaseCategoryId;

      if (applicatorUserGroupId != null)
        entity.ApplicatorUserGroupId = applicatorUserGroupId;

      if (applicatorConfirmerUserGroupId != null)
        entity.ApplicatorConfirmerUserGroupId = applicatorConfirmerUserGroupId;

      if (requestConfirmerUserGroupId != null)
        entity.RequestConfirmerUserGroupId = requestConfirmerUserGroupId;


      repository.Update(entity, rowVersion: rowVersion);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteStuffPurchaseCategoryDetail(int id)
    {

      var entity = GetStuffPurchaseCategoryDetail(id: id);

      repository.Delete(entity);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffPurchaseCategoryDetailResult> SortStuffPurchaseCategoryDetailResult(IQueryable<StuffPurchaseCategoryDetailResult> input, SortInput<StuffPurchaseCategoryDetailSortType> options)
    {
      switch (options.SortType)
      {
        case StuffPurchaseCategoryDetailSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case StuffPurchaseCategoryDetailSortType.ApplicatorUserGroupName:
          return input.OrderBy(i => i.ApplicatorUserGroupName, options.SortOrder);
        case StuffPurchaseCategoryDetailSortType.ApplicatorConfirmerUserGroupName:
          return input.OrderBy(i => i.ApplicatorConfirmerUserGroupName, options.SortOrder);
        case StuffPurchaseCategoryDetailSortType.StuffPurchaseCategoryName:
          return input.OrderBy(i => i.StuffPurchaseCategoryName, options.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffPurchaseCategoryDetailResult> SearchStuffPurchaseCategoryDetailResultQuery(IQueryable<StuffPurchaseCategoryDetailResult> query, string searchText, AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
      {
        query = from q in query
                where q.StuffPurchaseCategoryName.Contains(searchText) ||
                q.ApplicatorUserGroupName.Contains(searchText) ||
                q.ApplicatorConfirmerUserGroupName.Contains(searchText) ||
                q.ApplicatorConfirmerUserGroupName.Contains(searchText)
                select q;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;


    }
    #endregion
    #region ToResult
    public Expression<Func<StuffPurchaseCategoryDetail, StuffPurchaseCategoryDetailResult>> ToStuffPurchaseCategoryDetailResult =>
        entity => new StuffPurchaseCategoryDetailResult
        {
          Id = entity.Id,
          StuffPurchaseCategoryId = entity.StuffPurchaseCategoryId,
          StuffPurchaseCategoryName = entity.StuffPurchaseCategory.Title,
          ApplicatorUserGroupId = entity.ApplicatorUserGroupId,
          ApplicatorUserGroupName = entity.ApplicatorUserGroup.Name,
          ApplicatorConfirmerUserGroupId = entity.ApplicatorConfirmerUserGroupId,
          ApplicatorConfirmerUserGroupName = entity.ApplicatorConfirmerUserGroup.Name,
          RequestConfirmerUserGroupId = entity.RequestConfirmerUserGroupId,
          RequestConfirmerUserGroupName = entity.RequestConfirmerUserGroup.Name,
          RowVersion = entity.RowVersion
        };
    #endregion


  }
}
