//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Accounting.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Accounting
{
  public partial class Accounting
  {
    #region Get
    public FinancialDocumentDiscount GetFinancialDocumentDiscount(int id) =>
        GetFinancialDocumentDiscount(selector: e => e, id: id);
    public TResult GetFinancialDocumentDiscount<TResult>(
        Expression<Func<FinancialDocumentDiscount, TResult>> selector,
        int id)
    {

      var financialDocumentDiscount = GetFinancialDocumentDiscounts(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (financialDocumentDiscount == null)
        throw new FinancialDocumentDiscountNotFoundException(id);
      return financialDocumentDiscount;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetFinancialDocumentDiscounts<TResult>(
        Expression<Func<FinancialDocumentDiscount, TResult>> selector,
        TValue<int> id = null,
        TValue<int> financialAccountId = null,
        TValue<double> cargoWeight = null,
        TValue<double> ladingWeight = null,
        TValue<DateTime> fromDocumentDateTime = null,
        TValue<DateTime> toDocumentDateTime = null,
        TValue<int> financialDocumentId = null,
        TValue<DiscountType> discountType = null)
    {

      var financialDocumentDiscounts = repository.GetQuery<FinancialDocumentDiscount>();
      if (id != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i => i.Id == id);
      if (financialDocumentId != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i => i.FinancialDocument.Id == financialDocumentId);
      if (financialAccountId != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i =>
                  i.FinancialDocument.FinancialAccountId == financialAccountId);
      if (fromDocumentDateTime != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i =>
                  i.FinancialDocument.DocumentDateTime > fromDocumentDateTime);
      if (toDocumentDateTime != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i =>
                  i.FinancialDocument.DocumentDateTime < toDocumentDateTime);
      if (discountType != null)
        financialDocumentDiscounts = financialDocumentDiscounts.Where(i => i.DiscountType == discountType);

      return financialDocumentDiscounts.Select(selector);
    }
    #endregion
    #region Add
    public FinancialDocumentDiscount AddFinancialDocumentDiscount(
        DiscountType discountType,
        FinancialDocument financialDocument)
    {

      var financialDocumentDiscount = repository.Create<FinancialDocumentDiscount>();
      financialDocumentDiscount.DiscountType = discountType;
      financialDocumentDiscount.FinancialDocument = financialDocument;
      repository.Add(financialDocumentDiscount);

      return financialDocumentDiscount;
    }
    #endregion
    #region Edit
    public FinancialDocumentDiscount EditFinancialDocumentDiscount(
        int id,
        byte[] rowVersion,
        TValue<DiscountType> discountType = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      var financialDocumentDiscount = GetFinancialDocumentDiscount(id: id);

      return EditFinancialDocumentDiscount(
                    financialDocumentDiscount: financialDocumentDiscount,
                    rowVersion: rowVersion,
                    discountType: discountType,
                    financialDocument: financialDocument);
    }

    public FinancialDocumentDiscount EditFinancialDocumentDiscount(
        FinancialDocumentDiscount financialDocumentDiscount,
        byte[] rowVersion,
        TValue<DiscountType> discountType = null,
        TValue<FinancialDocument> financialDocument = null)
    {

      if (discountType != null) financialDocumentDiscount.DiscountType = discountType;
      if (financialDocument != null) financialDocumentDiscount.FinancialDocument = financialDocument;

      repository.Update(rowVersion: rowVersion, entity: financialDocumentDiscount);
      return financialDocumentDiscount;
    }
    #endregion
  }
}
