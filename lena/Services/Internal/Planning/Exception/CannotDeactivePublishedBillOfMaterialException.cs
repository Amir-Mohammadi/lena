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
  public class CannotDeactivePublishedBillOfMaterialException : InternalServiceException
  {
    public int StuffId { get; set; }
    public int VersionId { get; set; }


    public CannotDeactivePublishedBillOfMaterialException(int? stuffId = null, int? versionId = null)
    {
      if (stuffId != null)
        this.StuffId = stuffId.Value;
      if (versionId != null)
        this.VersionId = versionId.Value;
    }
  }
}
