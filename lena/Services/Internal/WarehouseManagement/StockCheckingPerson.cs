using System;
using System.Linq;
using lena.Services.Common;
using lena.Services.Internals.Exceptions;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models;
using lena.Models.Common;
using lena.Models.StockCheckingPerson;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {
    internal void DeleteStockCheckingPerson(int stockCheckingId, int userId)
    {
      repository.Delete(GetStockCheckingPerson(stockCheckingId: stockCheckingId, userId: userId));
    }
    internal StockCheckingPerson GetStockCheckingPerson(int stockCheckingId, int userId)
    {
      var data = GetStockCheckingPersons(stockCheckingId: stockCheckingId, userId: userId).FirstOrDefault();
      if (data == null)
        throw new StockCheckingPersonNotFoundException();
      return data;
    }
    internal IQueryable<StockCheckingPerson> GetStockCheckingPersons(TValue<int> stockCheckingId = null, TValue<int> userId = null)
    {
      var isStockCheckingIdNull = stockCheckingId == null;
      var isUserIdNull = userId == null;
      var data = from d in repository.GetQuery<StockCheckingPerson>()
                 where
                       (isStockCheckingIdNull || d.StockCheckingId == stockCheckingId) &&
                       (isUserIdNull || d.UserId == userId)
                 select d;
      return data;
    }
    internal StockCheckingPerson AddStockCheckingPerson(int stockCheckingId, int userId)
    {
      var scp = repository.Create<StockCheckingPerson>();
      scp.StockCheckingId = stockCheckingId;
      scp.UserId = userId;
      repository.Add(scp);
      return scp;
    }
    internal StockCheckingPerson EditStockCheckingPerson(byte[] rowVersion, int stockCheckingId, int userId)
    {
      var stockCheckingPerson = GetStockCheckingPerson(stockCheckingId, userId);
      stockCheckingPerson.StockCheckingId = stockCheckingId;
      stockCheckingPerson.UserId = userId;
      repository.Update(stockCheckingPerson, rowVersion);
      return stockCheckingPerson;
    }
    internal IOrderedQueryable<StockCheckingPersonResult> SortStockCheckingPersonResult(
        IQueryable<StockCheckingPersonResult> input, SortInput<StockCheckingPersonSortType> options)
    {
      switch (options.SortType)
      {
        case StockCheckingPersonSortType.StockCheckingTitle:
          return input.OrderBy(a => a.StockCheckingTitle, options.SortOrder);
        case StockCheckingPersonSortType.StockCheckingStatus:
          return input.OrderBy(a => a.StockCheckingStatus, options.SortOrder);
        case StockCheckingPersonSortType.UserName:
          return input.OrderBy(a => a.UserName, options.SortOrder);
        case StockCheckingPersonSortType.EmployeeName:
          return input.OrderBy(a => a.EmployeeName, options.SortOrder);
        case StockCheckingPersonSortType.EmployeeCode:
          return input.OrderBy(a => a.EmployeeCode, options.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    internal StockCheckingPersonResult ToStockCheckingPersonResult(StockCheckingPerson input)
    {
      var emp = input.User.Employee;
      var sc = input.StockChecking;
      return new StockCheckingPersonResult
      {
        StockCheckingId = input.StockCheckingId,
        UserId = input.UserId,
        UserName = input.User.UserName,
        EmployeeCode = emp.Code,
        EmployeeName = emp.FirstName + " " + emp.LastName,
        StockCheckingStatus = sc.Status,
        StockCheckingTitle = sc.Title,
      };
    }
    internal IQueryable<StockCheckingPersonResult> ToStockCheckingPersonResultQuery(
        IQueryable<StockCheckingPerson> input)
    {
      return
          from a in input
          let user = a.User
          let emp = a.User.Employee
          let sc = a.StockChecking
          select new StockCheckingPersonResult
          {
            StockCheckingId = a.StockCheckingId,
            UserId = a.UserId,
            UserName = user.UserName,
            EmployeeCode = emp.Code,
            EmployeeName = emp.FirstName + " " + emp.LastName,
            StockCheckingStatus = sc.Status,
            StockCheckingTitle = sc.Title,
          };
    }
  }
}