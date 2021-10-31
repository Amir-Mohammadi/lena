//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Production.Exception;
using lena.Models.Common;
using lena.Domains;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
using lena.Models.Production.ProductionOperation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Get
    public FaildProductionOperation GetFaildProductionOperation(int id) => GetFaildProductionOperation(selector: e => e, id: id);
    public TResult GetFaildProductionOperation<TResult>(
        Expression<Func<FaildProductionOperation, TResult>> selector,
        int id)
    {

      var faildProductionOperation = GetFaildProductionOperations(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (faildProductionOperation == null)
        throw new FaildProductionOperationNotFoundException(id);
      return faildProductionOperation;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFaildProductionOperations<TResult>(
            Expression<Func<FaildProductionOperation, TResult>> selector,
            TValue<int> id = null,
            TValue<int> repairProductionId = null
            )
    {

      var query = repository.GetQuery<FaildProductionOperation>();
      if (id != null) query = query.Where(x => x.Id == id);
      if (repairProductionId != null) query = query.Where(x => x.RepairProductionId == repairProductionId);

      return query.Select(selector);
    }
    #endregion
    #region Add
    public FaildProductionOperation AddFaildProductionOperation(
            int? repairProductionId,
            int productionOperationId
        )
    {

      var faildProductionOperation = repository.Create<FaildProductionOperation>();
      var productionOperation = GetProductionOperation(productionOperationId);
      faildProductionOperation.BaseProductionOperation = productionOperation;
      faildProductionOperation.ReworkProductionOperation = null;
      faildProductionOperation.RepairProductionId = repairProductionId;

      repository.Add(faildProductionOperation);
      return faildProductionOperation;
    }
    #endregion
    #region Edit
    public FaildProductionOperation EditFaildProductionOperation(
        int id,
        byte[] rowVersion,
        TValue<int> repairProductionId = null,
        TValue<int> baseProductionOperationId = null,
        TValue<int> reworkProductionOperationId = null
        )
    {

      var faildProductionOperation = GetFaildProductionOperation(id: id);
      return EditFaildProductionOperation(
                 faildProductionOperation: faildProductionOperation,
                rowVersion: rowVersion,
                repairProductionId: repairProductionId,
                baseProductionOperationId: baseProductionOperationId,
                reworkProductionOperationId: reworkProductionOperationId

                );

    }

    public FaildProductionOperation EditFaildProductionOperation(
                FaildProductionOperation faildProductionOperation,
                byte[] rowVersion,
                TValue<int> repairProductionId = null,
                TValue<int> baseProductionOperationId = null,
                TValue<int> reworkProductionOperationId = null
                )
    {


      if (repairProductionId != null) faildProductionOperation.RepairProductionId = repairProductionId;
      if (baseProductionOperationId != null)
      {
        var baseProductionOperaation = GetProductionOperation(baseProductionOperationId);
        faildProductionOperation.BaseProductionOperation = baseProductionOperaation;
      }

      if (reworkProductionOperationId != null)
      {
        var reworkProductionOperaation = GetProductionOperation(reworkProductionOperationId);
        faildProductionOperation.ReworkProductionOperation = reworkProductionOperaation;
      }
      repository.Update(rowVersion: rowVersion, entity: faildProductionOperation);
      return faildProductionOperation;
    }

    #endregion
    #region Delete
    public void DeleteFaildProductionOperation(int id)
    {

      var faildProductionOperation = GetFaildProductionOperation(id: id);
      repository.Delete(faildProductionOperation);
    }
    #endregion
    #region ToResult

    public Expression<Func<FaildProductionOperation, ProductionOperationResult>> ToFaildProductionOperationResult =
        entity => new ProductionOperationResult()
        {
          Id = entity.Id,
          OperationId = entity.BaseProductionOperation.OperationId,
          OperationName = entity.BaseProductionOperation.Operation.Title
        };






    #endregion
  }
}
