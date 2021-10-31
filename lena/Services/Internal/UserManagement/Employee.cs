using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;
using lena.Services.Internals.Exceptions;
using lena.Services.Internals.UserManagement.Exception;
//using Parlar.DAL;
//using Parlar.DAL.UnitOfWorks;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.UserManagement
{
  public partial class UserManagement
  {
    #region Get
    public Employee GetEmployee(int id) => GetEmployee(selector: e => e, id: id);
    public TResult GetEmployee<TResult>(
            Expression<Func<Employee, TResult>> selector,
            int id)
    {
      var employee = GetEmployees(
                selector: selector,
                id: id)
                .FirstOrDefault();
      if (employee == null)
        throw new EmployeeNotFoundException(id);
      return employee;
    }
    public Employee GetEmployee(string code) => GetEmployee(selector: e => e, code: code);
    public TResult GetEmployee<TResult>(
            Expression<Func<Employee, TResult>> selector,
            string code)
    {
      var employee = GetEmployees(
                selector: selector,
                code: code)
                .FirstOrDefault();
      if (employee == null)
        throw new EmployeeNotFoundException(code: code);
      return employee;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetManagerEmployeesCombo<TResult>(
    Expression<Func<Employee, TResult>> selector
    )
    {
      var userId = App.Providers.Security.CurrentLoginData.UserId;
      var userOrganizationPostId = GetUser(userId).Employee.OrganizationPost.Id;
      var departmentManager = GetDepartmentManagers(selector: e => e, organizationPostId: userOrganizationPostId).FirstOrDefault();
      if (departmentManager == null)
      {
        throw new UserIsntDepartmentManagerException();
      }
      var departmentManagerId = departmentManager.Department.Id;
      var query = repository.GetQuery<Employee>();
      if (departmentManagerId != null)
        query = query.Where(i => i.DepartmentId == departmentManagerId);
      return query.Select(selector);
    }
    public IQueryable<TResult> GetEmployees<TResult>(
        Expression<Func<Employee, TResult>> selector,
        TValue<int> id = null,
        TValue<int> excludedId = null,
        TValue<int[]> ids = null,
        TValue<string> firstName = null,
        TValue<string> lastName = null,
        TValue<DateTime> fromDate = null,
        TValue<DateTime> toDate = null,
        TValue<byte[]> image = null,
        TValue<byte[]> file = null,
        TValue<string> code = null,
        TValue<bool> hasUser = null,
        TValue<short> departmentId = null,
        TValue<short[]> departmentIds = null,
        TValue<bool> isActive = null,
        TValue<byte[]> documentId = null,
        TValue<int> organizationPostId = null,
        TValue<int> organizationJobId = null
        )
    {
      var query = repository.GetQuery<Employee>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (excludedId != null)
        query = query.Where(i => i.Id != excludedId);
      if (ids != null)
        query = query.Where(i => ids.Value.Contains(i.Id));
      if (firstName != null)
        query = query.Where(i => i.FirstName == firstName);
      if (lastName != null)
        query = query.Where(i => i.LastName == lastName);
      if (fromDate != null)
        query = query.Where(i => i.EmployeementDate >= fromDate.Value);
      if (toDate != null)
        query = query.Where(i => i.EmployeementDate <= toDate.Value);
      if (code != null)
        query = query.Where(i => i.Code == code);
      if (departmentId != null)
        query = query.Where(i => i.DepartmentId == departmentId);
      if (departmentIds != null)
        query = query.Where(i => departmentIds.Value.Contains(i.DepartmentId.Value));
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      if (hasUser != null)
        query = query.Where(i => (hasUser == false && i.User == null) || (hasUser && i.User != null));
      if (organizationPostId != null)
        query = query.Where(i => i.OrgnizationPostId == organizationPostId);
      if (organizationJobId != null)
        query = query.Where(i => i.OrgnizationPostId == organizationJobId);
      return query.Select(selector);
    }
    public IQueryable<TResult> GetEmployees<TResult>(
       Expression<Func<Employee, TResult>> selector,
       int[] departmentIds,
       TValue<bool> isActive = null
       )
    {
      var query = repository.GetQuery<Employee>();
      if (departmentIds != null)
        query = query.Where(i => departmentIds.Contains(i.DepartmentId ?? 0));
      if (isActive != null)
        query = query.Where(i => i.IsActive == isActive);
      return query.Select(selector);
    }
    #endregion
    #region Delete
    public void DeleteEmployee(int id)
    {
      var employee = GetEmployee(id: id);
      repository.Delete(employee);
    }
    #endregion
    #region Add
    public Employee AddEmployee(
        int? code,
        string firstName,
        string lastName,
        DateTime? employeementDate,
        byte[] image,
        Guid? documentId,
        string nationalCode,
        string fatherName,
        DateTime? birthDate,
        string birthPlace,
        short? departmentId,
        bool isActive,
        int? organizationPostId,
        int? organizationJobId)
    {
      var employee = repository.Create<Employee>();
      employee.FirstName = firstName;
      employee.LastName = lastName;
      employee.EmployeementDate = employeementDate;
      employee.Image = image;
      employee.Code = code.ToString();
      employee.FatherName = fatherName;
      employee.NationalCode = nationalCode;
      employee.BirthDate = birthDate;
      employee.BirthPlace = birthPlace;
      employee.DepartmentId = departmentId;
      employee.IsActive = isActive;
      employee.OrgnizationPostId = organizationPostId;
      employee.OrganizationJobId = organizationJobId;
      employee.DocumentId = documentId;
      repository.Add(employee);
      return employee;
    }
    #endregion
    #region AddProcess
    public Employee AddEmployeeProcess(
        int? code,
        string firstName,
        string lastName,
        DateTime? employeementDate,
        byte[] image,
        UploadFileData uploadFileData,
        string nationalCode,
        string fatherName,
        DateTime? birthDate,
        string birthPlace,
        short? departmentId,
        bool isActive,
        int? organizationPostId,
        int? organizationJobId)
    {
      if (code == null)
      {
        code = Convert.ToInt32(GetEmployees(e => e).Max(a => a.Code) ?? "0") + 1;
      }
      else
      {
        var employees = GetEmployees(e => e, code: code.ToString());
        if (employees.Any())
        {
          throw new EmployeeCodeExsits(code.ToString());
        }
      }
      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = Core.App.Internals.ApplicationBase.AddDocument(
                     name: uploadFileData.FileName,
                     fileStream: uploadFileData.FileData);
        documentId = document.Id;
      }
      var employee = AddEmployee(
                                       firstName: firstName,
                                       lastName: lastName,
                                       employeementDate: employeementDate,
                                       image: image,
                                       code: code,
                                       fatherName: fatherName,
                                       nationalCode: nationalCode,
                                       birthDate: birthDate,
                                       birthPlace: birthPlace,
                                       departmentId: departmentId,
                                       isActive: isActive,
                                       organizationPostId: organizationPostId,
                                       organizationJobId: organizationJobId,
                                       documentId: documentId
                                       );
      return employee;
    }
    #endregion
    #region Edit
    public Employee EditEmployee(
        int id,
        byte[] rowVersion,
        TValue<string> firstName = null,
        TValue<string> lastName = null,
        TValue<Guid?> documentId = null,
        TValue<DateTime?> employeementDate = null,
        TValue<byte[]> image = null,
        TValue<string> code = null,
        TValue<string> fatherName = null,
        TValue<DateTime?> birthDate = null,
        TValue<string> birthPlace = null,
        TValue<string> nationalCode = null,
        TValue<short?> departmentId = null,
        TValue<bool> isActive = null,
        TValue<int?> organizationPostId = null,
        TValue<int?> organizationJobId = null
        )
    {
      var employee = GetEmployee(id: id);
      if (firstName != null)
        employee.FirstName = firstName;
      if (lastName != null)
        employee.LastName = lastName;
      if (employeementDate != null)
        employee.EmployeementDate = employeementDate;
      if (image != null)
        employee.Image = image;
      if (code != null)
      {
        var codeExists = GetEmployees(e => e, code: code);
        if (codeExists.Any(r => r.Id != employee.Id))
          throw new EmployeeCodeExsits(code);
        employee.Code = code;
      }
      if (nationalCode != null)
        employee.NationalCode = nationalCode;
      if (fatherName != null)
        employee.FatherName = fatherName;
      if (birthDate != null)
        employee.BirthDate = birthDate;
      if (birthPlace != null)
        employee.BirthPlace = birthPlace;
      if (departmentId != null)
        employee.DepartmentId = departmentId;
      if (isActive != null)
        employee.IsActive = isActive;
      if (organizationPostId != null)
        employee.OrgnizationPostId = organizationPostId;
      if (organizationJobId != null)
        employee.OrganizationJobId = organizationJobId;
      if (documentId != null)
        employee.DocumentId = documentId;
      repository.Update(employee, rowVersion: rowVersion);
      return employee;
    }
    #endregion
    #region EditProcess
    public Employee EditEmployeeProcess(
        int id,
        byte[] rowVersion,
        TValue<UploadFileData> uploadFileData,
        TValue<string> firstName = null,
        TValue<string> lastName = null,
        TValue<DateTime?> employeementDate = null,
        TValue<byte[]> image = null,
        TValue<string> code = null,
        TValue<string> fatherName = null,
        TValue<DateTime?> birthDate = null,
        TValue<string> birthPlace = null,
        TValue<string> nationalCode = null,
        TValue<short?> departmentId = null,
        TValue<bool> isActive = null,
        TValue<int?> organizationPostId = null,
        TValue<int?> organizationJobId = null)
    {
      var employee = GetEmployee(id: id);
      Guid? documentId = null;
      if (uploadFileData != null)
      {
        var document = Core.App.Internals.ApplicationBase.AddDocument(
                     name: uploadFileData.Value.FileName,
                     fileStream: uploadFileData.Value.FileData);
        documentId = document.Id;
      }
      employee = EditEmployee(
                    id: id,
                    firstName: firstName,
                    rowVersion: rowVersion,
                    lastName: lastName,
                    employeementDate: employeementDate,
                    image: image,
                    code: code,
                    fatherName: fatherName,
                    nationalCode: nationalCode,
                    birthDate: birthDate,
                    birthPlace: birthPlace,
                    departmentId: departmentId,
                    isActive: isActive,
                    organizationPostId: organizationPostId,
                    organizationJobId: organizationJobId,
                    documentId: documentId
            );
      return employee;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<EmployeeResult> SortEmployeeResult(IQueryable<EmployeeResult> input,
        SortInput<EmployeeSortType> options)
    {
      switch (options.SortType)
      {
        case EmployeeSortType.FirstName:
          return input.OrderBy(i => i.FirstName, options.SortOrder);
        case EmployeeSortType.LastName:
          return input.OrderBy(i => i.LastName, options.SortOrder);
        case EmployeeSortType.EmployeeCode:
          return input.OrderBy(i => i.EmployeeCode, options.SortOrder);
        case EmployeeSortType.EmployeeDate:
          return input.OrderBy(i => i.EmployeementDate, options.SortOrder);
        case EmployeeSortType.DepartmentName:
          return input.OrderBy(i => i.DepartmentName, options.SortOrder);
        case EmployeeSortType.IsActive:
          return input.OrderBy(i => i.IsActive, options.SortOrder);
        case EmployeeSortType.OrganizationPostName:
          return input.OrderBy(i => i.OrganizationPostName, options.SortOrder);
        case EmployeeSortType.OrganizationJobName:
          return input.OrderBy(i => i.OrganizationJobName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region Search
    public IQueryable<EmployeeResult> SearchEmployeeResult(
        IQueryable<EmployeeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.EmployeeCode.Contains(searchText) ||
                      item.FirstName.Contains(searchText) ||
                      item.LastName.Contains(searchText) ||
                      item.DepartmentName.Contains(searchText) ||
                      item.OrganizationPostName.Contains(searchText) ||
                      item.OrganizationJobName.Contains(searchText)
                select item;
      if (advanceSearchItems.Any())
        query = query.Where(advanceSearchItems);
      return query;
    }
    #endregion    
    #region ToResult
    public EmployeeResult ToEmployeeResult(Employee employee)
    {
      byte[] file = null;
      if (employee.DocumentId.HasValue)
      {
        var documnet = Core.App.Internals.ApplicationBase.GetDocument(employee.DocumentId.Value);
        file = documnet.FileStream;
      }
      var result = new EmployeeResult()
      {
        Id = employee.Id,
        FirstName = employee.FirstName,
        LastName = employee.LastName,
        EmployeeCode = employee.Code,
        EmployeementDate = employee.EmployeementDate,
        Image = employee.Image,
        File = file,
        BirthDate = employee.BirthDate,
        BirthPlace = employee.BirthPlace,
        FatherName = employee.FatherName,
        NationalCode = employee.NationalCode,
        DepartmentId = employee.DepartmentId,
        DepartmentName = employee.Department.Name,
        IsActive = employee.IsActive,
        OrganizationPostId = employee.OrgnizationPostId,
        OrganizationPostName = employee.OrganizationPost?.Title,
        OrganizationJobId = employee.OrganizationJobId,
        OrganizationJobName = employee.OrganizationJob?.Title,
        RowVersion = employee.RowVersion
      };
      return result;
    }
    public IQueryable<EmployeeResult> ToEmployeeResultQuery(IQueryable<Employee> query)
    {
      var resultQuery = from item in query
                        let employee = item
                        select new EmployeeResult()
                        {
                          Id = employee.Id,
                          FirstName = employee.FirstName,
                          LastName = employee.LastName,
                          EmployeeCode = employee.Code,
                          EmployeementDate = employee.EmployeementDate,
                          Image = employee.Image,
                          BirthDate = employee.BirthDate,
                          BirthPlace = employee.BirthPlace,
                          FatherName = employee.FatherName,
                          NationalCode = employee.NationalCode,
                          DepartmentId = employee.DepartmentId,
                          DepartmentName = employee.Department.Name,
                          IsActive = employee.IsActive,
                          OrganizationPostId = employee.OrgnizationPostId,
                          OrganizationPostName = employee.OrganizationPost.Title,
                          OrganizationJobId = employee.OrganizationJobId,
                          OrganizationJobName = employee.OrganizationJob.Title,
                          RowVersion = employee.RowVersion
                        };
      return resultQuery;
    }
    public Expression<Func<Employee, EmployeeComboResult>> ToEmployeeComboResult =
        employee => new EmployeeComboResult()
        {
          Id = employee.Id,
          UserId = employee.User.Id,
          FirstName = employee.FirstName,
          LastName = employee.LastName,
          EmployeeCode = employee.Code
        };
    #endregion
  }
}