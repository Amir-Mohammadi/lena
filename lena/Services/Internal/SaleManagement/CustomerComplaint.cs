using lena.Services.Common;
using lena.Services.Core;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Domains.Enums.SortTypes;
using lena.Models.Common;
using lena.Models.SaleManagement.CustomerComplaint;
using lena.Models.SaleManagement.CustomerComplaintSummary;
using System;
using System.Linq;
using System.Linq.Expressions;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public CustomerComplaint AddCustomerComplaint(
        TValue<int> customerId,
        DateTime? dateOfComplaint,
        DateTime? responseDeadline,
        string complaintTypeDescription,
        TValue<ComplaintTypes> complaintTypes,
        CustomerComplaintType customerComplaintType
    )
    {

      var customerComplaint = repository.Create<CustomerComplaint>();
      customerComplaint.CustomerId = customerId;
      customerComplaint.DateOfComplaint = dateOfComplaint;
      customerComplaint.ResponseDeadline = responseDeadline;
      customerComplaint.ComplaintTypeDescription = complaintTypeDescription;
      customerComplaint.ComplaintTypes = complaintTypes;
      customerComplaint.CustomerComplaintType = customerComplaintType;
      customerComplaint.RegisterarUserId = App.Providers.Security.CurrentLoginData.UserId;
      customerComplaint.RegisterarDateTime = DateTime.UtcNow;
      repository.Add(customerComplaint);
      return customerComplaint;
    }
    #endregion
    #region AddProcess
    public void AddCustomerComplaintProcess(
        int customerId,
        DateTime? dateOfComplaint,
        DateTime? responseDeadline,
        string complaintTypeDescription,
        CustomerComplaintType customerComplaintType,
        TValue<ComplaintTypes[]> complaintTypes = null,
        AddCustomerComplaintSummaryInput[] customerComplaintSummaries = null
        )
    {

      var complaintTypeEnum = ComplaintTypes.None;
      var saleManagements = App.Internals.SaleManagement;
      if (complaintTypes != null)
      {
        foreach (var item in complaintTypes.Value)
          complaintTypeEnum = complaintTypeEnum | item;
      }
      var customerComplaint = AddCustomerComplaint(
                customerId: customerId,
                dateOfComplaint: dateOfComplaint,
                responseDeadline: responseDeadline,
                complaintTypeDescription: complaintTypeDescription,
                customerComplaintType: customerComplaintType,
                complaintTypes: complaintTypeEnum
               );
      foreach (var item in customerComplaintSummaries)
      {
        saleManagements.AddCustomerComplaintSummaryProcess(
              customerComplaintId: customerComplaint.Id,
              selectedDepartmentIds: item.SelectedDepartmentIds,
              complaintClassificationTypes: item.ComplaintClassificationTypes,
              complaintClassificationTypeDescription: item.ComplaintClassificationTypeDescription,
              occurrenceSeverityStatus: item.OccurrenceSeverityStatus,
              occurrenceProbabilityStatus: item.OccurrenceProbabilityStatus,
              riskLevelStatus: item.RiskLevelStatus,
              complaintTitle: item.ComplaintTitle,
              uploadFileData: string.IsNullOrWhiteSpace(item.FileKey)
              ? null
              : Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey));


        var organizationPosts = App.Internals.UserManagement.GetOrganizationPosts(
                  selector: e => e,
                  isAdmin: true);

        var employees = App.Internals.UserManagement.GetEmployees(
                  selector: e => e,
                  departmentIds: item.SelectedDepartmentIds);

        var resultQuery = from organizationPost in organizationPosts
                          join employee in employees on
                                organizationPost.Id equals employee.OrganizationPost.Id
                          where organizationPost.IsAdmin == true
                          select new
                          {
                            userId = employee.User.Id
                          };

        foreach (var result in resultQuery)
        {
          App.Internals.Notification.NotifyToUser(
                    userId: result.userId,
                    title: "شکایت مشتریان",
                    description: $"`{customerComplaint.Customer.Name} - {item.ComplaintClassificationTypes}`");
        }
      }
    }
    #endregion

    #region Edit
    public CustomerComplaint EditCustomerComplaint(
        int id,
        TValue<int> customerId = null,
        DateTime? dateOfComplaint = null,
        DateTime? responseDeadline = null,
        string complaintTypeDescription = null,
        string complaintClassificationTypeDescription = null,
        TValue<ComplaintTypes> complaintTypes = null,
        TValue<ComplaintClassificationTypes> complaintClassificationTypes = null,
        TValue<int> registerarUserId = null,
        DateTime? registerarDateTime = null,
        string inhibitionAction = null,
        TValue<ComplaintStatus> status = null,
        string qaOpinion = null,
        TValue<Guid> documentId = null,
        string correctiveAction = null,
        DateTime? dateOfAnnouncement = null,
        string customerOpinion = null,
        TValue<int> correctiveActionUserId = null,
        DateTime? correctiveActionDateTime = null,
        TValue<CustomerComplaintType> customerComplaintType = null
    )
    {

      var customerComplaint = GetCustomerComplaint(id: id);
      if (customerId != null)
        customerComplaint.CustomerId = customerId;
      if (dateOfComplaint != null)
        customerComplaint.DateOfComplaint = dateOfComplaint;
      if (responseDeadline != null)
        customerComplaint.ResponseDeadline = responseDeadline;
      if (complaintTypeDescription != null)
        customerComplaint.ComplaintTypeDescription = complaintTypeDescription;
      if (registerarUserId != null)
        customerComplaint.RegisterarUserId = registerarUserId;
      if (complaintTypes != null)
        customerComplaint.ComplaintTypes = complaintTypes;
      if (registerarUserId != null)
        customerComplaint.RegisterarUserId = registerarUserId;
      if (registerarDateTime != null)
        customerComplaint.RegisterarDateTime = registerarDateTime;
      if (customerComplaintType != null)
        customerComplaint.CustomerComplaintType = customerComplaintType;
      customerComplaint.RegisterarDateTime = DateTime.UtcNow;
      repository.Update(rowVersion: customerComplaint.RowVersion, entity: customerComplaint);
      return customerComplaint;
    }

    public CustomerComplaint EditCustomerComplaint(
        CustomerComplaint customerComplaint,
        byte[] rowVersion,
        TValue<int> customerId = null,
        TValue<Risk> risk = null)
    {

      if (customerId != null)
        customerComplaint.CustomerId = customerId;
      repository.Update(rowVersion: rowVersion, entity: customerComplaint);
      return customerComplaint;
    }
    #endregion

    #region EditProcess
    public void EditCustomerComplaintProcess(
        int id,
        int customerId,
        DateTime? dateOfComplaint,
        DateTime? responseDeadline,
        string complaintTypeDescription,
        AddCustomerComplaintSummaryInput[] addCustomerComplaintSummaryInput = null,
        EditCustomerComplaintSummaryInput[] editCustomerComplaintSummaryInput = null,
        int[] deleteCustomerComplaintSummaryInput = null,

        TValue<ComplaintTypes[]> complaintTypes = null

        )
    {

      var complaintTypeEnum = ComplaintTypes.None;
      var saleManagements = App.Internals.SaleManagement;
      if (complaintTypes != null)
      {
        foreach (var item in complaintTypes.Value)
          complaintTypeEnum = complaintTypeEnum | item;
      }
      var customersComplaint = GetCustomerComplaint(id: id);
      var customerComplaint = EditCustomerComplaint(
                    id: id,
                    customerId: customerId,
                    dateOfComplaint: dateOfComplaint,
                    responseDeadline: responseDeadline,
                    complaintTypeDescription: complaintTypeDescription,
                    complaintTypes: complaintTypeEnum
                   );
      foreach (var item in addCustomerComplaintSummaryInput)
      {
        saleManagements.AddCustomerComplaintSummaryProcess(
              customerComplaintId: id,
              selectedDepartmentIds: item.SelectedDepartmentIds,
              complaintClassificationTypes: item.ComplaintClassificationTypes,
              complaintClassificationTypeDescription: item.ComplaintClassificationTypeDescription,
              occurrenceSeverityStatus: item.OccurrenceSeverityStatus,
              occurrenceProbabilityStatus: item.OccurrenceProbabilityStatus,
              riskLevelStatus: item.RiskLevelStatus,
              complaintTitle: item.ComplaintTitle,
              uploadFileData: string.IsNullOrWhiteSpace(item.FileKey)
              ? null
              : Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey)
            );
      }

      foreach (var item in editCustomerComplaintSummaryInput)
      {
        saleManagements.EditCustomerComplaintSummaryProcess(
              id: item.Id,
              selectedDepartmentIds: item.SelectedDepartmentIds,
              complaintClassificationTypes: item.ComplaintClassificationTypes,
              complaintClassificationTypeDescription: item.ComplaintClassificationTypeDescription,
              occurrenceSeverityStatus: item.OccurrenceSeverityStatus,
              occurrenceProbabilityStatus: item.OccurrenceProbabilityStatus,
              riskLevelStatus: item.RiskLevelStatus,
              complaintTitle: item.ComplaintTitle,
              status: item.Status,
              qaOpinion: item.QAOpinion,
              dateOfAnnouncement: item.DateOfAnnouncement,
              customerOpinion: item.CustomerOpinion,
              correctiveAction: item.CorrectiveAction,
              correctiveActionDateTime: item.CorrectiveActionDateTime,
              correctiveActionUserId: item.CorrectiveActionUserId,
              uploadFileData: string.IsNullOrWhiteSpace(item.FileKey)
              ? null
              : Core.App.Providers.Session.GetAs<UploadFileData>(item.FileKey)
            ); ;
      }

      foreach (var item in deleteCustomerComplaintSummaryInput)
      {
        saleManagements.DeleteCustomerComplaintSummaryProccess(
              id: item
            );
      }
    }
    #endregion

    #region Get
    public CustomerComplaint GetCustomerComplaint(int id) => GetCustomerComplaint(selector: e => e, id: id);
    public TResult GetCustomerComplaint<TResult>(
        Expression<Func<CustomerComplaint, TResult>> selector,
        int id)
    {

      var customerComplaint = GetCustomerComplaints(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      return customerComplaint;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCustomerComplaints<TResult>(
        Expression<Func<CustomerComplaint, TResult>> selector,
        TValue<int> id = null,
        TValue<int> customerId = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null

        )
    {

      var baseQuery = repository.GetQuery<CustomerComplaint>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);
      if (customerId != null)
        baseQuery = baseQuery.Where(i => i.CustomerId == customerId);
      if (fromDate != null)
        baseQuery = baseQuery.Where(i => i.DateOfComplaint >= fromDate);
      if (toDate != null)
        baseQuery = baseQuery.Where(i => i.DateOfComplaint <= toDate);
      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public IQueryable<CustomerComplaintResult> ToCustomerComplaintResult(
       IQueryable<CustomerComplaint> customerComplaints
       )
    {


      var supplies = App.Internals.Supplies;
      var saleManagements = App.Internals.SaleManagement;

      var resultQuery = from customerComplaint in customerComplaints
                        select new CustomerComplaintResult
                        {
                          Id = customerComplaint.Id,
                          CustomerId = customerComplaint.CustomerId,
                          CustomerName = customerComplaint.Customer.Name,
                          CustomerComplaintType = customerComplaint.CustomerComplaintType,
                          ComplaintRegistrarName = customerComplaint.RegisterarUser.Employee.FirstName + " " + customerComplaint.RegisterarUser.Employee.LastName,
                          RegisterDateTime = customerComplaint.RegisterarDateTime,
                          DateOfComplaint = customerComplaint.DateOfComplaint,
                          ResponseDeadline = customerComplaint.ResponseDeadline,
                          ComplaintTypes = customerComplaint.ComplaintTypes,
                          ComplaintTypeDescription = customerComplaint.ComplaintTypeDescription,
                          CustomerComplaintSummaries = customerComplaint.CustomerComplaintSummaries.AsQueryable().Select(App.Internals.SaleManagement.ToCustomerComplaintSummaryResult),
                          DepartmentCount = customerComplaint.CustomerComplaintSummaries.SelectMany(i => i.CustomerComplaintDepartments).Count(),
                        };
      return resultQuery;
    }
    #endregion


    #region ToReportResult
    public IQueryable<CustomerComplaintResult> ToCustomerComplaintReportResult(
       IQueryable<CustomerComplaint> customerComplaints
       )
    {


      var supplies = App.Internals.Supplies;
      var saleManagements = App.Internals.SaleManagement;
      var customerComplaintDepartments = saleManagements.GetCustomerComplaintDepartments(selector: e => e); ; var customerComplaintSummaries = saleManagements.GetCustomerComplaintSummaries(selector: e => e);

      var resultQuery = from customerComplaint in customerComplaints
                        join customerComplaintSummary in customerComplaintSummaries on customerComplaint.Id
                              equals customerComplaintSummary.CustomerComplaintId into allCustomerComplaint
                        from customerComplaintSummaryLeftJoin in allCustomerComplaint.DefaultIfEmpty()
                        join customerComplaintDepartment in customerComplaintDepartments on customerComplaintSummaryLeftJoin.Id
                              equals customerComplaintDepartment.CustomerComplaintSummaryId into allCustomerComplaintSummary
                        from customerComplaintDepartmentLeftJoin in allCustomerComplaintSummary.DefaultIfEmpty()
                        select new CustomerComplaintResult
                        {
                          Id = customerComplaint.Id,
                          ComplaintRegistrarName = customerComplaint.RegisterarUser.Employee.FirstName + " " + customerComplaint.RegisterarUser.Employee.LastName,
                          RegisterDateTime = customerComplaint.RegisterarDateTime,
                          CustomerName = customerComplaint.Customer.Name,
                          DateOfComplaint = customerComplaint.DateOfComplaint,
                          ResponseDeadline = customerComplaint.ResponseDeadline,
                          ComplaintTitle = customerComplaintSummaryLeftJoin.ComplaintTitle,
                          ComplaintClassificationTypes = customerComplaintSummaryLeftJoin.ComplaintClassificationTypes,
                          RiskLevelStatus = customerComplaintSummaryLeftJoin.RiskLevelStatus,
                          DateOfAnnouncement = customerComplaintSummaryLeftJoin.DateOfAnnouncement,
                          CustomerOpinion = customerComplaintSummaryLeftJoin.CustomerOpinion,
                          DepartmentFullName = customerComplaintDepartmentLeftJoin.Department.Name,
                          InhibitionAction = customerComplaintDepartmentLeftJoin.InhibitionAction,
                          DateOfInhibition = customerComplaintDepartmentLeftJoin.DateOfInhibition,
                          CorrectiveAction = customerComplaintSummaryLeftJoin.CorrectiveAction,
                          Status = customerComplaintSummaryLeftJoin.Status,
                          QAOpinion = customerComplaintSummaryLeftJoin.QAOpinion,
                        };
      return resultQuery;
    }
    #endregion

    #region GetFull       
    public CustomerComplaintResult GetFullCustomerComplaint(
       int id)
    {

      var saleManagements = App.Internals.SaleManagement;
      var customerComplaints = saleManagements.GetCustomerComplaints(
                selector: e => e,
                id: id);
      var customerComplaintResults = saleManagements.ToCustomerComplaintResult(customerComplaints: customerComplaints)

                .FirstOrDefault();
      return customerComplaintResults;
    }
    #endregion

    #region Search
    public IQueryable<CustomerComplaintResult> SearchCustomerComplaint(IQueryable<CustomerComplaintResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.CustomerName.Contains(searchText) ||
            item.ComplaintRegistrarName.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<CustomerComplaintResult> SortCustomerComplaintResult(IQueryable<CustomerComplaintResult> query,
        SortInput<CustomerComplaintSortType> sort)
    {
      switch (sort.SortType)
      {
        case CustomerComplaintSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case CustomerComplaintSortType.ComplaintRegistrarName:
          return query.OrderBy(a => a.ComplaintRegistrarName, sort.SortOrder);
        case CustomerComplaintSortType.ComplaintTypes:
          return query.OrderBy(a => a.ComplaintTypes, sort.SortOrder);
        case CustomerComplaintSortType.CustomerName:
          return query.OrderBy(a => a.CustomerName, sort.SortOrder);
        case CustomerComplaintSortType.DateOfComplaint:
          return query.OrderBy(a => a.DateOfComplaint, sort.SortOrder);
        case CustomerComplaintSortType.ResponseDeadline:
          return query.OrderBy(a => a.ResponseDeadline, sort.SortOrder);
        case CustomerComplaintSortType.RegisterDateTime:
          return query.OrderBy(a => a.RegisterDateTime, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion


  }
}
