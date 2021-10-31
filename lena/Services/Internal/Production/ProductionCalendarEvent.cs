using System;
using System.Linq;
using lena.Services.Core;
////using lena.Services.Core.Foundation.Action;
//using lena.Services.Core.Foundation.Service.Internal.Action;
using lena.Services.Internals.ApplictaionBase.Exception;
using lena.Models.Common;
using lena.Domains;
using lena.Domains.Enums;
using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Production
{
  public partial class Production
  {
    #region Gets
    public IQueryable<CalendarEvent> GetProductionCalendarEvents(TValue<int> id = null,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null,
        TValue<int> productionOrderId = null)
    {

      var calendarEventQuery = App.Internals.ApplicationBase
                .GetCalendarEvents(id: id,
                    dateTime: dateTime,
                    duration: duration,
                    type: CalendarEventType.Production);
      if (productionOrderId != null)
        calendarEventQuery = calendarEventQuery.Where(i => i.ProductionOrder.Id == productionOrderId);
      return calendarEventQuery;
    }
    #endregion
    #region Get
    public CalendarEvent GetProductionCalendarEvent(int id)
    {

      var calendarEvent = GetProductionCalendarEvents(id: id).FirstOrDefault();
      if (calendarEvent == null)
        throw new CalendarEventNotFoundException(id);
      return calendarEvent;
    }
    #endregion
    #region Add
    public CalendarEvent AddProductionCalendarEvent(DateTime dateTime,
        long duration,
        ProductionOrder productionOrder)
    {

      var productionCalendarEvent = repository.Create<CalendarEvent>();
      productionCalendarEvent.ProductionOrder = productionOrder;
      var result = App.Internals.ApplicationBase
                .AddCalendarEvent(productionCalendarEvent,
                    dateTime: dateTime,
                    duration: duration,
                    workShiftId: null,
                    type: CalendarEventType.Production);
      return result;
    }
    #endregion
    #region Delete
    public void DeleteProductionCalendarEvent(int id)
    {

      var productionCalendarEvent = GetProductionCalendarEvent(id);
      repository.Delete(productionCalendarEvent);
    }
    #endregion
    #region Edit
    public CalendarEvent EditProductionCalendarEvent(
        int id,
        byte[] rowVersion,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null)
    {

      var productionCalendarEvent = this.GetProductionCalendarEvent(id: id);
      return EditProductionCalendarEvent(
                productionCalendarEvent: productionCalendarEvent,
                rowVersion: rowVersion,
                dateTime: dateTime,
                duration: duration);
    }


    public CalendarEvent EditProductionCalendarEvent(
        CalendarEvent productionCalendarEvent,
        byte[] rowVersion,
        TValue<DateTime> dateTime = null,
        TValue<long> duration = null)
    {


      var result = App.Internals.ApplicationBase.EditCalendarEvent(
                calendarEvent: productionCalendarEvent,
                rowVersion: rowVersion,
                dateTime: dateTime,
                duration: duration);
      return result;
    }
    #endregion
  }
}
