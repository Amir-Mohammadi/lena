//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Production.ProductionLineEmployeeIntervalDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    public ProductionLineEmployeeIntervalDetail AddProductionLineEmployeeIntervalDetail(
        int ProductionLineEmployeeIntervalId,
        short operationId,
        long time)
    {

      var productionLineEmployeeIntervalDetail = repository.Create<ProductionLineEmployeeIntervalDetail>();
      productionLineEmployeeIntervalDetail.OperationId = operationId;
      productionLineEmployeeIntervalDetail.ProductionLineEmployeeIntervalId = ProductionLineEmployeeIntervalId;
      productionLineEmployeeIntervalDetail.Time = time;
      repository.Add(productionLineEmployeeIntervalDetail);
      return productionLineEmployeeIntervalDetail;
    }


    #region Gets
    public IQueryable<TResult> GetProductionLineEmployeeIntervalDetails<TResult>(
        Expression<Func<ProductionLineEmployeeIntervalDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> operationId = null,
        TValue<int> ProductionLineEmployeeIntervalId = null,
        TValue<int> time = null)
    {

      var query = repository.GetQuery<ProductionLineEmployeeIntervalDetail>();
      if (id != null)
        query = query.Where(x => x.Id == id);
      if (operationId != null)
        query = query.Where(x => x.OperationId == operationId);
      if (ProductionLineEmployeeIntervalId != null)
        query = query.Where(x => x.ProductionLineEmployeeIntervalId == ProductionLineEmployeeIntervalId);
      if (time != null)
        query = query.Where(x => x.Time == time);
      return query.Select(selector);
    }
    #endregion


    #region ToResult

    public Expression<Func<ProductionLineEmployeeIntervalDetail, ProductionLineEmployeeIntervalDetailResult>> ToProductionLineEmplodyeeIntervalDetail =
                ProductionLineEmployeeIntervalDetailResult => new ProductionLineEmployeeIntervalDetailResult
                {
                  Id = ProductionLineEmployeeIntervalDetailResult.Id,
                  OperationId = ProductionLineEmployeeIntervalDetailResult.OperationId,
                  OperationTitle = ProductionLineEmployeeIntervalDetailResult.Operation.Title,
                  Time = ProductionLineEmployeeIntervalDetailResult.Time,
                  ProductionLineEmployeeIntervalId = ProductionLineEmployeeIntervalDetailResult.ProductionLineEmployeeIntervalId,
                  RowVersion = ProductionLineEmployeeIntervalDetailResult.RowVersion
                };


    #endregion

  }
}
