using System;
using System.Linq;
using lena.Services.Core;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.QualityControl.Exception;
using lena.Models.Common;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Domains;
using lena.Models.QualityControl.StuffQualityControlObservation;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.QualityControl
{
  public partial class QualityControl
  {
    #region Add
    public StuffQualityControlObservation AddStuffQualityControlObservation(
        string description,
        int stuffId
    )
    {
      var stuffQualityControlObservation = repository.Create<StuffQualityControlObservation>();
      stuffQualityControlObservation.Description = description;
      stuffQualityControlObservation.StuffId = stuffId;
      stuffQualityControlObservation.RegisterarUserId = App.Providers.Security.CurrentLoginData.UserId;
      stuffQualityControlObservation.RegisterDateTime = DateTime.UtcNow;
      repository.Add(stuffQualityControlObservation);
      return stuffQualityControlObservation;
    }
    #endregion
    #region Edit
    public StuffQualityControlObservation EditStuffQualityControlObservation(byte[] rowVersion, int id, TValue<string> description = null
        )
    {
      var stuffQualityControlObservation = GetStuffQualityControlObservation(id: id);
      if (description != null)
        stuffQualityControlObservation.Description = description;
      repository.Update(rowVersion: stuffQualityControlObservation.RowVersion, entity: stuffQualityControlObservation);
      return stuffQualityControlObservation;
    }
    #endregion
    #region Gets
    internal IQueryable<TResult> GetStuffQualityControlObservations<TResult>(
         Expression<Func<StuffQualityControlObservation, TResult>> selector,
         TValue<int> id = null,
         TValue<string> description = null,
         TValue<int> stuffId = null)
    {
      var baseQuery = repository.GetQuery<StuffQualityControlObservation>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (description != null)
        baseQuery = baseQuery.Where(i => i.Description == description);
      if (stuffId != null)
        baseQuery = baseQuery.Where(i => i.StuffId == stuffId);
      return baseQuery.Select(selector);
    }
    #endregion
    #region ToResult
    public IQueryable<StuffQualityControlObservationResult> ToStuffQualityControlObservationResultQuery(
       IQueryable<StuffQualityControlObservation> stuffQualityControlObservations
       )
    {
      var resultQuery = from stuffQualityControlObservation in stuffQualityControlObservations
                        select new StuffQualityControlObservationResult
                        {
                          Id = stuffQualityControlObservation.Id,
                          Description = stuffQualityControlObservation.Description,
                          RowVersion = stuffQualityControlObservation.RowVersion,
                          RegisterarUserName = stuffQualityControlObservation.RegisterarUser.Employee.FirstName + " " + stuffQualityControlObservation.RegisterarUser.Employee.LastName,
                          RegisterDateTime = stuffQualityControlObservation.RegisterDateTime
                        };
      return resultQuery;
    }
    #endregion
    #region Search
    public IQueryable<StuffQualityControlObservationResult> SearchStuffQualityControlObservation(IQueryable<StuffQualityControlObservationResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Description.Contains(searchText) ||
            item.RegisterarUserName.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffQualityControlObservationResult> SortStuffQualityControlObservationResult(IQueryable<StuffQualityControlObservationResult> query,
        SortInput<StuffQualityControlObservationSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffQualityControlObservationSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case StuffQualityControlObservationSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        case StuffQualityControlObservationSortType.RegisterarUserName:
          return query.OrderBy(a => a.RegisterarUserName, sort.SortOrder);
        case StuffQualityControlObservationSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Get
    public StuffQualityControlObservation GetStuffQualityControlObservation(int id)
    {
      var stuffQualityControlObservation = GetStuffQualityControlObservations(selector: e => e, id: id).FirstOrDefault();
      return stuffQualityControlObservation;
    }
    #endregion
    #region Delete
    public void DeleteStuffQualityControlObservation(int stuffQualityControlObservationId)
    {
      var stuffQualityControlObservation = GetStuffQualityControlObservation(stuffQualityControlObservationId);
      repository.Delete(stuffQualityControlObservation);
    }
    #endregion
  }
}