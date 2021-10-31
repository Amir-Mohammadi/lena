using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Core;

using lena.Services.Internals.Exceptions;
using lena.Services.Internals.SaleManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using System.Collections.Generic;
using System.Linq.Expressions;
using lena.Services.Internals.ProjectManagement.Exception;
using lena.Models.ApplicationBase.BaseEntity;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.SaleManagement
{
  public partial class SaleManagement
  {
    public Cooperator AddCooperator(
        Cooperator cooperator,
        string detailedCode,
        string name,
        CooperatorType cooperatorType)
    {

      cooperator.Name = name;
      cooperator.DetailedCode = detailedCode;
      cooperator.ConfirmationDetailedCode = false;
      cooperator.CooperatorType = cooperatorType;

      repository.Add(cooperator);

      GenerateCooperatorCode(cooperator: cooperator);

      return cooperator;
    }
    public Cooperator GenerateCooperatorCode(
        Cooperator cooperator)
    {

      var code = cooperator.Id.ToString("0000");
      EditCooperator(
                cooperator: cooperator,
                rowVersion: cooperator.RowVersion,
                code: code);
      return cooperator;
    }
    public Cooperator EditCooperator(
        Cooperator cooperator,
        byte[] rowVersion,
        TValue<string> detailedCode = null,
        TValue<string> name = null,
        TValue<string> code = null,
        TValue<short> cityId = null,
        TValue<CooperatorType> cooperatorType = null,
        TValue<ProviderType> providerType = null)
    {

      if (name != null)
        cooperator.Name = name;
      if (code != null)
        cooperator.Code = code;
      if (detailedCode != null)
        cooperator.DetailedCode = detailedCode;
      if (cooperatorType != null)
        cooperator.CooperatorType = cooperatorType;
      if (providerType != null)
        cooperator.ProviderType = providerType;
      if (cityId != null)
        cooperator.CityId = cityId;
      repository.Update(entity: cooperator, rowVersion: cooperator.RowVersion);
      return cooperator;
    }
    public Cooperator EditCooperator(
        int id,
        byte[] rowVersion,
        TValue<string> name = null,
        TValue<string> code = null,
        TValue<CooperatorType> cooperatorType = null,
        TValue<ProviderType> providerType = null)
    {

      var cooperator = GetCooperator(id: id);
      EditCooperator(
                    cooperator: cooperator,
                    rowVersion: rowVersion,
                    name: name,
                    code: code,
                    providerType: providerType,
                    cooperatorType: cooperatorType);
      return cooperator;
    }
    public void DeleteCooperator(int id)
    {

      var cooperator = GetCooperator(id: id);
      repository.Delete(cooperator);
    }
    public IQueryable<TResult> GetCooperators<TResult>(
        Expression<Func<Cooperator, TResult>> selector,
        TValue<int> id = null,
        TValue<bool> confirmationDetailedCode = null,
        TValue<string> name = null,
        TValue<string> Code = null,
        TValue<CooperatorType> cooperatorType = null,
        TValue<ProviderType> providerType = null)
    {

      var cooperators = repository.GetQuery<Cooperator>();

      if (id != null)
        cooperators = cooperators.Where(i => i.Id == id);
      if (confirmationDetailedCode != null)
        cooperators = cooperators.Where(i => i.ConfirmationDetailedCode == confirmationDetailedCode);
      if (name != null)
        cooperators = cooperators.Where(i => i.Name == name);
      if (Code != null)
        cooperators = cooperators.Where(i => i.Code == Code);
      if (cooperatorType != null)
        cooperators = cooperators.Where(i => i.CooperatorType == cooperatorType);
      if (providerType != null)
        cooperators = cooperators.Where(i => i.ProviderType == providerType);

      return cooperators.Select(selector);
    }
    public Cooperator GetCooperator(int id)
    {

      var cooperator = GetCooperators(
                selector: e => e,
                id: id)

            .FirstOrDefault();

      if (cooperator == null)
        throw new CooperatorNotFoundException(id);
      return cooperator;
    }
    public IOrderedQueryable<CooperatorResult> SortCoopratorResult(
        IQueryable<CooperatorResult> input,
        SortInput<CooperatorSortType> options)
    {
      switch (options.SortType)
      {
        case CooperatorSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    public IQueryable<CooperatorResult> SearchCoopratorResult(
        IQueryable<CooperatorResult> query,
        string searchText)
    {
      if (!string.IsNullOrWhiteSpace(searchText))
        query = from item in query
                where item.Name.Contains(searchText) ||
                item.Code.Contains(searchText)
                select item;
      return query;
    }
    public IQueryable<CooperatorResult> ToCooperatorResultQuery(IQueryable<Cooperator> query)
    {
      return from item in query
             select new CooperatorResult()
             {
               Id = item.Id,
               Name = item.Name,
               RowVersion = item.RowVersion,
               Code = item.Code,
             };
    }
    public CooperatorResult ToCooperatorResult(Cooperator cooperator)
    {
      CooperatorResult result = new CooperatorResult()
      {
        Id = cooperator.Id,
        Name = cooperator.Name,
        RowVersion = cooperator.RowVersion,
        Code = cooperator.Code
      };
      return result;
    }
    public IQueryable<CooperatorResult> ToCooperatorResults(IQueryable<Cooperator> cooperators)
    {
      return cooperators.Select(s => new CooperatorResult
      {
        Id = s.Id,
        Name = s.Name,
        RowVersion = s.RowVersion,
        Code = s.Code
      });
    }
  }
}
