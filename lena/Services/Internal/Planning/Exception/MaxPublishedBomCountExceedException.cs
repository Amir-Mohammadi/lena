using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Planning.Exception
{
  public class MaxPublishedBomCountExceedException : InternalServiceException
  {
    public int StuffId { get; }
    public string StuffCode { get; set; }
    public int PublishedBomCount { get; }
    public MaxPublishedBomCountExceedException(int stuffId, string stuffCode, int publishedCount)
    {
      StuffId = stuffId;
      PublishedBomCount = publishedCount;
      StuffCode = stuffCode;
    }
  }
}
