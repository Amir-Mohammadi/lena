using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public FinancialDocumentCost GetFinancialDocumentCost(int id) =>
        GetFinancialDocumentCost(selector: e => e, id: id);
    public TResult GetFinancialDocumentCost<TResult>(
        Expression<Func<FinancialDocumentCost, TResult>> selector,
        int id)
    {

      var financialDocumentCost = GetFinancialDocumentCosts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (financialDocumentCost == null)
        throw new FinancialDocumentCostNotFoundException(id);
      return financialDocumentCost;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocumentCosts<TResult>(
        Expression<Func<FinancialDocumentCost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> financialAccountId = null,
        TValue<double> cargoWeight = null,
        TValue<double> ladingWeight = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<int> financialDocumentId = null,
        TValue<CostType> costType = null)
    {

      var financialDocumentCosts = repository.GetQuery<FinancialDocumentCost>();
      if (id != null)
        financialDocumentCosts = financialDocumentCosts.Where(i => i.Id == id);
      if (financialDocumentId != null)
        financialDocumentCosts = financialDocumentCosts.Where(i => i.FinancialDocument.Id == financialDocumentId);
      if (financialAccountId != null)
        financialDocumentCosts = financialDocumentCosts.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentCosts = financialDocumentCosts.Where(i =>
                  i.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentCosts = financialDocumentCosts.Where(i =>
                  i.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (costType != null)
        financialDocumentCosts = financialDocumentCosts.Where(i => i.CostType == costType);
      if (cargoWeight != null)
        financialDocumentCosts = financialDocumentCosts.Where(i => i.CargoWeight == cargoWeight);
      if (ladingWeight != null)
        financialDocumentCosts = financialDocumentCosts.Where(i => i.LadingWeight == ladingWeight);

      return financialDocumentCosts.Select(selector);
    }
    #endregion
    #region Add
    public FinancialDocumentCost AddFinancialDocumentCost(
        CostType costType,
        FinancialDocument financialDocument,
        double? cargoWeight = null,
        double? ladingWeight = null,
        TValue<double> kotazhTransport = null,
        TValue<double> entranceRightsCost = null,
        TValue<double> purchaseOrderWeight = null)
    {

      var financialDocumentCost = repository.Create<FinancialDocumentCost>();
      financialDocumentCost.CostType = costType;
      financialDocumentCost.FinancialDocument = financialDocument;
      financialDocumentCost.CargoWeight = cargoWeight;
      financialDocumentCost.LadingWeight = ladingWeight;
      financialDocumentCost.PurchaseOrderWeight = purchaseOrderWeight;
      financialDocumentCost.EntranceRightsCost = entranceRightsCost;
      financialDocumentCost.KotazhTransPort = kotazhTransport;
      repository.Add(financialDocumentCost);

      return financialDocumentCost;
    }
    #endregion
    #region Edit
    public FinancialDocumentCost EditFinancialDocumentCost(
        int id,
        byte[] rowVersion,
        TValue<CostType> costType = null,
        TValue<double> kotazhTransPort = null,
        TValue<double> entranceRightsCost = null,
        TValue<FinancialDocument> financialDocument = null,
        TValue<double> cargoWeight = null,
        TValue<double> ladingWeight = null)
    {

      var financialDocumentCost = GetFinancialDocumentCost(id: id);

      return EditFinancialDocumentCost(
                    financialDocumentCost: financialDocumentCost,
                    rowVersion: rowVersion,
                    costType: costType,
                    kotazhTransPort: kotazhTransPort,
                    entranceRightsCost: entranceRightsCost,
                    financialDocument: financialDocument,
                    cargoWeight: cargoWeight,
                    ladingWeight: ladingWeight);
    }

    public FinancialDocumentCost EditFinancialDocumentCost(
        FinancialDocumentCost financialDocumentCost,
        byte[] rowVersion,
        TValue<CostType> costType = null,
        TValue<double> kotazhTransPort = null,
        TValue<double> entranceRightsCost = null,
        TValue<FinancialDocument> financialDocument = null,
        TValue<double> cargoWeight = null,
        TValue<double> ladingWeight = null)
    {

      if (costType != null)
        financialDocumentCost.CostType = costType;
      if (financialDocument != null)
        financialDocumentCost.FinancialDocument = financialDocument;
      if (cargoWeight != null)
        financialDocumentCost.CargoWeight = cargoWeight;
      if (ladingWeight != null)
        financialDocumentCost.LadingWeight = ladingWeight;
      if (entranceRightsCost != null)
        financialDocumentCost.EntranceRightsCost = entranceRightsCost;
      if (kotazhTransPort != null)
        financialDocumentCost.KotazhTransPort = kotazhTransPort;

      repository.Update(rowVersion: rowVersion, entity: financialDocumentCost);
      return financialDocumentCost;
    }
    #endregion
  }
}
