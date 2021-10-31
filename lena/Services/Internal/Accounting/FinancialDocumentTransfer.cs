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
    public FinancialDocumentTransfer GetFinancialDocumentTransfer(int id) =>
        GetFinancialDocumentTransfer(selector: e => e, id: id);
    public TResult GetFinancialDocumentTransfer<TResult>(
        Expression<Func<FinancialDocumentTransfer, TResult>> selector,
        int id)
    {

      var financialDocumentTransfer = GetFinancialDocumentTransfers(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (financialDocumentTransfer == null)
        throw new FinancialDocumentTransferNotFoundException(id);
      return financialDocumentTransfer;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocumentTransfers<TResult>(
        Expression<Func<FinancialDocumentTransfer, TResult>> selector,
        TValue<int> id = null,
        TValue<int> financialAccountId = null,
        TValue<int> toFinancialAccountId = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<int> financialDocumentId = null)
    {

      var financialDocumentTransfers = repository.GetQuery<FinancialDocumentTransfer>();
      if (id != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i => i.Id == id);
      if (financialDocumentId != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i => i.FinancialDocument.Id == financialDocumentId);
      if (financialAccountId != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (toFinancialAccountId != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i => i.ToFinancialAccountId == toFinancialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i =>
                  i.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentTransfers = financialDocumentTransfers.Where(i =>
                  i.FinancialDocument.DocumentDateTime < toDocumentDateTime);

      return financialDocumentTransfers.Select(selector);
    }
    #endregion
    #region Add
    public FinancialDocumentTransfer AddFinancialDocumentTransfer(
        int toFinancialAccountId,
        double toDebitAmount,
        FinancialDocument financialDocument)
    {

      var financialDocumentTransfer = repository.Create<FinancialDocumentTransfer>();
      financialDocumentTransfer.ToFinancialAccountId = toFinancialAccountId;
      financialDocumentTransfer.ToDebitAmount = toDebitAmount;
      financialDocumentTransfer.FinancialDocument = financialDocument;
      repository.Add(financialDocumentTransfer);
      return financialDocumentTransfer;
    }
    #endregion

    #region Edit
    public FinancialDocumentTransfer EditFinancialDocumentTransfer(
        int financialDocumentTransferId,
        byte[] rowVersion,
        TValue<int> toFinancialAccountId,
        TValue<double> toDebitAmount,
        TValue<FinancialDocument> financialDocument)
    {

      var financialDocumentTransfer = GetFinancialDocumentTransfer(id: financialDocumentTransferId);

      return EditFinancialDocumentTransfer(
                    financialDocumentTransfer: financialDocumentTransfer,
                    rowVersion: rowVersion,
                    toFinancialAccountId: toFinancialAccountId,
                    toDebitAmount: toDebitAmount,
                    financialDocument: financialDocument);
    }

    public FinancialDocumentTransfer EditFinancialDocumentTransfer(
        FinancialDocumentTransfer financialDocumentTransfer,
        byte[] rowVersion,
        TValue<int> toFinancialAccountId,
        TValue<double> toDebitAmount,
        TValue<FinancialDocument> financialDocument)
    {

      if (toFinancialAccountId != null) financialDocumentTransfer.ToFinancialAccountId = toFinancialAccountId;
      if (toDebitAmount != null) financialDocumentTransfer.ToDebitAmount = toDebitAmount;
      if (financialDocument != null) financialDocumentTransfer.FinancialDocument = financialDocument;

      repository.Update(rowVersion: rowVersion, entity: financialDocumentTransfer);
      return financialDocumentTransfer;
    }
    #endregion
  }
}
