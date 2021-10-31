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
  public class RecordNotFoundException : InternalServiceException
  {
    public int Id { get; }
    public Type EntityType { get; }

    public RecordNotFoundException(int id, Type entityType)
    {
      Id = id;
      EntityType = entityType;
    }
  }
}
