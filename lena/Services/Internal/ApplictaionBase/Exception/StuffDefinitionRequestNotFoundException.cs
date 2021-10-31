using lena.Services.Core.Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading.Tasks;
using lena.Domains.Enums;
namespace lena.Services.Internals.ApplictaionBase.Exception
{
  public class StuffDefinitionRequestNotFoundException : InternalServiceException
  {
    public int Id { get; set; }
    public StuffDefinitionRequestNotFoundException(int id)
    {
      this.Id = id;
    }
  }
}
