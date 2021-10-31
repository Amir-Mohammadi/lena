using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.CustomerStuffVersion;
using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    #region Add
    public CustomerStuffVersion AddCustomerStuffVersion(
        string code,
        string name,
        int customerStuffId)
    {
      var customerStuffVersion = repository.Create<CustomerStuffVersion>();
      customerStuffVersion.Code = code.ToUpper();
      customerStuffVersion.Name = name;
      customerStuffVersion.CustomerStuffId = customerStuffId;
      repository.Add(customerStuffVersion);
      return customerStuffVersion;
    }
    #endregion
    #region Edit
    public CustomerStuffVersion EditCustomerStuffVersion(
        byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<bool> isPublish = null,
        TValue<int> customerStuffId = null
        )
    {
      var customerStuffVersion = GetCustomerStuffVersion(id: id);
      if (code != null)
        customerStuffVersion.Code = code.Value.ToUpper();
      if (name != null)
        customerStuffVersion.Name = name;
      if (isPublish != null)
        customerStuffVersion.IsPublish = isPublish;
      if (customerStuffId != null)
        customerStuffVersion.CustomerStuffId = customerStuffId;
      repository.Update(customerStuffVersion, rowVersion);
      return customerStuffVersion;
    }
    #endregion
    #region Delete
    public void DeleteCustomerStuffVersion(int id)
    {
      var customerStuffVersion = GetCustomerStuffVersion(id: id);
      repository.Delete(customerStuffVersion);
    }
    #endregion
    #region Get
    public CustomerStuffVersion GetCustomerStuffVersion(int id) => GetCustomerStuffVersion(selector: e => e, id: id);
    internal TResult GetCustomerStuffVersion<TResult>(
    Expression<Func<CustomerStuffVersion, TResult>> selector,
    int id)
    {
      var customerStuffVersion = GetCustomerStuffVersions(
                    selector: selector,
                    id: id)
                .FirstOrDefault();
      if (customerStuffVersion == null)
        throw new CustomerStuffVersionNotFoundException(id: id);
      return customerStuffVersion;
    }
    #endregion
    #region Gets
    public IQueryable<TResult> GetCustomerStuffVersions<TResult>(
        Expression<Func<CustomerStuffVersion, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<bool> isPublish = null,
        TValue<int> customerStuffId = null)
    {
      var customerStuffVersion = repository.GetQuery<CustomerStuffVersion>();
      if (id != null)
        customerStuffVersion = customerStuffVersion.Where(i => i.Id == id);
      if (code != null)
        customerStuffVersion = customerStuffVersion.Where(i => i.Code == code);
      if (isPublish != null)
        customerStuffVersion = customerStuffVersion.Where(i => i.IsPublish == isPublish);
      if (customerStuffId != null)
        customerStuffVersion = customerStuffVersion.Where(i => i.CustomerStuffId == customerStuffId);
      return customerStuffVersion.Select(selector);
    }
    #endregion
    #region ToResult
    public Expression<Func<CustomerStuffVersion, CustomerStuffVersionResult>> ToCustomerStuffVersionResult =
     customerStuffVersion => new CustomerStuffVersionResult
     {
       Id = customerStuffVersion.Id,
       Code = customerStuffVersion.Code,
       Name = customerStuffVersion.Name,
       IsPublish = customerStuffVersion.IsPublish,
       CustomerStuffId = customerStuffVersion.CustomerStuffId,
       CustomerStuffCode = customerStuffVersion.CustomerStuff.Code,
       CustomerStuffName = customerStuffVersion.CustomerStuff.Name,
       CustomerStuffType = customerStuffVersion.CustomerStuff.Type,
       StuffId = customerStuffVersion.CustomerStuff.StuffId,
       StuffCode = customerStuffVersion.CustomerStuff.Stuff.Code,
       StuffName = customerStuffVersion.CustomerStuff.Stuff.Name,
       CustomerId = customerStuffVersion.CustomerStuff.CustomerId,
       CustomerCode = customerStuffVersion.CustomerStuff.Customer.Code,
       CustomerName = customerStuffVersion.CustomerStuff.Customer.Name,
       RowVersion = customerStuffVersion.RowVersion
     };
    #endregion
    #region Search
    public IQueryable<CustomerStuffVersionResult> SearchCustomerStuffVersion(IQueryable<CustomerStuffVersionResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = query.Where(item =>
            item.Code.Contains(searchText) ||
            item.Name.Contains(searchText));
      }
      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }
      return query;
    }
    #endregion
    #region Sort
    public IOrderedQueryable<CustomerStuffVersionResult> SortCustomerStuffVersionResult(IQueryable<CustomerStuffVersionResult> query,
        SortInput<CustomerStuffVersionSortType> sort)
    {
      switch (sort.SortType)
      {
        case CustomerStuffVersionSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);
        case CustomerStuffVersionSortType.Code:
          return query.OrderBy(a => a.Code, sort.SortOrder);
        case CustomerStuffVersionSortType.Name:
          return query.OrderBy(a => a.Name, sort.SortOrder);
        case CustomerStuffVersionSortType.CustomerStuffCode:
          return query.OrderBy(a => a.CustomerStuffCode, sort.SortOrder);
        case CustomerStuffVersionSortType.CustomerStuffName:
          return query.OrderBy(a => a.CustomerStuffName, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion
    #region ToCustomerStuffVersionComboList
    public IQueryable<CustomerStuffVersionComboResult> ToCustomerStuffVersionComboList(IQueryable<CustomerStuffVersion> input)
    {
      return from customerStuffVersion in input
             select new CustomerStuffVersionComboResult()
             {
               Id = customerStuffVersion.Id,
               Name = customerStuffVersion.Name,
               Code = customerStuffVersion.Code
             };
    }
    #endregion
    #region PublishCustomerStuffVersion
    public CustomerStuffVersion PublishCustomerStuffVersion(
        int id,
        byte[] rowVersion)
    {
      #region Update CustomerStuffVersion 
      var customerStuffVersion = EditCustomerStuffVersion(
              id: id,
              isPublish: true,
              rowVersion: rowVersion);
      #endregion
      return customerStuffVersion;
    }
    #endregion
    #region UnPublishCustomerStuffVersion
    public CustomerStuffVersion UnPublishCustomerStuffVersion(
        int id,
        byte[] rowVersion)
    {
      #region Update CustomerStuffVersion 
      var customerStuffVersion = EditCustomerStuffVersion(
                  id: id,
                  isPublish: false,
                  rowVersion: rowVersion);
      #endregion
      return customerStuffVersion;
    }
    #endregion
  }
}