using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StuffRequestMilestoneDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Get
    public StuffRequestMilestoneDetail GetStuffRequestMilestoneDetail(int id) => GetStuffRequestMilestoneDetail(selector: e => e, id: id);
    public TResult GetStuffRequestMilestoneDetail<TResult>(
        Expression<Func<StuffRequestMilestoneDetail, TResult>> selector,
        int id)
    {

      var stuffRequestMilestoneDetail = GetStuffRequestMilestoneDetails(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (stuffRequestMilestoneDetail == null)
        throw new StuffRequestMilestoneDetailNotFoundException(id);
      return stuffRequestMilestoneDetail;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetStuffRequestMilestoneDetails<TResult>(
            Expression<Func<StuffRequestMilestoneDetail, TResult>> selector,
            TValue<int> id = null,
            TValue<string> code = null,
            TValue<bool> isDelete = null,
            TValue<int> userId = null,
            TValue<int> transactionBatchId = null,
            TValue<string> description = null,
            TValue<int> stuffRequestMilestoneId = null,
            TValue<double> qty = null,
            TValue<int> stuffId = null,
            TValue<int> unitId = null
            )
    {

      var baseQuery = App.Internals.ApplicationBase.GetBaseEntities(
                        selector: e => e,
                        id: id,
                        code: code,
                        isDelete: isDelete,
                        userId: userId,
                        transactionBatchId: transactionBatchId,
                        description: description);
      var query = baseQuery.OfType<StuffRequestMilestoneDetail>();
      if (stuffRequestMilestoneId != null)
        query = query.Where(x => x.StuffRequestMilestoneId == stuffRequestMilestoneId);
      if (stuffId != null)
        query = query.Where(x => x.StuffId == stuffId);

      return query.Select(selector);
    }
    #endregion
    #region Add

    public StuffRequestMilestoneDetail AddStuffRequestMilestoneDetail(
        TransactionBatch transactionBatch,
        string description,
        double qty,
        int stuffRequestMilestoneId,
        int stuffId,
        byte unitId)
    {


      var stuffRequestMilestoneDetail = repository.Create<StuffRequestMilestoneDetail>();
      stuffRequestMilestoneDetail.Qty = qty;
      stuffRequestMilestoneDetail.StuffRequestMilestoneId = stuffRequestMilestoneId;
      stuffRequestMilestoneDetail.Status = StuffRequestMilestoneDetailStatus.Doing;
      stuffRequestMilestoneDetail.UnitId = unitId;
      stuffRequestMilestoneDetail.StuffId = stuffId;
      stuffRequestMilestoneDetail.Description = description;

      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: stuffRequestMilestoneDetail,
                    description: description,
                    transactionBatch: transactionBatch);
      return stuffRequestMilestoneDetail;
    }

    #endregion
    #region Add Process

    public StuffRequestMilestoneDetail AddStuffRequestMilestoneDetailProcess(
        TransactionBatch transactionBatch,
        string description,
        double qty,
        int stuffRequestMilestoneId,
        int stuffId,
        byte unitId)
    {


      var stuffRequestMilestoneDetail = AddStuffRequestMilestoneDetail(
                    transactionBatch: null,
                    description: description,
                    qty: qty,
                    stuffId: stuffId,
                    stuffRequestMilestoneId: stuffRequestMilestoneId,
                    unitId: unitId
                );
      var stuffRequestMilestoneDetailSummary = AddStuffRequestMilestoneDetailSummary(
                stuffRequestMilestoneDetailId: stuffRequestMilestoneDetail.Id,
                    orderdQty: 0,
                    cargoedQty: 0,
                    qualityControlPassedQty: 0,
                    reciptedQty: 0
                );
      return stuffRequestMilestoneDetail;
    }

    #endregion
    #region Edit
    public StuffRequestMilestoneDetail EditStuffRequestMilestoneDetail(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<double> qty = null,
        TValue<int> stuffId = null,
        TValue<byte> unitId = null,
        TValue<StuffRequestMilestoneDetailStatus> status = null
        )
    {

      var stuffRequestMilestoneDetail = GetStuffRequestMilestoneDetail(id: id);
      return EditStuffRequestMilestoneDetail(
                    stuffRequestMilestoneDetail: stuffRequestMilestoneDetail,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    qty: qty,
                    status: status,
                    unitId: unitId,
                    stuffId: stuffId);
    }

    public StuffRequestMilestoneDetail EditStuffRequestMilestoneDetail(
        StuffRequestMilestoneDetail stuffRequestMilestoneDetail,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> stuffRequestMilestoneId = null,
        TValue<double> qty = null,
        TValue<int> stuffId = null,
        TValue<byte> unitId = null,
        TValue<StuffRequestMilestoneDetailStatus> status = null)
    {


      if (stuffRequestMilestoneId != null)
        stuffRequestMilestoneDetail.StuffRequestMilestoneId = stuffRequestMilestoneId;
      if (qty != null) stuffRequestMilestoneDetail.Qty = qty;
      if (stuffId != null) stuffRequestMilestoneDetail.StuffId = stuffId;
      if (unitId != null) stuffRequestMilestoneDetail.UnitId = unitId;
      if (status != null) stuffRequestMilestoneDetail.Status = status;

      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stuffRequestMilestoneDetail,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return stuffRequestMilestoneDetail;
    }

    #endregion

    #region Edit Process
    public StuffRequestMilestoneDetail EditStuffRequestMilestoneDetailProcess(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<int> stuffRequestMilestoneId = null,
        TValue<double> qty = null,
        TValue<int> stuffId = null,
        TValue<byte> unitId = null,
        TValue<StuffRequestMilestoneDetailStatus> status = null)
    {

      var item = EditStuffRequestMilestoneDetail(
                    id: id,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description,
                    qty: qty,
                    stuffId: stuffId,
                    unitId: unitId,
                    status: status);
      ResetStuffRequestMilestoneDetailSummary(item.Id);
      return item;
    }
    #endregion
    #region Delete
    public void DeleteStuffRequestMilestoneDetail(
        int id,
        byte[] rowVersion)
    {

      var stuffRequestMilestoneDetail = GetStuffRequestMilestoneDetail(id: id);
      DeleteStuffRequestMilestoneDetail(
                stuffRequestMilestoneDetail: stuffRequestMilestoneDetail,
                rowVersion: stuffRequestMilestoneDetail.RowVersion);

    }

    public void DeleteStuffRequestMilestoneDetail(
      StuffRequestMilestoneDetail stuffRequestMilestoneDetail,
      byte[] rowVersion)
    {


      DeleteStuffRequestMilestoneDetailSummary(
                id: stuffRequestMilestoneDetail.StuffRequestMilestoneDetailSummary.Id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                    transactionBatchId: null,
                    baseEntity: stuffRequestMilestoneDetail,
                    rowVersion: rowVersion);
    }


    #endregion
    #region Sort
    public IOrderedQueryable<StuffRequestMilestoneDetailResult> SortStuffRequestMilestoneDetailResult(
        IQueryable<StuffRequestMilestoneDetailResult> query,
        SortInput<StuffRequestMilestoneDetailSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffRequestMilestoneDetailSortType.Code: return query.OrderBy(a => a.Code, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.DateTime: return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.DueDate: return query.OrderBy(a => a.DueDate, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.EmployeeFullName: return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.Status: return query.OrderBy(a => a.Status, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.StuffName: return query.OrderBy(a => a.StuffName, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.UnitName: return query.OrderBy(a => a.UnitName, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.Qty: return query.OrderBy(a => a.Qty, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.OrderedQty: return query.OrderBy(a => a.OrderedQty, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.CargoedQty: return query.OrderBy(a => a.CargoedQty, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.ReciptedQty: return query.OrderBy(a => a.ReciptedQty, sort.SortOrder);
        case StuffRequestMilestoneDetailSortType.QualityControlPassedQty: return query.OrderBy(a => a.QualityControlPassedQty, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffRequestMilestoneDetailResult> SearchStuffRequestMilestoneDetailResult(
        IQueryable<StuffRequestMilestoneDetailResult> query,
        StuffRequestMilestoneDetailStatus[] statuses,
        DateTime? fromDueDate,
        DateTime? toDueDate,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.EmployeeFullName.Contains(searchText) ||
                item.StuffName.Contains(searchText) ||
                item.StuffCode.Contains(searchText) ||
                item.UnitName.Contains(searchText) ||
                item.Code.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;

      if (statuses != null)
        query = query.Where(i => statuses.Contains(i.Status));
      if (fromDueDate != null)
        query = query.Where(i => i.DueDate >= fromDueDate);
      if (toDueDate != null)
        query = query.Where(i => i.DueDate <= toDueDate);

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffRequestMilestoneDetail, StuffRequestMilestoneDetailResult>> ToStuffRequestMilestoneDetailResult =
                stuffRequestMilestoneDetail => new StuffRequestMilestoneDetailResult
                {

                  Id = stuffRequestMilestoneDetail.Id,
                  Code = stuffRequestMilestoneDetail.Code,
                  DateTime = stuffRequestMilestoneDetail.DateTime,
                  DueDate = stuffRequestMilestoneDetail.StuffRequestMilestone.DueDate,
                  Description = stuffRequestMilestoneDetail.Description,
                  UserId = stuffRequestMilestoneDetail.UserId,
                  EmployeeFullName = stuffRequestMilestoneDetail.User.Employee.FirstName + " " + stuffRequestMilestoneDetail.User.Employee.LastName,
                  Status = stuffRequestMilestoneDetail.Status,
                  UnitId = stuffRequestMilestoneDetail.UnitId,
                  UnitName = stuffRequestMilestoneDetail.Unit.Name,
                  StuffId = stuffRequestMilestoneDetail.StuffId,
                  StuffName = stuffRequestMilestoneDetail.Stuff.Name,
                  StuffCode = stuffRequestMilestoneDetail.Stuff.Code,
                  Qty = stuffRequestMilestoneDetail.Qty,
                  OrderedQty = stuffRequestMilestoneDetail.StuffRequestMilestoneDetailSummary.OrderedQty,
                  CargoedQty = stuffRequestMilestoneDetail.StuffRequestMilestoneDetailSummary.CargoedQty,
                  ReciptedQty = stuffRequestMilestoneDetail.StuffRequestMilestoneDetailSummary.ReciptedQty,
                  QualityControlPassedQty = stuffRequestMilestoneDetail.StuffRequestMilestoneDetailSummary.QualityControlPassedQty,
                  RowVersion = stuffRequestMilestoneDetail.RowVersion
                };
    #endregion

  }
}

