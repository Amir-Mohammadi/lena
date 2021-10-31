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
  #region BackLog
  public class BackLogNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public BackLogNotFoundException(int id)
    {
      this.Id = id;
    }

  }

  #endregion
}
