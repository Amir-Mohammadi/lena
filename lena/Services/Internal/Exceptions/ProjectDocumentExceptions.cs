using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lena.Services.Core.Foundation;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.Exceptions
{
  #region Project Document

  public class ProjectDocumentNotFoundException : InternalServiceException
  {
    public ProjectDocumentNotFoundException(int id)
    {
      Id = id;
    }

    public int Id { get; }
  }


  #endregion
}
