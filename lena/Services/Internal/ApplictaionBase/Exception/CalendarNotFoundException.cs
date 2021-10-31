using System;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class CalendarNotFoundException : InternalServiceException
  {
    public DateTime Date { get; }

    public CalendarNotFoundException(DateTime date)
    {
      this.Date = date;
    }
  }
}
