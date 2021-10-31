using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.SaleManagement.Customer;
using System;
using System.Linq;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public Cooperator AddCustomer(
        string detailedCode,
        string name,
        short cityId)
    {

      var customer = repository.Create<Cooperator>();
      customer.Name = name;
      customer.CityId = cityId;
      customer.DetailedCode = detailedCode;

      var result = AddCooperator(
                cooperator: customer,
                detailedCode: detailedCode,
                name: name,
                cooperatorType: CooperatorType.Customer);

      return result;
    }
    public Cooperator EditCustomer(
        int id,
        byte[] rowVersion,
        TValue<string> detailedCode = null,
        TValue<string> name = null,
        TValue<string> code = null,
        TValue<short> cityId = null)
    {

      var customer = GetCustomer(id: id);
      var result = EditCooperator(
                    cooperator: customer,
                    rowVersion: rowVersion,
                    detailedCode: detailedCode,
                    cityId: cityId,
                    name: name,
                    code: code);

      return result;
    }
    public void DeleteCustomer(int id)
    {

      DeleteCooperator(id);
    }
    public IQueryable<Cooperator> GetCustomers(
        TValue<int> id = null,
        TValue<string> name = null,
        TValue<bool> confirmationDetailedCode = null,
        TValue<string> Code = null)
    {

      var cooperatorQuery = GetCooperators(
                selector: e => e,
                id: id,
                name: name,
                confirmationDetailedCode: confirmationDetailedCode,
                Code: Code,
                cooperatorType: CooperatorType.Customer);
      var customers = from customer in cooperatorQuery
                      select customer;
      return customers;
    }
    public Cooperator GetCustomer(int id)
    {

      var customer = GetCustomers(id: id).FirstOrDefault();
      if (customer == null)
        throw new CustomerNotFoundException(id);
      return customer;
    }

    public IOrderedQueryable<CustomerResult> SortCustomerResult(
        IQueryable<CustomerResult> input,
        SortInput<CustomerSortType> options)
    {
      switch (options.SortType)
      {
        case CustomerSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case CustomerSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        case CustomerSortType.Code:
          return input.OrderBy(i => i.Code, options.SortOrder);
        case CustomerSortType.DetailedCode:
          return input.OrderBy(i => i.DetailedCode, options.SortOrder);
        case CustomerSortType.ConfirmationDetailedCode:
          return input.OrderBy(i => i.ConfirmationDetailedCode, options.SortOrder);
        case CustomerSortType.CountryTitle:
          return input.OrderBy(i => i.CountryTitle, options.SortOrder);
        case CustomerSortType.CityTitle:
          return input.OrderBy(i => i.CityTitle, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public IQueryable<CustomerResult> ToCustomerResultQuery(IQueryable<Cooperator> query)
    {
      return (from item in query
              select new CustomerResult()
              {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                DetailedCode = item.DetailedCode,
                ConfirmationDetailedCode = item.ConfirmationDetailedCode,
                CityId = item.CityId,
                CountryId = item.City.CountryId,
                CityTitle = item.City.Title,
                CountryTitle = item.City.Country.Title,
                RowVersion = item.RowVersion
              });
    }
    public IQueryable<CustomerComboResult> ToCustomerComboResultQuery(IQueryable<Cooperator> query)
    {
      return (from item in query
              select new CustomerComboResult()
              {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                DetailedCode = item.DetailedCode,
                ConfirmationDetailedCode = item.ConfirmationDetailedCode,
                RowVersion = item.RowVersion
              });
    }
    public CustomerResult ToCustomerResult(Cooperator customer)
    {
      var result = new CustomerResult()
      {
        Id = customer.Id,
        Name = customer.Name,
        Code = customer.Code,
        DetailedCode = customer.DetailedCode,
        ConfirmationDetailedCode = customer.ConfirmationDetailedCode,
        CityId = customer.CityId,
        CountryId = customer.City.CountryId,
        CityTitle = customer.City.Title,
        CountryTitle = customer.City.Country.Title,
        RowVersion = customer.RowVersion
      };
      return result;
    }

    public IQueryable<CustomerResult> SearchCustomerResult(
        IQueryable<CustomerResult> query,
        string searchText,
        AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
      {
        query = from item in query
                where item.Name.Contains(searchText) ||
                      item.Code.Contains(searchText)
                select item;
      }

      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      return query;

    }

    #region ConfirmationCustomerDetailedCode
    public Cooperator ConfirmationCustomerDetailedCode(
        int id,
        string detailedCode,
        byte[] rowVersion)
    {

      var customer = GetCustomer(id: id);
      if (detailedCode != null)
        customer.DetailedCode = detailedCode;
      customer.ConfirmationDetailedCode = true;
      repository.Update(customer, rowVersion);
      return customer;
    }
    #endregion

    #region DisapprovalCustomerDetailedCode
    public Cooperator DisapprovalCustomerDetailedCode(
        int id,
        byte[] rowVersion,
        TValue<string> detailedCode = null
        )
    {

      var customer = GetCustomer(id: id);
      if (detailedCode != null)
        customer.DetailedCode = detailedCode;
      customer.ConfirmationDetailedCode = false;
      repository.Update(customer, rowVersion);
      return customer;
    }
    #endregion
  }
}
