using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Reports.Exception
{
  public class ReportVersionNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string ReportName { get; }

    public ReportVersionNotFoundException(string reportName)
    {
      this.ReportName = reportName;
    }

    public ReportVersionNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
