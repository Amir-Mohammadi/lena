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
  public class ReportNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public string Name { get; }

    public ReportNotFoundException(int id)
    {
      this.Id = id;
    }

    public ReportNotFoundException(string name)
    {
      this.Name = name;
    }
  }
}
