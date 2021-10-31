using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core;
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
    public FinancialDocumentBeginning GetFinancialDocumentBeginning(int id) =>
        GetFinancialDocumentBeginning(selector: e => e, id: id);
    public TResult GetFinancialDocumentBeginning<TResult>(
        Expression<Func<FinancialDocumentBeginning, TResult>> selector,
        int id)
    {

      var financialDocumentBeginning = GetFinancialDocumentBeginnings(
                selector: selector,
                id: id)


                .FirstOrDefault();
      if (financialDocumentBeginning == null)
        throw new FinancialDocumentBeginningNotFoundException(id);
      return financialDocumentBeginning;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocumentBeginnings<TResult>(
        Expression<Func<FinancialDocumentBeginning, TResult>> selector,
        TValue<int> id = null,
        TValue<int> financialAccountId = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<int> financialDocumentId = null)
    {

      var financialDocumentBeginnings = repository.GetQuery<FinancialDocumentBeginning>();
      if (id != null)
        financialDocumentBeginnings = financialDocumentBeginnings.Where(i => i.Id == id);
      if (financialDocumentId != null)
        financialDocumentBeginnings =
                  financialDocumentBeginnings.Where(i => i.FinancialDocument.Id == financialDocumentId);
      if (financialTransactionLevel != null)
        financialDocumentBeginnings = financialDocumentBeginnings.Where(i =>
                  i.FinancialTransactionLevel == financialTransactionLevel);
      if (financialAccountId != null)
        financialDocumentBeginnings = financialDocumentBeginnings.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentBeginnings = financialDocumentBeginnings.Where(i =>
                  i.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentBeginnings = financialDocumentBeginnings.Where(i =>
                  i.FinancialDocument.DocumentDateTime < toDocumentDateTime);

      return financialDocumentBeginnings.Select(selector);
    }
    #endregion
    #region Add FinancialDocumentBeginning
    public FinancialDocumentBeginning AddFinancialDocumentBeginning(
        FinancialTransactionLevel financialTransactionLevel,
        FinancialDocument financialDocument)
    {

      var financialDocumentBeginning = repository.Create<FinancialDocumentBeginning>();
      financialDocumentBeginning.FinancialTransactionLevel = financialTransactionLevel;
      financialDocumentBeginning.FinancialDocument = financialDocument;
      repository.Add(financialDocumentBeginning);
      return financialDocumentBeginning;
    }
    #endregion

    #region EditFinancialDocumentBeginning
    public FinancialDocumentBeginning EditFinancialDocumentBeginning(
        int id,
        byte[] rowVersion,
        TValue<bool> isActive = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      var financialDocumentBeginning = GetFinancialDocumentBeginning(id: id);

      return EditFinancialDocumentBeginning(
                    financialDocumentBeginning: financialDocumentBeginning,
                    rowVersion: rowVersion,
                    financialTransactionLevel: financialTransactionLevel,
                    financialDocument: financialDocument);
    }

    public FinancialDocumentBeginning EditFinancialDocumentBeginning(
        FinancialDocumentBeginning financialDocumentBeginning,
        byte[] rowVersion,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      if (financialTransactionLevel != null)
        financialDocumentBeginning.FinancialTransactionLevel = financialTransactionLevel;
      if (financialDocument != null)
        financialDocumentBeginning.FinancialDocument = financialDocument;

      repository.Update(rowVersion: rowVersion, entity: financialDocumentBeginning);
      return financialDocumentBeginning;
    }
    #endregion
  }
}
