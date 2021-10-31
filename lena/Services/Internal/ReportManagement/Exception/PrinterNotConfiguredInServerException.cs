using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement.Exception
{
  public class PrinterNotConfiguredInServerException : InternalServiceException
  {
    public string PrinterName { get; }

    public PrinterNotConfiguredInServerException(string printerName)
    {
      this.PrinterName = printerName;
    }
  }
}
