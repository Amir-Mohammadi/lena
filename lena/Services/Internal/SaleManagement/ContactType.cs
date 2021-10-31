using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public ContactType AddContactType(string name, EssentialContactType essentialContactType)
    {
      var contactType = repository.Create<ContactType>();
      contactType.Name = name;
      contactType.EssentialContactType = essentialContactType;
      repository.Add(contactType);
      return contactType;
    }
    public IQueryable<ContactType> GetContactTypes(TValue<int> id = null, TValue<string> name = null)
    {
      var isIdNull = (id == null);
      var isNameNull = name == null;
      var contactTypes = from contactType in repository.GetQuery<ContactType>()
                         where (isIdNull || id == contactType.Id) &&
                                     (isNameNull || name == contactType.Name)
                         select contactType;
      return contactTypes;
    }
    public ContactType GetContactType(int id)
    {
      var contactType = this.GetContactTypes(id: id).FirstOrDefault();
      if (null == contactType)
      {
        throw new ContactTypeNotFoundException(id);
      }
      return contactType;
    }
    public void DeleteContactType(int id)
    {
      var type = this.GetContactType(id);
      repository.Delete(type);
    }
    public ContactType EditContactType(byte[] rowVersion, int id, TValue<string> name)
    {
      var contactType = this.GetContactType(id: id);
      contactType.Name = name;
      repository.Update(entity: contactType, rowVersion: rowVersion);
      return contactType;
    }
    public ContactTypeResult ToContactTypeResult(ContactType contactType)
    {
      return new ContactTypeResult
      {
        Id = contactType.Id,
        Name = contactType.Name,
        EssentialContactType = contactType.EssentialContactType,
        RowVersion = contactType.RowVersion
      };
    }
    public IOrderedQueryable<ContactTypeResult> SortContactType(IQueryable<ContactTypeResult> input, SortInput<ContactTypeSortType> options)
    {
      switch (options.SortType)
      {
        case ContactTypeSortType.Id:
          return input.OrderBy(r => r.Id, options.SortOrder);
        case ContactTypeSortType.Name:
          return input.OrderBy(r => r.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<ContactTypeResult> SearchContactTypeResult(
        IQueryable<ContactTypeResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Id.ToString().Contains(searchText)
                select item;
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    public IQueryable<ContactTypeResult> ToContactTypeResultQuery(IQueryable<ContactType> contactTypes)
    {
      return from a in contactTypes
             select new ContactTypeResult
             {
               Id = a.Id,
               Name = a.Name,
               EssentialContactType = a.EssentialContactType,
               RowVersion = a.RowVersion
             };
    }
  }
}