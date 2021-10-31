using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.WarehouseManagement.StuffRequest;
using lena.Models.WarehouseManagement.StuffRequestItem;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Get
    public TResult GetProductionMaterialRequestDetail<TResult>(
        Expression<Func<ProductionMaterialRequestDetail, TResult>> selector,
        int id)
    {

      var result = GetProductionMaterialRequestDetails(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new ProductionMaterialRequestDetailNotFoundException(id);
      return result;
    }

    #endregion
    #region Gets
    public IQueryable<TResult> GetProductionMaterialRequestDetails<TResult>(
        Expression<Func<ProductionMaterialRequestDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> productionOrderId = null,
        TValue<int> productionMaterialRequestId = null
        )
    {

      var details = repository.GetQuery<ProductionMaterialRequestDetail>();

      if (productionOrderId != null)
        details = details.Where(r => r.ProductionOrderId == productionOrderId);

      if (productionMaterialRequestId != null)
        details = details.Where(r => r.ProductionMaterialRequestId == productionMaterialRequestId);

      return details.Select(selector);


    }
    #endregion
    #region Add
    /// <summary>
    /// 
    /// </summary>
    /// <param name="detail">Optional</param>
    /// <param name="productionOrderId"></param>
    /// <param name="productionMaterialRequestId"></param>
    /// <returns></returns>
    public ProductionMaterialRequestDetail AddProductionMaterialRequestDetail(
        ProductionMaterialRequestDetail detail,
        int productionOrderId,
        int productionMaterialRequestId)
    {

      detail = detail ?? repository.Create<ProductionMaterialRequestDetail>();
      detail.ProductionOrderId = productionOrderId;
      detail.ProductionMaterialRequestId = productionMaterialRequestId;
      repository.Add(detail);
      return detail;
    }
    #endregion
    #region Edit
    public ProductionMaterialRequestDetail EditProductionMaterialRequestDetail(
        int id,
        byte[] rowVersion,
        TValue<int> productionOrderId = null,
        TValue<int> productionMaterialRequestId = null)
    {

      var productionMaterialRequest = GetProductionMaterialRequestDetail(e => e, id: id);

      return EditProductionMaterialRequestDetail(
                    detail: productionMaterialRequest,
                    rowVersion: rowVersion,
                    productionOrderId: productionOrderId,
                    productionMaterialRequestId: productionMaterialRequestId);
    }
    public ProductionMaterialRequestDetail EditProductionMaterialRequestDetail(
        ProductionMaterialRequestDetail detail,
        byte[] rowVersion,
        TValue<int> productionOrderId = null,
        TValue<int> productionMaterialRequestId = null)
    {

      if (productionOrderId != null)
        detail.ProductionOrderId = productionOrderId;
      if (productionMaterialRequestId != null)
        detail.ProductionMaterialRequestId = productionMaterialRequestId;

      repository.Update(detail, rowVersion);

      return detail;
    }
    #endregion



  }
}
