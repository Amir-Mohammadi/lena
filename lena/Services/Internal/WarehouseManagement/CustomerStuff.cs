using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.Supplies.CustomerStuff;
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
    public CustomerStuff AddCustomerStuff(
        string code,
        string name,
        int stuffId,
        int customerId,
        string manufacturerCode,
        string technicalNumber,
        CustomerStuffType type)
    {

      var customerStuff = repository.Create<CustomerStuff>();
      customerStuff.Code = code.ToUpper();
      customerStuff.Name = name;
      customerStuff.StuffId = stuffId;
      customerStuff.CustomerId = customerId;
      customerStuff.Type = type;
      customerStuff.ManufacturerCode = manufacturerCode.ToUpper();
      customerStuff.TechnicalNumber = technicalNumber.ToUpper();
      repository.Add(customerStuff);
      return customerStuff;
    }
    #endregion

    #region Edit
    public CustomerStuff EditCustomerStuff(
        byte[] rowVersion,
        int id,
        TValue<string> code = null,
        TValue<string> name = null,
        TValue<CustomerStuffType> type = null,
        TValue<string> manufacturerCode = null,
        TValue<string> technicalNumber = null,
        TValue<int> stuffId = null,
        TValue<int> customerId = null
        )
    {

      var customerStuff = GetCustomerStuff(id: id);
      if (code != null)
        customerStuff.Code = code.Value.ToUpper();
      if (name != null)
        customerStuff.Name = name;
      if (type != null)
        customerStuff.Type = type;
      if (stuffId != null)
        customerStuff.StuffId = stuffId;
      if (customerId != null)
        customerStuff.CustomerId = customerId;
      if (manufacturerCode != null)
        customerStuff.ManufacturerCode = manufacturerCode.Value.ToUpper();
      if (technicalNumber != null)
        customerStuff.TechnicalNumber = technicalNumber.Value.ToUpper();

      repository.Update(customerStuff, customerStuff.RowVersion);
      return customerStuff;
    }
    #endregion

    #region Delete
    public void DeleteCustomerStuff(int id)
    {

      var customerStuff = GetCustomerStuff(id: id);
      repository.Delete(customerStuff);
    }
    #endregion

    #region Get
    public CustomerStuff GetCustomerStuff(int id) => GetCustomerStuff(selector: e => e, id: id);
    internal TResult GetCustomerStuff<TResult>(
    Expression<Func<CustomerStuff, TResult>> selector,
    int id)
    {


      var customerStuff = GetCustomerStuffs(
                    selector: selector,
                    id: id)


                .FirstOrDefault();
      if (customerStuff == null)
        throw new CustomerStuffNotFoundException(id: id);
      return customerStuff;

    }
    #endregion

    #region Gets
    public IQueryable<TResult> GetCustomerStuffs<TResult>(
        Expression<Func<CustomerStuff, TResult>> selector,
        TValue<int> id = null,
        TValue<string> code = null,
        TValue<int> year = null)
    {


      var customerStuff = repository.GetQuery<CustomerStuff>();

      if (id != null)
        customerStuff = customerStuff.Where(i => i.Id == id);
      if (code != null)
        customerStuff = customerStuff.Where(i => i.Code == code);

      return customerStuff.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<CustomerStuff, CustomerStuffResult>> ToCustomerStuffResult =
     customerStuff => new CustomerStuffResult
     {
       Id = customerStuff.Id,
       Code = customerStuff.Code,
       Name = customerStuff.Name,
       CustomerId = customerStuff.CustomerId,
       CustomerCode = customerStuff.Customer.Code,
       CustomerName = customerStuff.Customer.Name,
       StuffId = customerStuff.StuffId,
       StuffCode = customerStuff.Stuff.Code,
       StuffName = customerStuff.Stuff.Name,
       ManufacturerCode = customerStuff.ManufacturerCode,
       TechnicalNumber = customerStuff.TechnicalNumber,
       RowVersion = customerStuff.RowVersion
     };

    #endregion

    #region Search
    public IQueryable<CustomerStuffResult> SearchCustomerStuff(IQueryable<CustomerStuffResult> query,
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
    public IOrderedQueryable<CustomerStuffResult> SortCustomerStuffResult(IQueryable<CustomerStuffResult> query,
        SortInput<CustomerStuffSortType> sort)
    {
      switch (sort.SortType)
      {
        case CustomerStuffSortType.Id:
          return query.OrderBy(a => a.Id, sort.SortOrder);

        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region ToCustomerStuffComboList
    public IQueryable<CustomerStuffComboResult> ToCustomerStuffComboList(IQueryable<CustomerStuff> input)
    {
      return from customerStuff in input
             select new CustomerStuffComboResult()
             {
               Id = customerStuff.Id,
               Name = customerStuff.Name,
               Code = customerStuff.Code
             };
    }
    #endregion

  }
}
