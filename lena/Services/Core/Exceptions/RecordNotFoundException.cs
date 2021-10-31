using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lena.Services.Core.Exceptions
{
  public class RecordNotFoundException : InternalException
  {
    public int RecordId { get; }
    public Type RecordType { get; }
    public RecordNotFoundException() { }
    public RecordNotFoundException(int recordId, Type recordType) : this(null, recordId, recordType)
    {
    }
    public RecordNotFoundException(string message, Type recordType)
    {
      RecordType = recordType;
    }
    public RecordNotFoundException(string message, int recordId, Type recordType)
    {
      RecordId = recordId;
      RecordType = recordType;
    }

    public override string ToString()
    {
      return $"{RecordType.Name} Record with Id '{RecordId}' Not Found!";
    }
  }
}
