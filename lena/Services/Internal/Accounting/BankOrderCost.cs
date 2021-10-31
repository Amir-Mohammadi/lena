using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Accounting.FinancialDocument;
using lena.Models.Accounting.RialInvoice;
using lena.Models.StaticData;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public BankOrderCost GetBankOrderCost(int id) =>
        GetBankOrderCost(selector: e => e, id: id);
    public TResult GetBankOrderCost<TResult>(
        Expression<Func<BankOrderCost, TResult>> selector,
        int id)
    {
      var bankOrderCost = GetBankOrderCosts(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (bankOrderCost == null)
        throw new LadingCostNotFoundException(id);
      return bankOrderCost;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetBankOrderCosts<TResult>(
        Expression<Func<BankOrderCost, TResult>> selector,
        TValue<int> id = null,
        TValue<int> bankOrderId = null,
        TValue<int> financialDocumentCostId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<CostType> costType = null,
        TValue<double> amount = null,
        TValue<bool> isDelete = null)
    {
      var bankOrderCosts = repository.GetQuery<BankOrderCost>();
      if (id != null)
        bankOrderCosts = bankOrderCosts.Where(i => i.Id == id);
      if (bankOrderId != null)
        bankOrderCosts = bankOrderCosts.Where(i => i.BankOrderId == bankOrderId);
      if (financialDocumentCostId != null)
        bankOrderCosts = bankOrderCosts.Where(i => i.FinancialDocumentCostId == financialDocumentCostId);
      if (amount != null)
        bankOrderCosts = bankOrderCosts.Where(i => i.Amount == amount);
      if (fromDocumentDateTime != null)
        bankOrderCosts = bankOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        bankOrderCosts = bankOrderCosts.Where(i =>
                  i.FinancialDocumentCost.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (costType != null)
        bankOrderCosts = bankOrderCosts.Where(i => i.FinancialDocumentCost.CostType == costType);
      if (isDelete != null)
        bankOrderCosts =
                  bankOrderCosts.Where(i => i.FinancialDocumentCost.FinancialDocument.IsDelete == isDelete);
      return bankOrderCosts.Select(selector);
    }
    #endregion
    #region Add
    public BankOrderCost AddBankOrderCost(
        int financialDocumentCostId,
        double amount,
        int bankOrderId)
    {
      var bankOrderCost = repository.Create<BankOrderCost>();
      bankOrderCost.FinancialDocumentCostId = financialDocumentCostId;
      bankOrderCost.Amount = amount;
      bankOrderCost.BankOrderId = bankOrderId;
      repository.Add(bankOrderCost);
      return bankOrderCost;
    }
    #endregion
    #region AddProcess
    public void AddBankOrderCostProcess(
        double amount,
        CostType costType,
        FinancialDocument financialDocument,
        int[] bankOrderIds)
    {
      var addedFinancialDocumentCost = AddFinancialDocumentCost(
                 costType: costType,
                 financialDocument: financialDocument);
      var financialDocumentCostId = addedFinancialDocumentCost.Id;
      foreach (var bankorderId in bankOrderIds)
      {
        AddBankOrderCost(
                   financialDocumentCostId: financialDocumentCostId,
                   amount: amount,
                   bankOrderId: bankorderId
                  );
      }
    }
    #endregion
    #region EditProcess
    public void EditBankOrderCostProcess(
        double amount,
        FinancialDocument financialDocument)
    {
      List<int> bankOrderCostIds = new List<int>();
      var bankOrderCosts = financialDocument.FinancialDocumentCost.BankOrderCosts;
      foreach (var item in bankOrderCosts)
      {
        bankOrderCostIds.Add(item.Id);
      }
      var financialDocumentCostId = financialDocument.FinancialDocumentCost.Id;
      foreach (var bankOrderCostId in bankOrderCostIds)
      {
        EditBankOrderCost(
                   id: bankOrderCostId,
                   amount: amount
                  );
      }
    }
    #endregion
    #region Edit
    public BankOrderCost EditBankOrderCost(
        int id,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> bankOrderId = null)
    {
      var bankOrderCost = GetBankOrderCost(id: id);
      return EditBankOrderCost(
                    bankOrderCost: bankOrderCost,
                    rowVersion: bankOrderCost.RowVersion,
                    financialDocumentCost: financialDocumentCost,
                    amount: amount,
                    bankOrderId: bankOrderId);
    }
    public BankOrderCost EditBankOrderCost(
        BankOrderCost bankOrderCost,
        byte[] rowVersion,
        TValue<FinancialDocumentCost> financialDocumentCost = null,
        TValue<double> amount = null,
        TValue<int> bankOrderId = null)
    {
      if (financialDocumentCost != null)
        bankOrderCost.FinancialDocumentCost = financialDocumentCost;
      if (amount != null)
        bankOrderCost.Amount = amount;
      if (bankOrderId != null)
        bankOrderCost.BankOrderId = bankOrderId;
      repository.Update(rowVersion: rowVersion, entity: bankOrderCost);
      return bankOrderCost;
    }
    #endregion
    #region Delete
    public void DeleteBankOrderCost(int id)
    {
      var bankOrderCost = GetBankOrderCost(id: id);
      DeleteBankOrderCost(bankOrderCost);
    }
    public void DeleteBankOrderCost(BankOrderCost bankOrderCost)
    {
      repository.Delete(bankOrderCost);
    }
    #endregion
  }
}