using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.UserManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.UserManagement.QAReviewEmployeeComplain;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Get
    public QAReviewEmployeeComplain GetQAReviewEmployeeComplain(int id) => GetQAReviewEmployeeComplain(selector: e => e, id: id);
    public TResult GetQAReviewEmployeeComplain<TResult>(Expression<Func<QAReviewEmployeeComplain, TResult>> selector, int id)
    {

      var qaReviewEmployeeComplain = GetQAReviewEmployeeComplaints(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (qaReviewEmployeeComplain == null)
        throw new QAReviewHasNotFoundException();
      return qaReviewEmployeeComplain;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetQAReviewEmployeeComplaints<TResult>(
       Expression<Func<QAReviewEmployeeComplain, TResult>> selector,
        TValue<int> id = null,
        TValue<int> creatorUserId = null,
        TValue<int> employeeComplainItemId = null,
        TValue<string> actionDescription = null,
        TValue<int> actionResponsibleUserId = null,
        TValue<DateTime> actionStartDate = null,
        TValue<DateTime> actionFinishDate = null,
        TValue<string> actionResult = null,
        TValue<QAReviewEmployeeComplainStatus> status = null)

    {

      var query = repository.GetQuery<QAReviewEmployeeComplain>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (creatorUserId != null)
        query = query.Where(i => i.CreatorUserId == creatorUserId);
      if (employeeComplainItemId != null)
        query = query.Where(i => i.EmployeeComplainItemId == employeeComplainItemId);
      if (actionDescription != null)
        query = query.Where(i => i.ActionDescription == actionDescription);
      if (actionStartDate != null)
        query = query.Where(i => i.ActionStartDate >= actionStartDate);
      if (actionFinishDate != null)
        query = query.Where(i => i.ActionFinishDate <= actionFinishDate);
      if (actionResult != null)
        query = query.Where(i => i.ActionResult == actionResult);
      if (status != null)
        query = query.Where(i => i.Status == status);
      if (actionResponsibleUserId != null)
        query = query.Where(i => i.ActionResponsibleUserId == actionResponsibleUserId);
      return query.Select(selector);
    }
    #endregion


    #region Add
    public QAReviewEmployeeComplain AddQAReviewEmployeeComplain(
        int employeeComplainItemId,
        string actionDescription,
        int actionResponsibleUserId,
        DateTime actionStartDate,
        DateTime actionFinishDate,
        string actionResult,
        QAReviewEmployeeComplainStatus status)
    {

      var QAReviewEmployeeComplain = repository.Create<QAReviewEmployeeComplain>();

      QAReviewEmployeeComplain.EmployeeComplainItemId = employeeComplainItemId;
      QAReviewEmployeeComplain.CreatorUserId = App.Providers.Security.CurrentLoginData.UserId;
      QAReviewEmployeeComplain.DateTime = DateTime.Now.ToUniversalTime();
      QAReviewEmployeeComplain.ActionDescription = actionDescription;
      QAReviewEmployeeComplain.ActionResponsibleUserId = actionResponsibleUserId;
      QAReviewEmployeeComplain.ActionStartDate = actionStartDate;
      QAReviewEmployeeComplain.ActionFinishDate = actionFinishDate;
      QAReviewEmployeeComplain.ActionResult = actionResult;
      QAReviewEmployeeComplain.Status = status;
      repository.Add(QAReviewEmployeeComplain);

      return QAReviewEmployeeComplain;
    }
    #endregion



    #region AddProcess
    public QAReviewEmployeeComplain AddQAReviewEmployeeComplainProcess(
        int employeeComplainItemId,
        string actionDescription,
        int actionResponsibleUserId,
        DateTime actionStartDate,
        DateTime actionFinishDate,
        string actionResult,
        QAReviewEmployeeComplainStatus status)
    {

      var userManagement = App.Internals.UserManagement;





      var getQAReviewEmployeeComplains = GetQAReviewEmployeeComplaints(
                selector: e => e,
                employeeComplainItemId: employeeComplainItemId, creatorUserId: App.Providers.Security.CurrentLoginData.UserId);




      if (getQAReviewEmployeeComplains.Any())
      {
        throw new QAReviewEmployeeComplainHasAddedException();
      }

      var qAReviewEmployeeComplain = AddQAReviewEmployeeComplain(
                employeeComplainItemId: employeeComplainItemId,
                actionDescription: actionDescription,
                actionResponsibleUserId: actionResponsibleUserId,
                actionStartDate: actionStartDate,
                actionFinishDate: actionFinishDate,
                actionResult: actionResult,
                status: status);

      return qAReviewEmployeeComplain;


    }
    #endregion

    #region Edit
    public QAReviewEmployeeComplain EditQAReviewEmployeeComplain(
        int id,
        byte[] rowVersion,
        TValue<int> employeeComplainItemId = null,
        TValue<string> actionDescription = null,
        TValue<int> actionResponsibleUserId = null,
        TValue<DateTime> actionStartDate = null,
        TValue<DateTime> actionFinishDate = null,
        TValue<string> actionResult = null,
        TValue<QAReviewEmployeeComplainStatus> status = null
        )
    {

      var QAReviewEmployeeComplain = GetQAReviewEmployeeComplain(id: id);
      if (employeeComplainItemId != null)
        QAReviewEmployeeComplain.EmployeeComplainItemId = employeeComplainItemId;
      if (actionDescription != null)
        QAReviewEmployeeComplain.ActionDescription = actionDescription;
      if (actionResponsibleUserId != null)
        QAReviewEmployeeComplain.ActionResponsibleUserId = actionResponsibleUserId;
      if (actionStartDate != null)
        QAReviewEmployeeComplain.ActionStartDate = actionStartDate;
      if (actionFinishDate != null)
        QAReviewEmployeeComplain.ActionFinishDate = actionFinishDate;
      if (actionResult != null)
        QAReviewEmployeeComplain.ActionResult = actionResult;
      if (status != null)
        QAReviewEmployeeComplain.Status = status;
      repository.Update(rowVersion: QAReviewEmployeeComplain.RowVersion, entity: QAReviewEmployeeComplain);
      return QAReviewEmployeeComplain;
    }
    #endregion

    #region ToResult
    public Expression<Func<QAReviewEmployeeComplain, QAReviewEmployeeComplainResult>> ToQAReviewEmployeeComplain =
      QAReviewEmployeeComplain => new QAReviewEmployeeComplainResult()
      {
        Id = QAReviewEmployeeComplain.Id,
        DateTime = QAReviewEmployeeComplain.DateTime,
        CreatorUserId = QAReviewEmployeeComplain.CreatorUser.Id,
        CreatorFullName = QAReviewEmployeeComplain.CreatorUser.Employee.FirstName + " " + QAReviewEmployeeComplain.CreatorUser.Employee.LastName,
        ActionDescription = QAReviewEmployeeComplain.ActionDescription,
        EmployeeComplainItemId = QAReviewEmployeeComplain.EmployeeComplainItem.Id,
        ActionResponsibleUserId = QAReviewEmployeeComplain.ResponsibleUser.Id,
        ActionStartDate = QAReviewEmployeeComplain.ActionStartDate,
        ActionResponseFullName = QAReviewEmployeeComplain.ResponsibleUser.Employee.FirstName + " " + QAReviewEmployeeComplain.ResponsibleUser.Employee.LastName,
        ActionFinishDate = QAReviewEmployeeComplain.ActionFinishDate,
        ActionResult = QAReviewEmployeeComplain.ActionResult,
        Status = QAReviewEmployeeComplain.Status,
        RowVersion = QAReviewEmployeeComplain.RowVersion


      };

    #endregion

    #region sort
    public IOrderedQueryable<QAReviewEmployeeComplainResult> SortQAReviewEmployeeComplainResult(IQueryable<QAReviewEmployeeComplainResult> query,
      SortInput<QAReviewEmployeeComplainSortType> sort)
    {
      switch (sort.SortType)
      {
        case QAReviewEmployeeComplainSortType.Id:
          return query.OrderBy(i => i.Id, sort.SortOrder);

        default:
          return null;
      }
    }
    #endregion

    #region search
    public IQueryable<QAReviewEmployeeComplainResult> SearchQAReviewEmployeeComplain(IQueryable<QAReviewEmployeeComplainResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.ActionDescription.Contains(searchText) ||
            item.ActionResult.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion

  }
}


