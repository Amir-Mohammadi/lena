using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using lena.Models.SaleManagement.PaymentType;
using lena.Models.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Domains;
using lena.Models.Common;
using lena.Services.Common;
using lena.Domains.Enums;
using lena.Services.Internals.SaleManagement.Exception;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {

    #region Add
    public PaymentType AddPaymentType(
               string name,
               bool isActive)
    {

      var paymentType = repository.Create<PaymentType>();
      paymentType.Name = name;
      paymentType.IsActive = isActive;
      repository.Add(paymentType);
      return paymentType;
    }
    #endregion

    #region Get
    public PaymentType GetPaymentType(int id) => GetPaymentType(selector: e => e, id: id);
    public TResult GetPaymentType<TResult>(
        Expression<Func<PaymentType, TResult>> selector,
        int id)
    {

      var result = GetPaymentTypes(
                selector: selector,
                id: id).FirstOrDefault();
      if (result == null)
        throw new PaymentTypeNotFoundException(id);
      return result;
    }


    #endregion
    #region Gets
    public IQueryable<TResult> GetPaymentTypes<TResult>(
        Expression<Func<PaymentType, TResult>> selector,
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> isActive = null
        )
    {

      var query = repository.GetQuery<PaymentType>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (name != null)
        query = query.Where(i => i.Name == name);
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);

      return query.Select(selector);
    }
    #endregion

    #region Remove Forwarder
    public void RemovePaymentType(int id, byte[] rowVersion)
    {

      var paymentType = GetPaymentType(id: id);

    }
    #endregion

    #region Delete PaymentType
    public void DeletePaymentType(
        int id,
        byte[] rowVersion)
    {

      #region RemovePaymentType
      RemovePaymentType(
              id: id,
              rowVersion: rowVersion);
      #endregion

    }
    #endregion


    #region EditProcess

    public PaymentType EditPaymentType(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<bool> isActive = null)
    {

      var paymentType = GetPaymentType(id: id);
      if (name != null)
        paymentType.Name = name;
      if (isActive != null)
        paymentType.IsActive = isActive;
      repository.Update(paymentType, rowVersion);
      return paymentType;
    }
    #endregion

    #region IsActiveChgange

    public PaymentType ActiveChangePaymentType(
        int id,
        byte[] rowVersion,
        TValue<bool> isActive = null)
    {

      var paymentType = GetPaymentType(id: id);
      if (isActive != null)
        paymentType.IsActive = isActive;
      repository.Update(paymentType, rowVersion);
      return paymentType;
    }
    #endregion
    #region Search
    public IQueryable<PaymentTypeResult> SearchPaymentType(IQueryable<PaymentTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems

        )
    {

      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Name.Contains(searchText));

      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region SortCombo
    public IOrderedQueryable<PaymentTypeComboResult> SortPaymentTypeComboResult(
        IQueryable<PaymentTypeComboResult> query, SortInput<PaymentTypeSortType> type)
    {
      switch (type.SortType)
      {
        case PaymentTypeSortType.Id:
          return query.OrderBy(a => a.Id, type.SortOrder);
        case PaymentTypeSortType.Name:
          return query.OrderBy(a => a.Name, type.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Sort
    public IOrderedQueryable<PaymentTypeResult> SortPaymentTypeResult(IQueryable<PaymentTypeResult> query,
        SortInput<PaymentTypeSortType> sort)
    {
      switch (sort.SortType)
      {
        case PaymentTypeSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case PaymentTypeSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToPaymentTypeResult
    public Expression<Func<PaymentType, PaymentTypeResult>> ToPaymentTypeResult =
        PaymentType => new PaymentTypeResult
        {
          Id = PaymentType.Id,
          Name = PaymentType.Name,
          RowVersion = PaymentType.RowVersion,
          IsActive = PaymentType.IsActive
        };
    #endregion
    #region ToPaymentTypeComboResult
    public Expression<Func<PaymentType, PaymentTypeComboResult>> ToPaymentTypeComboResult =
        PaymentType => new PaymentTypeComboResult
        {
          Id = PaymentType.Id,
          Name = PaymentType.Name,
          IsActive = PaymentType.IsActive

        };
    #endregion

  }
}
