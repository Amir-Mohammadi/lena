using System;
using System.Linq;
using lena.Domains;
using lena.Services.Common;
using lena.Models.Common;
using lena.Domains;
using System.Linq.Expressions;
using lena.Models;
using lena.Models.Common;
using lena.Models.QualityAssurance.ApiInfo;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityAssurance
{
  public partial class QualityAssurance
  {
    #region Add
    public ApiInfo AddApiInfo(
        string name,
        string url,
        string param,
        string sortTypeName,
        string sortTypeFieldName,
        string description)
    {

      var ApiInfo = repository.Create<ApiInfo>();
      ApiInfo.Name = name;
      ApiInfo.Url = url;
      ApiInfo.Param = param;
      ApiInfo.SortTypeName = sortTypeName;
      ApiInfo.SortTypeFieldName = sortTypeFieldName;
      ApiInfo.Description = description;
      repository.Add(ApiInfo);
      return ApiInfo;
    }
    #endregion

    #region Get
    public ApiInfo GetApiInfo(int id) => GetApiInfo(selector: e => e, id: id);
    public TResult GetApiInfo<TResult>(
        Expression<Func<ApiInfo, TResult>> selector,
        int id)
    {

      var result = GetApiInfos(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new ApiInfoNotFoundException(id);
      return result;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetApiInfos<TResult>(
        Expression<Func<ApiInfo, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<string> url = null,
        TValue<string> param = null,
        TValue<string> sortTypeName = null,
        TValue<string> sortTypeFieldName = null
        )
    {

      var ApiInfos = repository.GetQuery<ApiInfo>();
      if (id != null)
        ApiInfos = ApiInfos.Where(x => x.Id == id);
      if (name != null)
        ApiInfos = ApiInfos.Where(i => i.Name == name);
      if (url != null)
        ApiInfos = ApiInfos.Where(i => i.Url == url);
      if (param != null)
        ApiInfos = ApiInfos.Where(i => i.Param == param);
      if (sortTypeFieldName != null)
        ApiInfos = ApiInfos.Where(i => i.SortTypeFieldName == sortTypeFieldName);
      if (sortTypeName != null)
        ApiInfos = ApiInfos.Where(i => i.SortTypeName == sortTypeName);

      return ApiInfos.Select(selector);
    }
    #endregion

    #region Delete
    public void DeleteApiInfo(int id)
    {

      var apiInfo = GetApiInfo(id);
      repository.Delete(apiInfo);
    }
    #endregion

    #region Edit ApiInfo

    public ApiInfo EditApiInfo(
        byte[] rowVersion,
        int id,
        TValue<string> name = null,
        TValue<string> url = null,
        TValue<string> param = null,
        TValue<string> sortTypeName = null,
        TValue<string> sortTypeFeildName = null,
        TValue<string> description = null)
    {

      var ApiInfo = GetApiInfo(id: id);
      if (name != null)
        ApiInfo.Name = name;
      if (url != null)
        ApiInfo.Url = url;
      if (param != null)
        ApiInfo.Param = param;
      if (description != null)
        ApiInfo.Description = description;
      if (sortTypeName != null)
        ApiInfo.SortTypeName = sortTypeName;
      if (sortTypeFeildName != null)
        ApiInfo.SortTypeFieldName = sortTypeFeildName;

      repository.Update(ApiInfo, ApiInfo.RowVersion);
      return ApiInfo;
    }
    #endregion

    #region Search
    public IQueryable<ApiInfoResult> SearchApiInfo(IQueryable<ApiInfoResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText));

      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

    #region SortCombo
    public IOrderedQueryable<ApiInfoComboResult> SortApiInfoComboResult(
        IQueryable<ApiInfoComboResult> query, SortInput<ApiInfoSortType> type)
    {
      switch (type.SortType)
      {
        case ApiInfoSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case ApiInfoSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region Sort
    public IOrderedQueryable<ApiInfoResult> SortApiInfoResult(IQueryable<ApiInfoResult> query,
        SortInput<ApiInfoSortType> sort)
    {
      switch (sort.SortType)
      {
        case ApiInfoSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case ApiInfoSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToApiInfoResult
    public Expression<Func<ApiInfo, ApiInfoResult>> ToApiInfoResult =
        ApiInfo => new ApiInfoResult
        {
          Id = ApiInfo.Id,
          Url = ApiInfo.Url,
          Name = ApiInfo.Name,
          Param = ApiInfo.Param,
          SortTypeName = ApiInfo.SortTypeName,
          SortTypeFieldName = ApiInfo.SortTypeFieldName,
          Description = ApiInfo.Description,
          RowVersion = ApiInfo.RowVersion
        };
    #endregion

    #region ToApiInfoComboResult
    public Expression<Func<ApiInfo, ApiInfoComboResult>> ToApiInfoComboResult =
        ApiInfo => new ApiInfoComboResult
        {
          Id = ApiInfo.Id,
          Url = ApiInfo.Url,
          Name = ApiInfo.Name,
          Param = ApiInfo.Param,
          SortTypeName = ApiInfo.SortTypeName,
          SortTypeFeildName = ApiInfo.SortTypeFieldName
        };
    #endregion

  }
}
