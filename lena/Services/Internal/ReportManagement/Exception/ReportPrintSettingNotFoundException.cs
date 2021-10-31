using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ReportManagement.Exception
{
  public class ReportPrintSettingNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public int ReportId { get; }
    public string ReportName { get; }
    public int UserId { get; }

    public ReportPrintSettingNotFoundException(int id)
    {
      this.Id = id;
    }

    public ReportPrintSettingNotFoundException(string reportName, int userId)
    {
      this.ReportName = reportName;
      this.UserId = userId;
    }

    public ReportPrintSettingNotFoundException(int reportId, int userId)
    {
      this.ReportId = reportId;
      this.UserId = userId;
    }
  }
}
