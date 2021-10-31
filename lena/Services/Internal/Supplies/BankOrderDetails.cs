using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Supplies.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.Supplies.BankOrderDetail;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Supplies
{
  public partial class Supplies
  {
    #region Add
    public BankOrderDetail AddBankOrderDetail(
        BankOrderDetail bankOrderDetail,
        int bankOrderId,
        int index,
        double price,
        double weight,
        int stuffHSGroupId,
        double fee,
        byte unitId,
        double qty,
        double grossWeight,
        string description)
    {
      bankOrderDetail = bankOrderDetail ?? repository.Create<BankOrderDetail>();
      bankOrderDetail.BankOrderId = bankOrderId;
      bankOrderDetail.Index = index;
      bankOrderDetail.Price = price;
      bankOrderDetail.Weight = weight;
      bankOrderDetail.StuffHSGroupId = stuffHSGroupId;
      bankOrderDetail.Fee = fee;
      bankOrderDetail.UnitId = unitId;
      bankOrderDetail.Qty = qty;
      bankOrderDetail.GrossWeight = grossWeight;
      bankOrderDetail.Description = description;
      repository.Add(bankOrderDetail);
      return bankOrderDetail;
    }
    #endregion
    #region AddProcess
    public BankOrderDetail AddBankOrderDetailProcess(
        BankOrderDetail bankOrderDetail,
        int bankOrderId,
        int index,
        double price,
        double weight,
        int stuffHSGroupId,
        double fee,
        byte unitId,
        double qty,
        double grossWeight,
        string description)
    {
      #region AddBankOrderDetail
      bankOrderDetail = AddBankOrderDetail(
              bankOrderDetail: bankOrderDetail,
              bankOrderId: bankOrderId,
              index: index,
              price: price,
              weight: weight,
              stuffHSGroupId: stuffHSGroupId,
              fee: fee,
              qty: qty,
              unitId: unitId,
              grossWeight: grossWeight,
              description: description);
      #endregion
      return bankOrderDetail;
    }
    #endregion
    #region Edit
    public BankOrderDetail EditBankOrderDetail(
        int id,
        byte[] rowVersion,
        TValue<int> index = null,
        TValue<int> stuffHSGroupId = null,
        TValue<double> price = null,
        TValue<double> weight = null,
        TValue<double> fee = null,
        TValue<byte> unitId = null,
        TValue<double> qty = null,
        TValue<double> grossWeight = null,
        TValue<string> description = null)
    {
      var BankOrderDetail = GetBankOrderDetail(id: id);
      return EditBankOrderDetail(
                    bankOrderDetail: BankOrderDetail,
                    rowVersion: rowVersion,
                    index: index,
                    stuffHSGroupId: stuffHSGroupId,
                    price: price,
                    weight: weight,
                    fee: fee,
                    unitId: unitId,
                    qty: qty,
                    grossWeight: grossWeight,
                    description: description);
    }
    public BankOrderDetail EditBankOrderDetail(
        BankOrderDetail bankOrderDetail,
        byte[] rowVersion,
        TValue<int> index = null,
        TValue<int> stuffHSGroupId = null,
        TValue<double> price = null,
        TValue<double> weight = null,
        TValue<double> fee = null,
        TValue<byte> unitId = null,
        TValue<double> qty = null,
        TValue<double> grossWeight = null,
        TValue<string> description = null)
    {
      if (index != null)
        bankOrderDetail.Index = index;
      if (stuffHSGroupId != null)
        bankOrderDetail.StuffHSGroupId = stuffHSGroupId;
      if (price != null)
        bankOrderDetail.Price = price;
      if (weight != null)
        bankOrderDetail.Weight = weight;
      if (description != null)
        bankOrderDetail.Description = description;
      if (fee != null)
        bankOrderDetail.Fee = fee;
      if (unitId != null)
        bankOrderDetail.UnitId = unitId;
      if (qty != null)
        bankOrderDetail.Qty = qty;
      if (grossWeight != null)
        bankOrderDetail.GrossWeight = grossWeight;
      repository.Update(bankOrderDetail, rowVersion);
      return bankOrderDetail;
    }
    #endregion
    #region RemoveProcess
    public BankOrderDetail RemoveBankOrderDetailProcess(
        int id,
        byte[] rowVersion)
    {
      var bankOrderDetail = GetBankOrderDetail(id: id);
      repository.Delete(bankOrderDetail);
      return bankOrderDetail as BankOrderDetail;
    }
    #endregion
    #region Get
    public BankOrderDetail GetBankOrderDetail(int id) => GetBankOrderDetail(selector: e => e, id: id);
    public TResult GetBankOrderDetail<TResult>(
        Expression<Func<BankOrderDetail, TResult>> selector,
        int id)
    {
      var result = GetBankOrderDetails(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (result == null)
        throw new BankOrderDetailNotFoundException(id);
      return result;
    }
    public IQueryable<TResult> GetBankOrderDetails<TResult>(
        Expression<Func<BankOrderDetail, TResult>> selector,
        TValue<int> id = null,
        TValue<int> index = null,
        TValue<int> stuffHSGroupId = null,
        TValue<double> weight = null,
        TValue<double> price = null,
        TValue<int> bankOrderId = null,
        TValue<string> description = null)
    {
      var query = repository.GetQuery<BankOrderDetail>();
      if (index != null)
        query = query.Where(r => r.Index == index);
      if (stuffHSGroupId != null)
        query = query.Where(r => r.StuffHSGroupId == stuffHSGroupId);
      if (weight != null)
        query = query.Where(r => r.Weight == weight);
      if (price != null)
        query = query.Where(r => r.Price == price);
      if (bankOrderId != null)
        query = query.Where(i => i.BankOrderId == bankOrderId);
      return query.Select(selector);
    }
    #endregion
    #region ToBankOrderDetailResult
    public Expression<Func<BankOrderDetail, BankOrderDetailResult>> ToBankOrderDetailResult =
        bankOrderDetail => new BankOrderDetailResult
        {
          Id = bankOrderDetail.Id,
          Index = bankOrderDetail.Index,
          Price = bankOrderDetail.Price,
          Weight = bankOrderDetail.Weight,
          StuffHSGroupId = bankOrderDetail.StuffHSGroupId,
          Description = bankOrderDetail.Description,
          StuffHSGroupCode = bankOrderDetail.StuffHSGroup.Code,
          StuffHSGroupTitle = bankOrderDetail.StuffHSGroup.Title,
          Fee = bankOrderDetail.Fee,
          GrossWeight = bankOrderDetail.GrossWeight,
          Qty = bankOrderDetail.Qty,
          UnitId = bankOrderDetail.UnitId,
          UnitName = bankOrderDetail.Unit.Name,
          RowVersion = bankOrderDetail.RowVersion
        };
    #endregion
    #region Search
    public IQueryable<BankOrderDetailResult> SearchBankOrderDetailResult(IQueryable<BankOrderDetailResult> query,
        string search
        )
    {
      if (!string.IsNullOrEmpty(search))
        query = query.Where(item =>
            item.Description.Contains(search) ||
            item.StuffHSGroupTitle.Contains(search));
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<BankOrderDetailResult> SortBankOrderDetailResult(IQueryable<BankOrderDetailResult> query,
        SortInput<BankOrderDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case BankOrderDetailSortType.Index:
          return query.OrderBy(a => a.Index, sort.SortOrder);
        case BankOrderDetailSortType.Price:
          return query.OrderBy(a => a.Price, sort.SortOrder);
        case BankOrderDetailSortType.StuffHSGroupTitle:
          return query.OrderBy(a => a.StuffHSGroupTitle, sort.SortOrder);
        case BankOrderDetailSortType.Weight:
          return query.OrderBy(a => a.Weight, sort.SortOrder);
        case BankOrderDetailSortType.Description:
          return query.OrderBy(a => a.Description, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
  }
}