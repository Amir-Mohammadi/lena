
using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.WarehouseManagement.StuffRequestMilestone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using lena.Models.WarehouseManagement.StuffRequestMilestoneDetail;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Get
    public StuffRequestMilestone GetStuffRequestMilestone(int id) => GetStuffRequestMilestone(selector: e => e, id: id);
    public TResult GetStuffRequestMilestone<TResult>(
        Expression<Func<StuffRequestMilestone, TResult>> selector,
        int id)
    {

      var stuffRequestMilestone = GetStuffRequestMilestones(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (stuffRequestMilestone == null)
        throw new StuffRequestMilestoneNotFoundException(id);
      return stuffRequestMilestone;
    }
    #endregion

    #region ToFullResult
    public Expression<Func<StuffRequestMilestone, FullStuffRequestMilestoneResult>> ToFullStuffRequestMilestoneResult =
                stuffRequestMilestone => new FullStuffRequestMilestoneResult
                {
                  Id = stuffRequestMilestone.Id,
                  Code = stuffRequestMilestone.Code,
                  DateTime = stuffRequestMilestone.DateTime,
                  DueDate = stuffRequestMilestone.DueDate,
                  Description = stuffRequestMilestone.Description,
                  EmployeeFirstName = stuffRequestMilestone.User.Employee.FirstName,
                  EmployeeFullName = stuffRequestMilestone.User.Employee.FirstName + " " + stuffRequestMilestone.User.Employee.LastName,
                  EmployeeLastName = stuffRequestMilestone.User.Employee.LastName,
                  Status = (
                    stuffRequestMilestone.IsClosed ? StuffRequestMilestoneStatus.Closed
                    : stuffRequestMilestone.DueDate > DateTime.UtcNow ? StuffRequestMilestoneStatus.Upcoming
                    : StuffRequestMilestoneStatus.Expierd),
                  UserId = stuffRequestMilestone.UserId,
                  StuffRequestMilestoneDetails = stuffRequestMilestone.StuffRequestMilestoneDetails.AsQueryable().Select(App.Internals.WarehouseManagement.ToStuffRequestMilestoneDetailResult),
                  RowVersion = stuffRequestMilestone.RowVersion
                };
    #endregion


    #region Gets
    public IQueryable<TResult> GetStuffRequestMilestones<TResult>(
            Expression<Func<StuffRequestMilestone, TResult>> selector,
            TValue<int> id = null,
            TValue<string> code = null,
            TValue<bool> isDelete = null,
            TValue<int> userId = null,
            TValue<int> transactionBatchId = null,
            TValue<string> description = null,
            TValue<DateTime> dueDate = null,
            TValue<bool> isClosed = null
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
      var query = baseQuery.OfType<StuffRequestMilestone>();
      if (dueDate != null)
        query = query.Where(x => x.DueDate == dueDate);
      if (isClosed != null)
        query = query.Where(x => x.IsClosed == isClosed);


      return query.Select(selector);
    }
    #endregion
    #region Add

    public StuffRequestMilestone AddStuffRequestMilestone(
        TransactionBatch transactionBatch,
        string description,
        DateTime dueDate
    )
    {

      var stuffRequestMilestone = repository.Create<StuffRequestMilestone>();
      stuffRequestMilestone.DueDate = dueDate;
      stuffRequestMilestone.IsClosed = false;
      App.Internals.ApplicationBase.AddBaseEntity(
                    baseEntity: stuffRequestMilestone,
                    description: description,
                    transactionBatch: transactionBatch);
      return stuffRequestMilestone;
    }

    #endregion


    #region AddProcess
    public StuffRequestMilestone AddStuffRequestMilestoneProcess(

        string description,
        DateTime dueDate,
        AddStuffRequestMilestoneDetailInput[] stuffRequestMilestoneDetails
    )
    {

      var stuffRequestMilestone = AddStuffRequestMilestone(
                    transactionBatch: null,
                    description: description,
                    dueDate: dueDate);

      if (stuffRequestMilestone != null)
      {
        foreach (var item in stuffRequestMilestoneDetails)
        {
          AddStuffRequestMilestoneDetailProcess(
                        transactionBatch: null,
                        description: item.Description,
                        qty: item.Qty,
                        stuffRequestMilestoneId: stuffRequestMilestone.Id,
                        stuffId: item.StuffId,
                        unitId: item.UnitId);
        }
      }

      return stuffRequestMilestone;
    }

    #endregion

    #region Edit
    public StuffRequestMilestone EditStuffRequestMilestone(
        int id,
        byte[] rowVersion,
        TValue<bool> isDelete = null,
        TValue<string> description = null,
        TValue<DateTime> dueDate = null,
        TValue<bool> isClosed = null
        )
    {

      var stuffRequestMilestone = GetStuffRequestMilestone(id: id);
      return EditStuffRequestMilestone(
                stuffRequestMilestone: stuffRequestMilestone,
                rowVersion: rowVersion,
                dueDate: dueDate,
                 isClosed: isClosed,
                 isDelete: isDelete,
                 description: description

                );

    }

    public StuffRequestMilestone EditStuffRequestMilestone(
                StuffRequestMilestone stuffRequestMilestone,
                byte[] rowVersion,
                TValue<bool> isDelete = null,
                TValue<string> description = null,
                TValue<DateTime> dueDate = null,
                TValue<bool> isClosed = null


                )
    {


      if (dueDate != null) stuffRequestMilestone.DueDate = dueDate;
      if (isClosed != null) stuffRequestMilestone.IsClosed = isClosed;

      App.Internals.ApplicationBase.EditBaseEntity(
                    baseEntity: stuffRequestMilestone,
                    rowVersion: rowVersion,
                    isDelete: isDelete,
                    description: description);
      return stuffRequestMilestone;
    }

    #endregion
    #region Edit
    public StuffRequestMilestone EditStuffRequestMilestoneProcess(
        int id,
        byte[] rowVersion,
        string description,
        DateTime dueDate,
        AddStuffRequestMilestoneDetailInput[] addStuffRequestMilestoneDetails,
        EditStuffRequestMilestoneDetailInput[] editStuffRequestMilestoneDetails,
        DeleteStuffRequestMilestoneDetailInput[] deleteStuffRequestMilestoneDetails)
    {

      var stuffRequestMilestone = EditStuffRequestMilestone(
                    id: id,
                    rowVersion: rowVersion,
                    dueDate: dueDate,
                    description: description);
      if (addStuffRequestMilestoneDetails != null)
      {
        foreach (var item in addStuffRequestMilestoneDetails)
        {
          AddStuffRequestMilestoneDetailProcess(
                        transactionBatch: null,
                        description: item.Description,
                        qty: item.Qty,
                        stuffRequestMilestoneId: stuffRequestMilestone.Id,
                        stuffId: item.StuffId,
                        unitId: item.UnitId);
        }
      }
      if (editStuffRequestMilestoneDetails != null)
      {
        foreach (var item in editStuffRequestMilestoneDetails)
        {
          EditStuffRequestMilestoneDetailProcess(
                        id: item.Id,
                        rowVersion: item.RowVersion,
                        description: item.Description,
                        qty: item.Qty,
                        stuffId: item.StuffId,
                        unitId: item.UnitId);
        }
      }
      if (deleteStuffRequestMilestoneDetails != null)
      {
        foreach (var item in deleteStuffRequestMilestoneDetails)
        {
          DeleteStuffRequestMilestoneDetail(
                        id: item.Id,
                        rowVersion: item.RowVersion);
        }
      }
      return stuffRequestMilestone;

    }



    #endregion
    #region Delete
    public void DeleteStuffRequestMilestone(
        int id,
        byte[] rowVersion)
    {

      var stuffRequestMilestone = GetStuffRequestMilestone(id: id);
      App.Internals.ApplicationBase.RemoveBaseEntityProcess(
                transactionBatchId: null,
                baseEntity: stuffRequestMilestone,
                rowVersion: rowVersion);

    }
    #endregion

    #region DeleteProcess
    public void DeleteStuffRequestMilestoneProcess(
        int id,
        byte[] rowVersion)
    {


      var stuffRequestMilestoneDetails = GetStuffRequestMilestoneDetails(
                selector: e => e,
                stuffRequestMilestoneId: id);
      foreach (var stuffRequestMilestoneDetail in stuffRequestMilestoneDetails)
      {
        DeleteStuffRequestMilestoneDetail(stuffRequestMilestoneDetail: stuffRequestMilestoneDetail,
                  rowVersion: stuffRequestMilestoneDetail.RowVersion);
      }
      DeleteStuffRequestMilestone(id: id, rowVersion: rowVersion);
    }
    #endregion

    #region Open
    public StuffRequestMilestone OpenStuffRequestMilestone(
        int id,
        byte[] rowVersion)
    {

      return EditStuffRequestMilestone(
                id: id,
                rowVersion: rowVersion,
                isClosed: false);
    }
    #endregion

    #region Close
    public StuffRequestMilestone CloseStuffRequestMilestone(
        int id,
        byte[] rowVersion)
    {

      return EditStuffRequestMilestone(
                    id: id,
                    rowVersion: rowVersion,
                    isClosed: true);
    }
    #endregion
    #region Sort
    public IOrderedQueryable<StuffRequestMilestoneResult> SortStuffRequestMilestoneResult(
        IQueryable<StuffRequestMilestoneResult> query,
        SortInput<StuffRequestMilestoneSortType> sort)
    {
      switch (sort.SortType)
      {
        case StuffRequestMilestoneSortType.Code: return query.OrderBy(a => a.Code, sort.SortOrder);
        case StuffRequestMilestoneSortType.DateTime: return query.OrderBy(a => a.DateTime, sort.SortOrder);
        case StuffRequestMilestoneSortType.DueDate: return query.OrderBy(a => a.DueDate, sort.SortOrder);
        case StuffRequestMilestoneSortType.EmployeeFullName: return query.OrderBy(a => a.EmployeeFullName, sort.SortOrder);
        case StuffRequestMilestoneSortType.Status: return query.OrderBy(a => a.Status, sort.SortOrder);


        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<StuffRequestMilestoneResult> SearchStuffRequestMilestoneResult(
        IQueryable<StuffRequestMilestoneResult> query,
        StuffRequestMilestoneStatus[] statuses,
        DateTime? fromDueDate,
        DateTime? toDueDate,
        DateTime? fromDate,
        DateTime? toDate,
        string searchText)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where
                item.EmployeeFullName.Contains(searchText) ||
                item.Code.Contains(searchText) ||
                item.Description.Contains(searchText)
                select item;

      if (statuses != null)
        query = query.Where(i => statuses.Contains(i.Status));
      if (fromDate != null)
        query = query.Where(i => i.DateTime >= fromDate);
      if (toDate != null)
        query = query.Where(i => i.DateTime <= toDate);
      if (fromDueDate != null)
        query = query.Where(i => i.DueDate >= fromDueDate);
      if (toDueDate != null)
        query = query.Where(i => i.DueDate <= toDueDate);

      return query;
    }
    #endregion
    #region ToResult
    public Expression<Func<StuffRequestMilestone, StuffRequestMilestoneResult>> ToStuffRequestMilestoneResult =
                stuffRequestMilestone => new StuffRequestMilestoneResult
                {

                  Id = stuffRequestMilestone.Id,
                  Code = stuffRequestMilestone.Code,
                  DateTime = stuffRequestMilestone.DateTime,
                  DueDate = stuffRequestMilestone.DueDate,
                  Description = stuffRequestMilestone.Description,
                  IsClosed = stuffRequestMilestone.IsClosed,
                  IsDelete = stuffRequestMilestone.IsDelete,
                  UserId = stuffRequestMilestone.UserId,
                  EmployeeFullName = stuffRequestMilestone.User.Employee.FirstName + " " + stuffRequestMilestone.User.Employee.LastName,
                  Status = (
                    stuffRequestMilestone.IsClosed ? StuffRequestMilestoneStatus.Closed
                    : stuffRequestMilestone.DueDate > DateTime.UtcNow ? StuffRequestMilestoneStatus.Upcoming
                    : StuffRequestMilestoneStatus.Expierd
                    ),

                  RowVersion = stuffRequestMilestone.RowVersion
                };
    #endregion

  }
}

