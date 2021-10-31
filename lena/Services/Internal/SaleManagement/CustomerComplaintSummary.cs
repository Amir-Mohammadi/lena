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
using lena.Models.SaleManagement.CustomerComplaintDepartment;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Internals.SaleManagement.Exception;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    #region Add
    public CustomerComplaintSummary AddCustomerComplaintSummary(
        int customerComplaintId,
        TValue<ComplaintClassificationTypes> complaintClassificationTypes = null,
        TValue<Guid> documentId = null,
        string complaintClassificationTypeDescription = null,
        TValue<OccurrenceSeverityStatus> occurrenceSeverityStatus = null,
        TValue<OccurrenceProbabilityStatus> occurrenceProbabilityStatus = null,
        RiskLevelStatus riskLevelStatus = RiskLevelStatus.Low,
        string complaintTitle = null
    )
    {

      var customerComplaintSummary = repository.Create<CustomerComplaintSummary>();
      customerComplaintSummary.ComplaintClassificationTypes = complaintClassificationTypes;
      customerComplaintSummary.CustomerComplaintId = customerComplaintId;
      customerComplaintSummary.DocumentId = documentId;
      customerComplaintSummary.ComplaintClassificationTypeDescription = complaintClassificationTypeDescription;
      customerComplaintSummary.OccurrenceSeverityStatus = occurrenceSeverityStatus;
      customerComplaintSummary.OccurrenceProbabilityStatus = occurrenceProbabilityStatus;
      customerComplaintSummary.RiskLevelStatus = riskLevelStatus;
      customerComplaintSummary.ComplaintTitle = complaintTitle;
      customerComplaintSummary.Status = ComplaintStatus.Open;
      repository.Add(customerComplaintSummary);
      return customerComplaintSummary;
    }
    #endregion
    #region AddProcess
    public void AddCustomerComplaintSummaryProcess(
        int customerComplaintId,
        ComplaintClassificationTypes complaintClassificationTypes,
        string complaintClassificationTypeDescription,
        UploadFileData uploadFileData,
        OccurrenceSeverityStatus occurrenceSeverityStatus,
        OccurrenceProbabilityStatus occurrenceProbabilityStatus,
        RiskLevelStatus riskLevelStatus,
        string complaintTitle,
        short[] selectedDepartmentIds
        )
    {


      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
              name: uploadFileData.FileName,
              fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }

      var customerComplaintSummary = AddCustomerComplaintSummary(
                    customerComplaintId: customerComplaintId,
                    documentId: documentId,
                    complaintClassificationTypes: complaintClassificationTypes,
                    complaintClassificationTypeDescription: complaintClassificationTypeDescription,
                    occurrenceSeverityStatus: occurrenceSeverityStatus,
                    occurrenceProbabilityStatus: occurrenceProbabilityStatus,
                    riskLevelStatus: riskLevelStatus,
                    complaintTitle: complaintTitle
                  );

      var customerComplaintDepartment = AddCustomerComplaintDepartments(
                    customerComplaintSummaryId: customerComplaintSummary.Id,
                    departmentIds: selectedDepartmentIds
                  );
    }
    #endregion

    #region Edit
    public CustomerComplaintSummary EditCustomerComplaintSummary(
        int id,
        string complaintClassificationTypeDescription = null,
        TValue<ComplaintClassificationTypes> complaintClassificationTypes = null,
        TValue<ComplaintStatus> status = null,
        string qaOpinion = null,
        string customerOpinion = null,
        DateTime? dateOfAnnouncement = null,
        TValue<Guid> documentId = null,
        TValue<Guid> correctiveActionDocumentId = null,
        TValue<OccurrenceSeverityStatus> occurrenceSeverityStatus = null,
        TValue<OccurrenceProbabilityStatus> occurrenceProbabilityStatus = null,
        TValue<RiskLevelStatus> riskLevelStatus = null,
        string complaintTitle = null,
        string correctiveAction = null,
        TValue<int> correctiveActionUserId = null,
        DateTime? correctiveActionDateTime = null
    )
    {

      var customerComplaintSummary = GetCustomerComplaintSummary(id: id);
      if (complaintClassificationTypeDescription != null)
        customerComplaintSummary.ComplaintClassificationTypeDescription = complaintClassificationTypeDescription;
      if (complaintClassificationTypes != null)
        customerComplaintSummary.ComplaintClassificationTypes = complaintClassificationTypes;
      if (status != null)
        customerComplaintSummary.Status = status;
      if (qaOpinion != null)
        customerComplaintSummary.QAOpinion = qaOpinion;
      if (customerOpinion != null)
        customerComplaintSummary.CustomerOpinion = customerOpinion;
      if (dateOfAnnouncement != null)
        customerComplaintSummary.DateOfAnnouncement = dateOfAnnouncement;
      if (documentId != null)
        customerComplaintSummary.DocumentId = documentId;
      if (correctiveActionDocumentId != null)
        customerComplaintSummary.CorrectiveActionDocumentId = correctiveActionDocumentId;
      if (occurrenceSeverityStatus != null)
        customerComplaintSummary.OccurrenceSeverityStatus = occurrenceSeverityStatus;
      if (occurrenceProbabilityStatus != null)
        customerComplaintSummary.OccurrenceProbabilityStatus = occurrenceProbabilityStatus;
      if (riskLevelStatus != null)
        customerComplaintSummary.RiskLevelStatus = riskLevelStatus;
      if (complaintTitle != null)
        customerComplaintSummary.ComplaintTitle = complaintTitle;
      if (correctiveAction != null)
        customerComplaintSummary.CorrectiveAction = correctiveAction;
      if (correctiveActionUserId != null)
        customerComplaintSummary.CorrectiveActionUserId = correctiveActionUserId;
      if (correctiveActionDateTime != null)
        customerComplaintSummary.CorrectiveActionDateTime = correctiveActionDateTime;
      repository.Update(rowVersion: customerComplaintSummary.RowVersion, entity: customerComplaintSummary);
      return customerComplaintSummary;
    }

    public CustomerComplaint EditCustomerComplaintSummary(
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
    public void EditCustomerComplaintSummaryProcess(
        int id,
        UploadFileData uploadFileData,
        OccurrenceSeverityStatus occurrenceSeverityStatus,
        OccurrenceProbabilityStatus occurrenceProbabilityStatus,
        short[] selectedDepartmentIds,
        RiskLevelStatus riskLevelStatus,
        string complaintClassificationTypeDescription = null,
        TValue<ComplaintClassificationTypes> complaintClassificationTypes = null,
        TValue<ComplaintStatus> status = null,
        string qaOpinion = null,
        DateTime? dateOfAnnouncement = null,
        string customerOpinion = null,
        string correctiveAction = null,
        TValue<int> correctiveActionUserId = null,
        DateTime? correctiveActionDateTime = null,
        string complaintTitle = null
        )
    {


      var customersComplaintSummary = GetCustomerComplaintSummary(id: id);
      Guid? documentId;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
                   name: uploadFileData.FileName,
                   fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }
      else
      {
        documentId = customersComplaintSummary.DocumentId;
      }

      var customerComplaintSummary = EditCustomerComplaintSummary(
                    id: id,
                    complaintClassificationTypeDescription: complaintClassificationTypeDescription,
                    complaintClassificationTypes: complaintClassificationTypes,
                    documentId: documentId,
                    qaOpinion: qaOpinion,
                    dateOfAnnouncement: dateOfAnnouncement,
                    customerOpinion: customerOpinion,
                    complaintTitle: complaintTitle,
                    status: status,
                    occurrenceSeverityStatus: occurrenceSeverityStatus,
                    occurrenceProbabilityStatus: occurrenceProbabilityStatus
                   );

      EditCustomerComplaintDepartmentProcess(
                    customerComplaintSummaryId: customersComplaintSummary.Id,
                    departmentIds: selectedDepartmentIds);
    }

    public void ReviewCustomerComplaintSummaryByDepartmentProcess(
        int id,
        string inhibitionAction,
        DateTime? dateOfInhibition
        )
    {

      var department = GetCustomerComplaintDepartment(id: id);
      var departmentId = department.Department.Id;
      var UserDepartmentId = App.Providers.Security.CurrentLoginData.DepartmentId;
      if (departmentId != UserDepartmentId)
      {
        throw new UserCantAccessToThisDepartmentException(id);
      }
      var customerComplaint = EditCustomerComplaintDepartment(
                    id: id,
                    inhibitionAction: inhibitionAction,
                    dateOfInhibition: dateOfInhibition
                   );
    }

    public void ReviewCustomerComplaintSummaryByQAProcess(
        int id,
        ComplaintStatus status,
        string qaOpinion,
        string correctiveAction,
        UploadFileData uploadFileData
        )
    {

      int? correctiveActionUserId = null;
      DateTime? correctiveActionDateTime = null;
      Guid? correctiveActionDocumentId = null;
      if (uploadFileData != null)
      {
        var document = App.Internals.ApplicationBase.AddDocument(
              name: uploadFileData.FileName,
              fileStream: uploadFileData.FileData);
        correctiveActionDocumentId = document.Id;
      }
      if (correctiveAction != null)
      {
        correctiveActionUserId = App.Providers.Security.CurrentLoginData.UserId;
        correctiveActionDateTime = DateTime.UtcNow;
      }
      var customerComplaint = EditCustomerComplaintSummary(
                    id: id,
                    status: status,
                    qaOpinion: qaOpinion,
                    correctiveActionDocumentId: correctiveActionDocumentId,
                    correctiveAction: correctiveAction,
                    correctiveActionUserId: correctiveActionUserId,
                    correctiveActionDateTime: correctiveActionDateTime
                    );
    }

    public void ReviewCustomerComplaintSummaryBySaleProcess(
        int id,
        DateTime? dateOfAnnouncement,
        string customerOpinion
        )
    {

      var customerComplaint = EditCustomerComplaintSummary(
                    id: id,
                    dateOfAnnouncement: dateOfAnnouncement,
                    customerOpinion: customerOpinion
                    );
    }
    #endregion

    #region Get
    public CustomerComplaintSummary GetCustomerComplaintSummary(int id) => GetCustomerComplaintSummary(selector: e => e, id: id);
    public TResult GetCustomerComplaintSummary<TResult>(
        Expression<Func<CustomerComplaintSummary, TResult>> selector,
        int id)
    {

      var customerComplaint = GetCustomerComplaintSummaries(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      return customerComplaint;
    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCustomerComplaintSummaries<TResult>(
        Expression<Func<CustomerComplaintSummary, TResult>> selector,
        TValue<int> id = null,
        TValue<int> customerId = null
        )
    {

      var baseQuery = repository.GetQuery<CustomerComplaintSummary>();
      if (id != null)
        baseQuery = baseQuery.Where(i => i.Id == id);

      return baseQuery.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<CustomerComplaintSummary, CustomerComplaintSummaryResult>> ToCustomerComplaintSummaryResult =
        (customerComplaintSummary) => new CustomerComplaintSummaryResult
        {
          Id = customerComplaintSummary.Id,
          ComplaintClassificationTypeDescription = customerComplaintSummary.ComplaintClassificationTypeDescription,
          ComplaintClassificationTypes = customerComplaintSummary.ComplaintClassificationTypes,
          CustomerComplaintDepartments = customerComplaintSummary.CustomerComplaintDepartments.AsQueryable().Select(App.Internals.SaleManagement.ToCustomerComplaintDepartmentResult),
          ComplaintTitle = customerComplaintSummary.ComplaintTitle,
          SelectedDepartmentIds = customerComplaintSummary.CustomerComplaintDepartments.Select(x => x.DepartmentId).AsQueryable(),
          Status = customerComplaintSummary.Status,
          QAOpinion = customerComplaintSummary.QAOpinion,
          DateOfAnnouncement = customerComplaintSummary.DateOfAnnouncement,
          CustomerOpinion = customerComplaintSummary.CustomerOpinion,
          DocumentId = customerComplaintSummary.DocumentId,
          CorrectiveActionDocumentId = customerComplaintSummary.CorrectiveActionDocumentId,
          RiskLevelStatus = customerComplaintSummary.RiskLevelStatus.Value,
          OccurrenceSeverityStatus = customerComplaintSummary.OccurrenceSeverityStatus,
          OccurrenceProbabilityStatus = customerComplaintSummary.OccurrenceProbabilityStatus,
          CorrectiveAction = customerComplaintSummary.CorrectiveAction,
          ComplaintCorrectiveActionRegistrarName = customerComplaintSummary.CorrectiveActionUser.Employee.FirstName + " " + customerComplaintSummary.CorrectiveActionUser.Employee.LastName
        };
    #endregion
    #region Search
    public IQueryable<CustomerComplaintSummaryResult> SearchCustomerComplaintSummary(IQueryable<CustomerComplaintSummaryResult>
    query,
    string searchText,
    AdvanceSearchItem[] advanceSearchItems
    )
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.InhibitionAction.Contains(searchText) ||
            item.CustomerOpinion.Contains(searchText) ||
            item.QAOpinion.Contains(searchText)
            );
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;
    }
    #endregion
    #region Delete

    public void DeleteCustomerComplaintSummary(int id)
    {

      var customerComplaintSummary = GetCustomerComplaintSummary(id: id);

      DeleteCustomerComplaintSummary(customerComplaintSummary);
    }

    public void DeleteCustomerComplaintSummary(CustomerComplaintSummary customerComplaintSummary)
    {

      repository.Delete(customerComplaintSummary);
    }
    #endregion

    #region DeleteProcess

    public void DeleteCustomerComplaintSummaryProccess(int id)
    {

      var customerComplaintSummary = GetCustomerComplaintSummary(id: id);

      var customerComplaintDepartments = GetCustomerComplaintDepartments(
                e => e,
                customerComplaintSummaryId: id
                );
      foreach (var item in customerComplaintDepartments)
      {
        DeleteCustomerComplaintDepartment(item);
      }
      DeleteCustomerComplaintSummary(
                id: id
                );
    }
    #endregion
  }
}