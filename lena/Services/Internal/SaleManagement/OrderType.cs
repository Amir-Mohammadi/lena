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
    public OrderType AddOrderType(string name)
    {
      var orderType = repository.Create<OrderType>();
      orderType.Name = name;
      repository.Add(orderType);
      return orderType;
    }
    public OrderTypeResult ToOrderTypeResult(OrderType orderType)
    {
      var result = new OrderTypeResult
      {
        Id = orderType.Id,
        Name = orderType.Name,
        RowVersion = orderType.RowVersion
      };
      return result;
    }
    public IQueryable<OrderTypeResult> ToOrderTypeResultQuery(IQueryable<OrderType> query)
    {
      return (from item in query
              select new OrderTypeResult()
              {
                Id = item.Id,
                Name = item.Name,
                RowVersion = item.RowVersion
              });
    }
    public void DeleteOrderType(int id)
    {
      var orderType = GetOrderType(id: id);
      repository.Delete(orderType);
    }
    public OrderType EditOrderType(byte[] rowVersion, int id, TValue<string> name = null)
    {
      var orderType = GetOrderType(id: id);
      orderType.Name = name;
      repository.Update(orderType, rowVersion: rowVersion);
      return orderType;
    }
    public OrderType GetOrderType(int id)
    {
      var orderType = GetOrderTypes(id: id).FirstOrDefault();
      if (orderType == null)
        throw new OrderTypeNotFoundException(id);
      return orderType;
    }
    public IQueryable<OrderType> GetOrderTypes(TValue<int> id = null, TValue<string> name = null)
    {
      var isIdNUll = id == null;
      var isNameNull = name == null;
      var orderTypes = from orderType in repository.GetQuery<OrderType>()
                       where (isIdNUll || orderType.Id == id) &&
                                     (isNameNull || orderType.Name == name)
                       select orderType;
      return orderTypes;
    }
    public IQueryable<OrderTypeResult> SearchOrderTypeResult(IQueryable<OrderTypeResult> query, string search)
    {
      if (string.IsNullOrEmpty(search)) return query;
      return from item in query
             where
             item.Name.Contains(search)
             select item;
    }
    public IOrderedQueryable<OrderTypeResult> SortOrderTypeResult(IQueryable<OrderTypeResult> input, SortInput<OrderTypeSortType> options)
    {
      switch (options.SortType)
      {
        case OrderTypeSortType.Id:
          return input.OrderBy(i => i.Id, options.SortOrder);
        case OrderTypeSortType.Name:
          return input.OrderBy(i => i.Name, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}