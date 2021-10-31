using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.TagType;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public TagType GetTagType(int id) => GetTagType(selector: e => e, id: id);
    public TResult GetTagType<TResult>(
        Expression<Func<TagType, TResult>> selector,
        int id)
    {

      var tagType = GetTagTypes(selector: selector, id: id)


                .FirstOrDefault();
      if (tagType == null)
        throw new TagTypeNotFoundException(id);
      return tagType;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetTagTypes<TResult>(
        Expression<Func<TagType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null)
    {

      var query = repository.GetQuery<TagType>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (name != null)
        query = query.Where(x => x.Name == name);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public TagType AddTagType(string name)
    {

      var entity = repository.Create<TagType>();
      entity.Name = name;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Edit
    public TagType EditTagType(
        int id,
        byte[] rowVersion,
        TValue<string> name = null)
    {

      var tagType = GetTagType(id: id);
      if (name != null)
        tagType.Name = name;

      repository.Update(rowVersion: rowVersion, entity: tagType);
      return tagType;
    }
    #endregion
    #region Delete
    public void DeleteTagType(int id)
    {

      var tagType = GetTagType(id: id);
      repository.Delete(tagType);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<TagTypeResult> SortTagTypeResult(
        IQueryable<TagTypeResult> query,
        SortInput<TagTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case TagTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case TagTypeSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<TagTypeResult> SearchTagTypeResult(
        IQueryable<TagTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.Name.Contains(searchText)
                select item;


      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<TagType, TagTypeResult>> ToTagTypeResult =
        tagType => new TagTypeResult
        {
          Id = tagType.Id,
          Name = tagType.Name,
          RowVersion = tagType.RowVersion
        };
    #endregion
    #region ToComboResult
    public IQueryable<TagTypeComboResult> ToTagTypeComboResult(IQueryable<TagType> query)
    {
      var result = from tagType in query
                   select new TagTypeComboResult()
                   {
                     Id = tagType.Id,
                     Name = tagType.Name
                   };
      return result;
    }
    #endregion
  }
}
