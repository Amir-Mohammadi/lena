using System;
using System.Linq;
using System.Linq.Expressions;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Get
    public ProductionPlanDetailLevel GetProductionPlanDetailLevel(int id) => GetProductionPlanDetailLevel(selector: e => e, id: id);
    public TResult GetProductionPlanDetailLevel<TResult>(
        Expression<Func<ProductionPlanDetailLevel, TResult>> selector,
        int id)
    {

      var entity = GetProductionPlandDetailLevels(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (entity == null)
        throw new OperationNotFoundException(id);
      return entity;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionPlandDetailLevels<TResult>(
        Expression<Func<ProductionPlanDetailLevel, TResult>> selector,
        TValue<int> id = null,
        TValue<int?> parentId = null)
    {

      var query = repository.GetQuery<ProductionPlanDetailLevel>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (parentId != null)
        query = query.Where(x => x.ParentId == parentId);
      return query.Select(selector);
    }
    #endregion
    #region Add
    public ProductionPlanDetailLevel AddProductionPlanDetailLevel(
        int? parentId)
    {

      var entity = repository.Create<ProductionPlanDetailLevel>();
      entity.ParentId = parentId;
      repository.Add(entity);
      return entity;
    }
    #endregion
    #region Delete
    public void DeleteProductionPlanDetailLevel(int id)
    {

      var productionPlanDetailLevel = GetProductionPlanDetailLevel(id: id);
      repository.Delete(productionPlanDetailLevel);
    }
    #endregion
  }
}
