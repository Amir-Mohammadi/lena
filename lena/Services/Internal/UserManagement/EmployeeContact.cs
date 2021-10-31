using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
using lena.Services.Core;

using lena.Services.Internals.ApplictaionBase.Exception;
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
    public Contact AddEmployeeContact(string title, string contactText, int contactTypeId,
        bool isMain, int employeeId)
    {

      var employeeContact = repository.Create<Contact>();
      employeeContact.EmployeeId = employeeId;
      var result = App.Internals.ApplicationBase.AddContact(
                employeeContact,
                title: title,
                contactText: contactText,
                contactTypeId: contactTypeId,
                cooperatorId: null,
                employeeId: employeeId,
                isMain: isMain);
      return result;
    }

    public IQueryable<Contact> GetEmployeeContacts(TValue<int> id = null,
        TValue<string> title = null, TValue<string> contactText = null, TValue<int> contactTypeId = null,
        TValue<bool> isMain = null, TValue<int?> employeeId = null)
    {

      var contactQuery = App.Internals.ApplicationBase
                .GetContacts(
                id: id,
                title: title,
                contactText: contactText,
                contactTypeId: contactTypeId,
                employeeId: employeeId);
      var employeeContacts = contactQuery.OfType<Contact>();
      if (employeeId != null)
        employeeContacts = employeeContacts.Where(i => i.EmployeeId == employeeId);
      return employeeContacts;
    }

    public Contact GetEmployeeContact(int id)
    {

      var contact = this.GetEmployeeContacts(id: id).FirstOrDefault();
      if (contact == null)
        throw new ContactNotFoundException(id);
      return contact;
    }

    public Contact EditEmployeeContact(byte[] rowVersion, int id, TValue<string> title = null,
        TValue<string> contactText = null, TValue<int> contactTypeId = null, TValue<bool> isMain = null,
        TValue<int?> employeeId = null)
    {

      var employeeContact = this.GetEmployeeContact(id: id);
      if (employeeId != null)
        employeeContact.EmployeeId = employeeId;
      var result = App.Internals.ApplicationBase.EditContact(
                contact: employeeContact,
                rowVersion: rowVersion,
                id: id,
                title: title,
                contactText: contactText,
                contactTypeId: contactTypeId,
                isMain: isMain,
                employeeId: employeeId
                );
      return result as Contact;
    }

    public void DeleteEmployeeContact(int id)
    {

      var employeeContact = GetEmployeeContact(id);
      repository.Delete(employeeContact);
    }
    public IQueryable<EmployeeContactResult> ToEmployeeContactResultQuery(IQueryable<Contact> employeeContacts)
    {
      var results =
          from employeeContact in employeeContacts
          let contactType = employeeContact.ContactType
          select new EmployeeContactResult
          {
            Id = employeeContact.Id,
            Title = employeeContact.Title,
            IsMain = employeeContact.IsMain,
            ContactText = employeeContact.ContactText,
            ContactTypeId = contactType.Id,
            ContactTypeName = contactType.Name,
            EmployeeId = employeeContact.EmployeeId,
            RowVersion = employeeContact.RowVersion
          };
      return results;
    }

    public EmployeeContactResult ToEmployeeContactResult(Contact employeeContact)
    {
      return new EmployeeContactResult
      {
        Id = employeeContact.Id,
        ContactText = employeeContact.ContactText,
        IsMain = employeeContact.IsMain,
        ContactTypeId = employeeContact.ContactTypeId,
        ContactTypeName = employeeContact.ContactType.Name,
        EmployeeId = employeeContact.EmployeeId,
        Title = employeeContact.Title,
        RowVersion = employeeContact.RowVersion
      };
    }

    public IOrderedQueryable<EmployeeContactResult> SortEmployeeContactResult(IQueryable<EmployeeContactResult> input, SortInput<EmployeeContactSortType> options)
    {
      switch (options.SortType)
      {
        case EmployeeContactSortType.Title:
          return input.OrderBy(r => r.Title, options.SortOrder);
        case EmployeeContactSortType.ContactText:
          return input.OrderBy(r => r.ContactText, options.SortOrder);
        case EmployeeContactSortType.ContactTypeName:
          return input.OrderBy(r => r.ContactTypeName, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException(nameof(options.SortOrder), options.SortType, null);
      }
    }
  }
}