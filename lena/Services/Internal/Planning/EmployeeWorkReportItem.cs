//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Planning.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Models.Planning.EmployeeWorkReportItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning
{
  public partial class Planning
  {
    #region Add
    public EmployeeWorkReportItem AddEmployeeWorkReportItem(
        int employeeWorkReportId,
        string operation,
        TimeSpan fromTime,
        TimeSpan toTime,
        string description)
    {

      var employeeWorkReportItem = repository.Create<EmployeeWorkReportItem>();
      employeeWorkReportItem.EmployeeWorkReportId = employeeWorkReportId;
      employeeWorkReportItem.Operation = operation;
      employeeWorkReportItem.FromTime = fromTime;
      employeeWorkReportItem.ToTime = toTime;
      employeeWorkReportItem.Operation = operation;
      employeeWorkReportItem.Description = description;
      employeeWorkReportItem.DateTime = DateTime.UtcNow;
      repository.Add(employeeWorkReportItem);
      return employeeWorkReportItem;
    }
    #endregion


    #region Edit
    public EmployeeWorkReportItem EditEmployeeWorkReportItem(
        int id,
        byte[] rowVersion,
        TValue<int> employeeWorkReportId = null,
        TValue<TimeSpan> fromTime = null,
        TValue<TimeSpan> toTime = null,
        TValue<string> operation = null,
        TValue<string> description = null)
    {

      var employeeWorkReportItem = GetEmployeeWorkReportItem(id: id);

      EditEmployeeWorkReportItem(
                employeeWorkReportItem: employeeWorkReportItem,
                rowVersion: rowVersion, employeeWorkReportId: employeeWorkReportId,
                fromTime: fromTime,
                toTime: toTime,
                operation: operation,
                description: description);
      return employeeWorkReportItem;
    }



    public EmployeeWorkReportItem EditEmployeeWorkReportItem(
        EmployeeWorkReportItem employeeWorkReportItem,
        byte[] rowVersion,
        TValue<int> employeeWorkReportId = null,
        TValue<TimeSpan> fromTime = null,
        TValue<TimeSpan> toTime = null,
        TValue<string> operation = null,
        TValue<string> description = null)
    {



      if (employeeWorkReportId != null)
        employeeWorkReportItem.EmployeeWorkReportId = employeeWorkReportId;

      if (fromTime != null)
        employeeWorkReportItem.FromTime = fromTime;

      if (toTime != null)
        employeeWorkReportItem.ToTime = toTime;

      if (operation != null)
        employeeWorkReportItem.Operation = operation;

      if (description != null)
        employeeWorkReportItem.Description = description;


      repository.Update(employeeWorkReportItem, rowVersion);

      return employeeWorkReportItem;
    }
    #endregion

    #region Delete
    public void DeleteEmployeeWorkReportItem(int id)
    {

      var employeeWorkReportItem = GetEmployeeWorkReportItem(id: id);

      DeleteEmployeeWorkReportItem(employeeWorkReportItem: employeeWorkReportItem);
    }

    public void DeleteEmployeeWorkReportItem(EmployeeWorkReportItem employeeWorkReportItem)
    {

      repository.Delete(employeeWorkReportItem);
    }
    #endregion

    #region Get
    public EmployeeWorkReportItem GetEmployeeWorkReportItem(int id) => GetEmployeeWorkReportItem(selector: e => e, id: id);
    public TResult GetEmployeeWorkReportItem<TResult>(
        Expression<Func<EmployeeWorkReportItem, TResult>> selector,
        int id)
    {

      var employeeWorkReportItem = GetEmployeeWorkReportItems(
                selector: selector,
                id: id).FirstOrDefault();
      if (employeeWorkReportItem == null)
        throw new EmployeeWorkReportItemNotFoundException(id);
      return employeeWorkReportItem;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetEmployeeWorkReportItems<TResult>(
        Expression<Func<EmployeeWorkReportItem, TResult>> selector,
        TValue<int> id = null,
        TValue<int> employeeWorkReportId = null
        )
    {


      var employeeWorkReportItems = repository.GetQuery<EmployeeWorkReportItem>();



      if (id != null)
        employeeWorkReportItems = employeeWorkReportItems.Where(x => x.Id == id);

      if (employeeWorkReportId != null)
        employeeWorkReportItems = employeeWorkReportItems.Where(i => i.EmployeeWorkReportId == employeeWorkReportId);



      return employeeWorkReportItems.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<EmployeeWorkReportItem, EmployeeWorkReportItemResult>> ToEmployeeWorkReportItemResult =
        employeeWorkReportItem => new EmployeeWorkReportItemResult
        {
          Id = employeeWorkReportItem.Id,
          EmployeeWorkReportId = employeeWorkReportItem.EmployeeWorkReportId,
          FromTime = employeeWorkReportItem.FromTime,
          ToTime = employeeWorkReportItem.ToTime,
          Operation = employeeWorkReportItem.Operation,
          Description = employeeWorkReportItem.Description,
          RowVersion = employeeWorkReportItem.RowVersion,
        };
    #endregion
  }
}
