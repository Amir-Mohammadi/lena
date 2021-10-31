using System;
using System.Linq;
using System.Linq.Expressions;
using lena.Services.Common;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.WarehouseManagement.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using lena.Models.Common;
using lena.Models.WarehouseManagement.ExitReceiptDeleteRequestStuffSerial;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.WarehouseManagement
{
  public partial class WarehouseManagement
  {

    #region Add
    public ExitReceiptDeleteRequestStuffSerial AddExitReceiptDeleteRequestStuffSerial(
        int exitReceiptDeleteRequestId,
        int stuffSerialStuffId,
        long stuffSerialStuffCode,
        byte unitId,
        double amount)
    {

      var exitReceiptDeleteRequestStuffSerial = repository.Create<ExitReceiptDeleteRequestStuffSerial>();
      exitReceiptDeleteRequestStuffSerial.ExitReceiptDeleteRequestId = exitReceiptDeleteRequestId;
      exitReceiptDeleteRequestStuffSerial.StuffSerialId = stuffSerialStuffId;
      exitReceiptDeleteRequestStuffSerial.StuffSerialCode = stuffSerialStuffCode;
      exitReceiptDeleteRequestStuffSerial.Amount = amount;
      exitReceiptDeleteRequestStuffSerial.UnitId = unitId;
      repository.Add(exitReceiptDeleteRequestStuffSerial);
      return exitReceiptDeleteRequestStuffSerial;
    }
    #endregion

    #region Get
    public ExitReceiptDeleteRequestStuffSerial GetExitReceiptDeleteRequestStuffSerial(int id)
        => GetExitReceiptDeleteRequestStuffSerial(selector: e => e, id: id);

    public TResult GetExitReceiptDeleteRequestStuffSerial<TResult>(
        Expression<Func<ExitReceiptDeleteRequestStuffSerial, TResult>> selector, int id)
    {

      var getExitReceiptDeleteRequestStuffSerials = GetExitReceiptDeleteRequestStuffSerials(
                selector: selector,
                id: id)


            .FirstOrDefault();

      if (getExitReceiptDeleteRequestStuffSerials == null)
        throw new ExitReceiptDeleteRequestHasNotFoundException(id);
      return getExitReceiptDeleteRequestStuffSerials;
    }

    #endregion

    #region Gets
    public IQueryable<TResult> GetExitReceiptDeleteRequestStuffSerials<TResult>(
        Expression<Func<ExitReceiptDeleteRequestStuffSerial, TResult>> selector,
        TValue<int> id = null,
        TValue<int> exitReceiptDeleteRequestId = null,
        TValue<int> stuffSerialId = null,
        TValue<long> stuffSerialCode = null)
    {

      var query = repository.GetQuery<ExitReceiptDeleteRequestStuffSerial>();
      if (id != null)
        query = query.Where(i => i.Id == id);
      if (exitReceiptDeleteRequestId != null)
        query = query.Where(i => i.ExitReceiptDeleteRequestId == exitReceiptDeleteRequestId);
      if (stuffSerialId != null)
        query = query.Where(i => i.StuffSerialId == stuffSerialId);
      if (stuffSerialCode != null)
        query = query.Where(i => i.StuffSerialCode == stuffSerialCode);

      return query.Select(selector);
    }
    #endregion

    #region ToResult
    public Expression<Func<ExitReceiptDeleteRequestStuffSerial, ExitReceiptDeleteRequestStuffSerialResult>> ToExitReceiptDeleteRequestDeleteStuffSerialResult =
        exitReceiptDeleteRequestStuffSerialResult => new ExitReceiptDeleteRequestStuffSerialResult
        {
          Id = exitReceiptDeleteRequestStuffSerialResult.Id,
          ExitReceiptDeleteRequestId = exitReceiptDeleteRequestStuffSerialResult.ExitReceiptDeleteRequest.Id,
          StuffSerialId = exitReceiptDeleteRequestStuffSerialResult.StuffSerial.StuffId,
          StuffSerialCode = exitReceiptDeleteRequestStuffSerialResult.StuffSerial.Code,
          Serial = exitReceiptDeleteRequestStuffSerialResult.StuffSerial.Serial,
          StuffName = exitReceiptDeleteRequestStuffSerialResult.StuffSerial.Stuff.Name,
          StuffCode = exitReceiptDeleteRequestStuffSerialResult.StuffSerial.Stuff.Code,
          Amount = exitReceiptDeleteRequestStuffSerialResult.Amount,
          UnitId = exitReceiptDeleteRequestStuffSerialResult.UnitId,
          UnitName = exitReceiptDeleteRequestStuffSerialResult.Unit.Name,
          RowVersion = exitReceiptDeleteRequestStuffSerialResult.RowVersion
        };

    #endregion

    #region sort
    public IOrderedQueryable<ExitReceiptDeleteRequestStuffSerialResult> SortExitReceiptDeleteRequestStuffSerialResult(IQueryable<ExitReceiptDeleteRequestStuffSerialResult> query,
       SortInput<ExitReceiptDeleteRequestStuffSerialSortType> sort)
    {
      switch (sort.SortType)
      {
        case ExitReceiptDeleteRequestStuffSerialSortType.Id:
          return query.OrderBy(x => x.Id, sort.SortOrder);
        case ExitReceiptDeleteRequestStuffSerialSortType.StuffSerialId:
          return query.OrderBy(x => x.StuffSerialId, sort.SortOrder);
        case ExitReceiptDeleteRequestStuffSerialSortType.StuffSerialCode:
          return query.OrderBy(x => x.StuffSerialCode, sort.SortOrder);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
    #endregion

    #region search
    public IQueryable<ExitReceiptDeleteRequestStuffSerialResult> SearchExitReceiptDeleteRequestStuffSerialResult(
    IQueryable<ExitReceiptDeleteRequestStuffSerialResult> query,
    string searchText,
    TValue<int> exitReceiptDeleteRequestId,
    AdvanceSearchItem[] advanceSearchItems)
    {
      if (!string.IsNullOrEmpty(searchText))
        query = from item in query
                where item.StuffName.Contains(searchText)
                select item;


      if (advanceSearchItems.Any())
      {
        query = query.Where(advanceSearchItems);
      }

      if (exitReceiptDeleteRequestId != null)
        query = query.Where(i => i.ExitReceiptDeleteRequestId == exitReceiptDeleteRequestId);
      return query;
    }
  }
  #endregion
}

