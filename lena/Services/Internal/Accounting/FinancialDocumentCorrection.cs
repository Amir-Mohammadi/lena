using System;
using System.Linq;
using System.Linq.Expressions;
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
    public FinancialDocumentCorrection GetFinancialDocumentCorrection(int id) =>
        GetFinancialDocumentCorrection(selector: e => e, id: id);
    public TResult GetFinancialDocumentCorrection<TResult>(
        Expression<Func<FinancialDocumentCorrection, TResult>> selector,
        int id)
    {

      var financialDocumentCorrection = GetFinancialDocumentCorrections(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (financialDocumentCorrection == null)
        throw new FinancialDocumentCorrectionNotFoundException(id);
      return financialDocumentCorrection;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocumentCorrections<TResult>(
        Expression<Func<FinancialDocumentCorrection, TResult>> selector,
        TValue<int> id = null,
        TValue<bool> isDelete = null,
        TValue<bool> isActive = null,
        TValue<int> financialAccountId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<int> financialDocumentId = null)
    {

      var financialDocumentCorrections = repository.GetQuery<FinancialDocumentCorrection>();
      if (id != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i => i.Id == id);
      if (isDelete != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i => i.FinancialDocument.IsDelete == isDelete);
      if (isActive != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i => i.IsActive == isActive);
      if (financialDocumentId != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i => i.FinancialDocument.Id == financialDocumentId);
      if (financialTransactionLevel != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i => i.FinancialTransactionLevel == financialTransactionLevel);
      if (financialAccountId != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i =>
                  i.FinancialDocument.DocumentDateTime >= fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentCorrections = financialDocumentCorrections.Where(i =>
                  i.FinancialDocument.DocumentDateTime <= toDocumentDateTime);

      return financialDocumentCorrections.Select(selector);
    }
    #endregion
    #region Add FinancialDocumentCorrection
    public FinancialDocumentCorrection AddFinancialDocumentCorrection(
        bool isActive,
        FinancialTransactionLevel financialTransactionLevel,
        FinancialDocument financialDocument)
    {

      var financialDocumentCorrection = repository.Create<FinancialDocumentCorrection>();
      financialDocumentCorrection.IsActive = isActive;
      financialDocumentCorrection.FinancialTransactionLevel = financialTransactionLevel;
      financialDocumentCorrection.FinancialDocument = financialDocument;
      repository.Add(financialDocumentCorrection);
      return financialDocumentCorrection;
    }
    #endregion

    #region EditFinancialDocumentCorrection
    public FinancialDocumentCorrection EditFinancialDocumentCorrection(
        int id,
        byte[] rowVersion,
        TValue<bool> isActive = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      var financialDocumentCorrection = GetFinancialDocumentCorrection(id: id);

      return EditFinancialDocumentCorrection(
                    financialDocumentCorrection: financialDocumentCorrection,
                    rowVersion: rowVersion,
                    isActive: isActive,
                    financialTransactionLevel: financialTransactionLevel,
                    financialDocument: financialDocument);
    }

    public FinancialDocumentCorrection EditFinancialDocumentCorrection(
        FinancialDocumentCorrection financialDocumentCorrection,
        byte[] rowVersion,
        TValue<bool> isActive = null,
        TValue<FinancialTransactionLevel> financialTransactionLevel = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      if (isActive != null)
        financialDocumentCorrection.IsActive = isActive;
      if (financialTransactionLevel != null)
        financialDocumentCorrection.FinancialTransactionLevel = financialTransactionLevel;
      if (financialDocument != null)
        financialDocumentCorrection.FinancialDocument = financialDocument;

      repository.Update(rowVersion: rowVersion, entity: financialDocumentCorrection);
      return financialDocumentCorrection;
    }
    #endregion
  }
}
